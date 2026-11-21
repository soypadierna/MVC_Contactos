using Microsoft.EntityFrameworkCore;
using MVC_Contactos.Data;
using MVC_Contactos.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ContactosDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ContactosConnection")));

builder.Services.AddScoped<InformesService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseStatusCodePages();
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Principal}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();