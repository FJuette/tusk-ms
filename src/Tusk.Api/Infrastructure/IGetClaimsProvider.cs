using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Tusk.Api.Infrastructure
{
    public interface IGetClaimsProvider
    {
        string UserId { get; }
    }

    public class GetClaimsFromUser : IGetClaimsProvider
    {
        public GetClaimsFromUser(IHttpContextAccessor accessor)
        {
            var username = accessor.HttpContext?
                .User.Claims.SingleOrDefault(x =>
                    x.Type == ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                UserId = "Admin"; // Dummy value if no user id found in the jwt token from the request
            }
            else
            {
                UserId = username;
            }
        }

        public string UserId { get; private set; }
    }
}
