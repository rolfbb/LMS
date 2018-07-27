using Microsoft.AspNet.Identity.EntityFramework;

namespace LMS.Models
{
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

        public System.Data.Entity.DbSet<LMS.Models.Course> Courses { get; set; }

        public System.Data.Entity.DbSet<LMS.Models.Module> Modules { get; set; }

        public System.Data.Entity.DbSet<LMS.Models.Activity> Activities { get; set; }

        public System.Data.Entity.DbSet<LMS.Models.ActivityType> ActivityTypes { get; set; }
        public object UserProfiles { get; internal set; }

        public System.Data.Entity.DbSet<LMS.Models.Document> Documents { get; set; }
    }
}