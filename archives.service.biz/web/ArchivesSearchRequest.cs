using System;
namespace archives.service.biz.web
{
    public class ArchivesSearchRequest : BaseRequest
	{
        public string Keyword { get; set; }
    }

    public class ArchivesSearchResult
	{
		/// <summary>
		/// 档号
		/// </summary>
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
	}
}
