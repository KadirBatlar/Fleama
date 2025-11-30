using Fleama.Core.Validators;
using Fleama.Data;
using Fleama.Service.Abstract;
using Fleama.Service.Concrete;
using Fleama.Shared.Validators;
using Fleama.WebUI.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Configure FluentValidation global options for Turkish language
ValidatorOptions.Global.LanguageManager.Culture = new System.Globalization.CultureInfo("tr-TR");

// Add services to the container.
services.AddControllersWithViews(options =>
{
    // Suppress default model validation to use only FluentValidation
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
})
    .ConfigureApiBehaviorOptions(options =>
    {
        // Disable default model state validation
        options.InvalidModelStateResponseFactory = context =>
        {
            return new BadRequestObjectResult(context.ModelState);
        };
    });

// Add FluentValidation with auto-validation and client-side adapters
services.AddFluentValidationAutoValidation();
services.AddFluentValidationClientsideAdapters();

services.AddSession(options =>
{
    options.Cookie.Name = ".Fleama.Session";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromDays(1);
    options.IOTimeout = TimeSpan.FromMinutes(10);
});

services.AddDbContext<DatabaseContext>(options  =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
services.AddScoped<ICategoryService, CategoryService>();
services.AddScoped<IProductService, ProductService>();
services.AddScoped<INewsService, NewsService>();
services.AddScoped<IBlogService, BlogService>();

// Register FluentValidation validators
services.AddValidatorsFromAssemblyContaining<AppUserValidator>();
services.AddValidatorsFromAssemblyContaining<BlogCreateDTOValidator>();
services.AddValidatorsFromAssemblyContaining<LoginViewModelValidator>();
services.AddValidatorsFromAssemblyContaining<ProductValidator>();

services.AddHttpContextAccessor();

services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(x =>
{
    x.LoginPath = "/Account/SignIn";
    x.AccessDeniedPath = "/AccessDenied";
    x.Cookie.Name = "Account"; 
    x.Cookie.MaxAge = TimeSpan.FromDays(7);
    x.Cookie.IsEssential = true;
});

services.AddAuthorization(x =>
{
    x.AddPolicy("AdminPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
    x.AddPolicy("UserPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "Admin", "User"));
});

var app = builder.Build();  

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllerRoute(
    name: "admin",
    pattern: "{area:exists}/{controller=Main}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}",
    defaults: new { area = "" });

app.Run();