using Microsoft.Owin;
using Owin;
using Umbraco.Core;
using Umbraco.Core.Security;
using Umbraco.Web;
using Umbraco.Web.Security.Identity;
using Umbraco.IdentityExtensions;
using ApiPotG;
using Umbraco.RestApi;
using System.Web.Cors;

//To use this startup class, change the appSetting value in the web.config called
// "owin:appStartup" to be "UmbracoStandardOwinStartup"

[assembly: OwinStartup("UmbracoStandardOwinStartup", typeof(UmbracoStandardOwinStartup))]

namespace ApiPotG
{
    /// <summary>
    /// The standard way to configure OWIN for Umbraco
    /// </summary>
    /// <remarks>
    /// The startup type is specified in appSettings under owin:appStartup - change it to "StandardUmbracoStartup" to use this class
    /// </remarks>
    public class UmbracoStandardOwinStartup : UmbracoDefaultOwinStartup
    {
        public override void Configuration(IAppBuilder app)
        {
            //ensure the default options are configured
            base.Configuration(app);
            app.ConfigureUmbracoRestApi(new UmbracoRestApiOptions()
            {
                //Modify the CorsPolicy as required
                CorsPolicy = new CorsPolicy()
                {
                    AllowAnyHeader = true,
                    AllowAnyMethod = true,
                    AllowAnyOrigin = true
                }
            });

            app.UseUmbracoCookieAuthenticationForRestApi(ApplicationContext.Current);

            app.UseUmbracoBackOfficeTokenAuth();
        }
    }
}
