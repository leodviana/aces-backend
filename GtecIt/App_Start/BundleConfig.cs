using System.Web.Optimization;

namespace GtecIt
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/typeahead").Include(
                                          "~/Scripts/typeahead.jquery.min.js",
                                          "~/Scripts/bloodhound.min.js",
                                          "~/Scripts/typeahead.bundle.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryajax").Include(
                      "~/Scripts/jquery.unobtrusive-ajax.js"));

            
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/datetimepicker").Include(
                                          "~/Scripts/datetimepicker/moment.min.js",
                                          "~/Scripts/datetimepicker/bootstrap-datetimepicker.js",
                                          "~/Scripts/datetimepicker/datetimepicker.pt-BR.js"));


            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                                         "~/Scripts/jquery.dataTables.js",
                                         "~/Scripts/dataTables.bootstrap.js",
                                         "~/Scripts/dataTables.responsive.js"));

            bundles.Add(new ScriptBundle("~/bundles/mask").Include(
                             "~/Scripts/jquery.inputmask.min.js",
                             "~/Scripts/jquery.maskMoney.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/default").Include(
                              "~/Scripts/defaults.js"));
                                         
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/dataTables.bootstrap.css",
                      "~/Content/dataTables.responsive.css",
                      "~/Content/bootstrap-datetimepicker.min.css",
                      "~/Content/typeahead.css",
                      "~/Content/site.css"));


        }
    }
}
