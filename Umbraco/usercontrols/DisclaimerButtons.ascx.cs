using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gam.Umbraco.Helpers;
using umbraco.MacroEngines;
using umbraco.NodeFactory;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace UmbracoWorkbench.usercontrols
{
    public partial class DisclaimerButtons : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void dsclmrBtn_Decline_Click(object sender, EventArgs e)
        {
            // on decline, simply return to the web root and let the standing compliance take hold
            Response.Redirect("/", true);
        }

        protected void dsclmrBtn_Accept_Click(object sender, EventArgs e)
        {

            // use the pending authorization details to self certify
            Gam.Umbraco.Services.Authorization.CertifyAsProfile(UserFacade.PendingAuthorization.ComplianceGroup, UserFacade.PendingAuthorization.Country, UserFacade.PendingAuthorization.InvestorType);

            // process any pending deep link; if none set the next location to web root
            string nextUrl = string.IsNullOrEmpty(UserFacade.PendingDeepLink) ? "/" : UserFacade.PendingDeepLink;

            // clear pending information
            UserFacade.PendingDeepLink = "";
            UserFacade.PendingAuthorization = new PendingAuthorizationInfo();

            // redirect to next location
            Response.Redirect(nextUrl, true);

        }
    }
}