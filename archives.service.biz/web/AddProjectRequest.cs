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
        public string ProjectId { get; set; }
    }
}
