using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SampleWebformApp.Models;

namespace SampleWebformApp.Account
{
   

    public partial class LoginByCode : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // execute this if request is post
            if (Request.HttpMethod == "POST")
            {
                // get the code from the request
                string code = Request.Form["code"];
                string email = Request.Form["email"];
                //string localAccountId = Request.Form["localAccountId"];

                if (string.IsNullOrEmpty(code))
                {
                    Response.Clear();
                    Response.ContentType = "application/json; charset=utf-8";
                    Response.Write("{\"status\":400, \"message\":\"Invalid code\"}");
                    Response.End();
                }

                var userInformation = GetUserInformation(code);
                //if (userInformation.Id != localAccountId)
                //{
                //    Response.Clear();
                //    Response.ContentType = "application/json; charset=utf-8";
                //    Response.Write("{\"status\":400, \"message\":\"User id mismatch with azure ad\"}");
                //    Response.End();
                //}
                

                var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();

                var user = manager.FindByEmail(email);

                if (user == null)
                {
                    Response.Clear();
                    Response.ContentType = "application/json; charset=utf-8";
                    Response.Write("{\"status\":400, \"message\":\"User not found\"}");
                    Response.End();
                }

                signinManager.SignIn(user, isPersistent: false, rememberBrowser: false);

                Response.Clear();
                Response.ContentType = "application/json; charset=utf-8";
                Response.Write("{\"status\":200, \"message\":\"Login success\"}");
                // add a lax cookie to the response
                Response.AddHeader("Set-Cookie", "loginByCode=1; Path=/; SameSite=Lax; domain=.localhost");
                Response.End();
            }
            
        }

        private UserInformation GetUserInformation(string code)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", code);
            var response = httpClient.GetAsync("https://graph.microsoft.com/v1.0/me").Result;
            var decodedString = response.Content.ReadAsStringAsync().Result;
            var userInformation = Newtonsoft.Json.JsonConvert.DeserializeObject<UserInformation>(decodedString);
            return userInformation;
        }

        public class UserInformation
        {
            public List<string> BusinessPhones { get; set; }
            public string DisplayName { get; set; }
            public string GivenName { get; set; }
            public string JobTitle { get; set; }
            public string Mail { get; set; }
            public string MobilePhone { get; set; }
            public string OfficeLocation { get; set; }
            public string PreferredLanguage { get; set; }
            public string Surname { get; set; }
            public string UserPrincipalName { get; set; }
            public string Id { get; set; }
        }
    }
}