using System;
namespace archives.service.biz.web
{
    /// <summary>
    /// 档案删除入参
    /// </summary>
    public class ArchivesDeleteRequest
    {
        /// <summary>
        /// 档案Id(唯一值，也就是查询档案返回的Id)
        /// </summary>
        public int ArchivesId { get; set; }
    }

    public class ArchivesDeleteResult
    {

    }
}
