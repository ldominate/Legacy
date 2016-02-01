using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Legacy.Domain.Operations;
using Legacy.WebClientMVC.ViewModels.Operation;

namespace Legacy.WebClientMVC.Controllers
{
	[RoutePrefix("operation")]
	public class OperationController : Controller
	{
		private readonly IOperationProvider _operationProvider;

		public OperationController(IOperationProvider operationProvider)
		{
			_operationProvider = operationProvider;
		}

		[Route("")]
		public ActionResult Index()
		{
			return View();
		}

		[Route("nodes/{id:int?}")]
		public JsonResult Nodes(int? id)
		{
			var result = _operationProvider.GetList(new OperationListRequest { GroupId = id, IsDelete = false });

			return Json(new { nodes = result.Result.Select(o => new Node(o))}, JsonRequestBehavior.AllowGet);
		}

		[HttpPost, Route("add")]
		public JsonResult Add(Node node)
		{
			if (ModelState.IsValid)
			{
				var operation = node.Operation;

				if (operation.GroupId > 0)
				{
					var parentResult = _operationProvider.GetById(operation.GroupId ?? 0);

					if (!parentResult.Success || parentResult.Result == null)
					{
						Response.StatusCode = (int)HttpStatusCode.BadRequest;
						return Json(parentResult.ErrorMessage);
					}
					operation.Level = parentResult.Result.Level + 1;
				}

				var maxOrderResult = _operationProvider.MaxOrder(operation.GroupId ?? 0);

				if (maxOrderResult.Success)
				{
					operation.Order = maxOrderResult.Result;
				}

				var result = _operationProvider.Add(operation);

				if (!result.Success)
				{
					Response.StatusCode = (int)HttpStatusCode.BadRequest;
					return Json(result.ErrorMessage);
				}
				return Json(new Node(operation));
			}
			Response.StatusCode = (int)HttpStatusCode.BadRequest;
			return Json(ModelState.ToString());
		}

		[HttpPost, Route("update")]
		public JsonResult Update(Node node)
		{
			if (ModelState.IsValid)
			{
				var operation = node.Operation;

				var getResult = _operationProvider.GetById(operation.Id);

				if (!getResult.Success || getResult.Result == null)
				{
					Response.StatusCode = (int)HttpStatusCode.BadRequest;
					return Json(getResult.ErrorMessage);
				}

				operation.Type = getResult.Result.Type;

				var updateResult = _operationProvider.Update(operation);

				if (!updateResult.Success)
				{
					Response.StatusCode = (int)HttpStatusCode.BadRequest;
					return Json(updateResult.ErrorMessage);
				}
				return Json(new Node(operation));
			}
			Response.StatusCode = (int)HttpStatusCode.BadRequest;
			return Json(ModelState.ToString());
		}

		[HttpPost, Route("delete")]
		public JsonResult Delete(int id)
		{
			var getResult = _operationProvider.GetById(id);

			if (!getResult.Success || getResult.Result == null)
			{
				Response.StatusCode = (int)HttpStatusCode.BadRequest;
				return Json(getResult.ErrorMessage);
			}

			var deleteResult = _operationProvider.ForceDelete(id);

			if (!deleteResult.Success)
			{
				Response.StatusCode = (int)HttpStatusCode.BadRequest;
				return Json(deleteResult.ErrorMessage);
			}
			return Json(deleteResult.Success);
		}

		[HttpPost, Route("move")]
		public JsonResult Move(MoveRequest move)
		{
			var getResult = _operationProvider.GetById(move.id);

			if (!getResult.Success || getResult.Result == null)
			{
				Response.StatusCode = (int)HttpStatusCode.BadRequest;
				return Json(getResult.ErrorMessage);
			}
			var operation = getResult.Result;

			var getRelatedResult = _operationProvider.GetById(move.related);

			if (!getResult.Success || getResult.Result == null)
			{
				Response.StatusCode = (int)HttpStatusCode.BadRequest;
				return Json(getRelatedResult.ErrorMessage);
			}
			var related = getRelatedResult.Result;

			if (move.position.Contains("before") || move.position.Contains("after"))
			{
				var listResult = _operationProvider.GetList(new OperationListRequest { GroupId = related.GroupId, IsDelete = false });

				if (!listResult.Success)
				{
					Response.StatusCode = (int)HttpStatusCode.BadRequest;
					return Json(getResult.ErrorMessage);
				}
				if (listResult.AllCount > 0)
				{
					var order = 0;
					foreach (var item in listResult.Result)
					{
						if(item.Id == operation.Id) continue;

						if (item.Id == move.related)
						{
							if (move.position.Contains("before"))
							{
								operation.Order = order++;

								item.Order = order++;

								var setResult = _operationProvider.SetOrder(item);

								if (!setResult.Success) break;
							}
							else if (move.position.Contains("after"))
							{
								item.Order = order++;

								operation.Order = order++;

								var setResult = _operationProvider.SetOrder(item);

								if (!setResult.Success) break;
							}
							operation.GroupId = item.GroupId;
							operation.Level = item.Level;
						}
						else
						{
							item.Order = order++;

							var setResult = _operationProvider.SetOrder(item);

							if (!setResult.Success) break;
						}
					}
					var setLevelResult = _operationProvider.SetLevel(operation);

					if (!setLevelResult.Success)
					{
						Response.StatusCode = (int)HttpStatusCode.BadRequest;
						return Json(setLevelResult.ErrorMessage);
					}
				}
				return Json(new Node(operation));
			}
			if (move.position.Contains("lastChild"))
			{
				var maxOrderResult = _operationProvider.MaxOrder(operation.GroupId ?? 0);

				if (!maxOrderResult.Success)
				{
					Response.StatusCode = (int)HttpStatusCode.BadRequest;
					return Json(maxOrderResult.ErrorMessage);
				}
				operation.Order = maxOrderResult.Result;

				operation.GroupId = related.Id;

				operation.Level = related.Level + 1;

				var setLevelResult = _operationProvider.SetLevel(operation);

				if (!setLevelResult.Success)
				{
					Response.StatusCode = (int)HttpStatusCode.BadRequest;
					return Json(setLevelResult.ErrorMessage);
				}
			}
			return Json(new Node(operation));
		}

	}
}