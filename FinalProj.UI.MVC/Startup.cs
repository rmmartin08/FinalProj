using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FinalProj.UI.MVC.Startup))]
namespace FinalProj.UI.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
