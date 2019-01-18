using Abp.Application.Services;
using Abp.Application.Services.Dto;
using StarryNight.MultiTenancy.Dto;

namespace StarryNight.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

