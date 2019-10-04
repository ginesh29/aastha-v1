using System.Web.Optimization;

namespace AASTHA
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquerymain").Include(
                   "~/Scripts/jquery-2.2.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.unobtrusive*"
                        ));
            bundles.Add(new ScriptBundle("~/bundles/corejs").Include(
                       "~/Scripts/main-gsap.js",
                       "~/Scripts/bootstrap.js",
                       "~/Scripts/resizeable.js",
                       "~/Scripts/neon-api.js",
                       "~/Scripts/neon-custom.js"
                       ));


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/Site.css"));

            bundles.Add(new StyleBundle("~/bundle/corecss").
                    Include("~/Content/neon-core.css", new CssRewriteUrlTransform()).
                    Include("~/Content/neon-theme.css", new CssRewriteUrlTransform()).
                    Include("~/Content/neon-forms.css", new CssRewriteUrlTransform()).
                    Include("~/Content/custom.css", new CssRewriteUrlTransform()));


        }
    }
}