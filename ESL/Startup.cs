using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ESL.DataAccess.Data;
using ESL.DataAccess.Repository.IRepository;
using ESL.DataAccess.Repository;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using ESL.DataAccess;
using ESL.Utility;
using Quartz.Spi;
using Quartz;
using Quartz.Impl;

namespace ESL
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
           /* services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("dbConnection")));*/

            services.AddDbContext<ApplicationDbContext>(options =>
          options.UseSqlServer(
              Configuration.GetConnectionString("dbConnection")).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            services.AddIdentity<IdentityUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                config.Password.RequiredLength = 2;
                config.SignIn.RequireConfirmedEmail = false;
                config.SignIn.RequireConfirmedPhoneNumber = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireDigit = true;
                config.Password.RequireUppercase = false;
                config.Password.RequireLowercase = true;

            })
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            services.AddSingleton<IReport, Report>();
            services.Configure<EmailOptions>(Configuration);
            services.AddControllersWithViews();
           
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<UnitPayrollInterface, UnitPayrollRepository>();
            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSession();
            services.AddRazorPages();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });

            services.AddHostedService<JobScheduler>();

            /*// Add Quartz services
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            // Add our job
            services.AddSingleton<HelloWorldJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(HelloWorldJob),
                cronExpression: "0/5 * * * * ?")); // run every 5 seconds*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.UseFastReport();
            app.UseAuthentication();
            app.UseAuthorization();

             app.UseEndpoints(endpoints =>
             {
                 endpoints.MapControllerRoute(
                     name: "default",
                     pattern: "{area=Admin}/{controller=Home}/{action=Index}/{id?}");
                 endpoints.MapRazorPages();
             });
            /*app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute().RequireAuthorization();
            });*/
        }
    }
}
