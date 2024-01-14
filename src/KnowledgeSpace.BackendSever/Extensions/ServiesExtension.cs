using KnowledgeSpace.Data;
using KnowledgeSpace.Data.Data;
using KnowledgeSpace.Data.Entities;
using Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeSpace.BackendSever.Extensions
{
    public static class ServiesExtension
    {
        public static void ConfigureSQLContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config["ConnectionStrings:DefaultConnection"];
            services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(connectionString));
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.Configure<IdentityOptions>(options =>
            {
                // Default Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.User.RequireUniqueEmail = true;
            });
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        public static void ConfigureService(this IServiceCollection services)
        {
            services.AddTransient<DbInitializer>();
        }

        public static void SeedingDataService(this IServiceProvider services)
        {
            var dbInitializer = services.GetService<DbInitializer>();
            dbInitializer.Seed().Wait();
        }
    }
}