using Microsoft.OpenApi.Models;
using RealWorldConduit_API.HostedJob;
using RealWorldConduit_Infrastructure.Extensions;
using RealWorldConduit_Infrastructure.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Set up swagger UI with JWT Bearer Token
builder.Services.AddSwaggerGen(swagger =>
{
    // Default UI of Swagger Documentation
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Dummy API with JWT Token Authentication",
        Description = ".NET 8 Web API"
    });
    // Enable authorization using Swagger (JWT)
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\""
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

// Hosted Services
builder.Services.AddHostedService<CreatePersonalUserJob>();

// Extensions and DI
builder.Services.BaseExtensionConfig()
                .MediatRExtensionConfig()
                .FluentValidationConfig()
                .AuthExtensionConfig(builder.Configuration)
                .RedisExtensionConfig(builder.Configuration)
                .QuartzExtensionConfig(builder.Configuration)
                .DatabaseExtensionConfig(builder.Configuration)
                .LocalizerExtensionConfig(builder.Configuration)        
                .AddExceptionHandler<RestfulAPIExceptionMiddleware>();



var app = builder.Build();

// Middlewares
app.UseExceptionHandler(opt => { }); // Global Exception Handler
app.UseMiddleware<LocalizerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Authentication - Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
