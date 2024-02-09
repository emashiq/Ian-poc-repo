using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MicrosoftAuthenticationBasedWebFormApp
{
    public partial class About : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // perform api call to get api data using access_token from claims if access token is not present then set title unauthorized
            if (User.Identity.IsAuthenticated)
            {
                var identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                var accessToken = claims.FirstOrDefault(c => c.Type == "access_token")?.Value;
                if (accessToken != null)
                {
                    // call api to get data
                    CallApi(accessToken);
                }
                else
                {
                    Title = "Unauthorized";
                }
            }
            else
            {
                Title = "Unauthorized";
            }
        }

        private void CallApi(string accessToken)
        {
            //call https://localhost:7093/WeatherForecast with access_token using httpclient
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = client.GetAsync("https://localhost:7093/WeatherForecast").Result;
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                Title = content;
            }
            else
            {
                //log error
            }


        }
    }
}