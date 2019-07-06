using System;
namespace archives.service.biz.web
{
    public class CommonSearchResponse<T> : CommonResponse<T> where T : class
    {
        public int TotalPage { get; set; }

        public int TotalCount { get; set; }


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
