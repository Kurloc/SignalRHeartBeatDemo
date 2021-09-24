using System;
using System.IO;
using System.Net.Mime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SignalRSharedModels;

namespace DashboardWebserver
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            
            // get file path for sqlite, this will be replaced by your typical DBContext setup with a server based DB.
            // this is done like so it makes it easier to drop in a real dbcontext later.
            var dbName = "test.db";
            var serverName = "DashboardWebserver";
            var filePath = env.ContentRootPath;
            int i = filePath.LastIndexOf(serverName, StringComparison.Ordinal);
            if (i >= 0)
                filePath = filePath.Substring(0, i) + filePath.Substring(i + serverName.Length);

            Configuration["contentRoot"] = $@"{filePath}\{dbName}";
            Console.WriteLine(Configuration["contentRoot"]);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "DashboardWebserver", Version = "v1"});
            });

            services.AddDbContext<HeartBeatModelContext>();
            services.AddScoped<IHeartBeatDbModelService, HeartBeatDbModelService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DashboardWebserver v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}