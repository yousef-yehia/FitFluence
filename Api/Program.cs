using Api.DTO;
using Api.Helper;
using Core.Interfaces;
using Core.Models;
using FitFluence.Repository;
using Infrastructure.Data.Seed;
using Infrastructure.Data;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Infrastructure.Services;
using Infrastructures.Repository;
using Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();

    await context.Database.MigrateAsync();
    await AppDbContextSeed.SeedDataAsync(context);

    //var foodSeeder = scope.ServiceProvider.GetRequiredService<FoodSeeder>();
    //foodSeeder.Seed();

    var RoleSeeder = scope.ServiceProvider.GetRequiredService<RolesSeeder>();

    await RoleSeeder.SeedRolesAsync();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(o => o.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

