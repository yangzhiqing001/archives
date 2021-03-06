﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using archives.service.biz.web;
using archives.service.dal.Entity;

namespace archives.service.biz.ifs
{
    public interface IArchivesInfoService
    {
        Task<CommonSearchResponse<List<ArchivesSearchResult>>> SearchArchives(ArchivesSearchRequest request);

        Task<CommonResponse<ArchivesInfo>> GetArchives(int id);

        Task<List<string>> QueryAllProject();

        Task<CommonResponse<ArchivesEditResult>> Edit(ArchivesEditRequest archives);

        Task<CommonResponse<ArchivesAddResult>> Add(ArchivesAddRequest request);

        Task<CommonResponse<ArchivesDeleteResult>> Delete(ArchivesDeleteRequest request);

        Task<List<ArchivesInfo>> QueryAllArchives();

        Task<List<ArchivesInfo>> QueryExportArchives(ArchivesSearchRequest request);

        Task<CommonResponse<string>> ChangPassword(ChangePsdRequest request);

        Task<List<string>> QueryProject(string name);

        Task<List<CategoryResult>> QueryAllCategory();

        Task<int> AddProject(AddProjectRequest request);

        Task<int> DeleteProject(DeleteProjectRequest request);

        Task<int> AddCategory(AddCategoryRequest request);

        Task<int> DeleteCategory(DeleteCategoryRequest request);

        Task<List<GetProjectResult>> GetAllProject();
    }
}
