using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MovieReviews.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MovieReviews.Models
{
   public class MovieReviewsDbContext : IdentityDbContext<ApplicationUser> // Use 1 context for identity models and EF Models
    { 
        public MovieReviewsDbContext()
            : base("name=alexashk_mr")
        {
            Database.SetInitializer<MovieReviewsDbContext>(new CustomInitializer()); // Set our custom initializer (see below)
        }

        public DbSet<Movie> Movies { get; set; } 
        public DbSet<UserReview> UserReviews { get; set; }
    }

    public class CustomInitializer : CreateDatabaseIfNotExists<MovieReviewsDbContext>
    {
          protected override void Seed(MovieReviewsDbContext context) // Let's seed the DB with default records
          {

              // Seed Movies
              IList<Movie> defaultMovies = new List<Movie>();
            
              defaultMovies.Add(new Movie()
              {
                  MovieName = "The Matrix",
                  MovieYear = 1999,
                  Category = "Sci-Fi",
                  PathToImage = "/Content/Images/matrix.jpg",
                  Description = " It depicts a dystopian future in which reality as perceived by most humans is actually a simulated reality called 'the Matrix', created by sentient machines to subdue the human population, while their bodies' heat and electrical activity are used as an energy source",
                  Stars = 10,
                  Review = "Without a doubt one of the best and most influential movies of all time, the Matrix is the defining science fiction film of the 1990's and the biggest leap the genre has taken since Stanley Kubrick's 2001: a Space Odyssey and Ridley Scott's Blade Runner"
              });
              defaultMovies.Add(new Movie()
              {
                  MovieName = "Fight Club",
                  MovieYear = 1999,
                  Category = "Drama",
                  PathToImage = "/Content/Images/fight_club.jpg",
                  Description = "An insomniac office worker and a devil-may-care soapmaker form an underground fight club that evolves into something much, much more.",
                  Stars = 9.5,
                  Review = "The script was tight, the theme fascinating, the acting incredible (especially Edward Norton, as one might expect), the direction inspired, and the cinematography stunning. It is one of the few films of the past five years that deserves to be seen multiple times. In fact, if you have seen it only once, you have missed something"
              });
              defaultMovies.Add(new Movie()
              {
                  MovieName = "Back to the Future",
                  MovieYear = 1985,
                  Category = "Sci-Fi",
                  PathToImage = "/Content/Images/back_future.jpg",
                  Description = "Marty McFly, a 17-year-old high school student, is accidentally sent thirty years into the past in a time-traveling DeLorean invented by his close friend, the maverick scientist Doc Brown.",
                  Stars = 8.5,
                  Review = "Back To The Future is such an inventive and exciting piece of filmmaking that it is impossible to forget about it. The casting of every character involved was absolutely perfect, and the performances were spectacular"
              });
              defaultMovies.Add(new Movie()
              {
                  MovieName = "Star Wars: Episode IV - A New Hope ",
                  MovieYear = 1977,
                  Category = "Fantasy",
                  PathToImage = "/Content/Images/star_wars.jpg",
                  Description = "Luke Skywalker joins forces with a Jedi Knight, a cocky pilot, a Wookiee and two droids to save the galaxy from the Empire's world-destroying battle station, while also attempting to rescue Princess Leia from the evil Darth Vader.",
                  Stars = 9,
                  Review = "Filmmakers have tried for decades, but they can't make a better space saga. It's the perfect stand-alone science fiction film that's only rivaled by its darker sequel."
              });
            
              context.Movies.AddRange(defaultMovies);
            
              // Seed Roles and Users
              if (!context.Roles.Any(i => i.Name == "Administrator")) // If there is no such role create it
              {
                  var store = new RoleStore<ApplicationRole>(context);
                  var manager = new RoleManager<ApplicationRole>(store);
                  var role = new ApplicationRole() { Name = "Administrator"};
                  manager.Create(role);
              }
            if (!context.Roles.Any(i => i.Name == "Normal")) // If there is no such role create it
            {
                var store = new RoleStore<ApplicationRole>(context);
                var manager = new RoleManager<ApplicationRole>(store);
                var role = new ApplicationRole() { Name = "Normal" };
                manager.Create(role);
            }
            if (!context.Roles.Any(i => i.Name == "Guest")) // If there is no such role create it
            {
                var store = new RoleStore<ApplicationRole>(context);
                var manager = new RoleManager<ApplicationRole>(store);
                var role = new ApplicationRole() { Name = "Guest" };
                manager.Create(role);
            }

            if (!context.Users.Any(i => i.UserName == "root")) //Since this is test environment, create root user with pw root 
              {
                  var store = new UserStore<ApplicationUser>(context);
                  var manager = new UserManager<ApplicationUser>(store);
                  var user = new ApplicationUser() { UserName = "root" , Email="root@root.com" , FirstName="root" , LastName="root"};
                   manager.Create(user, "root123");
                
                  manager.AddToRole(user.Id, "Administrator");
              }
            if (!context.Users.Any(i => i.UserName == "guest")) // guest user
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser() { UserName = "Guest", Email = "guest@guest.com", FirstName = "guest", LastName = "guest" };
                manager.Create(user, "guest1337");
                manager.AddToRole(user.Id, "Guest");
            }


            base.Seed(context);
          }
    } 
    }