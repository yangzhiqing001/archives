using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace archives.service.dal.Entity
{
    public class OperationLog
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 操作类型（关键字）
        /// </summary>
        public string OperationKeyword { get; set; }

        public OperationAction Action { get; set; }

        public string BeforeData { get; set; }

        public string AfterData { get; set; }

        public DateTime CreateTime { get; set; }
    }

    public enum OperationAction
    {
        Query = 0,
        Create = 1,
        Remove = 2,
        Update = 3,
        Delete = 4
    }
}
