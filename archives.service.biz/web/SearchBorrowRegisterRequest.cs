using System;
using System.Collections.Generic;
using System.Text;

namespace archives.service.biz.web
{
    public class SearchBorrowRegisterRequest : BaseRequest
    {
        public string Keyword { get; set; }
    }
}
