using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MovieReviews.Identity
{
    public class IdentityContext : IdentityDbContext<ApplicationUser>
    {
        public IdentityContext()
            : base("name=MovieReviewsDb")
        {
            Database.SetInitializer(new IdentityInitializer());
        }
    }

    public class IdentityInitializer : NullDatabaseInitializer<IdentityContext>
    {
        protected override void Seed(IdentityContext context)
        {
            if (!context.Roles.Any(i => i.Name == "Administrator")) // If there is no such role create it
            {
                var store = new RoleStore<ApplicationRole>(context);
                var manager = new RoleManager<ApplicationRole>(store);
                var role = new ApplicationRole() { Name = "Administrator" };
                manager.Create(role);
            }
            if (!context.Users.Any(i => i.UserName == "root")) //Since this is test environment, create root user with pw root 
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser() { UserName = "root", PasswordHash = "root" };
                manager.Create(user, "123123");
                manager.AddToRole(user.Id, "Administrator");
            }
            base.Seed(context);     
        }
    }
}