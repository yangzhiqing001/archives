using System;
using System.Threading.Tasks;
using archives.service.dal;
using IdentityServer4.Validation;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using archives.common;
using IdentityServer4.Models;
using System.Security.Claims;
using IdentityModel;

namespace archives.identityserver.Service
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public ArchivesContext _db;
        public ResourceOwnerPasswordValidator(ArchivesContext db)
        {
            _db = db;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            //根据context.UserName和context.Password与数据库的数据做校验，判断是否合法
            var pwd = context.Password.MD5Hash();
            var admin = await _db.AdminUser.FirstOrDefaultAsync(c => c.UserName == context.UserName && c.Password == pwd);

            if (admin == null)
            {
                var error = new System.Collections.Generic.Dictionary<string, object>
                {
                    { "success", false },
                    { "message", "账号密码出错" }
                };
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid custom credential", error);
                return;
            }
            context.Result = new GrantValidationResult(
                subject: admin.UserName,
                authenticationMethod: "password",
                claims: new Claim[]
                {
                    new Claim(JwtClaimTypes.Id, admin.Id.ToString()),
                    new Claim(JwtClaimTypes.Name, admin.UserName),
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
    }
}
