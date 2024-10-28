using Application.interfaces;
using ClothingBrand.Application.Behaviours;
using ClothingBrand.Application.Common.Interfaces;
using ClothingBrand.Application.Contract;
using ClothingBrand.Application.Services;
using ClothingBrand.Application.Settings;
using ClothingBrand.Domain.Models;
using ClothingBrand.Infrastructure.DataContext;
using ClothingBrand.Infrastructure.Emails;
using ClothingBrand.Infrastructure.File;
using ClothingBrand.Infrastructure.Repository;
using infrastructure.Repos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(
                config.GetConnectionString("DefaultConnection")
                );
            });

            services.Configure<StripeSettings>(config.GetSection("Stripe"));

          //  services.AddIdentityCore<ApplicationUser>(opt=>opt.SignIn.RequireConfirmedEmail=true).AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddSignInManager();
          services.AddIdentity<ApplicationUser,IdentityRole>(options =>
          {
              options.SignIn.RequireConfirmedEmail = true;
          }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
         services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromDays(3); // Extend to 3 days
            });

            services.AddAuthentication(op =>
            {

                op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.SaveToken = true;
                opt.RequireHttpsMetadata = false;
                opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = config["Jwt:ValidIssuer"],
                    ValidAudience = config["Jwt:ValidAudiance"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Secret"]!))
                };



            });
            services.AddAuthentication();
            services.AddAuthorization();
            services.AddCors(options =>
            {
                options.AddPolicy("Clean", bul => bul.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials());
            });


            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IShoppingCartService, ShoppingCartService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<ICustomClothingOrderService, CustomClothingOrderService>();
            services.AddScoped<IOrderProcessingService, OrderProcessingService>();
            services.AddScoped<IAccount, AccountRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IDiscountRepository, DiscountRepository>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IEnrollmentCourseService, EnrollmentCourseService>();




            services.AddScoped<IcategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IDiscountService, DiscountService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddScoped<IFileService, FileService>();
            services.AddSingleton<IAppConfiguration, AppConfiguration>();
            services.Configure<MailSettings>(config.GetSection("MailSettings"));
            services.AddAuthentication().AddGoogle(option
                =>
            {
                IConfigurationSection googleAuthSection = config.GetSection("Auth:Google");
                option.ClientId = googleAuthSection["ClientId"];
                option.ClientSecret = googleAuthSection["ClientSecret"];
            });

            return services;
        }
    }
}
