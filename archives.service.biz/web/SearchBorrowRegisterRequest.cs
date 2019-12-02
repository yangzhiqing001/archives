using System;
using System.Collections.Generic;
using System.Text;
using archives.service.dal.Entity;

namespace archives.service.biz.web
{
    /// <summary>
    /// 借阅登记查询入参
    /// </summary>
    public class SearchBorrowRegisterRequest : BaseRequest
    {
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Keyword { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string iSortCol_0 { get; set; }

        public string sSortDir_0 { get; set; }
    }

    public class SearchBorrowRegisterResult
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
        public string StatusDesc
        {
            get
            {
                if (Status == BorrowRegisterStatus.Normoal)
                    return "正常";
                else if (Status == BorrowRegisterStatus.Borrowed)
                    return "已借出";
                else if (Status == BorrowRegisterStatus.Closed)
                    return "已关闭";
                else if (Status == BorrowRegisterStatus.Overdue)
                    return "已逾期";
                else if (Status == BorrowRegisterStatus.Registered)
                    return "已登记";
                else if (Status == BorrowRegisterStatus.Renewed)
                    return "已延期";
                else if (Status == BorrowRegisterStatus.Returned)
                    return "已归还";
                else
                    return "未知状态";
            }
        }

        /// <summary>
        /// 借阅档案列表
        /// </summary>
        //public List<ArchivesSimple> ArchivesList { get; set; }


        public string ArchivesStr { get; set; }

        public string Receiver { get; set; }

        public string ProjectName { get; set; }
    }

    public class ArchivesSimple
    {
        public int BorrowRegisterId { get; set; }
        /// <summary>
        /// 档号
        /// </summary>

        public string ArchivesNumber { get; set; }

        /// <summary>
        /// 分类号
        /// </summary>
        public string CategoryId { get; set; }

        /// <summary>
        /// 案卷号
        /// </summary>
        public string FileNumber { get; set; }

        /// <summary>
        /// 卷内序号
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// 目录号
        /// </summary>
        public string CatalogNumber { get; set; }
    }
}
