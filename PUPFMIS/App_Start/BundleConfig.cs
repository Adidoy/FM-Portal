using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

namespace PUPFMIS
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            string ltecomponents = "~/Scripts/plugins/";
            string lte = "~/Scripts/dist/";

            bundles.Add(new StyleBundle("~/bundles/styles")
                .Include(ltecomponents + "tempusdominus-bootstrap-4/css/tempusdominus-bootstrap-4.min.css")
                .Include(ltecomponents + "icheck-bootstrap/icheck-bootstrap.min.css")
                .Include(ltecomponents + "jqvmap/jqvmap.min.css")
                .Include(lte + "css/adminlte.min.css")
                .Include(ltecomponents + "overlayScrollbars/css/OverlayScrollbars.min.css")
                .Include(ltecomponents + "daterangepicker/daterangepicker.css")
                .Include(ltecomponents + "summernote/summernote.min.css")
                .Include(ltecomponents + "select2/css/select2.css")
                .Include(ltecomponents + "datatables-bs4/css/dataTables.bootstrap4.min.css")
                .Include(ltecomponents + "datatables-responsive/css/responsive.bootstrap4.min.css")
                .Include(ltecomponents + "sweetalert2-theme-bootstrap-4/bootstrap-4.min.css")
            );

            bundles.Add(new ScriptBundle("~/bundles/preScripts")
                .Include(ltecomponents + "jquery/jquery.js")
                .Include(ltecomponents + "jquery-ui/jquery-ui.min.js")
                .Include(ltecomponents + "bootstrap/js/bootstrap.bundle.min.js")
                .Include("~/Scripts/jquery.validate.js")
                .Include("~/Scripts/jquery.validate.unobtrusive.js")
                .Include("~/Scripts/jquery.unobtrusive-ajax.js")
                .Include(lte + "js/adminlte.js")
                .Include(lte + "js/demo.js")
            );

            bundles.Add(new ScriptBundle("~/bundles/dashboardScripts")
                .Include(ltecomponents + "chart.js/Chart.min.js")
                .Include(ltecomponents + "sparklines/sparkline.js")
                .Include(ltecomponents + "jqvmap/jquery.vmap.min.js")
                .Include(ltecomponents + "jqvmap/maps/jquery.vmap.usa.js")
                .Include(ltecomponents + "jquery-knob/jquery.knob.min.js")
                .Include(ltecomponents + "moment/moment.min.js")
                .Include(ltecomponents + "daterangepicker/daterangepicker.js")
                .Include(ltecomponents + "tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js")
                .Include(ltecomponents + "summernote/summernote.min.js")
                .Include(ltecomponents + "overlayScrollbars/js/jquery.overlayScrollbars.min.js")
                .Include(lte + "js/adminlte.js")
                .Include(lte + "js/pages/dashboard.js")
                .Include(lte + "js/demo.js")
            );

            bundles.Add(new ScriptBundle("~/bundles/postScripts")
                .Include(ltecomponents + "moment/moment.min.js")
                .Include(ltecomponents + "daterangepicker/daterangepicker.js")
                .Include(ltecomponents + "tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js")
                .Include(ltecomponents + "summernote/summernote.min.js")
                .Include(ltecomponents + "overlayScrollbars/js/jquery.overlayScrollbars.min.js")
                .Include(ltecomponents + "select2/js/select2.js")
                .Include(ltecomponents + "bs-custom-file-input/bs-custom-file-input.min.js")
                .Include(ltecomponents + "datatables/jquery.dataTables.min.js")
                .Include(ltecomponents + "datatables-bs4/js/dataTables.bootstrap4.min.js")
                .Include(ltecomponents + "datatables-responsive/js/dataTables.responsive.min.js")
                .Include(ltecomponents + "datatables-responsive/js/responsive.bootstrap4.min.js")
                .Include(ltecomponents + "sweetalert2/sweetalert2.min.js")
            );

            BundleTable.EnableOptimizations = true;
        }
    }
}