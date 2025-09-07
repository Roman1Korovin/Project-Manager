using Microsoft.EntityFrameworkCore;
using Project_Manager.Data_Access.Extensions;
using Project_Manager.BusinessLogic.Extensions;
using System;
using Project_Manager.Data_Access;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


//Registering DbContext to work with DB via DI
builder.Services.AddDataAccess(builder.Configuration);

builder.Services.AddBusinessLogic();

builder.Services.AddSwaggerGen();

// Add session services
builder.Services.AddDistributedMemoryCache(); // или другой провайдер кэша
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session lifetime
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();


// Automatic database migration
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppContextDB>();
    db.Database.Migrate();
    db.Seed();
}

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

// Add middleware for session
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ProjectMvc}/{action=Index}/{id?}");

//Connect routing to Web API controllers
app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
