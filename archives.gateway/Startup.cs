﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using archives.gateway.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace archives.gateway
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("ocelot.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
            {
                o.LoginPath = new PathString("/user/Login");
                o.AccessDeniedPath = new PathString("/user/Login");
                o.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                o.SlidingExpiration = true;
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


            var ids = Configuration.GetSection("IdentityServerConfig").Get<IdentityServerConfig>();
            services.AddAuthentication()
                .AddIdentityServerAuthentication(ids.AuthProviderKey, o =>
                {
                    o.Authority = ids.Authority;
                    o.ApiName = ids.ApiName;
                    o.SupportedTokens = SupportedTokens.Both;
                    o.ApiSecret = ids.ApiSecret;
                    o.RequireHttpsMetadata = false;
                });
            services.AddOcelot(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}

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

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=User}/{action=login}");
                routes.MapRoute(null,
                 "da/edit/{id}",
                 new { controller = "da", action = "edit" },
                 new { id = @"\d+" } // Constraint to only allow numbers
                );
            });

            app.UseErrorHandling();
            app.UseOcelot().Wait();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("");
            });
        }
    }
}
