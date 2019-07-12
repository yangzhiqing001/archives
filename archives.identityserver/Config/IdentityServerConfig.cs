using System;
using System.Collections.Generic;
using System.Linq;
using IdentityServer4;
using IdentityServer4.Models;

namespace archives.identityserver.Config
{
    public class IdentityServerConfig
    {
        public string IssuerUri { get; set; }

        public IEnumerable<ClientConfig> Clients { get; set; }
    }

    public static class Extentions
    {
        public static IEnumerable<Client> ToIdentityModel(this IEnumerable<ClientConfig> clients)
        {
            return clients.Select(c => new Client
            {
                ClientId = c.ClientId,
                AllowedGrantTypes = new[] { c.GrantType },
                ClientSecrets = { new Secret(c.Secret.Sha256()) },
                AllowedScopes = { c.Scope, IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile },
                IdentityTokenLifetime = c.IdentityTokenLifetime,
                AccessTokenLifetime = c.AccessTokenLifetime
            });
        }
    }
}
