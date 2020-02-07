using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminApi.Data;
using AdminApi.Models.DataManager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AdminApi
{
    public class Startup
    {
        private const string _enableCrossOriginRequestsKey = "EnableCrossOriginRequests";

        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MainContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString(nameof(MainContext)));

                // Enable lazy loading.
                options.UseLazyLoadingProxies();
            });

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                // Make the session cookie essential.
                options.Cookie.IsEssential = true;
                options.IdleTimeout = TimeSpan.FromMinutes(1);
            });

            services.AddCors(options =>
                options.AddPolicy(_enableCrossOriginRequestsKey,
                builder => builder.WithOrigins("https://localhost:44349").AllowAnyHeader().AllowAnyMethod()));

            services.AddControllers();

            services.AddTransient<CustomerManager>();
            services.AddTransient<LoginManager>();
            services.AddTransient<BillPayManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseCors(_enableCrossOriginRequestsKey);

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
