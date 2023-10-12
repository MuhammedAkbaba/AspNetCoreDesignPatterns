using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Configuration;
using WebApp.Strategy.Models;
using WebApp.Strategy.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

///Identity 
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<AppIdentityDbContext>();

builder.Services.AddDbContext<AppIdentityDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("SqlPostGreSqlCon"));
    //options.UseSqlServer(builder.Configuration.GetConnectionString("SqlDbCon"));

});


builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IProductRepository>(sp =>
{
    var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>()?.HttpContext;

    ///cookie üzerinde claim deki deðeri okuma
    var claim = httpContextAccessor.User.Claims.FirstOrDefault(x => x.Type == Settings.claimDatabaseType);

    var _context = sp.GetRequiredService<AppIdentityDbContext>();

    var configuration = sp.GetRequiredService<IConfiguration>();

    ///claim  null ise default olarak verme
    if (claim == null) return new ProductRepositoryFromPostGreSql(_context);

    var databaseType = (EDatabaseType)int.Parse(claim.Value);

    return databaseType switch
    {
        EDatabaseType.SqlServer => new ProductRepositoryFromSqlServer(_context),
        EDatabaseType.PostGreSql => new ProductRepositoryFromPostGreSql(_context),
        EDatabaseType.MongoDb => new ProductRepositoryFromMongoDb(configuration),
        _ => new ProductRepositoryFromPostGreSql(_context),
    };

});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


DataSeeding.SeedUser(app);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
