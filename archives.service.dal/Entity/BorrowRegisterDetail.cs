using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace archives.service.dal.Entity
{
    public class BorrowRegisterDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int BorrowRegisterId { get; set; }

        public int ArchivesId { get; set; }

        public DateTime CreateTime { get; set; }

        public int ProjectId { get; set; }

        public string ProjectName { get; set; }

        public int CategoryId1 { get; set; }

        public string CategoryName1 { get; set; }

        public int CategoryId2 { get; set; }

        public string CategoryName2 { get; set; }

        public int CategoryId3 { get; set; }

        public string CategoryName3 { get; set; }

        /// <summary>
        /// 档案号
        /// </summary>
        public string ArchivesNumber { get; set; }

        /// <summary>
        /// 分类号
        /// </summary>
        public string CategoryNumber { get; set; }

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
}
