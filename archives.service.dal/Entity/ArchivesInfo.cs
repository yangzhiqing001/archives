using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace archives.service.dal.Entity
{
    /// <summary>
    /// 案卷内文件
    /// </summary>
    public class ArchivesInfo : BaseEntity
    {
        /// <summary>
        /// ID 维一值
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
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
        /// 题名
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 责任者
        /// </summary>
        public string ResponsibleObject { get; set; }

        /// <summary>
        /// 成文日期
        /// </summary>
        public string WrittenDate { get; set; }

        /// <summary>
        /// 页数
        /// </summary>
        public string Pages { get; set; }

        /// <summary>
        /// 保管期限（是否永久)
        /// </summary>
        public string IsPermanent { get; set; }

        /// <summary>
        /// 密级
        /// </summary>
        public string SecretLevel { get; set; }

        /// <summary>
        /// 归档部门
        /// </summary>
        public string ArchivingDepartment { get; set; }

        /// <summary>
        /// 归档日期
        /// </summary>
        public string ArchivingDate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 目录号
        /// </summary>
        public string CatalogNumber { get; set; }

        /// <summary>
        /// 提要
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 0 初使化（可删除） 1 正常可借阅状态 2 已借阅 (前端可根据状态值显示操作按钮)
        /// </summary>
        public ArchivesStatus Status { get; set; }
    }

    /// <summary>
    /// 档案状态
    /// </summary>
    public enum ArchivesStatus
    {
        /// <summary>
        /// 0 初使化（可删除）
        /// </summary>
        Init = 0,
        /// <summary>
        /// 1 正常可借阅状态
        /// </summary>
        Normal = 1,
        /// <summary>
        /// 2 已借阅
        /// </summary>
        Borrowed = 2,
    }
}
