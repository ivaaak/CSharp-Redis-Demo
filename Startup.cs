using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace Redis
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        // Add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var redis = ConnectionMultiplexer.Connect("localhost:6379");
            //Redis in a docker container on port 6379/tcp
            //[Environment]::SetEnvironmentVariable("REDIS_CONNECTIONSTRING", "localhost:6379", "Machine")
            services.AddScoped(s => redis.GetDatabase());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
