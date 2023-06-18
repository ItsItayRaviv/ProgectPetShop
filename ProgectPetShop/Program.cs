using Microsoft.EntityFrameworkCore;
using ProgectPetShop;
using System.Data.Entity;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews(); 
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapDefaultControllerRoute();

new Startup();

app.Run();