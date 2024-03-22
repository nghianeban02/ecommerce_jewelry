using System;
using AspNetCoreHero.ToastNotification;
using ElectronicCommerce.Areas.Admin.Services;
using ElectronicCommerce.Areas.Customer.Services;
using ElectronicCommerce.MiddleWares;
using ElectronicCommerce.Models;
using ElectronicCommerce.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ElectronicCommerce
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddRazorRuntimeCompilation().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            // Toast
            services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.BottomRight; });


            // INJECT REPO
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            // INJECT SERVICE ADMIN
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICategoryProductService, CategoryProductService>();
            services.AddScoped<IOrderProductService, OrderProductService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ICategoryProductService, CategoryProductService>();
            services.AddScoped<ISaleReportService, SaleReportService>();

            // INJECT PRODUCT SERVICE
            services.AddScoped<IProductService, ProductService>();

            // Them xac thuc google va facebook
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
                .AddCookie()
                .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
                {
                    options.ClientId = _configuration["Authentication:Google:ClientId"];
                    options.ClientSecret = _configuration["Authentication:Google:ClientSecret"];
                    options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
                })
                .AddFacebook(facebookOptions =>
                {
                    facebookOptions.ClientId = _configuration["Authentication:Facebook:ClientId"];
                    facebookOptions.ClientSecret = _configuration["Authentication:Facebook:ClientSecret"];
                });

            // INJECT SERVICE CUSTOMER

            var connectionString = _configuration["ConnectionStrings:DefaultConnection"].ToString();
            services.AddDbContext<DatabaseContext>(option => option.UseLazyLoadingProxies().UseSqlServer(connectionString));

            // Add session service
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(240);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                //options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSession();

            //app.UseMiddleware<AdminMiddleware>();
            //app.UseMiddleware<CustomerMiddleware>();

            app.UseRouting();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller}/{action}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}/{id?}");
            });
        }
    }
}
