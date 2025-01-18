using EMS.Data;
using EMS.Models;
using EMS.Profiles;
using EMS.Services;
using EMS.Services.Interfaces;
using EMS.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Database Service
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection"));
});

//Add Identity
builder.Services.AddIdentity<User , IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

//Register AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));


//Configure Utility Class
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));


//Add Custom Classes

builder.Services.AddScoped<IJwt, JwtService>();
builder.Services.AddScoped<IUser , UserService>();


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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
