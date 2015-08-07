using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Framework.OptionsModel;

namespace aspnet5.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Message> Messages { get; set; }

        public DbSet<UserAnswer> UserAnswers { get; set; }

        public DbSet<QuestTask> QuestTasks { get; set; }

        public DbSet<UserStat> UserStats { get; set; }

        private readonly IOptions<DataSettings> dataSettings;
        public ApplicationDbContext(IOptions<DataSettings> dataSettings)
        {
            this.dataSettings = dataSettings;
        }

        protected override void OnConfiguring(EntityOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(dataSettings.Options.ConnectionString);
        }
    }
}
