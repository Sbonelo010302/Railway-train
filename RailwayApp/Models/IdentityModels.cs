using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace RailwayApp.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        [Required]
        public int RailwayUserId { get; set; }
        [ForeignKey("RailwayUserId")]
        public RailwayUser RailwayUsers { get; set; }

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        }

        public System.Data.Entity.DbSet<RailwayApp.Models.RailwayUser> RailwayUsers { get; set; }

        public System.Data.Entity.DbSet<RailwayApp.Models.Customer> Customers { get; set; }

        public System.Data.Entity.DbSet<RailwayApp.Models.Employee> Employees { get; set; }

        public System.Data.Entity.DbSet<RailwayApp.Models.EmployeeType> EmployeeTypes { get; set; }

        public System.Data.Entity.DbSet<RailwayApp.Models.Route> Routes { get; set; }
        public System.Data.Entity.DbSet<RailwayApp.Models.DayToDayTrainRoute> DayToDayTrainRoutes { get; set; }

        public System.Data.Entity.DbSet<RailwayApp.Models.EmployeeRoster> EmployeeRosters { get; set; }

        public System.Data.Entity.DbSet<RailwayApp.Models.Payment> Payments { get; set; }

        public System.Data.Entity.DbSet<RailwayApp.Models.Reservation> Reservations { get; set; }

        public System.Data.Entity.DbSet<RailwayApp.Models.Train> Trains { get; set; }
    }
}