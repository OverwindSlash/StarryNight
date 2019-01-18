using System;
using System.Collections.Generic;
using System.Text;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace StarryNight.Dapper
{
    [DependsOn(typeof(StarryNightCoreModule))]
    public class StarryNightDapperModule : AbpModule
    {
        public override void PreInitialize()
        {
            base.PreInitialize();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(StarryNightDapperModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            base.PostInitialize();
        }
    }
}
