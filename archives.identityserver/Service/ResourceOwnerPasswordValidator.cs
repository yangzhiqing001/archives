using System;
using System.Threading.Tasks;
using IdentityServer4.Validation;

namespace archives.identityserver.Service
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public ResourceOwnerPasswordValidator()
        {

        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            //根据context.UserName和context.Password与数据库的数据做校验，判断是否合法
            await Task.Run(() =>
            {
                context.Result = PasswordValidation.GetResult(context.Request.Raw);
            });

        }
    }
}
