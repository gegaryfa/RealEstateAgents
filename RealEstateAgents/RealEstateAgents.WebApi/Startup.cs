using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

using RealEstateAgents.Application;
using RealEstateAgents.Infrastructure.Shared;
using RealEstateAgents.WebApi.Extensions;

namespace RealEstateAgents.WebApi
{
    public class Startup
    {
        public IConfiguration Config { get; }

        public Startup(IConfiguration configuration)
        {
            Config = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationLayer();
            services.AddSharedInfrastructure(Config);
            services.AddSwaggerExtension();
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    var converter = new StringEnumConverter(namingStrategy: new CamelCaseNamingStrategy());
                    options.SerializerSettings.Converters.Add(converter);
                });
            services.AddApiVersioningExtension();
            services.AddHealthChecks();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseRouting();

            app.UseSwaggerExtension();

            app.UseHealthChecks("/health");

            app.UseEndpoints(endpoints =>
             {
                 endpoints.MapControllers();
             });
        }
    }
}