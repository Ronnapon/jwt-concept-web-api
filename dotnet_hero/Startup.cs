using Autofac;
using dotnet_hero.Data;
using dotnet_hero.Installers;
using dotnet_hero.Interfaces;
using dotnet_hero.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace dotnet_hero
{
    public class Startup
    {

        // Dependencies Injection �� Service ��� .NET ��������������¡��
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // �ա�û�С�Ȥ��Ẻ Public
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        //ŧ����¹ Service �Դ��� Database, Auth, Sharing , UploadFile, Service ��˹�����ͧ �繵�
        public void ConfigureServices(IServiceCollection services)
        {
            services.InstallServiceInAssembly(Configuration);
            services.ConfigureIISIntegration();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            //User AutoFac ŧ����¹ Service
            //services.AddTransient<IProductService, ProductService>();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // Middle ware �зӡ�� Run ������ǹ ��ҹ���л�е�
        // 1 ����� �������»�е�
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            // ����� Dev ���ѹ Middle ware 3 ���
            if (env.IsDevelopment())
            {
                // Shoe Error ���������캹 Production
                app.UseDeveloperExceptionPage();

                // ��੾�� Development Swagger
                app.UseSwagger();
                // ����ö����˹�Ҩ� ��д֧ Document Json �ͧ Swagger
                // Endpoint �����鹷ҧ� Map �Ѻ Controller
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "dotnet_hero v1"));
            }
            else
            {
                app.UseHsts();
            }

            // http to https
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });
            // ��鹷ҧ
            app.UseRouting();
            app.UseCors("AllowSpecificOrigins");
            app.UseAuthentication();
            // ��Ǩ�ͺ�Է����
            app.UseAuthorization();
            // ��е��ش���� ���������ѡ��
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
