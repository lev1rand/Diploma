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

namespace TestTask
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("Default Connection");

            services.AddDbContext<TestContext>(options =>
                options.UseSqlServer(connection));

            services.AddTransient<IUnitOfWork, UnitOfWork>(e => new UnitOfWork(e.GetService<TestContext>()));
            services.AddTransient<ICodeService, CodeService>();
            services.AddTransient<IRepository<Code, int>, CodeRepository>();

            services.AddMvc().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateCodeValidator>());
            services.AddMvc().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<UpdateCodeValidator>());
            services.AddMvc().AddFluentValidation();

            services.AddControllers();
            
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new CodeMapperProfile());
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseCors();
        }
    }
}
