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

            // ��� UmDbContext ����������ָ������������Ϊ scoped��ÿ�����󴴽�һ��ʵ����
            // ���� UmDbContext ���������ע�룬ʹ���ܹ�������Ӧ�ó����б�ע�뵽��������
            services.AddDbContext<UmDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("UmConnection")));

            // ע�� Repository �� Service

            /*
             * ע�� IUserRepository �������ʵ���� UserRepository
             * �� IUserRepository �ӿ�ע��Ϊ�������Ӧ�ľ���ʵ������ UserRepository��
             * ����������������Ҫ IUserRepository ʵ��ʱ��
             * ����ע�������Ḻ�𴴽� UserRepository ��ʵ�����ṩ�������ߡ�
             * 
             * �����services.AddScoped<IUserService, UserService>();ͬ��
             */
            services.AddScoped<IUserRepository, UserRepository>();


            // ע�� IUserService �������ʵ���� UserService
            services.AddScoped<IUserService, UserService>();


            // ��� Redis ����
            // ��ȡappsettings�е�����
            string redisConnectionString = _configuration.GetSection("RedisOptions")["ConnectionString"];
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));


            // ���������������
            // ������һ����Ϊ "AllowAnyOrigin" �� CORS ���ԣ������������κ�������������κ�����ͷ���ͷ�����
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
                // �����������д����쳣
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            // ͨ��ʹ�� UseCors ��������ָ����ǰ����� CORS �������ƣ��� CORS �м����ӵ�����ܵ��С�
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
