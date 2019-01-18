using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using StarryNight.Authorization;

namespace StarryNight
{
    [DependsOn(
        typeof(StarryNightCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class StarryNightApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<StarryNightAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(StarryNightApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddProfiles(thisAssembly)
            );
        }
    }
}
