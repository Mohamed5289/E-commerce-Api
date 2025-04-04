
using E_Commerce.Data;
using E_Commerce.ModelHelpers;
using E_Commerce.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.RateLimiting;

namespace E_Commerce
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // RateLimiter with Username 
            builder.Services.AddRateLimiter(
                option => option.AddPolicy("UserLimiter", httpContext => 
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.User.Identity!.Name,
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 2,
                            Window = TimeSpan.FromSeconds(20)
                        }

                    )
                )
            );

            builder.Services.AddDbContext<IdContext>(options =>
            {
                options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });


            //Configuration for JWT, ImageSettings, MailSettings, and PayPal
            builder.Services.Configure<Jwt>(builder.Configuration.GetSection("Jwt"));
            builder.Services.Configure<ImageSetting>(builder.Configuration.GetSection("ImageSettings"));
            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));



            // Add Identity and Suppress the default model state validation filter
            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true; // Require email confirmation
                options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider; // Set the email confirmation token provider

                options.Password.RequireDigit = false;   // No digit required
                options.Password.RequireLowercase = false; // No lowercase required
                options.Password.RequireUppercase = false; // No uppercase required
                options.Password.RequireNonAlphanumeric = false; // No special characters required
                options.Password.RequiredLength = 1; // Minimum length set to 1
            })
                            .AddEntityFrameworkStores<IdContext>().AddDefaultTokenProviders();

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            // Add JWT Authentication
            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            builder.Services.AddScoped<Token>();
            builder.Services.AddScoped<ImageService>();
            builder.Services.AddScoped<ModelHelpers.IMailService, ModelHelpers.MailService>();
            builder.Services.AddHttpClient();

            builder.Services.AddMediatR(typeof(Program).Assembly);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
                RequestPath = "/wwwroot"

            });
            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseAuthentication();


            app.MapControllers();

            app.Run();
        }
    }
}
