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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace archives.gateway.Controllers
{
    public class UserController : Controller
    {
        private readonly string _identityServerClientId;
        private readonly string _identityServerSecret;

        public UserController(IConfiguration configuration)
        {
            _identityServerClientId = configuration.GetValue<string>("IdentityServerClientId");
            _identityServerSecret = configuration.GetValue<string>("IdentityServerSecret");
        }
        // GET: /<controller>/
        public IActionResult ChangePsd()
        {
            return View();
        }
        // GET: /<controller>/
        public IActionResult Login()
        {
            return View(new ErrorViewModel { RequestId = _identityServerClientId });
            //return View();
            //var parameters = new NameValueCollection();

            //parameters.Add("grant_type", "password");
            //parameters.Add("username", "yzq");
            //parameters.Add("password", "123");
            //parameters.Add("client_id", "bf0b2b168b37b7a5c3db403b57cab1f2");
            //parameters.Add("client_secret", "1eed954f309ce95e894cfd1dfb0c21e9");

            //using (var client = new WebClient())
            //{
            //    client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            //    var response_data = client.UploadValues("http://localhost:5001/token",
            //                             "POST",
            //                             parameters);
            //    JObject json = JObject.Parse(Encoding.UTF8.GetString(response_data));
            //    CookieOptions option = new CookieOptions();
            //    option.Expires = DateTime.Now.AddMinutes(10);
            //    Response.Cookies.Append("nnn", (string)json["access_token"], option);
            //    return View(new ErrorViewModel { RequestId =_localPath });
            //}
        }

        [HttpPost]
        public async Task<IActionResult> doLogin(IFormCollection forms)
        {
            var parameters = new NameValueCollection();

            parameters.Add("grant_type", "password");
            parameters.Add("username", forms["username"]);
            parameters.Add("password", forms["psd"]);
            parameters.Add("client_id", _identityServerClientId);
            parameters.Add("client_secret", _identityServerSecret);

            using (var client = new WebClient())
            {
                try {
                    client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    var response_data = client.UploadValues("http://localhost:5001/token",
                                             "POST",
                                             parameters);
                    var ret = Encoding.UTF8.GetString(response_data);
                    JObject json = JObject.Parse(ret);

                    //user.AuthenticationType = CookieAuthenticationDefaults.AuthenticationScheme;
                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim(ClaimTypes.Name, forms["username"]));
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                    return RedirectToAction("Upload", "da");

                    //CookieOptions option = new CookieOptions();
                    //option.Expires = DateTime.Now.AddMinutes(10);
                    //Response.Cookies.Append("nnn", (string)json["access_token"], option);
                }
                catch (Exception x) {
                    return View("Login");
                }

            }
        }

        public class LoginUser
        {
            public string UserName
            {
                get;
                set;
            }
            public string Psd
            {
                get;
                set;
            }
        }
    }
}
