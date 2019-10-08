using MovieReviews.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MovieReviews.Identity;
using System.Globalization;
using System.Threading;

namespace MovieReviews.Controllers
{
    public class ReviewController : Controller
    {
        private UserManager<ApplicationUser> UserManager;
        private RoleManager<ApplicationRole> RoleManager;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (Session["CurrentCulture"] != null)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["CurrentCulture"].ToString());
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Session["CurrentCulture"].ToString());
            }
        }
        public ActionResult Index(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Home");
            AuthenticateUser();
            Movie movieInDb = null;
            using (var context = new MovieReviewsDbContext())
            {
                 movieInDb = context.Movies.Where(obj => obj.MovieId == id).Include((obj => obj.UserReviews.Select(o => o.User))).SingleOrDefault();
            }
            if (movieInDb != null) return View(movieInDb);
            return Content("<script>alert('Movie not found') </script>");
        }

        [NonAction]
        public bool AuthenticateUser() // This is a method that checks if user is logged in. It will return TRUE and set a ViewBag if user is logged in.
        {                              // then it will see if user is administrator and set a Viewbag.IsAdmin to true if he is.
            var id = User.Identity.GetUserId();
            if (id != null)
            {                           // If user is registered show him 'profile' and 'logout' options 
                ViewBag.IsRegistered = true;
                using (var context = new MovieReviewsDbContext())
                {
                    if (UserManager == null)
                    {
                        UserStore<ApplicationUser> userStore =
          new UserStore<ApplicationUser>(new MovieReviewsDbContext());
                        UserManager = new UserManager<ApplicationUser>(userStore);
                    }
                    if (UserManager.IsInRole(id, "Administrator")) ViewBag.IsAdmin = true;
                    if (UserManager.IsInRole(id, "Guest")) ViewBag.IsGuest = true;
                }
                return true; // if user is registered
            }
            return false; // if user is not registered
        }
    }
}