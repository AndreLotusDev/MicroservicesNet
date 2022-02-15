using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Discount.GRPC.Protos;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace Basket.API
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
            AddDefaultServices(services);
            AddCustomServices(services);
        }

        private void AddCustomServices(IServiceCollection services)
        {
            //GrpcConfiguration
            var grpcCuponCodeAddress = Configuration["GrpcSettings:DiscountUrl"];
            services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options => options.Address = new Uri(grpcCuponCodeAddress));
            services.AddScoped<DiscountGrpcService>();

            services.AddScoped<IBasketRepository, BasketRepository>();

            //REDIS IN CACHE configuration
            services.AddStackExchangeRedisCache(options =>
            {

                options.Configuration = Configuration.GetValue<string>("CacheSettings:RedisConnection");

            });

            //RabbitMQ configuration
            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((ctx, cfg) => 
                {
                    cfg.Host("amqp://guest:guest@localhost:5672");
                });
            });

            //AutoMapper
            //Assembly of startup
            services.AddAutoMapper(typeof(Startup));
        }

        private static void AddDefaultServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket.API v1"));
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
