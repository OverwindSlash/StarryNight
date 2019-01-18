using System.Threading.Tasks;
using Abp.Application.Services;
using StarryNight.Authorization.Accounts.Dto;

namespace StarryNight.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
