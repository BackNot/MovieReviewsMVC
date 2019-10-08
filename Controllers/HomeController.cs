using MovieReviews.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PagedList;
using PagedList.Mvc;
using MovieReviews.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading;
using System.Globalization;

namespace MovieReviews.Controllers
{
    public class HomeController : Controller
    {
        private UserManager<ApplicationUser> UserManager;
        private RoleManager<ApplicationRole> RoleManager;

        //initilizing culture on controller initialization
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (Session["CurrentCulture"] != null)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["CurrentCulture"].ToString());
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Session["CurrentCulture"].ToString());
            }
        }

        // changing culture
        public ActionResult ChangeCulture(string ddlCulture)
        {
            CultureInfo culture = new CultureInfo(ddlCulture);
            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture.Name);
            Session["CurrentCulture"] = ddlCulture;
            return Redirect(Request.UrlReferrer.AbsolutePath); // send him to the same view
        }
        public HomeController()// Create usermanager  here
        {
            UserStore<ApplicationUser> userStore =
                new UserStore<ApplicationUser>(new MovieReviewsDbContext());
            UserManager = new UserManager<ApplicationUser>(userStore);
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


        public ActionResult Index()
        {
            AuthenticateUser();
            return View();
        }
        public PartialViewResult AjaxMovies(string searchString, int? page) // Use this Ajax method for the search field
        {

            using (var context = new MovieReviewsDbContext())
            {

                var movies = from s in context.Movies // save only the matched movies
                             where s.MovieName.Contains(searchString)
                             select s;
                movies = movies.OrderBy(i => i.MovieId);
                ViewBag.searchString = searchString; // pass the search string to the view ( see bottom of the view )

                int pageSize = 3;
                int pageNumber = (page ?? 1); // if page is null then value is 1
                return PartialView(movies.ToPagedList(pageNumber, pageSize));
            }

        }
        public ViewResult Movies(string sortOrder, string searchString, int? page)
        {
            AuthenticateUser();

            ViewBag.CurrentSort = sortOrder; // set the sort

            MovieReviewsDbContext context = new MovieReviewsDbContext();
            var movies = from s in context.Movies
                         select s;
            if (searchString != null) // if this is not null we've been sent here from ajax method 
            {
                movies = movies.Where(i => i.MovieName.Contains(searchString));
                movies = movies.OrderBy(i => i.MovieId);

            }
            else
            { // if we are here then this method is called normally. We need to see if the user has any sorting options activated.
                switch (sortOrder)
                {
                    case "oldest":
                        movies = movies.OrderBy(s => s.MovieYear);
                        break;
                    case "newest":
                        movies = movies.OrderByDescending(s => s.MovieYear);
                        break;
                    case "best":
                        movies = movies.OrderByDescending(s => s.Stars);
                        break;
                    case "worst":
                        movies = movies.OrderBy(s => s.Stars);
                        break;
                    default:  // id
                        movies = movies.OrderBy(s => s.MovieId);
                        break;
                }
            }
            int pageSize = 3; // 3 records per page
            int pageNumber = (page ?? 1); // if page is null set it to 1
            return View(movies.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Contact()
        {
            AuthenticateUser();

            return View();
        }

        public ActionResult FAQ()
        {
            AuthenticateUser();

            return View();
        }

        public ActionResult Theme(string option)
        {
            if (option == "light") Session["theme"] = "light";
            else Session["theme"] = null;
            return Redirect(Request.UrlReferrer.AbsolutePath); // send him to the same view
        }
    }
}