using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace archives.service.dal.Entity
{
    /// <summary>
    /// 案卷
    /// </summary>
    public class ArchivesInfo : BaseEntity
    {
        /// <summary>
        /// 档号
        /// </summary>
        [Key, Required]
        public string ArchivesId { get; set; }

        /// <summary>
        /// 分类号
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// 案卷号
        /// </summary>
        public int FileNumber { get; set; }

        /// <summary>
        /// 题名
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get; set; }
        
        /// <summary>
        /// 总件数
        /// </summary>
        public int TotalNumber { get; set; }

        /// <summary>
        /// 起日期
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 止日期
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 保管期限（是否永久)
        /// </summary>
        [Column(TypeName = "bit")]
        public bool IsPermanent { get; set; }

        /// <summary>
        /// 密级
        /// </summary>
        public string SecretLevel { get; set; }

        /// <summary>
        /// 归档日期
        /// </summary>
        public DateTime ArchivingDate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 目录号
        /// </summary>
        public int CatalogNumber { get; set; }

        /// <summary>
        /// 提要
        /// </summary>
        public string Summary { get; set; }
    }
}
