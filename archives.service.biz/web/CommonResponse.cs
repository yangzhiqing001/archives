using System;
namespace archives.service.biz.web
{
    public class CommonResponse<T> where T : class
    {
        /// <summary>
        /// 响应时间
        /// </summary>
		public string ResponseTime { get; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        /// <summary>
        /// 返回对象
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 结果描述
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 错误码（http状态一般为正数，其它业务码应为负数）
        /// </summary>
        public int ErrorCode { get; set; }
    }
}
