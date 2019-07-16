using System;
namespace archives.service.biz.web
{
    public class AddFileResult
    {
        /// <summary>
        /// 文件唯一标识
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// 文件访问地址（用这个提交给其它接口）
        /// </summary>
        public string AccessUrl { get; set; }
    }
}
