using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TOTO.Startup))]
namespace TOTO
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
