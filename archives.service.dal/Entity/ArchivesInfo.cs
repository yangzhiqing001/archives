using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace archives.service.dal.Entity
{
    public class ArchivesInfo
    {
        [Key, Required]
        public string Id { get; set; }

        public int CategoryId { get; set; }

        public int FileNumber { get; set; }

        public string Title { get; set; }

        public string ProjectName { get; set; }
    }
}
