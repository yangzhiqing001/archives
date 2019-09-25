using System;
using System.Collections.Generic;

namespace archives.service.biz.web
{
    public class ConfirmUploadRequest
    {
        public ConfirmUploadRequest()
        {
        }

        public List<string> FileIds { get; set; }
    }

    public class ConfirmUploadResult
    {
        public List<string> ErrorList { get; set; }

        public int AddTotoal { get; set; }

        //public int UpdateTotal { get; set; }
    }
}
