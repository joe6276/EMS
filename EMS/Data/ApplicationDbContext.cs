using EMS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EMS.Data
{
    public class ApplicationDbContext:IdentityDbContext<User>
    {

        public ApplicationDbContext( DbContextOptions<ApplicationDbContext> options) : base(options) { }


        //Create Tables

        public DbSet<User> Users { get; set; }

        public DbSet<Message> Messages { get; set; }

    }
}
