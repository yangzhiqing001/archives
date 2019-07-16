using System;
using System.Collections.Generic;
using System.Text;

namespace archives.service.biz.web
{
    /// <summary>
    /// 借阅登记查询入参
    /// </summary>
    public class SearchBorrowRegisterRequest : BaseRequest
    {
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Keyword { get; set; }
    }
}
