﻿using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

namespace PUPFMIS
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            string ltecomponents = "~/Scripts/adminlte/components/";
            string lte = "~/Content/adminlte/";

            
            bundles.Add(new StyleBundle("~/bundles/fonts")
                .Include(ltecomponents + "ionicons/css/ionicons.min.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/styles")
                .Include(ltecomponents + "bootstrap/dist/css/bootstrap.min.css")
                .Include(ltecomponents + "tempusdominus-bootstrap-4/css/tempusdominus-bootstrap-4.min.css")
                .Include(ltecomponents + "icheck-bootstrap/icheck-bootstrap.min.css")
                .Include(ltecomponents + "jqvmap/jqvmap.min.css")
                .Include(lte + "css/adminlte.min.css")
                .Include(ltecomponents + "overlayScrollbars/css/OverlayScrollbars.min.css")
                .Include(ltecomponents + "daterangepicker/daterangepicker.css")
                .Include(ltecomponents + "summernote/summernote-bs4.css")
                .Include(ltecomponents + "datatables-bs4/css/dataTables.bootstrap4.css")
                .Include(ltecomponents + "bootstrap-datepicker/dist/css/bootstrap-datepicker.min.css")
                .Include(ltecomponents + "sweetalert2/sweetalert2.min.css")
                .Include(ltecomponents + "toastr/toastr.min.css")
                );

            bundles.Add(new ScriptBundle("~/bundles/jquery")
                .Include(ltecomponents + "jquery/dist/jquery.js")
                .Include(ltecomponents + "jquery-ui/jquery-ui.min.js")
                .Include("~/Scripts/jquery.validate.min.js")
                .Include("~/Scripts/jquery.unobtrusive-ajax.min.js")
                .Include(ltecomponents + "jquery-knob/jquery.knob.min.js")
                .Include(ltecomponents + "datatables/jquery.dataTables.js")
                .Include(ltecomponents + "jqvmap/jquery.vmap.min.js")
                .Include(ltecomponents + "jqvmap/maps/jquery.vmap.usa.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/scripts")
                .Include(ltecomponents + "sweetalert2/sweetalert2.all.min.js")
                .Include(ltecomponents + "toastr/toastr.min.js")
                .Include(ltecomponents + "bootstrap/dist/js/bootstrap.bundle.min.js")
                .Include("~/Scripts/adminlte/js/demo.js")
                .Include(ltecomponents + "chart.js/Chart.min.js")
                .Include(ltecomponents + "sparklines/sparkline.js")
                .Include(ltecomponents + "moment/moment.min.js")
                .Include(ltecomponents + "daterangepicker/daterangepicker.js")
                .Include(ltecomponents + "tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js")
                .Include(ltecomponents + "summernote/summernote-bs4.min.js")
                .Include(ltecomponents + "overlayScrollbars/js/jquery.overlayScrollbars.min.js")
                .Include("~/Scripts/adminlte/js/adminlte.js")
                .Include("~/Scripts/adminlte/js/pages/dashboard.js")
                .Include(ltecomponents + "datatables-bs4/js/dataTables.bootstrap4.js"));

            BundleTable.EnableOptimizations = true;
        }
    }
}