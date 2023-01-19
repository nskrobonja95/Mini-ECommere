using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Infrastructure.Persistence;
using Application.Common.Interfaces;
using Infrastructure.Identity;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Infrastructure.Persistence.Interceptors;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Infrastructure;

public static class ConfigureServices
{

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("EcommerceApp"));
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<ApplicationDbContextSeeding>();

        services
            .AddDefaultIdentity<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddIdentityServer()
            .AddDeveloperSigningCredential()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();        
        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<IIdentityService, IdentityService>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })

        // Adding Jwt Bearer
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = configuration["JWT:ValidAudience"],
                ValidIssuer = configuration["JWT:ValidIssuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])),
                ValidateIssuerSigningKey = true
            };
        });

        services.AddAuthorization(options =>
            options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator")));

        return services;
    }

}
