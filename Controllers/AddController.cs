using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MovieReviews.Identity;
using MovieReviews.Models;
using MovieReviews.Models.VM;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MovieReviews.Controllers
{
    public class AddController : Controller
    {
        private UserManager<ApplicationUser> UserManager;
        private RoleManager<ApplicationRole> RoleManager;
        [Authorize]
        public ActionResult Index(string movie)
        {
            //UserReview review = new UserReview();
            //Movie movieInDb;
            AuthenticateUser();
            AddReviewModel userReviewVM = new AddReviewModel();
            using (var context = new MovieReviewsDbContext())
            {
                // movieInDb = context.Movies.Where(mv => mv.MovieName == movie).SingleOrDefault();
               
                userReviewVM.MovieName = movie;

            }
            if (userReviewVM.MovieName != null) return View(userReviewVM);
            return Content("<script>alert('Movie not found');</script>");
        }
        [Authorize]
        [HttpPost]
        public ActionResult Index([Bind(Exclude = "Id")] AddReviewModel userReviewVM, string movie)
        {
            AuthenticateUser();
            if (ModelState.IsValid)
            {
                UserReview userReview = new UserReview();
                using (var context = new MovieReviewsDbContext())
                {
                    try
                    {
                        var currentUser = User.Identity.GetUserId();
                        var movieInDb = context.Movies.Where(o => o.MovieName == userReviewVM.MovieName).SingleOrDefault();
                        ApplicationUser user = context.Users.Where(usr => usr.Id == currentUser).SingleOrDefault();
                        userReview.User = user;
                        userReview.Movie = movieInDb;
                        userReview.Review = userReviewVM.Review;
                        ModelState.AddModelError("MovieName", "Wrong movie name");
                        if (userReview.Movie == null) return View(userReview);
                        context.UserReviews.Add(userReview);
                        movieInDb.UserReviews.Add(userReview);
                        context.SaveChanges();


                    }
                    catch (DbEntityValidationException dbEx)
                    {
                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                Trace.TraceInformation("Property: {0} Error: {1}",
                                                        validationError.PropertyName,
                                                        validationError.ErrorMessage);
                            }
                        }
                    }
                    // return Content("<script>alert('Done!')</script>");
                    return RedirectToAction("Movies", "Home");

                }
            }
            else
            {
                if (movie != null) ViewBag.movieName = movie;
                return View(userReviewVM);
            }
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