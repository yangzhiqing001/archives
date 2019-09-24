using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using archives.gateway.Models;
using archives.common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace archives.gateway.Controllers
{
    public class UserController : BaseController
    {
        private readonly string _identityServerClientId;
        private readonly string _identityServerSecret;

        public UserController(IConfiguration configuration)
        {
            _identityServerClientId = configuration.GetValue<string>("IdentityServerClientId");
            _identityServerSecret = configuration.GetValue<string>("IdentityServerSecret");
        }
        // GET: /<controller>/
        [Authorize]
        public IActionResult ChangePsd()
        {
            return View(getUser());
        }
        // GET: /<controller>/
        public IActionResult Login()
        {
            LoginViewModel lv = new LoginViewModel();
            bool rm = false;
            
            if (Request.Cookies["rememberme"] != null) {
                bool.TryParse(Request.Cookies["rememberme"], out rm);
                lv.RememberMe = rm;
            }
            return View(lv);
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            //return RedirectToAction("Index", "Home");
            return View("login");
        }

        [HttpPost]
        public async Task<IActionResult> doLogin(LoginViewModel model)
        {
            var parameters = new NameValueCollection();

            parameters.Add("grant_type", "password");
            parameters.Add("username", model.UserName);
            parameters.Add("password", model.Password);
            parameters.Add("client_id", _identityServerClientId);
            parameters.Add("client_secret", _identityServerSecret);

            using (var client = new WebClient())
            {
                try {
                    client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

                    string myurl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";

                    var response_data = client.UploadValues(myurl + "/token",
                                             "POST",
                                             parameters);
                    var ret = Encoding.UTF8.GetString(response_data);
                    JObject json = JObject.Parse(ret);

                    //user.AuthenticationType = CookieAuthenticationDefaults.AuthenticationScheme;
                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    LoginUserModel lm = new LoginUserModel { UserName = model.UserName, Token = (string)json["access_token"] };
                    identity.AddClaim(new Claim(ClaimTypes.Name, JsonHelper.Serialize(lm)));
                    AuthenticationProperties ap = null;
                    if (model.RememberMe) {
                        ap = new AuthenticationProperties
                        {
                            IsPersistent = model.RememberMe,
                            ExpiresUtc = DateTime.UtcNow.AddDays(1)
                        };
                    }
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), ap);
                    return RedirectToAction("manage", "da");
                }
                catch (Exception x) {
                    return View("Login");
                }

            }
        }
    }
}
