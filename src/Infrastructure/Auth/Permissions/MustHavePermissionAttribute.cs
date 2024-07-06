using MCH.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace MCH.Infrastructure.Auth.Permissions;

public class MustHavePermissionAttribute : AuthorizeAttribute
{
    public MustHavePermissionAttribute(string action, string resource) =>
        Policy = MCHPermission.NameFor(action, resource);
}