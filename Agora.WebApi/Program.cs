using Agora.BLL.Infrastructure;
using Agora.Hubs;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using StackExchange.Redis;
using System;
using System.Text;

namespace Agora
{
  public class Program
    {
        public static void Main(string[] args)
        {
            Env.Load();// загружает переменные из .env файла

            var builder = WebApplication.CreateBuilder(args);

            string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();
            builder.Services.AddAgoraContext(connection);

            builder.Services.AddUnitOfWorkService();
            builder.Services.AddBusinessServices();

            builder.Services.AddIdentityServices();

            builder.Services.AddSignalR(options =>
            {
                options.MaximumReceiveMessageSize = 10 * 1024 * 1024;
            });

            // for Redis caching:
            try
            {
                var redis = ConnectionMultiplexer.Connect("redis:6379");
                builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
            }            
            catch
            {
                Console.WriteLine("Redis not connected..");
                builder.Services.AddSingleton<IConnectionMultiplexer>(sp => null);
            }            

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            //JWT
            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", policy =>
                {
                    policy.WithOrigins("https://api.agorastore.pp.ua", "https://agorastore.pp.ua", "https://www.liqpay.ua")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            builder.Configuration
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables();

            var app = builder.Build();

         
            app.MapOpenApi();
            app.MapScalarApiReference();  // Scalar UI will be available at: http://localhost:5193/scalar/v1
            

            app.UseCors("AllowSpecificOrigin");
            app.UseAuthentication();  
            app.UseAuthorization(); 
            //app.UseMiddleware<JwtValidationMiddleware>();
            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseSession();
    
            app.UseStaticFiles();
            //app.UseCors(builder => builder.WithOrigins("http://localhost:3000", "http://localhost:5193")// for React и Scalar
            //                           .AllowAnyHeader()
            //                           .AllowAnyMethod()
            //                            .AllowCredentials());

            app.MapControllers();

            app.MapHub<ChatHub>("/chatHub");

            app.Run();
            



        }
    }
}
