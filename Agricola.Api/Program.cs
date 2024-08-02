using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text.Json.Serialization;
using AgricolaApi.Api;
using AgricolaApi.Application.Interfaces;
using AgricolaApi.Application;
using AgricolaApi.Applications;
using AgricolaApi.Infrastructure.Context;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// configure CORS, so this api is only accesed by client origin http://localhost:5173 for now
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy
                           .WithOrigins("http://localhost:5173")
                           .AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                      });
});

builder.Services.AddControllers();

// Swagger tool
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// add authentication with Jwt Bearer strategy [Authorize]/[AllowAnonymous]
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

// configure options from appsettings.json
builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

// configure database connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? ""));

// add for avoiding circular references
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

// add dependency injection
builder.Services.AddTransient<AuthenticatedUser>();
builder.Services.AddTransient<IUserService, UserServices>();
builder.Services.AddTransient<IAuthServices, AuthService>();
builder.Services.AddTransient<IJwtProvider, JwtProvider>();
builder.Services.AddTransient<IFarmService, FarmServices>();
builder.Services.AddTransient<ILotService, LotServices>();
builder.Services.AddTransient<IGroupService, GroupServices>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

// needed for authentication
app.UseAuthentication();
app.UseAuthorization();

// add the middleware
app.UseMiddleware<AuthenticatedUser>();

app.MapControllers();

app.Run();
