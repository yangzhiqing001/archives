using System;
namespace archives.service.biz.web
{
    public class ArchivesAddResult
    {
        /// <summary>
        /// 档案ID，唯一值
        /// </summary>
        public int Id { get; set; }
    }
    /// <summary>
    /// 添加档案入参
    /// </summary>
    public class ArchivesAddRequest
    {
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
        public DateTime WrittenDate { get; set; }

        /// <summary>
        /// 页数
        /// </summary>
        public int Pages { get; set; }

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
        public DateTime ArchivingDate { get; set; }

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
    }
}
