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
        public IConfiguration Configuration { get; }

        public Instance Instance { get; }
        public Database Settings { get; }

        public Startup(IConfiguration configuration)
        {
            Instance = new TypeDB.Core(Mode.Standalone)
            .UsePersistence(new Persistence
            {
                Type = PersistenceType.Iteration,
                Location = Persistence.TemporaryLocation
            })
            .Connect();
            Settings = Instance.OpenDatabase("settings", true);

            Settings.Set("date_time", DateTime.Now);

            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Database>(Settings);

            // services.AddTypeDBCache(Instance);   // Under Construction

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
