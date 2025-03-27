
using Microsoft.EntityFrameworkCore;
using ContactDetails_Api.Data;
using ContactDetails_Api.Model;
using ContactDetails_Api.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;
using Microsoft.Extensions.Logging;
using ContactDetails_Api.Custom_Execption_Filtter;
using NLog;
using NLog.Web;

namespace ContactDetails_Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var logger = NLog.LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();
            logger.Info("Application starting...");
            builder.Logging.ClearProviders();

            builder.Host.UseNLog();



            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            var Connection = builder.Configuration.GetConnectionString("DB");
            builder.Services.AddDbContext<AppDbContex>(p => p.UseSqlServer(Connection));
            builder.Services.AddScoped<Icontact_Repository, ContactRepository>();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
        };
    });
            builder.Services.AddControllers(p => { p.Filters.Add<CustomExpextion>(); });
            builder.Services.AddCors(p => p.AddPolicy("Open", q => q.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin()));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("Open");

            app.MapControllers();

            app.Run();
        }
    }
}
