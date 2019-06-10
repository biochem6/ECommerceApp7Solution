using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ECommerceApp7.Startup))]
namespace ECommerceApp7
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
