// Program.cs
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PersonaKey.BusinessLayer.Abstract;
using PersonaKey.BusinessLayer.Concrete;
using PersonaKey.BusinessLayer.Validators;
using PersonaKey.CoreLayer.Configuration;
using PersonaKey.CoreLayer.Services;
using PersonaKey.DataAccessLayer.Context;
using PersonaKey.DataAccessLayer.Repository.Abstract;
using PersonaKey.DataAccessLayer.Repository.Concrete;
using PersonaKey.DataAccessLayer.UnitOfWorks.Abstract;
using PersonaKey.DataAccessLayer.UnitOfWorks.Concrete;
using PersonaKey.WebUI.Authorization;
using PersonaKey.WebUI.Validators;

var builder = WebApplication.CreateBuilder(args);

// Configure JWT options from appsettings.json
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();

// Add Authentication with JWT Bearer
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // Validate the issuer of the token
        ValidateAudience = true, // Validate the audience of the token
        ValidateLifetime = true, // Validate token expiry
        ValidateIssuerSigningKey = true, // Validate signature key
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
    };
});

// Add Authorization policies
builder.Services.AddAuthorization(options =>
{
    // Policy requiring the claim "CanLogin" to be "True"
    options.AddPolicy("OnlyLoggedInUsers", policy =>
    {
        policy.RequireClaim("CanLogin", "True");
    });
});

// Add MVC controllers with views support
builder.Services.AddControllersWithViews();

// Setup Entity Framework Core with SQL Server
builder.Services.AddDbContext<PersonaKeyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PersonaKeyConnection")));

// Dependency Injection for repositories, unit of work, services
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IPersonService, PersonManager>();
builder.Services.AddScoped<IDepartmentService, DepartmentManager>();
builder.Services.AddScoped<IRoleService, RoleManager>();
builder.Services.AddScoped<IPermissionService, PermissionManager>();
builder.Services.AddScoped<IDoorService, DoorManager>();
builder.Services.AddScoped<ICardService, CardManager>();
builder.Services.AddScoped<IAccessLogService, AccessLogManager>();
builder.Services.AddScoped<IAppUserService, AppUserManager>();

// Custom JWT token generator service
builder.Services.AddScoped<TokenService>();

// FluentValidation registrations
builder.Services.AddValidatorsFromAssemblyContaining<LoginViewModelValidator>();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddFluentValidationAutoValidation(options =>
{
    options.DisableDataAnnotationsValidation = true; // Disable default DataAnnotations validation
});
builder.Services.AddFluentValidationClientsideAdapters();

// Middleware pipeline configuration
var app = builder.Build();

// Middleware to read JWT token from cookie and set it to Authorization header
app.Use(async (context, next) =>
{
    var token = context.Request.Cookies["jwt"];
    if (!string.IsNullOrEmpty(token))
    {
        context.Request.Headers.Authorization = "Bearer " + token;
    }
    await next();
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
