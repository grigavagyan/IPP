using FluentValidation;
using IPP.Application.Exceptions;
using IPP.Application.Projects.Interfaces;
using IPP.Application.Services;
using IPP.Infrastructure;
using IPP.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidatorsFromAssembly(typeof(CommandDispatcher).Assembly);
builder.Services.AddScoped<ICommandDispatcher, CommandDispatcher>();
builder.Services.AddScoped<IQueryDispatcher, QueryDispatcher>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.Scan(scan => scan
    .FromAssemblies(
        Assembly.GetExecutingAssembly(),
        Assembly.Load("IPP.Application"))
    .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
    .AsImplementedInterfaces()
    .WithScopedLifetime()
);

builder.Services.Scan(scan => scan
    .FromAssemblies(
        Assembly.GetExecutingAssembly(),
        Assembly.Load("IPP.Application"))
    .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
    .AsImplementedInterfaces()
    .WithScopedLifetime()
);

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var jwtConfigurations = builder.Configuration.GetSection("Jwt");
var keyBytes = Encoding.UTF8.GetBytes(jwtConfigurations["Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtConfigurations["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtConfigurations["Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromSeconds(30)
    };
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Company–Employee Management API",
        Version = "v1",
        Description = "Project is made by Grigor Avagyan",
        Contact = new OpenApiContact
        {
            Name = "Grigor Avagyan",
            Email = "grigor.avagyan@actualsolutions.com"
        }
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT token: Bearer {your_token}"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//app.Use(async (context, next) =>
//{
//    try
//    {
//        await next();
//    }
//    catch (NotFoundException ex)
//    {
//        context.Response.StatusCode = StatusCodes.Status404NotFound;
//        await context.Response.WriteAsJsonAsync(new { message = ex.Message });
//    }
//});

app.MapControllers();

app.Run();