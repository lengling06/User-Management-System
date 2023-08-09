using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UM.DataAccess;
using UM.IService;
using UM.Service;
using UM.IRepository;
using UM.Repository;
using StackExchange.Redis;

namespace um_asp.net_core
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "um_asp.net_core", Version = "v1" });
            });

            // 添加 UmDbContext 服务到容器，指定其生命周期为 scoped（每个请求创建一个实例）
            // 配置 UmDbContext 服务的依赖注入，使其能够在整个应用程序中被注入到其他类中
            services.AddDbContext<UmDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("UmConnection")));

            // 注册 Repository 和 Service

            /*
             * 注册 IUserRepository 服务和其实现类 UserRepository
             * 将 IUserRepository 接口注册为服务，其对应的具体实现类是 UserRepository。
             * 当其他类或控制器需要 IUserRepository 实例时，
             * 依赖注入容器会负责创建 UserRepository 的实例并提供给调用者。
             * 
             * 下面的services.AddScoped<IUserService, UserService>();同理
             */
            services.AddScoped<IUserRepository, UserRepository>();


            // 注册 IUserService 服务和其实现类 UserService
            services.AddScoped<IUserService, UserService>();


            // 添加 Redis 配置
            // 读取appsettings中的设置
            string redisConnectionString = _configuration.GetSection("RedisOptions")["ConnectionString"];
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));


            // 解决跨域请求问题
            // 创建了一个名为 "AllowAnyOrigin" 的 CORS 策略，它允许来自任何域的请求，允许任何请求头部和方法。
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "um_asp.net_core v1"));
            }
            else
            {
                // 在生产环境中处理异常
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            // 通过使用 UseCors 方法，并指定先前定义的 CORS 策略名称，将 CORS 中间件添加到请求管道中。
            app.UseCors("AllowAnyOrigin");


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
