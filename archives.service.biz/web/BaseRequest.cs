using System;
namespace archives.service.biz.web
{
    public class BaseRequest
    {
        public int PageNumber { get; set; } = 0;

        public int PageSize { get; set; } = 20;

    }
}
