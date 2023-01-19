using Application;
using Application.Common.Interfaces;
using EcommerceApp.FIlter;
using EcommerceApp.Services;
using FluentValidation.AspNetCore;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();
builder.Services.AddHttpContextAccessor();
//use named client for mock call
builder.Services.AddHttpClient("Supplier", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://www.random.org/integers/");
});

builder.Services.AddHealthChecks();
builder.Services.AddControllers(options =>
            options.Filters.Add<ApiExceptionFilterAttribute>())
                .AddFluentValidation(x => x.AutomaticValidationEnabled = false);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseDeveloperExceptionPage();

    //Initialise and seed database
    using (var scope = app.Services.CreateScope())
    {
        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextSeeding>();
        await initialiser.InitialiseAsync();
        await initialiser.SeedAsync();
    }
}

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.Run();

public partial class Program { }
