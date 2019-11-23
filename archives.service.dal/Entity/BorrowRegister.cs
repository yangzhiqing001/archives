using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace archives.service.dal.Entity
{
    public class BorrowRegister : BaseEntity
    {
        /// <summary>
        /// 借阅登记Id(唯一值，用在查看详情，以及其它确认借阅，归还等接口中)
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

        /// <summary>
        /// 签名照片
        /// </summary>
        public string SignPhoto { get; set; }

        /// <summary>
        /// 状态 0.正常 1.已登记 2.已借出 3.已延期 4.已归还 5.逾期
        /// </summary>
        public BorrowRegisterStatus Status { get; set; }

        /// <summary>
        /// 是否已归还并通知
        /// </summary>
        [Column(TypeName = "bit")]
        public bool? ReturnNotified { get; set; }

        /// <summary>
        /// 通知次数
        /// </summary>
        public int? NotifyCount { get; set; }

        /// <summary>
        /// 归还人
        /// </summary>
        public string Receiver { get; set; }
    }

    /// <summary>
    /// 借阅状态
    /// </summary>
    public enum BorrowRegisterStatus
    {
        /// <summary>
        /// 0.正常
        /// </summary>
        Normoal = 0,
        /// <summary>
        /// 1.已登记
        /// </summary>
        Registered = 1,

        /// <summary>
        /// 2.已借出
        /// </summary>
        Borrowed = 2,

        /// <summary>
        /// 3.已延期
        /// </summary>
        Renewed = 3,

        /// <summary>
        /// 4.已归还
        /// </summary>
        Returned = 4,

        /// <summary>
        /// 5.逾期
        /// </summary>
        Overdue = 5,

        /// <summary>
        /// 已关闭
        /// </summary>
        Closed = 6,
    }
}
