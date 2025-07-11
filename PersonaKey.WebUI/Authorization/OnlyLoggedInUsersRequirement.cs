using Microsoft.AspNetCore.Authorization;

namespace PersonaKey.WebUI.Authorization
{
    public class OnlyLoggedInUsersRequirement : IAuthorizationRequirement
    {
        // You can add additional requirements if needed
    }
}
