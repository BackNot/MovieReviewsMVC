using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Cookies;

[assembly: OwinStartup(typeof(MovieReviews.App_Start.Startup))]

namespace MovieReviews.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ApplicationCookie",
                LoginPath = new PathString("/auth/login"),
                ExpireTimeSpan = TimeSpan.FromMinutes(30) // maximum lifespan => 30 minutes for the cookie.

            });
        }
    }
}
