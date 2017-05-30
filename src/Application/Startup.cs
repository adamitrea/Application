using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Application.Models;
using Application.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Application_DbAccess;
using Application_BusinessRules;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;

namespace Application
{
    public class Startup
    {
        private TelemetryClient telemetryClient = new TelemetryClient();

        public Startup(IHostingEnvironment env)
        {
            telemetryClient.InstrumentationKey = "10f4489b-25f0-4a02-aa9d-1879f936d81f";
            telemetryClient.TrackTrace($"Startup: {env.EnvironmentName}", SeverityLevel.Information);

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            //if (env.IsDevelopment())
            //{
            //    // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
            //    //builder.AddUserSecrets<Startup>();
            //    builder.AddUserSecrets();
                
            //}

            //if (env.EnvironmentName == "Publish")
            //{
            //    builder.AddUserSecrets();
            //}

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddDbContext<ApplicationContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

          
            services.AddIdentity<ApplicationUser, IdentityRole>(cfg =>
                {
                    
                })
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();
           
           // services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddMvc();

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());

            });

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddTransient<ICurrentUserService, CurrentUserService>();
            services.AddTransient<IAuthorizeMovieSet, AuthorizeMovieSet>();
            services.AddTransient<IAuthorizeMyMovie, AuthorizeMyMovie>();
            services.AddTransient<IAuthorizeMovie, AuthorizeMovie>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<RepositoryMyMovie, RepositoryMyMovie>();
            services.AddTransient<RepositoryMovieSet, RepositoryMovieSet>();
            services.AddTransient<RepositoryMovie, RepositoryMovie>();
            services.AddTransient<RepositoryTMDbGenre, RepositoryTMDbGenre>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, ApplicationContext _context)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();

            app.UseIdentity();

            var options = new RewriteOptions()
                    .AddRedirectToHttps();

            app.UseRewriter(options);

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715
            app.UseFacebookAuthentication(new FacebookOptions()
            {
                AppId = Configuration["Authentication:Facebook:AppId"],
                AppSecret = Configuration["Authentication:Facebook:AppSecret"]
            });

            app.UseGoogleAuthentication(new GoogleOptions()
            {
                ClientId = Configuration["Authentication:Google:ClientId"],
                ClientSecret = Configuration["Authentication:Google:ClientSecret"]
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=MovieSets}/{action=Index}/{id?}");
            });
            var userManager = app.ApplicationServices.GetService<UserManager<ApplicationUser>>();
            DbInitializer.Initialize(_context,userManager);
        }

    }
}
