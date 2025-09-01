using Microsoft.EntityFrameworkCore;
using Project_Manager.Data_Access.Extensions;
using Project_Manager.BusinessLogic.Extensions;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


//Registering DbContext to work with DB via DI
builder.Services.AddDataAccess(builder.Configuration);

builder.Services.AddBusinessLogic();

builder.Services.AddSwaggerGen();



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
    pattern: "{controller=ProjectMvc}/{action=Index}/{id?}");

//Connect routing to Web API controllers
app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
