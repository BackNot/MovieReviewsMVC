using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using MovieReviews.Identity;
using MovieReviews.Models;
using MovieReviews.Models.VM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MovieReviews.Controllers
{ // TODO : Implement Identity
    public class AuthController : Controller
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
        public AuthController()// Create usermanager and rolemanager here
        {
            UserStore<ApplicationUser> userStore =
                new UserStore<ApplicationUser>(new MovieReviewsDbContext());
            UserManager = new UserManager<ApplicationUser>(userStore);

            var roleStore = new RoleStore<ApplicationRole>(new MovieReviewsDbContext());
            RoleManager = new RoleManager<ApplicationRole>(roleStore);
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
                    try
                    {
                        if (UserManager.IsInRole(id, "Administrator")) ViewBag.IsAdmin = true;

                    }
                    catch (Exception)
                    {

                    }

                }
                return true;
            }
            return false;
        }
        public ActionResult Register()
        {
            AuthenticateUser();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            AuthenticateUser();

            if (ModelState.IsValid) // If ViewModel is OK try to register the user
            {
                

                // Map the properties to User object
                var user = new ApplicationUser();
                user.UserName = model.UserName;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;

                IdentityResult result = UserManager.Create(user, model.Password);
                if (!result.Succeeded)
                { // Add  error if something fails
                    //  ModelState.AddModelError("usercreateerror", "User could not be created");
                }
                else
                {
                    UserManager.AddToRole(user.Id, "Normal");
                    return RedirectToAction("Login");
                }
            }
            return View(model);
        }
        public ActionResult Login()
        {
            if (AuthenticateUser()) // If user is logged him redirect him go the index so he don't log in twice.
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LogInModel model, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.Find(model.UserName, model.Password);
                if (user != null) // if user exist log him in
                {
                    await SignIn(model.UserName, model.Password, model.RememberMe); // SignIn method (see below)
                    if (!String.IsNullOrEmpty(ReturnUrl))
                    {
                        return Redirect(ReturnUrl); // if there is return url redirect him back 
                    }
                    return RedirectToAction("Index", "Home"); // if there is no return url go to home page
                }
            }
            else
            {
                ModelState.AddModelError("LoginUserError", "Login failed"); // If something go wrong show an error
            }
            return View();

        }

        public ActionResult Logout()
        {
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();
            return RedirectToAction("Index", "Home");
        }
        [Authorize(Roles = "Normal,Administrator")]
        public ActionResult MyProfile()
        {
            if (!AuthenticateUser()) RedirectToAction("Index", "Home"); // if user is not logged in redirect to index
            var matchedId = User.Identity.GetUserId();
            using (MovieReviewsDbContext context = new MovieReviewsDbContext())
            {
                var user = context.Users.Where(obj => obj.Id == matchedId).SingleOrDefault();
                return View("Profile", user);
            }
        }
        public async Task SignIn(string name, string password, bool remember)
        {
            var user = UserManager.Find(name, password);

            var authManager = HttpContext.GetOwinContext().Authentication;
            var identityclaims = await UserManager.CreateIdentityAsync(user, "ApplicationCookie"); // set up the cookie
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = remember // be or don't be remembered
            };
            authManager.SignIn(authProperties, identityclaims); // sign him


        }

        public ActionResult Change()
        {
            AuthenticateUser();
            return View();
        }
        [HttpPost]
        public ActionResult Change(ChangePasswordModel model)
        {

            if (ModelState.IsValid)
            {
                IdentityResult canChangePw = UserManager.ChangePassword(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                if (canChangePw == IdentityResult.Success)
                {
                    return View("Index");
                }
                else
                    ModelState.AddModelError("", "Old password is wrong");
            }
            else
            {
                ModelState.AddModelError("", "Change failed. Please try again");
            }
            return View(model);
        }
        // login without registration
        public async Task<ActionResult> GuestLogin()
        {
            var user = await UserManager.FindByNameAsync("Guest");
            if (user != null)
            {
                await SignIn("guest", "guest1337", true);
                ViewBag.IsGuest = true;
            }
            return RedirectToAction("Index","Home");
        }
        // Check if Old password field is valid
        public ActionResult IsPasswordValid(string oldPassword)
        {
            var user = UserManager.FindById(User.Identity.GetUserId()); // get the current user
            if (UserManager.Find(user.UserName, oldPassword) == null) return Json("Old password is wrong", JsonRequestBehavior.AllowGet);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        // Check if username is allowed
        public ActionResult IsUserNameValid(string username)
        {
            if (UserManager.FindByName(username) != null)
                return Json("Username is already taken", JsonRequestBehavior.AllowGet);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        // Check if email is valid
        public ActionResult IsEmailValid(string email)
        {
            if (UserManager.FindByEmail(email) != null)
                return Json("Email is already used", JsonRequestBehavior.AllowGet);
            return Json(true, JsonRequestBehavior.AllowGet);
        }


    }
}