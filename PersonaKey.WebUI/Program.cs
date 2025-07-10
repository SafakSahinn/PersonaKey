using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using PersonaKey.WebUI.Validators;

var builder = WebApplication.CreateBuilder(args);

// --------------------------------------------------
// Configuration: JWT options from appsettings.json
// --------------------------------------------------
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();

// --------------------------------------------------
// Authentication & Authorization (JWT based)
// --------------------------------------------------
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
    };
});

builder.Services.AddAuthorization();

// --------------------------------------------------
// MVC & Controller support
// --------------------------------------------------
builder.Services.AddControllersWithViews();

// --------------------------------------------------
// Database: EF Core with SQL Server
// --------------------------------------------------
builder.Services.AddDbContext<PersonaKeyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PersonaKeyConnection")));

// --------------------------------------------------
// Dependency Injection: Service & Repository registration
// --------------------------------------------------
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

builder.Services.AddValidatorsFromAssemblyContaining<LoginViewModelValidator>(); // WebUI validator

// Custom Token service
builder.Services.AddScoped<TokenService>();

// --------------------------------------------------
// FluentValidation Configuration
// --------------------------------------------------
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly); // Registers all validators
builder.Services.AddFluentValidationAutoValidation(options =>
{
    options.DisableDataAnnotationsValidation = true;
});
builder.Services.AddFluentValidationClientsideAdapters();

var app = builder.Build();

// --------------------------------------------------
// Middleware Pipeline
// --------------------------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Enables JWT authentication
app.UseAuthorization();  // Enables authorization checks

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
