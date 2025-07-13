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

// Load JWT settings from appsettings.json
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();

// Configure JWT Bearer Authentication
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

// Define Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OnlyLoggedInUsers", policy =>
    {
        policy.RequireClaim("CanLogin", "True");
    });
});

// Add MVC Controllers with Views
builder.Services.AddControllersWithViews();

// Configure Entity Framework Core with SQL Server
builder.Services.AddDbContext<PersonaKeyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PersonaKeyConnection")));

// Register Dependency Injection for Repositories, UnitOfWork, Services
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

// JWT Token generation service
builder.Services.AddScoped<TokenService>();

// Register FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<LoginViewModelValidator>();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddFluentValidationAutoValidation(options =>
{
    options.DisableDataAnnotationsValidation = true;
});
builder.Services.AddFluentValidationClientsideAdapters();

var app = builder.Build();

// Middleware: Read JWT token from cookie and set it to the Authorization header
app.Use(async (context, next) =>
{
    var token = context.Request.Cookies["jwt"];
    if (!string.IsNullOrEmpty(token))
    {
        context.Request.Headers.Authorization = $"Bearer {token}";
    }
    await next();
});

// Error handling
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Redirect 401 errors to login BEFORE authentication/authorization
app.UseStatusCodePages(context =>
{
    if (context.HttpContext.Response.StatusCode == 401)
    {
        context.HttpContext.Response.Redirect("/Authentication/Login");
    }
    return Task.CompletedTask;
});

app.UseAuthentication(); // Enables authentication
app.UseAuthorization();  // Enables authorization

// Default route mapping
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
