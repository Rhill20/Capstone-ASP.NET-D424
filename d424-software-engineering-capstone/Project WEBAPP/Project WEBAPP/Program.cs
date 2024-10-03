using Microsoft.EntityFrameworkCore;
using Project_WEBAPP.Data;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorPages();


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; 
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("MySqlConnection"),
        new MySqlServerVersion(new Version(8, 0, 27))));

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // Add session middleware

app.UseAuthorization();

app.MapRazorPages();

app.Run();
