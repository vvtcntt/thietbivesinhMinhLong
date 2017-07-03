using System.Web;
using System.Web.Optimization;

namespace TOTO
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
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

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Content/Display/Css/Style").Include(
                "~/Content/Display/Css/Data.css",
                "~/Content/Display/Css/Index.css",
                "~/Content/Display/Css/Index_Rs.css",
                "~/Content/Display/Css/jquery.mmenu.all.css",
                "~/Content/Display/Css/News.css",
                "~/Content/Display/Css/News_Rs.css",
                "~/Content/Display/Css/Order.css",
                "~/Content/themes/base/all.css",

                "~/Content/Display/Css/Order_res.css",
                "~/Content/Display/Css/prettyPhoto.css",
                "~/Content/Display/Css/Product.css",
                "~/Content/Display/Css/Product_Rs.css",
                "~/Content/Display/Css/slide.css",
                "~/Content/Display/Css/Baogia.css",
                "~/Content/Display/Css/Baogia_Rs.css",
                "~/Content/Display/Css/Maps.css",
                "~/Content/Display/Css/linhnguyen.css"

                ));
            BundleTable.EnableOptimizations = true;
        }
    }
}
