using System;
using System.Collections.Generic;
using System.Text;

namespace archives.service.biz.web
{
    public class AddCategoryRequest
    {
        public int ParentId { get; set; }

        public int Level { get; set; }

        public string CategoryName { get; set; }
    }

    public class DeleteCategoryRequest
    {
        public int CategoryId { get; set; }

    }
}
