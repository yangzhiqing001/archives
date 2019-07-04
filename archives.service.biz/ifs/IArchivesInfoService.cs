using System;
using System.Collections.Generic;
using archives.service.dal.Entity;

namespace archives.service.biz.ifs
{
    public interface IArchivesInfoService
    {
        List<ArchivesInfo> GetList();
    }
}
