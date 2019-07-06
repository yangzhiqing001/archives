using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using archives.service.biz.web;
using archives.service.dal.Entity;

namespace archives.service.biz.ifs
{
    public interface IArchivesInfoService
    {
        Task<CommonSearchResponse<List<ArchivesSearchResult>>> SearchArchives(ArchivesSearchRequest request);

        Task<CommonResponse<ArchivesDteailResult>> GetArchives(string archivesId);
    }
}
