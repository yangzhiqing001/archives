using System;
using System.Collections.Specialized;
using System.Security.Claims;
using archives.identityserver.Config;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace archives.identityserver.Service
{
    public class PasswordValidation
    {
        public static GrantValidationResult GetResult(NameValueCollection nvc)
        {
            var userName = nvc["username"];
            var password = nvc["password"];
            if (userName == "yzq" && password == "123")
            {
                return new GrantValidationResult(
                    subject: userName,
                    authenticationMethod: "password",
                    claims: new Claim[]
                    {
                        new Claim(JwtClaimTypes.Id, "1"),
                        new Claim(JwtClaimTypes.Name, userName),
                        new Claim(JwtClaimTypes.Role, "admin")
                    },
                    customResponse: new System.Collections.Generic.Dictionary<string, object>
                    {
                        { "success", true },
                        { "message", "管理员登录" },
                        { "ResponseTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                        { "ErrorCode", 0 },

                    }
                );
            }
            else
            {
                var error = new System.Collections.Generic.Dictionary<string, object>
                {
                    { "success", false },
                    { "message", "账号密码出错" }
                };
                return new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid custom credential", error);
            }
        }
    }
}
