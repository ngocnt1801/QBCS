using System.Web.Optimization;

namespace QBCS.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //jquery
            bundles.Add(new ScriptBundle("~/bundles/script/jquery").Include(
                "~/Scripts/vendor/jquery/jquery.js"
                ));

            //basic template
            bundles.Add(new StyleBundle("~/bundles/content/template").Include(
                "~/Content/sb-admin-2.css",
                "~/Content/font-awesome/all.css",
                "~/Content/switch-button-style.css",
                "~/Content/custom-css.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/script/template").Include(
                "~/Scripts/bootstrap.bundle.js",
                "~/Scripts/js/sb-admin-2.js"
                ));

            //data table
            bundles.Add(new StyleBundle("~/bundles/content/datatables").Include(
                "~/Content/datatables/dataTables.bootstrap4.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/script/datatables").Include(
                "~/Scripts/datatables/jquery.dataTables.js",
                "~/Scripts/datatables/dataTables.bootstrap4.js",
                "~/Scripts/datatables/dataTables-demo.js",
                "~/Scripts/myjs/init-datatable.js"
                ));

            //data table
            bundles.Add(new StyleBundle("~/bundle/content/importfile").Include(
                "~/Content/style_dashboard.css"
                ));

            bundles.Add(new ScriptBundle("~/bundle/scripts/importfile").Include(
                "~/Scripts/myjs/import-file.js"
                ));

            //process text
            bundles.Add(new ScriptBundle("~/bundle/scripts/processtext").Include(
                "~/Scripts/myjs/text-process.js"
                ));
            //tab import result
            bundles.Add(new ScriptBundle("~/bundle/scripts/tabresult").Include(
                "~/Scripts/myjs/change-partial-view.js"
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

            //Confirm box
            bundles.Add(new ScriptBundle("~/bundle/scripts/confirmbox").Include("~/Scripts/myjs/bootbox.min.js"));

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

            //checkbox
            bundles.Add(new ScriptBundle("~/bundle/scripts/checkbox").Include(
                "~/Scripts/myjs/checkbox/checkbox-process.js"
                ));

            //categories tree
            bundles.Add(new ScriptBundle("~/bundle/scripts/category").Include(
                "~/Scripts/myjs/colapse-category.js",
                "~/Scripts/myjs/colapse-category-exam.js"
                ));

            bundles.Add(new StyleBundle("~/bundle/content/category").Include(
                "~/Content/category-style.css"
                ));

            //error page
            bundles.Add(new StyleBundle("~/bundle/content/error").Include(
                "~/Content/error/font-awesome.min.css",
                "~/Content/error/style.css"
                ));
            //for checking diff

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

            //Autocomplete Lecturer
            bundles.Add(new ScriptBundle("~/bundle/scripts/autocompleteLecturer").Include(
                "~/Scripts/jquery-3.1.1.min.js",
                "~/Scripts/jquery-ui.min.js",
                "~/Scripts/myjs/autocompleteLecturer.js",
                "~/Scripts/myjs/autocompleteCourse.js"
                ));
            bundles.Add(new StyleBundle("~/bundle/content/autocompleteLecturer").Include(
               "~/Content/jquery-ui.css"
               ));

            //Tagsinput in rule
            bundles.Add(new ScriptBundle("~/bundle/scripts/tagsinputrule").Include(
                "~/Scripts/bootstrap-tagsinput.js",
                "~/Scripts/sweetalert.min.js",
                "~/Scripts/myjs/staffjs/edit-rule.js"
                ));
            bundles.Add(new StyleBundle("~/bundle/content/tagsinputrule").Include(
               "~/Content/bootstrap-tagsinput.css"
               ));

            //Image zoom onclick
            bundles.Add(new ScriptBundle("~/bundle/scripts/img-zoom").Include(
                "~/Scripts/myjs/img-zoom/img-zoom.js"

                ));
            bundles.Add(new StyleBundle("~/bundle/content/img").Include(
                       "~/Content/img/image-zoom.css"
                       ));

            //Track window
            bundles.Add(new ScriptBundle("~/bundle/scripts/tracking").Include(
                "~/Scripts/myjs/track-window.js",
                "~/Scripts/myjs/modal/manage-modal.js",
                 "~/Scripts/jquery.ba-hashchange.min.js"
                ));

            //AJAX FORM
            bundles.Add(new ScriptBundle("~/bundle/scripts/unobtrusive").Include(
                 "~/Scripts/jquery.unobtrusive-ajax.js"

                ));
            bundles.Add(new ScriptBundle("~/bundle/scripts/dragFile").Include(
                "~/Scripts/myjs/dragAndDrop/filedrag.js"
                ));

            //Different highlight
            bundles.Add(new ScriptBundle("~/bundle/scripts/diff").Include(
                //"~/Scripts/myjs/diff/diff_match_patch.js",
                //"~/Scripts/myjs/diff/jquery.pretty-text-diff.min.js",
                //"~/Scripts/myjs/diff/compare.js"
                "~/Scripts/myjs/diff/diff-text.js"
                ));
            bundles.Add(new StyleBundle("~/bundle/content/diff").Include(
                "~/Content/diff/jquery.picadiff.css"
                ));


            //Toggle question
            bundles.Add(new ScriptBundle("~/bundle/scripts/togglequestion").Include(
                "~/Scripts/myjs/toggle-question.js"
                ));

            //Edit question
            bundles.Add(new StyleBundle("~/bundle/content/detailquestion").Include(
                      "~/Content/style-detail-question.css"
                      ));

            //DataTable Activity
            bundles.Add(new ScriptBundle("~/bundle/scripts/tableActivity").Include(
                "~/Scripts/myjs/tableActivity.js"
                ));

            //DataTable Log
            bundles.Add(new ScriptBundle("~/bundle/scripts/tableLog").Include(
                "~/Scripts/myjs/tableLog.js"
                ));

            //CourseStatistic
            bundles.Add(new ScriptBundle("~/bundle/scripts/CourseStatistic").Include(
                "~/Scripts/myjs/CourseStatisticsjs.js"
                ));

            //manage syllabus
            bundles.Add(new ScriptBundle("~/bundle/scripts/syllabus").Include(
                "~/Scripts/myjs/syllabus-manage.js"
                ));

            //manage syllabus
            bundles.Add(new ScriptBundle("~/bundle/scripts/managecategory").Include(
                "~/Scripts/myjs/manage-category.js"
                ));
        }
    }
}
