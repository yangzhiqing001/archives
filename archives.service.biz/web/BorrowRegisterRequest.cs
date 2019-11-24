using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace archives.service.biz.web
{
    /// <summary>
    /// 借阅登记入参
    /// </summary>
    public class BorrowRegisterRequest
    {
        ///// <summary>
        ///// 档案Id(唯一值，也就是查询档案返回的Id)
        ///// </summary>
        //public List<int> ArchivesId { get; set; }

        /// <summary>
        /// 借阅明细
        /// </summary>
        public List<BorrowRegisterDetailRequest> Details { get; set; }

        /// <summary>
        /// 借阅人名
        /// </summary>
        [Required]
        public string Borrower { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        [Required]
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
        [Required]
        public DateTime ReturnDate { get; set; }

        /// <summary>
        /// 借阅日期
        /// </summary>
        public DateTime CreateTime { get; set; }

        public string CreateTimeStr { get; set; }

        /// <summary>
        /// 图片（用上传接口返回的图片地址）
        /// </summary>
        [Required]
        public string SignPhoto { get; set; }
    }

    public class BorrowRegisterDetailRequest
    {
        public int CategoryId1 { get; set; }

        public string CategoryName1 { get; set; }

        public int CategoryId2 { get; set; }

        public string CategoryName2 { get; set; }

        public int CategoryId3 { get; set; }

        public string CategoryName3 { get; set; }

        public int ProjectId { get; set; }

        public string ProjectName { get; set; }

        /// <summary>
        /// 档案号
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
        /// 题名
        /// </summary>
        public string Title { get; set; }
    }

    /// <summary>
    /// 借阅登记返回
    /// </summary>
    public class BorrowRegisterResult
    {
        /// <summary>
        /// 借阅登记ID
        /// </summary>
        public int BorrowRegisterId { get; set; }
    }
}
