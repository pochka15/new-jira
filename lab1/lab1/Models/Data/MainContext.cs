using Microsoft.EntityFrameworkCore;

namespace lab1.Models.Data {
public class MainContext : DbContext {
    public MainContext(DbContextOptions<MainContext> options) : base(options) {
    }

    public DbSet<Project> Projects { get; set; }
    public DbSet<MonthReport> MonthReports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Project>().ToTable("Project");
        modelBuilder.Entity<Subproject>().ToTable("Subproject");
        modelBuilder.Entity<MonthReport>().ToTable("MonthReport");
        modelBuilder.Entity<Activity>().ToTable("Activity");
        modelBuilder.Entity<ProjectTimeSummary>().ToTable("ProjectTimeSummary");
    }
}
}