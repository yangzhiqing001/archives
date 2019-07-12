using System;
namespace archives.identityserver.Config
{
    public class ClientConfig
    {
        public string Scope { get; set; }
        public string ClientId { get; set; }

        public string GrantType { get; set; }

        public string Secret { get; set; }

        private int _identityTokenLifetime;
        /// <summary>
        /// Lifetime of identity token in seconds (defaults to 300 seconds / 5 minutes)
        /// </summary>
        public int IdentityTokenLifetime
        {
            get
            {
                if (_identityTokenLifetime == 0)
                    _identityTokenLifetime = 300;
                return _identityTokenLifetime;
            }
            set
            {
                _identityTokenLifetime = value;
            }
        }

        private int _accessTokenLifetime;

        /// <summary>
        /// Lifetime of access token in seconds (defaults to 3600 seconds / 1 hour)
        /// </summary>
        public int AccessTokenLifetime
        {
            get
            {
                if (_accessTokenLifetime == 0)
                    _accessTokenLifetime = 3600;
                return _accessTokenLifetime;
            }
            set
            {
                _accessTokenLifetime = value;
            }
        }
    }
}
