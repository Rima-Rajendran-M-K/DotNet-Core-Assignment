using _1RestApiCRUDDemo.EmployeeData;
using _1RestApiCRUDDemo.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1RestApiCRUDDemo
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
            services.AddControllers();

            services.AddDbContextPool<EmployeeContext>(options => options.UseSqlServer(Configuration.GetConnectionString("EmployeeContextConnectionString")));

            services.AddScoped<IEmployeeData, SqlEmployeeData>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Use(async (context, next) => {
                await context.Response.WriteAsync("Incoming response from first middleware.\n");
                await next();
                await context.Response.WriteAsync("End of first middleware.\n");
            });

            app.Use(async (context, next) => {
                await context.Response.WriteAsync("Incoming response from second middleware.\n");
                await context.Response.WriteAsync("Incoming response from second middleware without using next() method.\n");
                await next();
                await context.Response.WriteAsync("End of second middleware.\n");
            });

            app.Run(async (context) => {
                await context.Response.WriteAsync("Incoming response handled and response generated.\n");
            });
        }

    }
}
