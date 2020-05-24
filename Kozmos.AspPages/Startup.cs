using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication;
using Kozmos.Data;
using Kozmos.Models;

namespace Kozmos.AspPages
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
            services.AddMvc();

            services.AddDbContext<KozmosDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<KozmosUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<KozmosDbContext>()
                .AddDefaultTokenProviders();

            services.AddRazorPages();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "cookie";
                options.DefaultChallengeScheme = "oidc";
            }).AddCookie("cookie")
              .AddOpenIdConnect("oidc", options =>
              {
                  options.Authority = "https://localhost:5000";
                  options.RequireHttpsMetadata = true;

                  options.ClientId = "mvc";
                  options.ClientSecret = "secret";
                  options.ResponseType = "code";
                  options.UsePkce = true;

                  options.Scope.Clear();
                  options.Scope.Add("openid");
                  options.Scope.Add("profile");
                  options.Scope.Add("custom");
                  options.Scope.Add("offline_access");
                  options.Scope.Add("email");
                  options.Scope.Add("phone");
                  options.Scope.Add("location");
                  options.Scope.Add("companyDetails");
                  options.Scope.Add("api1");
                  options.Scope.Add("api1.admin");

                  // not mapped by default
                  options.ClaimActions.MapJsonKey("website", "website");

                  options.ClaimActions.MapUniqueJsonKey("MappedEmployeeID", "employee_id");
                  options.ClaimActions.MapUniqueJsonKey("MappedLocation", "location");
                  options.ClaimActions.MapUniqueJsonKey("MappedCity", "kasi");

                  options.ClaimActions.MapUniqueJsonKey("MappedCustomEmail", "custom.email");
                  options.ClaimActions.MapUniqueJsonKey("MappedCustomPhone", "custom.phone");

                  // keeps id_token smaller
                  options.GetClaimsFromUserInfoEndpoint = true;
                  options.SaveTokens = true;

                  options.SignInScheme = "cookie";

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
                endpoints.MapRazorPages();
            });
        }
    }
}
