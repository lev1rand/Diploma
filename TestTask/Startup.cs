using DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using TestTaskServices.Interfaces;
using TestTaskServices.Services;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using DataAccess.Entities;
using AutoMapper;
using TestTaskServices.Mapping;
using FluentValidation.AspNetCore;
using TestTaskServices.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using DataAccess.Authentification;
using TestTaskServices.Services.Interfaces;
using System;

namespace TestTask
{
    public class Startup
    {
        private const int SESSION_EXPIRATION_TIME_IN_MINUTES = 60;
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("Default Connection");

            services.AddDbContext<DataAccess.AppContext>(options =>
                options.UseSqlServer(connection));

            services.AddTransient<IUnitOfWork, UnitOfWork>(e => new UnitOfWork(e.GetService<DataAccess.AppContext>()));
            services.AddTransient<ICodeService, CodeService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IJWTManagmentService, JWTManagmentService>();
            services.AddTransient<IEmailVerificator, EmailVerificator>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRepository<User, int>, UserRepository>();
            services.AddTransient<IRepository<Code, int>, CodeRepository>();

            services.AddMvc().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateUserValidator>());
            services.AddMvc().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<UpdateUserValidator>());
            services.AddMvc().AddFluentValidation();

            services.AddControllers().AddNewtonsoftJson();

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new CodeMapperProfile());
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            // укзывает, будет ли валидироваться издатель при валидации токена
                            ValidateIssuer = true,
                            // строка, представляющая издателя
                            ValidIssuer = AuthOptions.ISSUER,

                            // будет ли валидироваться потребитель токена
                            ValidateAudience = true,
                            // установка потребителя токена
                            ValidAudience = AuthOptions.AUDIENCE,
                            // будет ли валидироваться время существования
                            ValidateLifetime = true,

                            // установка ключа безопасности
                            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                            // валидация ключа безопасности
                            ValidateIssuerSigningKey = true,
                        };
                    });

            services.AddControllersWithViews()
                    .AddNewtonsoftJson();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(SESSION_EXPIRATION_TIME_IN_MINUTES);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseCors();
        }
    }
}

