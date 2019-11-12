using System;
using System.Collections.Generic;

namespace archives.service.biz.web
{
    public class CategoryResult
    {
        public int Id { get; set; }

        public string CategoryName { get; set; }

        public int Level { get; set; }

        public List<CategoryResult> Children { get; set; }
    }
}
