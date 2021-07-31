using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_hero.Installers
{
    public static class InstallerExtensions
    {
        //สร้าง Method InstallServiceInAssembly ให้กับ Service
        public static void InstallServiceInAssembly(this IServiceCollection services, IConfiguration configuration)
        {
            // ค้นหาใน Assembly ที่มีการ Interface หรือ Abstact Class installers  ให้ทำการลงทะเบียน Service
            var installers = typeof(Startup).Assembly.ExportedTypes.Where(x =>
                typeof(IInstallers).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(Activator.CreateInstance).Cast<IInstallers>().ToList();

            foreach (var installer in installers)
            {
                installer.InstallServices(services, configuration);
            }
        }

        public static void ConfigureIISIntegration(this IServiceCollection services) =>
         services.Configure<IISOptions>(options =>
         {

         });
    }
}
