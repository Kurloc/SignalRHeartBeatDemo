using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SignalRServer.Controllers;
using SignalRSharedModels;

namespace SignalRServer
{
    public class Startup
    {
        public const string MyAllowSpecificOrigins = "CorsPolicy";
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;

            // get file path for sqlite, this will be replaced by your typical DBContext setup with a server based DB.
            // this is done like so it makes it easier to drop in a real dbcontext later.
            var dbName = "test.db";
            var serverName = "SignalRServer";
            var filePath = env.ContentRootPath;
            int i = filePath.LastIndexOf(serverName, StringComparison.Ordinal);
            if (i >= 0)
                filePath = filePath.Substring(0, i) + filePath.Substring(i + serverName.Length);
            
            filePath = filePath.Replace(@"\\bin\Debug\net5.0", "");
            filePath = filePath.Replace(@"\bin\Debug\net5.0", "");
            filePath = filePath.Replace(@"\\bin\Debug\net5.0", "");
            Configuration["contentRoot"] = $@"{filePath}\{dbName}"; 
            Console.WriteLine(Configuration["contentRoot"]);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<HeartBeatModelContext>();
            services.AddScoped<IHeartBeatDbModelService, HeartBeatDbModelService>();
            
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    x => x
                        .AllowAnyHeader()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });
            services.AddControllers();

            services.AddSignalR(hubOptions =>
            {
                hubOptions.EnableDetailedErrors = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chatHub");
            });
        }
    }
}