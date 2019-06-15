using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using storingVedios2.Models;

[assembly: OwinStartupAttribute(typeof(storingVedios2.Startup))]
namespace storingVedios2
{
    public partial class Startup
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRoles();
            CreateUsers();
        }
        public void CreateUsers()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            ApplicationUser user = new ApplicationUser();
            user.Email = "ayaad@yahoo.com";
            user.UserName = "ayaad@yahoo.com";
           
            var check = userManager.Create(user, "306445520iD++");
            if (check.Succeeded)
            {
                userManager.AddToRoles(user.Id, "Admin");
            }



        }


        public void CreateRoles()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            IdentityRole role;
            if (!roleManager.RoleExists("Admins"))
            {
                role = new IdentityRole();

                role.Name = "Admins";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Worker"))
            {
                role = new IdentityRole();
                role.Name = "Worker";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("General"))
            {
                role = new IdentityRole();
                role.Name = "General";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Employee"))
            {
                role = new IdentityRole();
                role.Name = "Employee";
                roleManager.Create(role);
            }



        }
    }
}
