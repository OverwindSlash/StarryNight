using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using StarryNight.Configuration;

namespace StarryNight.Web.Host.Startup
{
    [DependsOn(
       typeof(StarryNightWebCoreModule))]
    public class StarryNightWebHostModule: AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public StarryNightWebHostModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(StarryNightWebHostModule).GetAssembly());
        }
    }
}
