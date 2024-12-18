﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sensors.Data;
using Sensors.IdentityManager;
using Sensors.SensorAuthentication;
using Sensors.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<UserManager<IdentityUser>, CustomUserManager>();
builder.Services.AddScoped<ISensorValidator, SensorValidator>();

builder.Services.AddAutoMapper(typeof(Program));

// Add custom authorizationfor the sensor values controller
builder.Services.AddAuthorization(authOptions =>
{
    authOptions.AddPolicy("SensorAuthorizationPolicy", authPolicy =>
    {
        authPolicy.Requirements.Add(new SensorAuthorizationRequirement());
    });
});

builder.Services.AddScoped<IAuthorizationHandler, SensorAuthorizationHandler>();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
