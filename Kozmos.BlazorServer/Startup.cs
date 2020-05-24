using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Kozmos.BlazorServer.Areas.Identity;
using Kozmos.BlazorServer.Data;
using Kozmos.Data;
using Kozmos.Models;
using Microsoft.AspNetCore.Authentication;

namespace Kozmos.BlazorServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<KozmosDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );
            
            services.AddDefaultIdentity<KozmosUser>(options => 
                options.SignIn.RequireConfirmedAccount = true
            ).AddEntityFrameworkStores<KozmosDbContext>();
            
            services.AddRazorPages();
            
            services.AddServerSideBlazor();
            
            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<KozmosUser>>();
            
            services.AddSingleton<WeatherForecastService>();


            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "cookie";
                options.DefaultChallengeScheme = "oidc";
            }).AddCookie("cookie")
              .AddOpenIdConnect("oidc", options =>
              {
                  options.Authority = "http://localhost:5000";
                  options.RequireHttpsMetadata = false;

                  options.ClientId = "mvc";
                  options.ClientSecret = "secret";
                  options.ResponseType = "code";
                  options.UsePkce = true;

                  options.Scope.Add("openid");
                  options.Scope.Add("profile");
                  //options.Scope.Add("email");
                  //options.Scope.Add("personal");
                  //options.Scope.Add("janus");
                  options.Scope.Add("web_api");
                  //options.Scope.Add("api2");

                  options.ClaimActions.MapUniqueJsonKey("EmployeeID", "employee_id");
                  //options.ClaimActions.MapUniqueJsonKey("sox.pozi", "kasi");
                  //options.ClaimActions.MapUniqueJsonKey("sox.mom", "mother");
                  //options.ClaimActions.MapUniqueJsonKey("sox.dad", "father");

                  options.GetClaimsFromUserInfoEndpoint = true;

                  options.SignInScheme = "cookie";

                  options.SaveTokens = true;


              });
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
