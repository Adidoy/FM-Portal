using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(PUPFMIS.Startup))]
namespace PUPFMIS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
