using System.Web;
using System.Web.Optimization;

namespace QBCS.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //basic template
            bundles.Add(new StyleBundle("~/bundles/content/template").Include(
                "~/Content/sb-admin-2.css",
                "~/Content/font-awesome/all.css",
                "~/Content/switch-button-style.css",
                "~/Content/custom-css.css"
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
                "~/Scripts/myjs/change-partial-view.js",
                "~/Scripts/myjs/text-process.js"
                ));
            //smartwizard
            bundles.Add(new ScriptBundle("~/bundle/scripts/smartwizard").Include(
                "~/Scripts/myjs/jquery.smartWizard.min.js",
                "~/Scripts/myjs/generate-question.js"
                ));

            bundles.Add(new StyleBundle("~/bundle/content/smartwizard").Include(
                "~/Content/smart_wizard_theme_arrows.min.css"
                ));
            //Checkbox 
            bundles.Add(new StyleBundle("~/bundle/content/checkbox").Include(
                "~/Content/checkbox.min.css",
                "~/Content/generate-exam.css"
                ));

            //signalR
            bundles.Add(new ScriptBundle("~/bundle/scripts/signalr").Include(
                "~/Scripts/jquery.signalR-2.4.1.js"
                ));

            bundles.Add(new ScriptBundle("~/bundle/scripts/notification").Include(
                "~/Scripts/myjs/notification.js"
                ));

            //Toastr 
            bundles.Add(new ScriptBundle("~/bundle/scripts/toastr").Include(
                "~/Scripts/toastr.js"
                ));

            bundles.Add(new StyleBundle("~/bundle/content/toastr").Include(
                "~/Content/toastr.css"
                ));

            //tab
            bundles.Add(new ScriptBundle("~/bundle/scripts/tab").Include(
                "~/Scripts/tab/cbpFWTabs.js",
                "~/Scripts/tab/modernizr.custom.js"
                ));

            bundles.Add(new StyleBundle("~/bundle/content/tab").Include(
                "~/Content/tab/demo.css",
                "~/Content/tab/normalize.css",
                "~/Content/tab/tabs.css",
                "~/Content/tab/tabstyles.css"
                ));

            //categories tree
            bundles.Add(new ScriptBundle("~/bundle/scripts/category").Include(
                "~/Scripts/myjs/colapse-category.js"
                ));

            bundles.Add(new StyleBundle("~/bundle/content/category").Include(
                "~/Content/category-style.css"
                ));

            //error page
            bundles.Add(new StyleBundle("~/bundle/content/error").Include(
                "~/Content/error/font-awesome.min.css",
                "~/Content/error/style.css"
                ));

            //Spinner
            bundles.Add(new ScriptBundle("~/bundle/scripts/spinner").Include(
                "~/Scripts/myjs/spinner.js"
                ));
            bundles.Add(new StyleBundle("~/bundle/content/spinner").Include(
               "~/Content/spinner/spinner.css"
               ));

            //Edit question
            bundles.Add(new ScriptBundle("~/bundle/scripts/editquestion").Include(
                "~/Scripts/myjs/edit-question.js"
                ));
        }
    }
}
