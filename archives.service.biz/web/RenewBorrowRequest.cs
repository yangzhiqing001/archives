using System;
using System.Collections.Generic;
using System.Text;

namespace archives.service.biz.web
{
    public class RenewBorrowRequest
    {
        public int BorrowRegisterId { get; set; }

        public DateTime RenewDate { get; set; }
    }

    public class ReturnBorrowRequest
    {
        public int BorrowRegisterId { get; set; }
    }

    public class ConfirmBorrowedRequest
    {
        public int BorrowRegisterId { get; set; }
    }
}
