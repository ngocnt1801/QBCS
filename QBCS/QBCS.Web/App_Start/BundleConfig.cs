﻿using System.Web;
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
                "~/Scripts/datatables/dataTables-demo.js",
                "~/Scripts/myjs/init-datatable.js"
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
                "~/Scripts/myjs/modal/manage-modal.js"
                ));

            //AJAX FORM
            bundles.Add(new ScriptBundle("~/bundle/scripts/unobtrusive").Include(
                 "~/Scripts/jquery.unobtrusive-ajax.js",
                 "~/Scripts/jquery.ba-hashchange.min.js"
                ));
            bundles.Add(new ScriptBundle("~/bundle/scripts/dragFile").Include(
                "~/Scripts/myjs/dragAndDrop/filedrag.js"
                ));
            
        }
    }
}
