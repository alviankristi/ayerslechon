using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.OAuth;
using AyerLechon.Api.Providers;
using System.Configuration;
using Microsoft.Owin.Security.Facebook;

[assembly: OwinStartup(typeof(AyerLechon.Api.Startup))]

namespace AyerLechon.Api
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }
        //public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }
        public static string PublicClientId { get; private set; }
        public static FacebookAuthenticationOptions FacebookAuthOptions { get; private set; }

        // For more information on configuring authentication, 
        // please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {

            app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);
            //OAuthBearerOptions = new OAuthBearerAuthenticationOptions();
            // Configure the application for OAuth based flow
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/api/login"),
                Provider = new ApplicationOAuthProvider(),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(int.Parse(ConfigurationManager.AppSettings["AccessTokenExpireTimeSpan"])),
                // In production mode set AllowInsecureHttp = false
                AllowInsecureHttp = true,
                RefreshTokenProvider = new SimpleRefreshTokenProvider()
            };


   
            FacebookAuthOptions = new FacebookAuthenticationOptions()
            {
                AppId = "1575555412466802",
                AppSecret = "4b1616b2818bb25acc6767cf3868d652",
                Provider = new FacebookAuthProvider()
            };
            app.UseFacebookAuthentication(FacebookAuthOptions);
            // Enable the application to use bearer tokens to authenticate users

            app.UseOAuthBearerTokens(OAuthOptions);
            //app.UseOAuthBearerAuthentication(OAuthBearerOptions);
        }
    }
}
