using System;
using System.Collections.Generic;

namespace archives.service.biz.web
{
    public class BorrowRegisterRequest
    {

        public List<int> ArchivesId { get; set; }

        public string Borrwoer { get; set; }

        public string Phone { get; set; }

        public DateTime ReturnDate { get; set; }

        public string SignPhoto { get; set; }
    }

    public class BorrowRegisterResult
    {
        public int BorrowRegisterId { get; set; }
    }
}
