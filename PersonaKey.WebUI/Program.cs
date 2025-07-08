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
using PersonaKey.DataAccessLayer.Context;
using PersonaKey.DataAccessLayer.Repository.Abstract;
using PersonaKey.DataAccessLayer.Repository.Concrete;
using PersonaKey.DataAccessLayer.UnitOfWorks.Abstract;
using PersonaKey.DataAccessLayer.UnitOfWorks.Concrete;

var builder = WebApplication.CreateBuilder(args);

// --------------------------------------------------
// Configuration: JWT options from appsettings.json
// --------------------------------------------------
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();

// --------------------------------------------------
// Authentication & Authorization (JWT)
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
// MVC & Controllers
// --------------------------------------------------
builder.Services.AddControllersWithViews();

// --------------------------------------------------
// DbContext (SQL Server Connection)
// --------------------------------------------------
builder.Services.AddDbContext<PersonaKeyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PersonaKeyConnection")));

// --------------------------------------------------
// Dependency Injection
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

// --------------------------------------------------
// FluentValidation
// --------------------------------------------------
builder.Services.AddValidatorsFromAssemblyContaining<DepartmentValidator>();
builder.Services.AddFluentValidationAutoValidation(options =>
{
    options.DisableDataAnnotationsValidation = true;
});
builder.Services.AddFluentValidationClientsideAdapters();

var app = builder.Build();

// --------------------------------------------------
// Middleware pipeline
// --------------------------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // JWT ile doðrulama
app.UseAuthorization();  // Yetkilendirme

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
