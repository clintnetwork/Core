using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TypeDB.Extensions.DependencyInjection.Cache;

namespace TypeDB.Samples
{
    public class Startup
    {
        public Instance Instance { get; }
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Instance = new TypeDB.Core(Mode.Standalone)
            .UsePersistence(new Persistence
            {
                Type = PersistenceType.Snapshot,
                Interval = TimeSpan.FromSeconds(10),
                Location = Persistence.TemporaryLocation
            })
            .Connect();
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTypeDBCache(Instance);
            
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseMvc();
        }
    }
}
