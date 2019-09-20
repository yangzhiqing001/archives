using System;
namespace archives.gateway.Models
{
    public class LoginUserModel
    {
        public LoginUserModel()
        {

        }
        public LoginUserModel(LoginUserModel lum) {
            this.UserName = lum.UserName;
            this.Token = lum.Token;
        }
        public string UserName { get; set; }
        public string Token { get; set; }
    }
}
