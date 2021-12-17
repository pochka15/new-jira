using Microsoft.EntityFrameworkCore;

namespace lab1.Models.Data {
public class MainContext : DbContext {
    public MainContext(DbContextOptions<MainContext> options) : base(options) {
    }

    public DbSet<Project> Projects { get; set; }
    public DbSet<MonthReport> MonthReports { get; set; }
    public DbSet<Activity> Activities { get; set; }

    protected override void OnModelCreating(ModelBuilder builder) {
        builder.Entity<Project>().ToTable(nameof(Project));
        builder.Entity<Subproject>().ToTable(nameof(Subproject));
        builder.Entity<MonthReport>().ToTable(nameof(MonthReport));
        builder.Entity<Activity>().ToTable(nameof(Activity));
        builder.Entity<AcceptedWork>().ToTable(nameof(AcceptedWork))
            .HasOne<Project>()
            .WithMany()
            .HasForeignKey(it => it.ProjectId);
    }
}
}