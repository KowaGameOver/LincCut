using LincCut;
using LincCut.AppSettings;
using LincCut.Data;
using LincCut.Repository;
using LincCut.ServiceLayer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultSQLConnection")));
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddControllers();
builder.Services.AddScoped<IService, Service>();
builder.Services.AddScoped<IUrlInfoRepository, UrlInfoRepository>();
builder.Services.AddScoped<IClickRepository, ClickRepository>();
builder.Services.Configure<HostName>(builder.Configuration.GetSection("HostName"));
builder.Services.Configure<Alphabet>(builder.Configuration.GetSection("Alphabet"));
builder.Services.AddDetection();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
