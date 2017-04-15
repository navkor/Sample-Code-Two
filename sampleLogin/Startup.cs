using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(sampleLogin.Startup))]
namespace sampleLogin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
