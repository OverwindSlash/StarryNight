using Abp.Authorization;
using StarryNight.Authorization.Roles;
using StarryNight.Authorization.Users;

namespace StarryNight.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
