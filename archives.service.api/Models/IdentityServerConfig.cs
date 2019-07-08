using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace archives.service.api.Models
{
    public class IdentityServerConfig
    {
        public string DefaultScheme { get; set; }

        public string Authority { get; set; }
    }
}
