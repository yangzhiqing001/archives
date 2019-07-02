using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using archives.identityserver.Config;
using IdentityServer4.Models;
using archives.identityserver.Service;

namespace archives.identityserver
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var ids = Configuration.GetSection("IdentityServerConfig").Get<IdentityServerConfig>();
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(new List<IdentityResource>
                {
                    new IdentityResources.OpenId(), //必须要添加，否则报无效的scope错误
                    new IdentityResources.Profile()
                })
                .AddInMemoryApiResources(new List<ApiResource>
                {
                    new ApiResource("api", "My API"),
                })
                .AddInMemoryClients(ids.Clients.ToIdentityModel())
                .AddExtensionGrantValidator<CustomGrantValidator>()
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                .AddProfileService<ProfileService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Map("/warmup", (t) => {
                t.Run(async context =>
                {
                    await context.Response.WriteAsync("ok");
                });
            });

            app.Map("/serverip", (t) => {
                t.Run(async context =>
                {
                    await context.Response.WriteAsync(context.Connection.LocalIpAddress.ToString());
                });
            });



            //app.UseSwaggerPage(SwaggerName);

            app.UseIdentityServer();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
