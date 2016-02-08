using System.Web.Optimization;
using BundleTransformer.Core.Builders;
using BundleTransformer.Core.Bundles;
using BundleTransformer.Core.Orderers;
using BundleTransformer.Core.Transformers;
using BundleTransformer.Core.Translators;
using BundleTransformer.Csso.Minifiers;
using BundleTransformer.Less.Translators;
using BundleTransformer.SassAndScss.Translators;

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

			var styleTransformer = new StyleTransformer(new KryzhanovskyCssMinifier(), new ITranslator[] {new LessTranslator() })
			{
				CombineFilesBeforeMinification = true,
				UsePreMinifiedFiles = true
			};

			var styleSassTransformer = new StyleTransformer(new KryzhanovskyCssMinifier(), new ITranslator[] { new SassAndScssTranslator() })
			{
				CombineFilesBeforeMinification = true,
				UsePreMinifiedFiles = true
			};

			#region UnitGridSystem

			var styleUgsBundle = new CustomStyleBundle("~/unitgs/style");
			styleUgsBundle.Include("~/content/css/styleUgs.scss");
			styleUgsBundle.Transforms.Add(styleSassTransformer);
			styleUgsBundle.Builder = nullBuilder;
			styleUgsBundle.Orderer = nullOrderer;
			bundles.Add(styleUgsBundle);

			#endregion

			var styleBundle = new CustomStyleBundle("~/site/style");
			styleBundle.Include("~/content/css/site.less");
			styleBundle.Transforms.Add(styleTransformer);
			styleBundle.Builder = nullBuilder;
			styleBundle.Orderer = nullOrderer;
			bundles.Add(styleBundle);

			var sBtBundle = new CustomStyleBundle("~/site/sbootstrap");
			sBtBundle.Include("~/content/css/bootstrap/bootstrap.css",
				"~/content/css/ie10-viewport-bug-workaround.css");
			sBtBundle.Transforms.Add(styleTransformer);
			sBtBundle.Builder = nullBuilder;
			sBtBundle.Orderer = nullOrderer;
			bundles.Add(sBtBundle);

			var sGtBundle = new CustomStyleBundle("~/site/sgtreetable");
			sGtBundle.Include("~/content/css/gtreetable/bootstrap-gtreetable.css");
			sGtBundle.Transforms.Add(styleTransformer);
			sGtBundle.Builder = nullBuilder;
			sGtBundle.Orderer = nullOrderer;
			bundles.Add(sGtBundle);

			var sFancyTree = new CustomStyleBundle("~/site/sfancytree");
			sFancyTree.Include("~/content/css/fancytree/skin-bootstrap/ui.fancytree.css");
			sFancyTree.Transforms.Add(styleTransformer);
			sFancyTree.Builder = nullBuilder;
			sFancyTree.Orderer = nullOrderer;
			bundles.Add(sFancyTree);





			var jqBundle = new CustomScriptBundle("~/site/jq");
			jqBundle.Include("~/content/js/jq/jquery-{version}.js");
			jqBundle.Transforms.Add(jsTransformer);
			jqBundle.Orderer = nullOrderer;
			bundles.Add(jqBundle);

			var jqUiBundel = new CustomScriptBundle("~/site/jqUi");
			jqUiBundel.Include("~/content/js/jq/ui/jquery-ui-{version}.js", "~/content/js/jq/browser/jquery.browser.js");
			jqUiBundel.Transforms.Add(jsTransformer);
			jqUiBundel.Orderer = nullOrderer;
			bundles.Add(jqUiBundel);

			var jBtBundle = new CustomScriptBundle("~/site/jbootstrap");
			jBtBundle.Include("~/content/js/bootstrap/bootstrap.js",
				"~/content/js/ie10-viewport-bug-workaround.js");
			jBtBundle.Transforms.Add(jsTransformer);
			jBtBundle.Orderer = nullOrderer;
			bundles.Add(jBtBundle);

			var jGtBundle = new CustomScriptBundle("~/site/jgtreetanle");
			jGtBundle.Include("~/content/js/gtreetable/bootstrap-gtreetable.js",
				"~/content/js/gtreetable/languages/bootstrap-gtreetable.ru.js");
			jGtBundle.Transforms.Add(jsTransformer);
			jGtBundle.Orderer = nullOrderer;
			bundles.Add(jGtBundle);

			var jqCkBundle = new CustomScriptBundle("~/site/jqCookie");
			jqCkBundle.Include("~/content/js/jq/cookie/jquery.cookie-{version}.js");
			jqCkBundle.Transforms.Add(jsTransformer);
			jqCkBundle.Orderer = nullOrderer;
			bundles.Add(jqCkBundle);

			var jqTsBundle = new CustomScriptBundle("~/site/jqToaster");
			jqTsBundle.Include("~/content/js/jq/toaster/jquery.toaster.js");
			jqTsBundle.Transforms.Add(jsTransformer);
			jqTsBundle.Orderer = nullOrderer;
			bundles.Add(jqTsBundle);

			var jqFancyTreeBundle = new CustomScriptBundle("~/site/jqFancyTree");
			jqFancyTreeBundle.Include("~/content/js/jq/fancytree/jquery.fancytree.js",
				"~/content/js/jq/fancytree/jquery.fancytree.glyph.js");
			jqFancyTreeBundle.Transforms.Add(jsTransformer);
			jqFancyTreeBundle.Orderer = nullOrderer;
			bundles.Add(jqFancyTreeBundle);
		}
	}
}