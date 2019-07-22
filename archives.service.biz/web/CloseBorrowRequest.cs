using System;
namespace archives.service.biz.web
{
    public class CloseBorrowRequest
    {
        /// <summary>
        /// 借阅登记Id(唯一值，也就是查询借阅列表及详情返回的Id)
        /// </summary>
        public int BorrowRegisterId { get; set; }
    }
}
