﻿using archives.service.dal.Entity;
using System;
namespace archives.service.biz.web
{
    /// <summary>
    /// 档案查询参数
    /// </summary>
    public class ArchivesSearchRequest : BaseRequest
	{
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 标签，（目前用在projectname上）
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 是否显示已借出（可为空）
        /// </summary>
        public bool? ShowBorrowed { get; set; }

        public string SEcho { get; set; }
    }

    /// <summary>
    /// 档案搜索结果 
    /// </summary>
    public class ArchivesSearchResult
	{
        /// <summary>
        /// ID (唯一值，用来做后续操作（编辑、借阅==）)
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
		/// 题名
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// 项目名称
		/// </summary>
		public string ProjectName { get; set; }

        public string OrderNumber { get; set; }

        /// <summary>
        /// 0 初使化（可删除） 1 正常可借阅状态 2 已借阅 (前端可根据状态值显示操作按钮)
        /// </summary>
        public ArchivesStatus Status { get; set; }

        public string StatusDesc { get {
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
}
