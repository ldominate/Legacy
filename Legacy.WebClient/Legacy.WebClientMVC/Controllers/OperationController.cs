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
			if (ModelState.IsValid)
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
			Response.StatusCode = (int)HttpStatusCode.BadRequest;
			return Json(ModelState.ToString());
		}

	}
}