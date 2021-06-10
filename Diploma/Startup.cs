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
using DiplomaServices.Mapping;
using DataAccess.Interfaces.Repositories;
using DiplomaServices.Services.AccountManagment;
using DiplomaServices.Interfaces;
using DiplomaServices.Services.TestServices;
using DiplomaServices.Services;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace Diploma
{
    public class Startup
    {
        private const int SESSION_EXPIRATION_TIME_IN_MINUTES = 160000;
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");

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
            services.AddTransient<IUsersCoursesRepository, UsersCoursesRepository>();
            services.AddTransient<IUsersTestsRepository, UsersTestsRepository>();

            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IJWTManagmentService, JWTManagmentService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ITestService, TestService>();
            services.AddTransient<ICourseService, CourseService>();
            services.AddTransient<IQuestionService, QuestionService>();
            services.AddTransient<IAnswersService, AnswersService>();

            services.AddCors();
            services.AddMvc()
                    .AddFluentValidation();


            var corsBuilder = new CorsPolicyBuilder();
            services.AddControllers()
                    .AddNewtonsoftJson();

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

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                   {
                     {
                        new OpenApiSecurityScheme
                 {
                           Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,

            },
                  new List<string>()
                    }
                });
            });
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

            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Diploma API");
                c.RoutePrefix = string.Empty;
            });
            app.UseStatusCodePages();
            app.UseRouting();
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseSession();
            app.Use(next => new RequestDelegate(
          async context =>
          {
              context.Request.EnableBuffering();
              await next(context);
          }
      ));
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}

