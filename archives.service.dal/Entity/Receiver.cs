using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace archives.service.dal.Entity
{
    public class Receiver
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ReceiverName { get; set; }
    }
}
