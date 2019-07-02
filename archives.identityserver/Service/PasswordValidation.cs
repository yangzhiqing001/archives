using System;
using System.Collections.Specialized;
using archives.identityserver.Config;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace archives.identityserver.Service
{
    public class PasswordValidation
    {
        public static GrantValidationResult GetResult(NameValueCollection nvc)
        {
            var userName = nvc["name"];
            if (userName == "yzq" && nvc["password"] == "123")
            {
                return new GrantValidationResult(
                        subject: userName,
                        authenticationMethod: ServerConfig.CustomGrantType,
                        claims: ClaimResult.GetUserClaims(1)
                    );
            }
            else
            {

                //验证失败
                return new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid custom credential");
            }

        }
    }
}
