using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyRecordCollection.Startup))]
namespace MyRecordCollection
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
