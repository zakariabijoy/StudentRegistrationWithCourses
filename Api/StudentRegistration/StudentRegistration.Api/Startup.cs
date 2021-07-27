using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using StudentRegistration.Api.Mapper;
using StudentRegistration.DataAccess;
using Serilog;
using Microsoft.AspNetCore.Antiforgery;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System;
using StudentRegistration.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace StudentRegistration.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper((typeof(AutoMapperProfile).Assembly));
            services.AddCors();
            services.AddDataAccessServices();
            services.AddScoped<ITokenService, TokenService>();


            #region Jwt Authentication
            var tokenvalidaionParams = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,  //Gets or sets a boolean that controls if validation of the SecurityKey that signed the securityToken is called.
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokenkey"])),  // Gets or sets the SecurityKey that is to be used for signature validation.
                ValidateIssuer = false,  //Gets or sets a boolean to control if the issuer will be validated during token validation.
                ValidateAudience = false, // Gets or sets a boolean to control if the audience will be validated during token validation.
                ValidateLifetime = true,  //Gets or sets a boolean to control if the lifetime will be validated during token validation.
                ClockSkew = TimeSpan.Zero  //Gets or sets the clock skew to apply when validating a time.

            };

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = tokenvalidaionParams;
                });
            #endregion




            services.AddControllersWithViews();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "StudentRegistration.Api", Version = "v1" });
            });

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IAntiforgery antiforgery)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "StudentRegistration.Api v1"));
            }

            app.UseHttpsRedirection();
            app.UseSerilogRequestLogging();

            app.UseRouting();
            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
