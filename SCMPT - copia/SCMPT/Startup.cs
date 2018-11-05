using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SCMPT.Startup))]
namespace SCMPT
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
