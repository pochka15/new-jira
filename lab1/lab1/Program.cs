using System;
using lab1.Models.Data;
using lab1.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace lab1 {
public class Program {
    public static void Main(string[] args) {
        var host = CreateHostBuilder(args).Build();
        CreateDbIfNotExists(host);
        host.Run();
    }

    private static void CreateDbIfNotExists(IHost host) {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        try {
            var context = services.GetRequiredService<MainContext>();
            var projectService = services.GetRequiredService<IProjectService>();
            var reportService = services.GetRequiredService<IReportService>();
            DbInitializer.Initialize(context, projectService, reportService);
        }
        catch (Exception ex) {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred creating the DB.");
        }
    }


    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
}
}