using LincCut;
using LincCut.AppSettings;
using LincCut.Data;
using LincCut.Repository;
using LincCut.ServiceLayer;
using LincCut.test;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultSQLConnection")));
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddControllers();
builder.Services.AddScoped<IServiceForShortCut, ServiceForShortCut>();
builder.Services.AddScoped<IUrlInfoRepository, UrlInfoRepository>();
builder.Services.AddScoped<IClickRepository, ClickRepository>();

//builder.Services.AddTransient<TokenManagerMiddleware>();
//builder.Services.AddTransient<ITokenManager, TokenManager>();
//builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//builder.Services.AddDistributedRedisCache(r => {
//r.Configuration = Configuration["redis:connectionString"];


builder.Services.Configure<HostName>(builder.Configuration.GetSection("HostName"));
builder.Services.Configure<Alphabet>(builder.Configuration.GetSection("Alphabet"));
builder.Services.AddDetection();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters
        = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("JWT:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standart Authorization header usng the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

//app.UseMiddleware<TokenManagerMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
