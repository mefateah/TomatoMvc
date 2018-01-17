using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using MvcTomato.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MvcTomato.DAL
{
    public class TomatoContext : IdentityDbContext<ApplicationUser>
    {
        public TomatoContext() : base("TomatoContext")
        {
        }
        
        public DbSet<WorkingDay> WorkingDays { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // make table names in database not to be pluralized ('student' instead 'students')
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}