using System.Web.Optimization;

namespace PhotoProspector
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterScriptBundles(bundles);
            RegisterStyleBundles(bundles);
        }

        public static void RegisterScriptBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
               "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryredirect").Include(
                "~/Scripts/jquery.redirect.js"));

            bundles.Add(new ScriptBundle("~/bundles/upload").Include(
                "~/Scripts/site.upload.js",
                "~/Scripts/site.upload.button.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryform").Include(
                "~/Scripts/jquery.form.js"));

            bundles.Add(new ScriptBundle("~/bundles/scan").Include(
                "~/Scripts/site.scan.js"));
        }

        public static void RegisterStyleBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/site.css",
                "~/Content/corner_ribbon.css",
                "~/Content/vicons-font.css"));

            bundles.Add(new StyleBundle("~/Content/upload").Include(
                "~/Content/site.upload.css",
                "~/Content/site.upload.button.css"));

            bundles.Add(new StyleBundle("~/Content/Control/uploading").Include(
                "~/Content/Uploading/uploading.css"));

            bundles.Add(new StyleBundle("~/Content/scan").Include(
                "~/Content/site.scan.css",
                "~/Content/site.scan.button.css"));
        }
    }
}
