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
        public BorrowRegisterSimple BorrowRegister { get; set; }

        /// <summary>
        /// 档案列表 （等同档案查询列表对象）
        /// </summary>
        public List<ArchivesSearchResult> ArchivesList { get; set; }
    }

    public class BorrowRegisterSimple
    {
        /// <summary>
        /// 借阅id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 借阅人
        /// </summary>
        public string Borrower { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 公司
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// 归还日期
        /// </summary>
        public DateTime ReturnDate { get; set; }

        public string ReturnDateStr { get; set; }

        /// <summary>
        /// 借阅日期
        /// </summary>
        public DateTime CreateTime { get; set; }

        public string CreateTimeStr { get; set; }

        /// <summary>
        /// 签名照片
        /// </summary>
        public string SignPhoto { get; set; }

        /// <summary>
        /// 状态 0.正常 1.已登记 2.已借出 3.已延期 4.已归还 5.逾期
        /// </summary>
        public BorrowRegisterStatus Status { get; set; }

    }
}
