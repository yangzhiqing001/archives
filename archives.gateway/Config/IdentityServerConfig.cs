using System;

namespace archives.gateway.Config
{
    public class IdentityServerConfig
    {
        public string Authority { get; set; }

        public string ApiName { get; set; }

        public string ApiSecret { get; set; }

        public string AuthProviderKey { get; set; }
    }
}
