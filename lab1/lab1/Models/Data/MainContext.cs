using Microsoft.EntityFrameworkCore;

namespace lab1.Models.Data {
public class MainContext : DbContext {
    public MainContext(DbContextOptions<MainContext> options) : base(options) {
    }

    public DbSet<Project> Projects { get; set; }
    public DbSet<MonthReport> MonthReports { get; set; }
    public DbSet<Activity> Activities { get; set; }

    protected override void OnModelCreating(ModelBuilder builder) {
        builder.Entity<Project>().ToTable("Project");
        builder.Entity<Subproject>().ToTable("Subproject");
        builder.Entity<MonthReport>().ToTable("MonthReport");
        builder.Entity<Activity>().ToTable("Activity");
        builder.Entity<AcceptedWork>().ToTable("ProjectTime");
    }
}
}