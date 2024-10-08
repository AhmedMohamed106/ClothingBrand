using Application.interfaces;
using ClothingBrand.Application.Common.Interfaces;
using ClothingBrand.Application.Services;
using ClothingBrand.Domain.Models;
using ClothingBrand.Infrastructure.DataContext;
using ClothingBrand.Infrastructure.Emails;
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

namespace infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services,IConfiguration config)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(
                config.GetConnectionString("DefaultConnection")
                );
            });


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
                options.AddPolicy("Clean", bul => bul.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()) ;
            });
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAccount,AccountRepository>();
           services.AddScoped<IProductRepository,ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IDiscountRepository, DiscountRepository>();
            services.Configure<MailSettings>(config.GetSection("MailSettings"));

            return services;
        }
    }
}
