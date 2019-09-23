using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace archives.service.dal.Entity
{
    public class FileStorage
    {
        [Key]
        public string Id { get; set; }

        public FileStorageType StorageType { get; set; }

        public string StoragePath { get; set; }

        public string AccessUrl { get; set; }

        public string ContentType { get; set; }

        public long Size { get; set; }

        public DateTime CreateTime { get; set; }

        public FileStorageBizType? BizType { get; set; }

        public string OriginalFileName { get; set; }
    }

    public enum FileStorageType
    {
        Local = 0,
        AliOss = 1,
    }

    public enum FileStorageBizType
    {
        SignImage = 0,
        ArchivesExcel = 1,
    }
}
