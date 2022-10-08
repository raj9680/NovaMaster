using Imm.BLL;
using Imm.DAL.Data;
using JWT.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using NovaMaster.Controllers;
using NovaMaster.Controllers._Helpers;
using System;
using System.Text;

namespace NovaMaster
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddScoped<ServiceAccess, ServiceAccess>();
            services.AddScoped<ServiceClient, ServiceClient>();
            services.AddScoped<ServiceIdentity, ServiceIdentity>();
            services.AddScoped<ServiceCommon, ServiceCommon>();
            services.AddScoped<CommonController, CommonController>();
            services.AddHttpContextAccessor();
            services.AddScoped<_userClaims, _userClaims>();
            services.AddScoped<JwtAuthenticationManager, JwtAuthenticationManager>();
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
            });

            // services.AddTransient<NovaDbContext>();
            services.AddDbContext<NovaDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("TRMConnection")));

            /*
             *=========================================== 
             *           JWT Auth Regiteration
             *==========================================
             */

            var _jwtSetting = Configuration.GetSection("JwtSettings");
            services.Configure<JwtSettings>(_jwtSetting);


            var authkey = Configuration.GetValue<string>("JWTSettings:securitykey");
            
            services.AddAuthentication(item =>
            {
                item.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                item.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(item => {

                item.RequireHttpsMetadata = true;
                item.SaveToken = true;
                item.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authkey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };
            });

            /*
             *=========================================== 
             *         JWT Auth END
             *==========================================
             */
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();
            app.Use(async (context, next) =>
            {
                var token = context.Session.GetString("Token");
                if (!string.IsNullOrEmpty(token))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + token);
                }
                await next();
            });
            app.UseAuthentication(); // efor
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Access}/{action=Login}/{id?}");
            });
        }
    }
}
