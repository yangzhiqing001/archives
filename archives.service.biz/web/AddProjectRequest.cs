using System;
using System.Collections.Generic;
using System.Text;

namespace archives.service.biz.web
{
    public class AddProjectRequest
    {
        public string ProjectName { get; set; }
    }

    public class DeleteProjectRequest
    {
        public int ProjectId { get; set; }
    }
}
