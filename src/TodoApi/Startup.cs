using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using TodoApi.Models;

namespace MyFirstApp
{
    public class Startup
    {
        /// <summary>
        /// Start the Web App
        /// </summary>
        /// <param name="env"></param>
        public Startup(IHostingEnvironment env)
        {
            var root = env.ContentRootPath;
            Console.WriteLine("Content root is {0}", root);
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            var v = Configuration["MyName"];
            Console.WriteLine(v);
        }

        public IConfigurationRoot Configuration { get; }


        /// <summary>
        /// Configure services used in the application
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            var v = Configuration["MyName"];
            Console.WriteLine(v);

            // Add framework services.
            services.AddMvc();

            services.AddSingleton<ITodoRepository, TodoRemoteRepository>();

             v = Configuration["MyName"];
            Console.WriteLine(v);

            var w = Configuration["Swagger:Visible"];
            Console.WriteLine(w);

            var z = Configuration["Swagger:Path"];
            Console.WriteLine(z);



            Console.WriteLine("Test 0: {0}", Configuration["Logging"]);
            Console.WriteLine("Test 1: {0}", Configuration["Swagger"]);
            Console.WriteLine("Test 2: {0}", Configuration["Swagger:Path"]);


            // Inject a SwaggerProvider
            services.AddSwaggerGen();
            services.ConfigureSwaggerGen(options =>
            {
                options.SingleApiVersion(new Swashbuckle.Swagger.Model.Info
                {
                    Version = "v1",
                    Title = "DotNet Core investigation",
                    Description = "An attempt to simulate stuff using Dot Net Core",
                    TermsOfService = "All rigths reserved"
                });

                options.IncludeXmlComments(Configuration["Swagger:Path"]);
                options.DescribeAllEnumsAsStrings();
            });

            

            // services.AddDirectoryBrowser();
        }

        /// <summary>
        /// Configure the middleware in the application
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //if (env.IsDevelopment())
            //    app.UseDeveloperExceptionPage();
            //    app.UseBrowserLink();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //}

            // app.UseDirectoryBrowser()
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseMvcWithDefaultRoute();

            // Enable middleware to serve generated Swagger as a JSON endpoint, and
            // to serve swagger-UI assets
            app.UseSwagger();
            app.UseSwaggerUi();
        }
    }
}
