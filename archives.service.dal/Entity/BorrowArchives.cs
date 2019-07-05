using System;
using System.Collections.Generic;
using System.Text;

namespace archives.service.dal.Entity
{
    public class BorrowArchives
    {
        public int Id { get; set; }

        public int BorrowRegisterId { get; set; }

        public string ArchivesId { get; set; }

    }
}
