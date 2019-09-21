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
}
