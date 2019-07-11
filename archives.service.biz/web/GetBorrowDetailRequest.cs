using archives.service.dal.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace archives.service.biz.web
{
    public class GetBorrowDetailRequest
    {
        public int BorrowRegisterId { get; set; }
    }

    public class GetBorrowDetailResult
    {
        /// <summary>
        /// 借阅信息
        /// </summary>
        public BorrowRegister BorrowRegister { get; set; }

        /// <summary>
        /// 档案列表 （等同档案查询列表对象）
        /// </summary>
        public List<ArchivesSearchResult> ArchivesList { get; set; }
    }
}
