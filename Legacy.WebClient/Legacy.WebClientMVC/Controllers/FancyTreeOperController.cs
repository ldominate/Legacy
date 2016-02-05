using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Legacy.Domain.Operations;
using Legacy.WebClientMVC.ViewModels.FancyTree;

namespace Legacy.WebClientMVC.Controllers
{
	[RoutePrefix("fancyoper")]
	public class FancyTreeOperController : Controller
	{
		private readonly IOperationProvider _provider;

		public FancyTreeOperController(IOperationProvider provider)
		{
			_provider = provider;
		}

		[Route("")]
		public ActionResult Index()
		{
			return View();
		}

		[Route("nodes/{id:int?}")]
		public JsonResult Nodes(int? id)
		{
			var result = _provider.GetList(new OperationListRequest { GroupId = id, IsDelete = false });

			return Json(result.Result.Select(o => new FancyTreeNode(o)), JsonRequestBehavior.AllowGet);
		}

	}
}