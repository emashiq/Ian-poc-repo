﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MicrosoftAuthenticationBasedWebFormApp
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                var result = Request.GetOwinContext().Authentication.GetAuthenticationTypes();
                //string token = result.Properties.Dictionary["access_token"];
                //Title = token;
            }   
          
        }
    }
}