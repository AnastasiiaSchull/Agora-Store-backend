using Microsoft.EntityFrameworkCore;
using Agora.BLL.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Scalar.AspNetCore;
using StackExchange.Redis;
using Microsoft.AspNetCore.Identity;
using System;

using DotNetEnv;

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

            // for Redis caching:
            try
            {
                var redis = ConnectionMultiplexer.Connect("localhost:6379");
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
                    policy.WithOrigins("http://localhost:3000", "http://localhost:5193")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });
            


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();  // Scalar UI will be available at: http://localhost:5193/scalar/v1
            }
            


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

            
            app.Run();
            



        }
    }
}
