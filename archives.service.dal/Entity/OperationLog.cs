using System;
using System.Collections.Generic;
using System.Text;

namespace archives.service.dal.Entity
{
    public class OperationLog
    {
        public long Id { get; set; }

        /// <summary>
        /// 操作类型（关键字）
        /// </summary>
        public string OperationKey { get; set; }

        public OperationAction Action { get; set; }

        public string BeforeData { get; set; }

        public string AfterData { get; set; }
    }

    public enum OperationAction
    {
        Create = 0,
        Remove = 1,
        Update = 2,
        Delete = 3
    }
}
