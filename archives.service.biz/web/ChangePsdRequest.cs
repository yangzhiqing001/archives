using System;
namespace archives.service.biz.web
{
    /// <summary>
    /// 修改密码参数
    /// </summary>
    public class ChangePsdRequest : BaseRequest
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 旧密码
        /// </summary>
        public string OldPassword { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPassword { get; set; }
    }
}
