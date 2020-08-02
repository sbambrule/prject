using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenticationService.Models;
using AuthenticationService.Repository;
using AuthenticationService.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using AuthenticationService.Exceptions;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AuthenticationService.Filters;

namespace AuthenticationService
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
            //Register DbContext with connection string read from configuration
            //Register all dependencies here
            ValidateToken(Configuration, services);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddTransient<IAuthRepository, AuthRepository>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<ITokenGenerator, TokenGenerator>();
            services.AddTransient(typeof(AuthDbContext));
            services.AddDbContext<AuthDbContext>(options =>
                    options.UseSqlServer(Environment.GetEnvironmentVariable("SQL_DB_CON")));
              
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "API", Version = "v1" });
                c.OperationFilter<AddAuthorizationHeaderParameterOperationFilter>();

            });
            services.AddCors(options =>
            {
                options.AddPolicy("AllowMyOrigin",
                builder => builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            } 
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");

            });
            app.UseAuthentication();

            app.UseMvc();
            app.UseCors("AllowMyOrigin");
        }
        private void ValidateToken(IConfiguration configuration, IServiceCollection serviceCollection)
        {
            var audienceconfig = configuration.GetSection("Audience");
            var key = audienceconfig["key"];
            var keyByteArray = Encoding.ASCII.GetBytes(key);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var tokenValidationParamters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                ValidateIssuer = true,
                ValidIssuer = audienceconfig["lss"],

                ValidateAudience = true,
                ValidAudience = audienceconfig["Aud"],

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
            };
            serviceCollection.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = tokenValidationParamters;
            });

        }
    }
}
