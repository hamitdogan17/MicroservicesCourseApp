using Basket.Api.GrpcServices;
using Basket.Api.Repositories;
using Basket.Api.Repositories.Interfaces;
using Discount.Grpc.Protos;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace Basket.Api
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
            //Redis configuration
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetValue<string>("CacheSettings:ConnectionString");
            });

            services.AddControllers();

            // General Configuration 
            services.AddTransient<IBasketRepository, BasketRepository>();
            services.AddAutoMapper(typeof(Startup));

            // Grpc Configuration 
            services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>
                (o => o.Address = new Uri(Configuration["GrpcSettings:DiscountUrl"]));
            services.AddScoped<DiscountGrpcService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket API", Version = "v1" });
            });

            // MassTransit-RabbitMq Configuration 
            services.AddMassTransit(config => {
                config.UsingRabbitMq((ctx, cfg0) => {
                    cfg0.Host(Configuration["EventBusSettings:HostAddress"]);
                });
            });
            services.AddMassTransitHostedService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket API V1");
            });
        }
    }
}
