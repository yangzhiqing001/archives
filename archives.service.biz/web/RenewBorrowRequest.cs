using System;
using System.Collections.Generic;
using System.Text;

namespace archives.service.biz.web
{
    /// <summary>
    /// 延期入参
    /// </summary>
    public class RenewBorrowRequest
    {
        /// <summary>
        /// 借阅登记Id(唯一值，也就是查询借阅列表及详情返回的Id)
        /// </summary>
        public int BorrowRegisterId { get; set; }

        /// <summary>
        /// 延期日期
        /// </summary>
        public DateTime RenewDate { get; set; }
    }

    /// <summary>
    /// 归还入参
    /// </summary>
    public class ReturnBorrowRequest
    {
        /// <summary>
        /// 借阅登记Id(唯一值，也就是查询借阅列表及详情返回的Id)
        /// </summary>
        public int BorrowRegisterId { get; set; }
    }

    /// <summary>
    /// 确认借出入参
    /// </summary>
    public class ConfirmBorrowedRequest
    {
        /// <summary>
        /// 借阅登记Id(唯一值，也就是查询借阅列表及详情返回的Id)
        /// </summary>
        public int BorrowRegisterId { get; set; }
    }
}
