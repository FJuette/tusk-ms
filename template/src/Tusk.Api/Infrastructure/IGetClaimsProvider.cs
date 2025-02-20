using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace Tusk.Api.Infrastructure;
public interface IGetClaimsProvider
{
    string UserId { get; }
}

public class GetClaimsFromUser : IGetClaimsProvider
{
    [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>")]
    public GetClaimsFromUser(
        IHttpContextAccessor accessor)
    {
        var username = accessor.HttpContext?
            .User.Claims
            .SingleOrDefault(x => x.Type == ClaimTypes.Name)
            ?.Value;

        UserId = string.IsNullOrEmpty(username) ? "Admin" : username;
    }

    public string UserId { get; }
}
