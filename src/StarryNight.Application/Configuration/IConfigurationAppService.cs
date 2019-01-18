using System.Threading.Tasks;
using StarryNight.Configuration.Dto;

namespace StarryNight.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
