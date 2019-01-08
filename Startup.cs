using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Car.Startup))]
namespace Car
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            
        }
    }
}
