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
        public List<BorrowRegisterDetailSimple> ArchivesList { get; set; }
    }

    public class BorrowRegisterDetailSimple
    {
        /// <summary>
        /// ID (唯一值，用来做后续操作（编辑、借阅==）)
        /// </summary>
        public int Id { get; set; }
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
        public string CategoryNumber { get; set; }
        /// <summary>
        /// 分类号--app用了这个，所以有两个分类号
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

        /// <summary>
        /// 0 初使化（可删除） 1 正常可借阅状态 2 已借阅 (前端可根据状态值显示操作按钮)
        /// </summary>
        public ArchivesStatus Status { get; set; }

        public string StatusDesc
        {
            get
            {
                if (Status == ArchivesStatus.Init)
                    return "未借出";
                else if (Status == ArchivesStatus.Normal)
                    return "已归还";
                else if (Status == ArchivesStatus.Borrowed)
                    return "已借出";
                else
                    return "未知状态";
            }
        }
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
