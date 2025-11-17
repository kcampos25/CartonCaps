using CartonCaps.Application.Interfaces.Repositories;
using CartonCaps.Application.Interfaces.Services;
using CartonCaps.Application.Interfaces.Validators;
using CartonCaps.Application.Mappings;
using CartonCaps.Application.Services;
using CartonCaps.Application.Validators;
using CartonCaps.Common.Middleware;
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
builder.Services.AddScoped<IReferralVisitRepository, ReferralVisitRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

//Services
builder.Services.AddScoped<IReferralService, ReferralService>();
builder.Services.AddScoped<IReferralVisitService, ReferralVisitService>();
builder.Services.AddScoped<IUserServiceValidator, UserServiceValidator>();
builder.Services.AddScoped<IReferralServiceValidator, ReferralServiceValidator>();
builder.Services.AddTransient<IReferralCodeGenerator, ReferralCodeGenerator>();

var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "CartonCaps API",
        Version = "v1",
        Description = "API for Carton Caps App responsible for managing the module to refer friends."
    });
    c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});

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

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
