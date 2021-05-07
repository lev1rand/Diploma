using DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using DataAccess.Repositories;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using DataAccess.Authentification;
using System;
using DiplomaServices.Services.Interfaces;
using DiplomaServices.Mapping;
using DataAccess.Interfaces.Repositories;
using DiplomaServices.Services.AccountManagment;

namespace Diploma
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
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ICourseRepository, CourseRepository>();
            services.AddTransient<IQuestionRepository, QuestionRepository>();
            services.AddTransient<IResponseOptionRepository, ResponseOptionRepository>();
            services.AddTransient<IRightSimpleAnswerRepository, RightSimpleAnswerRepository>();
            services.AddTransient<ITestRepository, TestRepository>();
            services.AddTransient<IUserAnswerRepository, UserAnswerRepository>();

            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IJWTManagmentService, JWTManagmentService>();
            services.AddTransient<IEmailVerificator, EmailVerificator>();
            services.AddTransient<IUserService, UserService>();


            services.AddMvc().AddFluentValidation();

            services.AddControllers().AddNewtonsoftJson();

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperProfile());
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

