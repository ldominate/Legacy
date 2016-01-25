using System.Web.Optimization;
using BundleTransformer.Core.Builders;
using BundleTransformer.Core.Bundles;
using BundleTransformer.Core.Orderers;
using BundleTransformer.Core.Transformers;

namespace Legacy.WebClientMVC
{
	public class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			BundleTable.EnableOptimizations = false;

			var jsTransformer = new ScriptTransformer();
			var nullOrderer = new NullOrderer();
			var nullBuilder = new NullBuilder();

			var jqBundle = new CustomScriptBundle("~/site/jq");
			jqBundle.Include("~/content/js/jq/jquery-{version}.js");
			jqBundle.Transforms.Add(jsTransformer);
			jqBundle.Orderer = nullOrderer;
			bundles.Add(jqBundle);
		}
	}
}