using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using StarryNight.Configuration.Dto;

namespace StarryNight.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : StarryNightAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
