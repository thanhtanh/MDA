using System.Web;
using System.Web.Optimization;

namespace MobileDatingCMS
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //CSS for  Ace admin + Fuel ux
            bundles.Add(new StyleBundle("~/Style/Common").Include(
                 "~/Content/css/bootstrap.min.css",
                 "~/Content/css/font-awesome.min.css",
                 "~/Content/css/ace.min.css",
                 "~/Content/css/ace-rtl.min.css",
                 "~/Content/css/ace-skins.min.css",
                 "~/Content/css/ace-fonts.css",
                 "~/Content/css/datepicker.css",
                 "~/Content/css/font-awesome.min.css",
                 "~/Content/admincustom.css",
                 "~/Content/css/fuelux.min.css"
            ));

            //Scipts for  Ace admin + Fuel ux
            bundles.Add(new ScriptBundle("~/Scripts/Common").Include(
                "~/Content/js/jquery-2.0.3.min.js",
                "~/Content/js/jquery.validate.min.js",
                "~/Content/js/jquery.dataTables.min.js",
                "~/Content/js/jquery.dataTables.bootstrap.js",
                "~/Content/js/bootstrap.min.js",
                "~/Content/js/jquery-ui-1.10.3.full.min.js",
                "~/Content/js/date-time/bootstrap-datepicker.min.js",
                "~/Content/assets/js/ace-elements.min.js",
                "~/Content/js/ace.min.js",
                "~/Content/js/ace-extra.min.js",
                "~/Content/vpn/adminvpn.js",
                "~/Content/js/fuelux.min.js"
            ));
        }
    }
}
