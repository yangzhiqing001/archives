using System;
namespace archives.service.biz.web
{
    public class CommonResponse<T> where T : class
    {
		public string ResponseTime { get; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        public T Data { get; set; }

        public bool Success { get; set; }

        public string Message { get; set; }

        public int ErrorCode { get; set; }
    }
}
