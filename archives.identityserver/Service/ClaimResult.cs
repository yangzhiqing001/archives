using System;
using System.Security.Claims;
using IdentityModel;

namespace archives.identityserver.Service
{
    public class ClaimResult
    {
        public static Claim[] GetUserClaims(int userId, string userName)
        {
            return new Claim[]
            {
                new Claim(JwtClaimTypes.Id, userId.ToString()),
                new Claim(JwtClaimTypes.Name, userName)
            };
        }

    }
}
