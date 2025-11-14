using CartonCaps.Application.Interfaces.Repositories;
using CartonCaps.Application.Interfaces.Services;
using CartonCaps.Application.Mappings;
using CartonCaps.Application.Services;
using CartonCaps.Infrastructure.Data;
using CartonCaps.Infrastructure.MockData;
using CartonCaps.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Configure InMemoryDatabase
builder.Services.AddDbContext<CartonCapsContext>(options =>
    options.UseInMemoryDatabase("CartonCapsDB"));

//Add services to the container.
builder.Services.AddControllers();

//AutoMapper
builder.Services.AddAutoMapper(typeof(CartonCapsProfile));

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

//Repositories
builder.Services.AddScoped<IReferralRepository, ReferralRepository>();

//Services
builder.Services.AddScoped<IReferralService, ReferralService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Load test data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CartonCapsContext>();
    MockDataSeeder.Seed(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
