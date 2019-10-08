using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MovieReviews.Identity;
using MovieReviews.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace MovieReviews.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private UserManager<ApplicationUser> UserManager;

        public AdminController()
        {
            UserStore<ApplicationUser> userStore =
                   new UserStore<ApplicationUser>(new MovieReviewsDbContext());
            var UserManager = new UserManager<ApplicationUser>(userStore);
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
                    if(UserManager==null)
                    {
                        UserStore<ApplicationUser> userStore =
                              new UserStore<ApplicationUser>(new MovieReviewsDbContext());
                         UserManager = new UserManager<ApplicationUser>(userStore);
                    }
                        if (UserManager.IsInRole(id, "Administrator")) ViewBag.IsAdmin = true;
    
                }
                return true;
            }
            return false;
        }

        public ActionResult Index()
        {
            AuthenticateUser(); // call the method above
            return View();
        }
        public ActionResult ShowMovies()
        {
            AuthenticateUser();
            using (var context = new MovieReviewsDbContext())
            {
                var movies = (from movie in context.Movies
                              select movie).ToList();
                return View(movies);

            }
        }

        public ActionResult EditMovies(int id)
        {
            AuthenticateUser();
            using (var context = new MovieReviewsDbContext())
            {
                // if movie is not found go to index otherwise proceed
                var getMovie = context.Movies.Where(i => i.MovieId == id).SingleOrDefault();
                if (getMovie == null) return RedirectToAction("Index");
                return View(getMovie);


            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMovies([Bind(Include = "MovieId,MovieName,MovieYear,PathToImage,Description,Category,Stars,Review,RowVersion")] Movie movie, byte[] rowVersion)
        {

            using (var context = new MovieReviewsDbContext())
            {
                if (ModelState.IsValid)
                {
                    try // Check for optimistic concurrency
                    {
                        context.Entry(movie).State = EntityState.Modified;
                        context.SaveChanges();
                        return RedirectToAction("Index");

                    }
                    catch (DbUpdateConcurrencyException ex) // If there is concurrency
                    {
                        var entry = ex.Entries.Single(); // get the entry
                        var clientValues = (Movie)entry.Entity;
                        var databaseEntry = entry.GetDatabaseValues();

                        if (databaseEntry == null)
                        { // deleted
                            ModelState.AddModelError("", "Record has been deleted from another user");
                        }
                        else
                        { // values are changed by other user
                            var databaseValues = (Movie)databaseEntry.ToObject();

                            ModelState.AddModelError("", "Record has been update from another user. Please check the values in the list. Otherwise click 'Save' again and re-write the changes. ");
                            movie.RowVersion = databaseValues.RowVersion; // set the row version
                        }
                        return View(movie);
                    }
                }
                ModelState.AddModelError("", "Record is not valid.");
                return View(movie);
            }
        }
        public ActionResult AddMovie()
        {
            Movie movie = new Movie();
            return View(movie);
        }

        [HttpPost]
        public ActionResult AddMovie(Movie movie)
        {
            if (ModelState.IsValid)
            {
                using (var context = new MovieReviewsDbContext())
                {
                    context.Movies.Add(movie);
                    context.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(movie);
        }
        public ActionResult ShowUsers()
        {
            AuthenticateUser();
            using (var context = new MovieReviewsDbContext())
            {
                context.Configuration.LazyLoadingEnabled = false; // turn off lazy load and eager load the roles 

                var movies = (from movie in context.Users.Include(i => i.Roles)
                              select movie).ToList();
                return View(movies);

            }
        }


        [HttpPost]
        public ActionResult DeleteUser(string id)
        {
            using (var context = new MovieReviewsDbContext())
            { // find and delete user
                var user = (from usr in context.Users
                            where usr.Id == id
                            select usr).SingleOrDefault();


                UserManager.Delete(user); // done

            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult AddAdmin(string id)
        {
            using (var context = new MovieReviewsDbContext())
            { // find and attach role
                var user = (from usr in context.Users
                            where usr.Id == id
                            select usr).SingleOrDefault();

                UserStore<ApplicationUser> userStore =
                    new UserStore<ApplicationUser>(new MovieReviewsDbContext());
                var UserManager = new UserManager<ApplicationUser>(userStore);
                UserManager.AddToRole(id.ToString(), "Administrator");
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult RemoveAdmin(string id)
        {
            using (var context = new MovieReviewsDbContext())
            { // find and remove role
                var user = (from usr in context.Users
                            where usr.Id == id
                            select usr).SingleOrDefault();

                UserStore<ApplicationUser> userStore =
                    new UserStore<ApplicationUser>(new MovieReviewsDbContext());
                var UserManager = new UserManager<ApplicationUser>(userStore);
                UserManager.RemoveFromRole(id.ToString(), "Administrator");
            }
            return RedirectToAction("Index");
        }

        // these are Ajax methods for quick search
        public ActionResult AjaxMovies(string searchString)
        {
            using (var context = new MovieReviewsDbContext())
            {
                var resultMovie = context.Movies.Where(movie => movie.MovieId.ToString() == searchString).SingleOrDefault();
                if (resultMovie != null) return PartialView(resultMovie);
                else return JavaScript("<script>alert('Not found')</script>");
            }
        }

        public ActionResult AjaxUsers(string searchString)
        {
            using (var context = new MovieReviewsDbContext())
            {
                var user = context.Users.Where(usr => usr.UserName == searchString).Include(usr => usr.Roles).SingleOrDefault();
                if (user != null) return PartialView(user);
                else return JavaScript("<script>alert('Not found')</script>");
            }
        }
    }
}