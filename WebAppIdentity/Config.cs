using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace WebAppAuth
{
    public class Config
    {
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                // client credentials (public external api)
                new Client
                {
                    ClientId = "client_imgapp",
                    ClientName = "client_imgapp_api",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AccessTokenType = AccessTokenType.Jwt,
                    AccessTokenLifetime=3600,
                    AllowOfflineAccess=true,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    AllowedScopes =
                    {
                        StandardScopes.OpenId,
                        StandardScopes.Profile,
                        "api_img"
                    }
                },
                new Client
                {
                    ClientId = "client_imgapp_internal",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets =
                    {
                        new Secret("secret123".Sha256())
                    },
                    AllowedScopes = { "api_img_internal" }
                },
                new Client
                {
                    ClientId = "client_imgapp_mvc",
                    ClientName = "ImgApp",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    RequireConsent = false,
                    AlwaysSendClientClaims=true,
                    AlwaysIncludeUserClaimsInIdToken=true,
                    AllowAccessTokensViaBrowser=true,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    RedirectUris           = { "http://localhost:1000/signin-oidc", "http://localhost:1000/signin-google" },
                    PostLogoutRedirectUris = { "http://localhost:1000/signout-callback-oidc" },
                    // scopes that client has access to
                    AllowedScopes =
                    {
                        StandardScopes.OpenId,
                        StandardScopes.Profile,
                        "roles",
                        "api_img"
                    },
                    AllowOfflineAccess = true
                }
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api_img1", "My API",new []{ "role","name","offline_access" }),
                new ApiResource("api_img_internal", "My internal API")
                {
                    ApiSecrets = {new Secret("secret123".Sha256()) },
                    Scopes = {new Scope(){ Name= "api_img_internal"} }
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = "api_img",
                    UserClaims = new List<string>(){ "api_img" }
                },
                new IdentityResource
                {
                    Name="roles",
                    UserClaims=new List<string>{ "role" }
                }
            };
        }
    }
}
