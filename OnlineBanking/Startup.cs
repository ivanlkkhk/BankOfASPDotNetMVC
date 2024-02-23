using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OnlineBanking.Startup))]
namespace OnlineBanking
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
