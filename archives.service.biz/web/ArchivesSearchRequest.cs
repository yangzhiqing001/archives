using System;
namespace archives.service.biz.web
{
    public class ArchivesSearchRequest : BaseRequest
	{
        public string Keyword { get; set; }

        /// <summary>
        /// 标签，（目前用在projectname上）
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 是否显示已借出（可为空）
        /// </summary>
        public bool? ShowBorrowed { get; set; }
    }

    public class ArchivesSearchResult
	{
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
	}
}
