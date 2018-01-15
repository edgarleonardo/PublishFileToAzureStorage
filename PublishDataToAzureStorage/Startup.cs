using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PublishDataToAzureStorage.Startup))]
namespace PublishDataToAzureStorage
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
