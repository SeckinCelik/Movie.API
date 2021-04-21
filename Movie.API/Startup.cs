using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Movie.API.Authorization;
using Movie.API.BackgroundJobs;
using Movie.API.HostedServices;
using Movie.API.HostedServices.Quartz;
using Movie.Core.Model.MovieService;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System.Security.Claims;

namespace Movie.API
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
            AppConfiguration appConfiguration = new AppConfiguration();
            Configuration.GetSection("AppConfiguration").Bind(appConfiguration);
            services.AddSingleton<AppConfiguration>(appConfiguration);
            services.AddSingleton<Services.IMovieService, Services.MovieService>();
            services.AddSingleton<Services.IEmailService, Services.EmailService>();

            services.AddSwaggerGen(swagger =>
            {                
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "JWT Token Authentication API",
                    Description = "ASP.NET Core 3.1 Web API"
                });
                // To Enable authorization using Swagger (JWT)  
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });
            });

            services.AddControllers();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = "https://dev-uyycstli.us.auth0.com/";
                options.Audience = "movie-recommendation-api";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier
                };
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("read:movies", policy => policy.Requirements.Add(new HasScopeRequirement("read:movies", "https://dev-uyycstli.us.auth0.com/")));
                options.AddPolicy("write:movies", policy => policy.Requirements.Add(new HasScopeRequirement("write:movies", "https://dev-uyycstli.us.auth0.com/")));
            });
            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            services.AddSingleton(new JobSchedule(
                    jobType: typeof(MovieFetchJob),
                    cronExpression: "*/1 * * * * ?"));

            services.AddHostedService<QuartzHostedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
