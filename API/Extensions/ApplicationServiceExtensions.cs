using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<DataContext, DataContext>();

            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<LogUserActivity>();
            services.AddScoped<IPlatformUserDataManager, PlatformUserDataManager>();
            services.AddScoped<IPhotoDataManager, PhotoDataManager>();

            //services.AddDbContext<DataContext>(options =>
            //{
            //    options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            //}, ServiceLifetime.Transient);

            return services;
        }
    }
}
