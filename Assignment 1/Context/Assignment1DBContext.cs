using Assignment_1.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment_1.Context
{
    public class Assignment1DBContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Team> Teams { get; set; }

        public Assignment1DBContext(DbContextOptions<Assignment1DBContext> options) : base(options)
        {

        }
    }
}
