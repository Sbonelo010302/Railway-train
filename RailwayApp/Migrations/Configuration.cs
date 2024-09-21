namespace RailwayApp.Migrations
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using RailwayApp.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<RailwayApp.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(RailwayApp.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            var idManager = new IdentityManager(context);
            if (!idManager.RoleExists("Admin"))
                idManager.CreateRole("Admin");
            if (!idManager.RoleExists("Customer"))
                idManager.CreateRole("Customer");
            if (!idManager.RoleExists("Conductor"))
                idManager.CreateRole("Conductor");
            if (!idManager.RoleExists("Driver"))
                idManager.CreateRole("Driver");

            var userAdmin = new ApplicationUser()
            {
                UserName = "Admin",
                Email = "Iqsaanmia@gmail.com",
                EmailConfirmed = true,
                RailwayUsers = new RailwayUser()
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    UserName = "Admin",
                    EmailAddress = "Iqsaanmia@gmail.com"
                }
            };

            if (!idManager.UserExists(userAdmin.UserName))
            {
                idManager.CreateUser(userAdmin, "password");
                idManager.AddUserToRole(userAdmin.Id, "Admin");
            }

            var userDriver = new ApplicationUser()
            {
                UserName = "Driver01",
                Email = "Iqsaanmia@gmail.com",
                EmailConfirmed = true,
                RailwayUsers = new RailwayUser()
                {
                    FirstName = "Driver01",
                    LastName = "Driver01",
                    UserName = "Driver01",
                    EmailAddress = "Iqsaanmia@gmail.com"
                }
            };

            if (!idManager.UserExists(userDriver.UserName))
            {
                idManager.CreateUser(userDriver, "password");
                idManager.AddUserToRole(userDriver.Id, "Driver");
            }
        }
    }
    public class IdentityManager
    {
        private ApplicationDbContext _context;
        public UserManager<ApplicationUser> UserManager { get; set; }

        public IdentityManager(ApplicationDbContext context)
        {
            _context = context;
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
        }
        public bool RoleExists(string name)
        {
            var rm = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(_context));
            return rm.RoleExists(name);
        }
        public bool CreateRole(string name)
        {
            try
            {
                var rm = new RoleManager<IdentityRole>(
                    new RoleStore<IdentityRole>(_context));
                var idResult = rm.Create(new IdentityRole(name));

                return idResult.Succeeded;
            }
            catch (Exception x)
            {
                throw x;
            }
        }
        public bool UserExists(string name)
        {
            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));

            return um.FindByName(name) != null;
        }
        public bool CreateUser(ApplicationUser user, string password)
        {
            var idResult = UserManager.Create(user, password);
            //var idResult = UserManager.Create(user);

            return idResult.Succeeded;
        }

        public bool AddUserToRole(string userId, string roleName)
        {
            var idResult = UserManager.AddToRole(userId, roleName);

            return idResult.Succeeded;
        }
    }
}
