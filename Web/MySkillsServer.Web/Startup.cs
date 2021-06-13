namespace MySkillsServer.Web
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Reflection;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using MySkillsServer.Common;
    using MySkillsServer.Data;
    using MySkillsServer.Data.Common;
    using MySkillsServer.Data.Common.Repositories;
    using MySkillsServer.Data.Models;
    using MySkillsServer.Data.Repositories;
    using MySkillsServer.Data.Seeding;
    using MySkillsServer.Services.Data;
    using MySkillsServer.Services.Mapping;
    using MySkillsServer.Services.Messaging;
    using MySkillsServer.Web.Infrastructure.Middlewares.Authorization;
    using MySkillsServer.Web.Infrastructure.Settings;
    using MySkillsServer.Web.ViewModels;

    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly string allowSpecificOrigins = "AllowSpecificOrigins";

        // private readonly string corsAllowedUrl1 = "http://localhost:3000";
        // private readonly string corsAllowedUrl2 = "https://myskills-pd.web.app";
        // private readonly string corsAllowedUrl3 = "https://myskills.dotnetweb.net";
        private readonly string[] allowedDomains = new[]
        {
            "http://localhost:3000",
            "https://myskills-pd.web.app",
            "https://myskills.dotnetweb.net",
        };

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            // post configuration of the Identity Server Cookie:
            //  services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, option =>
            //  {
            //      option.Cookie.Name = GlobalConstants.JwtCookieName; // change cookie name

            // // option.Cookie.SameSite = SameSiteMode.None; // not needed
            // // option.ExpireTimeSpan = TimeSpan.FromSeconds(60); // change cookie expire time span - ignored by IdentityServer!
            //  });

            // Configure strongly typed settings objects
            var jwtSettingsSection = this.configuration.GetSection("JwtSettings");
            services.Configure<JwtSettings>(jwtSettingsSection);
            var facebookLoginSettingsSection = this.configuration.GetSection("FacebookLoginSettings");
            services.Configure<FacebookLoginSettings>(facebookLoginSettingsSection);

            // Configure JWT authentication services
            var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
            var jwtSecretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret));

            //// JWT Authentication services for 1 and 2
            services.Configure<TokenProviderOptions>(opts =>
            {
                opts.Audience = jwtSettings.Audience;
                opts.Issuer = jwtSettings.Issuer;
                opts.Path = "/api/accounts/login";
                opts.Expiration = TimeSpan.FromDays(7);
                opts.SigningCredentials = new SigningCredentials(jwtSecretKey, SecurityAlgorithms.HmacSha256);
            });

            services.AddAuthentication(options =>
                {
                    // JWT Authentication services 2 and 1 (when 1 is applied without cookie)
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddFacebook(options =>
                {
                    options.AppId = this.configuration["FacebookLoginSettings:AppId"];
                    options.AppSecret = this.configuration["FacebookLoginSettings:AppSecret"];
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = jwtSecretKey,
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtSettings.Audience,
                        ValidateLifetime = true,
                    };
                });

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                    .AddRoles<ApplicationRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>();

            //services.Configure<CookiePolicyOptions>(
            //    options =>
            //        {
            //            options.CheckConsentNeeded = context => true;
            //            options.MinimumSameSitePolicy = SameSiteMode.None;
            //        });

            services.AddCors(options =>
            {
                options.AddPolicy(
                                    name: this.allowSpecificOrigins,
                                    builder => builder
                                            // for a list with URLs:
                                            .WithOrigins(origins: this.allowedDomains)
                                            // for a specific URL:
                                            // .SetIsOriginAllowed((host) => { return host == this.corsAllowedUrl1; })
                                            // for all subdomeins:
                                            // .SetIsOriginAllowedToAllowWildcardSubdomains()
                                            // .WithOrigins("https://*.dotnetweb.net")
                                            .AllowAnyMethod()
                                            //.AllowCredentials() // to be able to send cookies to FE
                                            .AllowAnyHeader());
            });

            services.AddControllersWithViews(
                options =>
                    {
                        // options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    }).AddRazorRuntimeCompilation()
                    .AddXmlSerializerFormatters();

            services.AddRazorPages();

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddSingleton(this.configuration);

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IEducationsSeedService, EducationsSeedService>();
            services.AddTransient<IExperiencesSeedService, ExperiencesSeedService>();
            services.AddTransient<IContactsSeedService, ContactsSeedService>();
            services.AddTransient<IEmailSender, NullMessageSender>();
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<IEducationsService, EducationsService>();
            services.AddTransient<IExperiencesService, ExperiencesService>();
            services.AddTransient<IContactsService, ContactsService>();
            services.AddTransient<IAccountsService, AccountsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            //// Seed data on application startup
            //using (var serviceScope = app.ApplicationServices.CreateScope())
            //{
            //    var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            //    dbContext.Database.Migrate();
            //    new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            //}

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseCors(this.allowSpecificOrigins);

            app.UseAuthentication();
            app.UseAuthorization();

            // JWT Authentication services 2
            app.UseJwtBearerTokens(
                             app.ApplicationServices.GetRequiredService<IOptions<TokenProviderOptions>>(),
                             PrincipalResolver);

            app.UseEndpoints(
                endpoints =>
                    {
                        // without views and default loading page:
                        endpoints.MapControllers();

                        //// with view and default loading page:
                        // endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                        // endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                        // endpoints.MapRazorPages();
                    });
        }

        // JWT Authentication services 2
        private static async Task<GenericPrincipal> PrincipalResolver(HttpContext context)
        {
            var userManager = context.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
            var email = context.Request.Form["email"];
            var user = await userManager.FindByEmailAsync(email);
            if (user == null || user.IsDeleted)
            {
                return null;
            }

            var password = context.Request.Form["password"];

            var isValidPassword = await userManager.CheckPasswordAsync(user, password);
            if (!isValidPassword)
            {
                return null;
            }

            var roles = await userManager.GetRolesAsync(user);

            var identity = new GenericIdentity(email, "Token");
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));

            return new GenericPrincipal(identity, roles.ToArray());
        }
    }
}
