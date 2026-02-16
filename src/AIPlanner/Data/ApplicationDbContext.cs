using Microsoft.EntityFrameworkCore;
using AIPlanner.Models;
namespace AIPlanner.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
        {

        }
        public DbSet<DailyPlan> DailyPlans => Set<DailyPlan>();
        public DbSet<PlanTask> PlanTasks => Set<PlanTask>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DailyPlan>()
                .HasMany(p => p.Tasks)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
