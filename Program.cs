using BookHive.Models;
using BookHive.Services;
using Microsoft.EntityFrameworkCore;

namespace BookHive;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddDbContext<AppDbContext>(opts =>
        {
            opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            opts.EnableSensitiveDataLogging();
        });
        builder.Services.AddSession(opts =>
        {
            opts.IdleTimeout = TimeSpan.FromMinutes(10);
            opts.Cookie.IsEssential = true;
        });
        builder.Services
            .AddScoped<BookRepository>()
            .AddScoped<UserRepository>()
            .AddScoped<ActivityLogRepository>()
            .AddScoped<CommentRepository>();

        var app = builder.Build();

        app.UseStaticFiles();
        app.UseSession();
        app.UseMiddleware<AuthenticationMiddleware>();
        app.UseRouting();
        app.MapControllerRoute("default", "{controller=Catalog}/{action=Index}/{id?}/{param1?}/{param2?}/{param3?}");

        // var context = app.Services.CreateScope().ServiceProvider
        //     .GetRequiredService<AppDbContext>();
        // SeedData.SeedDatabase(context);

        app.Run();
    }
}