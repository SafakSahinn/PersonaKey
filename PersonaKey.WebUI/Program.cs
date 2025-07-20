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

// Load JWT settings from configuration
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();

// Configure JWT authentication
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

// Define authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OnlyLoggedInUsers", policy =>
    {
        policy.RequireClaim("CanLogin", "True");
    });

    options.AddPolicy("OnlyEditors", policy =>
    {
        policy.RequireClaim("CanEditSite", "True");
    });
});

// Add MVC with views
builder.Services.AddControllersWithViews();

// Configure EF Core
builder.Services.AddDbContext<PersonaKeyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PersonaKeyConnection")));

// Register DI services
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

builder.Services.AddScoped<TokenService>();

// FluentValidation registration
builder.Services.AddValidatorsFromAssemblyContaining<LoginViewModelValidator>();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddFluentValidationAutoValidation(options =>
{
    options.DisableDataAnnotationsValidation = true;
});
builder.Services.AddFluentValidationClientsideAdapters();

var app = builder.Build();

// Middleware to read JWT token from cookie and set it in Authorization header
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

// Redirect 401 Unauthorized responses to login page
app.UseStatusCodePagesWithRedirects("/Authentication/Login");

app.UseAuthentication(); // Enable authentication
app.UseAuthorization();  // Enable authorization

// Default route mapping
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();