using System;
using System.Threading.Tasks;
using archives.identityserver.Config;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace archives.identityserver.Service
{
    public class CustomGrantValidator : IExtensionGrantValidator
    {
        private readonly ITokenValidator _validator;
        
        public CustomGrantValidator(ITokenValidator validator)
        {
            _validator = validator;
        }
        public string GrantType => ServerConfig.CustomGrantType;

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            await Task.Run(() =>
            {
                var code = context.Request.Raw.Get("code");//根据weixin code获取微信的 session_key 与 access_token
                var code_type = context.Request.Raw.Get("code_type");
                switch (code_type)
                {
                    case "weixin":
                        context.Result = null;
                        break;
                    case "admin":
                        context.Result = PasswordValidation.GetResult(context.Request.Raw);
                        break;
                    default:
                        context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, string.Format("错误的请求参数：code_type:{0}", code_type));
                        break;
                }
            });


        }
    }
}
