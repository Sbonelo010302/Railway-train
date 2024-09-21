using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RailwayApp.Startup))]
namespace RailwayApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
