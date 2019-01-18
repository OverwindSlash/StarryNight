using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using StarryNight.Roles.Dto;
using StarryNight.Users.Dto;

namespace StarryNight.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>
    {
        Task<ListResultDto<RoleDto>> GetRoles();

        Task ChangeLanguage(ChangeUserLanguageDto input);
    }
}
