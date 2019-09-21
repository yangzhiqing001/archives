using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using archives.service.biz.exp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace archives.service.api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ArchivesBaseController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        public string IdentityName
        {
            get
            {
                var identity = HttpContext.User.Identity;
                if (!identity.IsAuthenticated)
                {
                    throw new BizException("身份认证失败");
                }
                return identity.Name;
            }
        }
    }
}