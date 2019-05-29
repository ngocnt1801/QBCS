using System.Web;
using System.Web.Optimization;

namespace QBCS.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/vendor/jquery-{version}.js",
            //            "~/Scripts/vendor/jquery-easing/jquery.easing.min.js",
            //            "~/Scripts/vendor/jquery/jquery.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/vendor/bootstrap/js/bootstrap.js",
            //          "~/Scripts/vendor/bootstrap/js/bootstrap.bundle.min.js"));

            //bundles.Add(new StyleBundle("~/bundles/content/css").Include(
            //          "~/Content/sb-admin-2.min.css",
            //          "~/Scripts/vendor/fontawesome-free/css/all.min.css",
            //          "~/Content/switch-button-style.css",
            //          "~/Content/dataTables.bootstrap4.css",
            //          "~/Scripts/vendor/bootstrap/scss/bootstrap.scss"));
            //bundles.Add(new ScriptBundle("~/bundles/js/datatable").Include(
            //          "~/Scripts/js/demo/datatables-demo.js",
            //          "~/Scripts/js/demo/sb-admin-2.js",
            //          "~/Scripts/vendor/datatables/dataTables.bootstrap4.js",
            //          "~/Scripts/vendor/datatables/jquery.dataTables.js"));

            //basic template
            bundles.Add(new StyleBundle("~/bundles/content/template").Include(
                "~/Content/sb-admin-2.css",
                "~/Content/font-awesome/all.css",
                "~/Content/switch-button-style.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/script/template").Include(
                "~/Scripts/bootstrap.bundle.js",
                "~/Scripts/js/sb-admin-2.js",
                "~/Scripts/vendor/jquery/jquery.js"           
                ));

            //data table
            bundles.Add(new StyleBundle("~/bundles/content/datatables").Include(
                "~/Content/datatables/dataTables.bootstrap4.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/script/datatables").Include(
                "~/Scripts/datatables/jquery.dataTables.js",
                "~/Scripts/datatables/dataTables.bootstrap4.js",
                "~/Scripts/datatables/dataTables-demo.js"
                ));

            //data table
            bundles.Add(new StyleBundle("~/bundle/content/importfile").Include(
                "~/Content/style_dashboard.css"
                ));

            bundles.Add(new ScriptBundle("~/bundle/scripts/importfile").Include(
                "~/Scripts/myjs/import-file.js",
                "~/Scripts/myjs/change-partial-view.js"
                ));
        }
    }
}
