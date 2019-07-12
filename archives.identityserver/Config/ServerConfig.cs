using System;
using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace archives.identityserver.Config
{
    public static class ServerConfig
    {
        public static IEnumerable<IdentityResource> GetIdentityResourceResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(), //必须要添加，否则报无效的scope错误
                new IdentityResources.Profile()
            };
        }
        // scopes define the API resources in your system
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api", "My API"),
            };
        }

        public const string CustomGrantType = "customcode";
        public const string CustomSecret = "1eed954f309ce95e894cfd1dfb0c21e9";//minilobo 两md5
        public const string ArchivesClientId = "ArchivesClient";

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {
                new Client
                {
                    ClientId = "test1",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret(CustomSecret.Sha256())
                    },
                    AllowedScopes = { "api",IdentityServerConstants.StandardScopes.OpenId, //必须要添加，否则报forbidden错误
                  IdentityServerConstants.StandardScopes.Profile},
                },
                // resource owner password grant client
                new Client
                {
                    ClientId = "archivesClient",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api",IdentityServerConstants.StandardScopes.OpenId, //必须要添加，否则报forbidden错误
                  IdentityServerConstants.StandardScopes.Profile }
                },
                new Client
                {
                    ClientId = ArchivesClientId,
                    AllowedGrantTypes = new []{ CustomGrantType },
                    ClientSecrets =
                    {
                        new Secret(CustomSecret.Sha256())
                    },
                    AllowedScopes = { "api",IdentityServerConstants.StandardScopes.OpenId, //必须要添加，否则报forbidden错误
                  IdentityServerConstants.StandardScopes.Profile }
                }
            };
        }
    }
}
