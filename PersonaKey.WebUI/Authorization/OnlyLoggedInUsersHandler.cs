using Microsoft.AspNetCore.Authorization;
using PersonaKey.BusinessLayer.Abstract;

namespace PersonaKey.WebUI.Authorization
{
    public class OnlyLoggedInUsersHandler : AuthorizationHandler<OnlyLoggedInUsersRequirement>
    {
        private readonly IAppUserService _appUserService;

        public OnlyLoggedInUsersHandler(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OnlyLoggedInUsersRequirement requirement)
        {
            var username = context.User.Identity?.Name;

            if (string.IsNullOrEmpty(username))
            {
                return;
            }

            var user = await _appUserService.GetByUsernameAsync(username);

            if (user?.Role?.RoleAccess?.CanLogin == true)
            {
                context.Succeed(requirement); // Authorized
            }
        }
    }
}
