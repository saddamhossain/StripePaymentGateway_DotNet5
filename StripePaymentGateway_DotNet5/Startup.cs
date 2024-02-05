using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stripe;
using StripePaymentGateway_DotNet5.Context;
using StripePaymentGateway_DotNet5.Interface;
using StripePaymentGateway_DotNet5.SeedData;
using StripePaymentGateway_DotNet5.Service;
using StripePaymentGateway_DotNet5.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripePaymentGateway_DotNet5
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            StripeConfiguration.ApiKey = configuration.GetValue<string>("Stripe:SecretKey");
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // Getting connection string from appsettings.json
            var connection = Configuration.GetConnectionString("DatabaseConnection");
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connection));

            services.AddTransient<IUser, UserService>();
            services.AddTransient<IPayment, PaymentService>();
            services.AddTransient<IBook, BookService>();
            services.AddScoped<IDbInitializer, DbInitializer>();

            services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbInitializer dbInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            dbInitializer.Initialize();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Book}/{action=Index}/{id?}");
            });
        }
    }
}
