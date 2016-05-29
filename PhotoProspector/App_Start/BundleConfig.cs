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
               "~/Scripts/jquery-{version}.js",
               "~/Scripts/imagesloaded.pkgd.min.js"));

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

            bundles.Add(new ScriptBundle("~/bundles/loadgo").Include(
                "~/Scripts/LoadGo/loadgo.js"));

            bundles.Add(new ScriptBundle("~/bundles/masonry").Include(
                "~/Scripts/Masonry/masonry.pkgd.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/fancybox").Include(
                "~/Scripts/FancyBox/jquery.fancybox.pack.js"));

            bundles.Add(new ScriptBundle("~/bundles/site").Include(
                "~/Scripts/site.js"));

            bundles.Add(new ScriptBundle("~/bundles/upload").Include(
                "~/Scripts/site.upload.js",
                "~/Scripts/site.upload.button.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryform").Include(
                "~/Scripts/jquery.form.js"));

            bundles.Add(new ScriptBundle("~/bundles/scan").Include(
                "~/Scripts/site.scan.js"));

            bundles.Add(new ScriptBundle("~/bundles/scanresult").Include(
                "~/Scripts/site.scan.result.js"));

            bundles.Add(new ScriptBundle("~/bundles/about").Include(
                "~/Scripts/site.about.js"));

            bundles.Add(new ScriptBundle("~/bundles/signup").Include(
                "~/Scripts/site.signup.js"));

            bundles.Add(new ScriptBundle("~/bundles/deleteuser").Include(
                "~/Scripts/site.delete.user.js"));

            bundles.Add(new ScriptBundle("~/bundles/search").Include(
                "~/Scripts/site.search.js"));

            bundles.Add(new ScriptBundle("~/bundles/onedrivesearch").Include(
                "~/Scripts/site.onedrive.search.js"));

            bundles.Add(new ScriptBundle("~/bundles/searchresult").Include(
                "~/Scripts/site.search.result.js"));
        }

        public static void RegisterStyleBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/site.css",
                "~/Content/corner_ribbon.css",
                "~/Content/vicons-font.css"));

            bundles.Add(new StyleBundle("~/Content/fancybox").Include(
                "~/Content/FancyBox/jquery.fancybox.css"));

            bundles.Add(new StyleBundle("~/Content/custombutton").Include(
                "~/Content/custom_button.css"));

            bundles.Add(new StyleBundle("~/Content/upload").Include(
                "~/Content/site.upload.css",
                "~/Content/site.upload.button.css",
                "~/Content/site.upload.menu.css"));

            bundles.Add(new StyleBundle("~/Content/Control/uploading").Include(
                "~/Content/Uploading/uploading.css"));

            bundles.Add(new StyleBundle("~/Content/scan").Include(
                "~/Content/site.scan.css",
                "~/Content/site.scan.button.css"));

            bundles.Add(new StyleBundle("~/Content/scanresult").Include(
                "~/Content/site.scan.result.css",
                "~/Content/site.scan.result.table.css"));

            bundles.Add(new StyleBundle("~/Content/about").Include(
                "~/Content/site.about.css"));

            bundles.Add(new StyleBundle("~/Content/profile").Include(
                "~/Content/site.profile.css"));

            bundles.Add(new StyleBundle("~/Content/search").Include(
                "~/Content/site.search.css"));

            bundles.Add(new StyleBundle("~/Content/onedrivesearch").Include(
                "~/Content/site.onedrive.search.css"));

            bundles.Add(new StyleBundle("~/Content/searchresult").Include(
                "~/Content/site.search.result.css"));
        }
    }
}
