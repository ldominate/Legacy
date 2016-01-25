using Microsoft.Owin;
using Owin;

namespace Legacy.WebClientMVC
{
	[assembly: OwinStartup(typeof(Startup))]
	public partial class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			
		}
	}
}