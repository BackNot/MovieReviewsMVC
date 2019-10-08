using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;
using MovieReviews.App_Start;
using MovieReviews.Models;
using System.Web.Helpers;

namespace MovieReviews
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            // Force create DB 
            // MovieReviewsDbContext context = new MovieReviewsDbContext();

            AntiForgeryConfig.SuppressXFrameOptionsHeader = true;
        using (var context = new MovieReviewsDbContext())
             {
                 context.Database.Initialize(true);
              } 
         
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.AddHeader("X-Frame-Options", "SAMEORIGIN");
        }
    }
}
