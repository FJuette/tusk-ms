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
            UserId = "Admin"; // Dummy value
            // TODO get id and optional more data from the user claims
            /*
            var user = accessor.HttpContext?
                .User;
            UserId = accessor.HttpContext?
                .User.Claims.SingleOrDefault(x =>
                    x.Type == ClaimTypes.Name)?.Value;
                    */
        }

        public string UserId { get; private set; }
    }
}
