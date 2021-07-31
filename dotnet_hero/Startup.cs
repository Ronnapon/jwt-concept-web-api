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

        // Dependencies Injection เป็น Service ที่ .NET เตรียมไว้ให้เรียกใช้
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // มีการประกาศค่าแบบ Public
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        //ลงทะเบียน Service ติดต่อ Database, Auth, Sharing , UploadFile, Service กำหนดขึ้นเอง เป็นต้น
        public void ConfigureServices(IServiceCollection services)
        {
            services.InstallServiceInAssembly(Configuration);
            services.ConfigureIISIntegration();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            //User AutoFac ลงทะเบียน Service
            //services.AddTransient<IProductService, ProductService>();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // Middle ware จะทำการ Run ทีละส่วน ผ่านทีละประตู
        // 1 คฤหาส จะมีหลายประตู
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            // ถ้าเป็น Dev จะรัน Middle ware 3 ตัว
            if (env.IsDevelopment())
            {
                // Shoe Error แต่ไม่ควรโชว์บน Production
                app.UseDeveloperExceptionPage();

                // ใช้เฉพาะ Development Swagger
                app.UseSwagger();
                // สามารถสร้าหน้าจอ และดึง Document Json ของ Swagger
                // Endpoint คือเส้นทางไป Map กับ Controller
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
            // เส้นทาง
            app.UseRouting();
            app.UseCors("AllowSpecificOrigins");
            app.UseAuthentication();
            // ตรวจสอบสิทธิ์์
            app.UseAuthorization();
            // ประตูสุดท้าย เข้าไปหาใครสักคน
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
