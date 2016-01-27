using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Legacy.Domain.Operations;
using Legacy.WebClientMVC.ViewModels.Operation;

namespace Legacy.WebClientMVC.Controllers
{
	public class OperationController : Controller
	{
		private readonly IOperationProvider _operationProvider;

		public OperationController(IOperationProvider operationProvider)
		{
			_operationProvider = operationProvider;
		}

		// GET: Operation
		public ActionResult Index()
		{
			return View();
		}

		public JsonResult Nodes()
		{
			var result = _operationProvider.GetList(new OperationListRequest { IsDelete = false });

			return Json(new { nodes = result.Result.Select(o => new Node(o))}, JsonRequestBehavior.AllowGet);
		}
	}
}