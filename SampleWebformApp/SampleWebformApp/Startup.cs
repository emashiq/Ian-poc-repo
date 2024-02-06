using System.IO;
using Microsoft.Owin;
using Owin;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

[assembly: OwinStartupAttribute(typeof(SampleWebformApp.Startup))]
namespace SampleWebformApp
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
    public class ProxyHandler : IHttpHandler
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        public void ProcessRequest(HttpContext context)
        {

            // Your SPA server base URL
            string spaBaseUrl = "http://localhost:5173";

            // Construct the SPA server URL by appending the requested path
            string spaRequestUrl = spaBaseUrl + context.Request.Url.PathAndQuery;

            // Forward the request to the SPA server
            Task<HttpResponseMessage> responseTask = HttpClient.GetAsync(spaRequestUrl);
            HttpResponseMessage response = responseTask.Result;

            // check mimetype from response
            context.Response.ContentType = response.Content.Headers.ContentType.MediaType;



            // Write the SPA server response to the original response
            context.Response.Write(response.Content.ReadAsStringAsync().Result);
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
    //write a proxy handler for @vite
    public class LocalProxyHandler : IHttpHandler
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        private static readonly string LocalContentPath = "~/";

        public void ProcessRequest(HttpContext context)
        {
            ServeLocalContent(context, context.Request.Url.PathAndQuery);
        }

        private void ServeLocalContent(HttpContext context, string localPath)
        {
            // Assume local content is stored in the local-content directory
            string localFilePath = context.Server.MapPath(LocalContentPath + localPath);

            if (File.Exists(localFilePath))
            {
                // Read local content and set content type
                context.Response.ContentType = MimeMapping.GetMimeMapping(localFilePath);
                context.Response.WriteFile(localFilePath);
            }
            else
            {
                // If local content not found, return a not found response
                context.Response.StatusCode = 404;
                context.Response.StatusDescription = "Not Found";
                context.Response.Write("Content not found");
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}
