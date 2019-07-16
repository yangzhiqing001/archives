using System;
namespace archives.service.biz.web
{
    public class BaseRequest
    {
        /// <summary>
        /// 页码 (默认0开始)
        /// </summary>
        public int PageNumber { get; set; } = 0;

        /// <summary>
        /// 每页记录条数 （默认20）
        /// </summary>
        public int PageSize { get; set; } = 20;

    }
}
