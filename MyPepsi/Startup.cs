using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyPepsi.Startup))]
namespace MyPepsi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
