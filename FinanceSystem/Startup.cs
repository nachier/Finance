using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FinanceSystem.Startup))]
namespace FinanceSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
