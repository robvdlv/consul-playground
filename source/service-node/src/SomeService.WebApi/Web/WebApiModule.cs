using System.Reflection;
using Nancy;
using PXR.Logging;

namespace SomeService.Web.Web
{
    /// <summary>
    /// Defines endpoints related to actual dataset creation.
    /// </summary>
    public class WebApiModule : NancyModule
    {
        static readonly AssemblyName Assembly = typeof(WebNode).Assembly.GetName();

        public WebApiModule()
        {
            Get["/"] = r => Response.AsText($"{Assembly.Name} v{Assembly.Version}");

            Get["/status"] = r => Response.AsText("200 OK");
        }
    }
}
