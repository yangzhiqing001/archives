using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace archives.service.dal.Entity
{
    public class BorrowRegisterDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int BorrowRegisterId { get; set; }

        public int ArchivesId { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
