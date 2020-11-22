using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RedisDemo.Repository.Common;
using RedisDemo.Repository.Common.Helper;
using RedisDemo.Repository.Common.Interface;
using RedisDemo.Repository.Repository;
using RedisDemo.Repository.Repository.Interface;

namespace RedisDemo.WebAPI
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
            services.AddControllers();

            services.AddSingleton<IDatabaseConnectionHelper, AzureDbConnectionHelper>();
            services.AddSingleton<IRedisConnectionMultiplexer, RedisConnectionMultiplexer>();
            services.AddSingleton<IRedisCacheHelper, RedisCacheHelper>();
            services.AddTransient<IECommerceRepository, EcommerceRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
