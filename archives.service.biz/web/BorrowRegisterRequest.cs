using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace archives.service.biz.web
{
    public class BorrowRegisterRequest
    {
        public List<int> ArchivesId { get; set; }
        [Required]
        public string Borrwoer { get; set; }
        [Required]
        public string Phone { get; set; }

        public string Compnay { get; set; }

        public string Department { get; set; }

        [Required]
        public DateTime ReturnDate { get; set; }
        [Required]
        public string SignPhoto { get; set; }
    }

    public class BorrowRegisterResult
    {
        public int BorrowRegisterId { get; set; }
    }
}
