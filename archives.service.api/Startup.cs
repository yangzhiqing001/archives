using System;
using System.IO;
using archives.common;
using archives.service.api.Models;
using archives.service.biz;
using archives.service.dal;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace archives.service.api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ApplicationLog.Init();            
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            var ids = Configuration.GetSection("IdentityServerConfig").Get<IdentityServerConfig>();
            services.AddAuthentication(ids.DefaultScheme)
                .AddIdentityServerAuthentication(o =>
                {
                    o.Authority = ids.Authority;
                    o.RequireHttpsMetadata = false;
                });
            services.AddDbContext<ArchivesContext>(d => d.UseMySQL(Configuration.GetConnectionString("Default")));
            services.AddBizService();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "archives api",
                    Version = "v1",
                    Description = "",
                });

                var basePath = AppContext.BaseDirectory;
                var files = Directory.GetFiles(basePath, "*.xml");
                foreach (var filePath in files)
                {
                    c.IncludeXmlComments(filePath);
                }
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "archives api");
            });
        }
    }
}
