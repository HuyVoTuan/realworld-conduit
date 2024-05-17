using RealWorldConduit_API.HostedJob;
using RealWorldConduit_Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Hosted Services
builder.Services.AddHostedService<CreatePersonalUserJob>();

// Extensions and DI
builder.Services.BaseExtensionConfig()
                .AuthExtensionConfig()
                .QuartzExtensionConfig(builder.Configuration)
                .DatabaseExtensionConfig(builder.Configuration);


var app = builder.Build();

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
