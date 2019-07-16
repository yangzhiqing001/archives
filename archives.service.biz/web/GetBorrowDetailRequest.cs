using archives.service.dal.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace archives.service.biz.web
{ 
    /// <summary>
    /// 借阅详情入参
    /// </summary>
    public class GetBorrowDetailRequest
    {
        /// <summary>
        /// 借阅登记Id(借阅列表返回的Id)
        /// </summary>
        public int BorrowRegisterId { get; set; }
    }

    /// <summary>
    /// 借阅列表返回
    /// </summary>
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
