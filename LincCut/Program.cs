using LincCut;
using LincCut.AppSettings;
using LincCut.Data;
using LincCut.Middleware;
using LincCut.Repository;
using LincCut.ServiceLayer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultSQLConnection")));
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddControllers();
builder.Services.AddScoped<IServiceForAuth, ServiceForAuth>();
builder.Services.AddScoped<IServiceForShortCut, ServiceForShortCut>();
builder.Services.AddScoped<IUrlInfoRepository, UrlInfoRepository>();
builder.Services.AddScoped<IClickRepository, ClickRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
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
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<JwtTokenMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
