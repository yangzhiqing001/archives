using System;
namespace archives.service.biz.exp
{
    public class BizException : Exception
    {
        public BizException(string message) : base(message) { }
    }
}
