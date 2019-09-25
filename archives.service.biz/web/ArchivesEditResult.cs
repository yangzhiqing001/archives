using System;
namespace archives.service.biz.web
{
    public class ArchivesEditResult
    {
        
    }

    public class ArchivesEditRequest
    {
        /// <summary>
        /// 档案Id(唯一值，也就是查询档案返回的Id)
        /// </summary>
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
        
    }
}
