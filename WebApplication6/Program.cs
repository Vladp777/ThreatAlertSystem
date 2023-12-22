using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThreadAlert.Entities;
using ThreadAlert.Persistance;
using ThreadAlert.Repositories;
using WebApplication6.Services;
using WebApplication6.Hubs;
using WebApplication6.SubscribeTableDependencies;
using WebApplication6.MiddlewareExtensions;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AppDbContextConnection") ?? throw new InvalidOperationException("Connection string 'AppDbContextConnection' not found.");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<AppUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddScoped<DangerousObjectsRepository>();
builder.Services.AddScoped<MessageRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<MessageService>();
builder.Services.AddScoped<AlertService>();

//builder.Services.AddDbContextFactory<AppDbContext>();
builder.Services.AddMvc()
    .AddRazorPagesOptions(options =>
{
    options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddSingleton<NotificationHub>();
builder.Services.AddSingleton<SubscribeNotificationTableDependency>();
builder.Services.AddHttpContextAccessor();

// Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthorization();
app.MapHub<NotificationHub>("/notificationHub");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.UseSqlTableDependency<SubscribeNotificationTableDependency>(connectionString);

using (var scope = app.Services.CreateScope())
{
    var roleManager = 
        scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Admin", "Member" };

    foreach (var role in roles)
    {
        if(!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager =
        scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

    string email = "admin@admin.com";
    string pass = "Admin1234!";

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new AppUser
        {
            FirstName = "admin",
            LastName = "admin",
            Email = email,
            UserName = email
        };

        await userManager.CreateAsync(user, pass);

        await userManager.AddToRoleAsync(user, "Admin");
    }
}

app.Run();
