using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebAppPhotoSiteImages.Database;
using WebAppStatistic.Services;
using WebSitePublic.Common;

namespace WebAppStatistic
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
            StatAppSettings.AuthSrvUrl = Configuration["Auth:Url"];

            string sqlConnectionString = Configuration.GetConnectionString("StatAccessPostgreSqlProvider");

            services.AddDbContext<DbMgmtStat>(options =>
            options.UseNpgsql(
            sqlConnectionString));

            services.AddScoped<StatService>();

            services.AddMvc();

            services.AddAuthorization();

            var authorityUrl = StatAppSettings.AuthSrvUrl;

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = authorityUrl;
                    options.RequireHttpsMetadata = false;
                    options.Audience = authorityUrl + "/resources";
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        RoleClaimType = JwtClaimTypes.Role,
                        NameClaimType = JwtClaimTypes.Name
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
