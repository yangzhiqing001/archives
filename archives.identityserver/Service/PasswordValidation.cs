﻿using System;
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
            var userName = nvc["username"];
            var password = nvc["password"];
            if (userName == "yzq" && password == "123")
            {
                return new GrantValidationResult(
                    subject: userName,
                    authenticationMethod: "password",
                    claims: ClaimResult.GetUserClaims(1, userName ?? string.Empty),
                    customResponse: new System.Collections.Generic.Dictionary<string, object>
                    {
                        { "Success", true },
                        { "Message", "管理员登录" }
                    }
                );
            }
            else
            {
                var error = new System.Collections.Generic.Dictionary<string, object>
                {
                    { "Success", false },
                    { "Message", "账号密码出错" }
                };
                return new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid custom credential", error);
            }
        }
    }
}
