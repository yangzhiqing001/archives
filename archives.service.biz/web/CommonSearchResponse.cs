using System;
namespace archives.service.biz.web
{
    /// <summary>
    /// 分页查询结果 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommonSearchResponse<T> : CommonResponse<T> where T : class
    {
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPage { get; set; }

        /// <summary>
        /// 总行数
        /// </summary>
        public int TotalCount { get; set; }

        public string SEcho { get; set; }
    }

    public static class ResponseHelp
    {
        public static int GetPages(this int total, int pageSize)
        {
            return (total + pageSize - 1) / pageSize;
            //(int)Math.Ceiling((float)total / request.PageSize),
        }
    }
}
