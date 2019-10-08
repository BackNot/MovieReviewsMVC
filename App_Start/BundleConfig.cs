using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace MovieReviews.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include( // jQuery and Ajax
                       "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryValidation").Include( // jQuery validation
                       "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/jquery.validate.unobtrusive.min.js")); 

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include( // bootstrap js
                           "~/Scripts/umd/popper.js",
                         "~/Scripts/bootstrap.js"
                         ));

            bundles.Add(new StyleBundle("~/Content/css").Include( // bootstrap css , pagedlist and our custom stylesheet
                   "~/Content/bootstrap.css",
                   "~/Content/PagedList.css",
                   "~/Content/mainStyle.css"));
        }
    }
}