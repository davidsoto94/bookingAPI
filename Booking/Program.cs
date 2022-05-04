using System;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Booking.Data;
using Booking.Models;
using Booking.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options=> {
    options.SuppressAsyncSuffixInActionNames=false;
});
//Interface Service for the SQL server
builder.Services.AddScoped<IRoomReservationRepository,SqlDbRepository>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>
{
    //This is to generate the Default UI of Swagger Documentation
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ASP.NET 5 Web API",
        Description = "Authentication and Authorization in ASP.NET 5 with JWT and Swagger"
    });
    // To Enable authorization using Swagger (JWT)
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


//DataBase Context Service
var SqlSetting=builder.Configuration.GetConnectionString("ApplicationDbContext");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(SqlSetting));
// Identity Service
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();
//AddHealthChecks
builder.Services.AddHealthChecks()
.AddSqlServer(SqlSetting,name:"SqlServer"
,timeout:TimeSpan.FromSeconds(3),tags:new[]{"ready"});

// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})


// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration.GetSection("JWT:ValidAudience").Value,
        ValidIssuer = builder.Configuration.GetSection("JWT:ValidIssuer").Value,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWT:SecretKey").Value))
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
}


app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/Health/Ready",new HealthCheckOptions{
    Predicate=(check)=>check.Tags.Contains("Ready"),
    ResponseWriter=async(context,report)=>{
        var result =JsonSerializer.Serialize(
            new {
                status=report.Status.ToString(),
                checks=report.Entries.Select(entry=> new {
                    name=entry.Key,
                    status=entry.Value.Status,
                    exception=entry.Value.Exception !=null ? entry.Value.Exception.Message:"none",
                    duration=entry.Value.Duration.ToString()
                }
                )
                }
            );
            context.Response.ContentType=MediaTypeNames.Application.Json;
            await context.Response.WriteAsync(result);
        }
    });

app.MapHealthChecks("/Health/Live",new HealthCheckOptions{Predicate=(_)=>false});

app.Run();
