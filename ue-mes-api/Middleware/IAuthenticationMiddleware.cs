using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using ue_mes_entities.Authorization;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace ue_mes_api.Middleware
{
    public interface IAuthenticationMiddleware
    {
        User Authenticate(Payload payload);
    }
}
