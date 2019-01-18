using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace StarryNight.Controllers
{
    public abstract class StarryNightControllerBase: AbpController
    {
        protected StarryNightControllerBase()
        {
            LocalizationSourceName = StarryNightConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
