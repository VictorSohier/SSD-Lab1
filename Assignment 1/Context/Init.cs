using Assignment_1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment_1.Context
{
    public class Init
    {
        private string[] _Emails { get; }

        private string[] _Roles { get; }

        private string _DefaultPass { get; }

        private IServiceProvider _ServiceProvider { get; }

        public Init(IServiceProvider serviceProvider)
        {
            IConfiguration configuration = ((IConfiguration)serviceProvider.GetService(typeof(IConfiguration)));
            string s = configuration.GetSection("Roles").Value;
            List<string> emailsList = new List<string>();
            for (int i = 0; configuration.GetSection($"Email{i}").Value != null; i++)
                emailsList.Add(configuration.GetSection($"Email{i}").Value);
            List<string> rolesList = new List<string>();
            for (int i = 0; configuration.GetSection($"Roles{i}").Value != null; i++)
                rolesList.Add(configuration.GetSection($"Roles{i}").Value);

            _Emails = emailsList.ToArray();
            _Roles = rolesList.ToArray();
            _DefaultPass = configuration.GetSection("DefaultPass").Value;
            _ServiceProvider = serviceProvider;
            Run().GetAwaiter();
        }

        private async Task Run()
        {
            ((Assignment1DBContext)_ServiceProvider.GetService(typeof(Assignment1DBContext))).Database.Migrate();
            UserManager<ApplicationUser> uManager = (UserManager<ApplicationUser>) _ServiceProvider.GetService(typeof(UserManager<ApplicationUser>));
            RoleManager<IdentityRole> rManager = (RoleManager<IdentityRole>) _ServiceProvider.GetService(typeof(RoleManager<IdentityRole>));

            if (rManager.Roles.Count() > 0 || uManager.Users.Count() > 0)
                return;

            foreach (string role in _Roles)
                await rManager.CreateAsync(new IdentityRole(role));

            for (int i = 0; i < _Emails.Length; i++)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = _Emails[i],
                    Email = _Emails[i],
                    FirstName = "John",
                    LastName = "Smith",
                    EmailConfirmed = true
                };
                uManager.CreateAsync(user, _DefaultPass).Wait();
                await uManager.AddToRoleAsync(user, _Roles[i % _Roles.Length]);
            }
        }
    }
}
