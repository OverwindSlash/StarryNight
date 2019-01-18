using System.Threading.Tasks;
using Abp.Application.Services;
using StarryNight.Sessions.Dto;

namespace StarryNight.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
