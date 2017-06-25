using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using umbraco.BusinessLogic;
using umbraco.MacroEngines;
using umbraco.NodeFactory;
using Gam.Umbraco.Helpers;
using System.Web.SessionState;
using log4net;
using umbraco.interfaces;
using System.Reflection;

namespace GAM.Umbraco.HttpModules
{
    public class GAMAuthorizationModule : IHttpModule
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Init(HttpApplication application)
        {
            application.PreRequestHandlerExecute += application_PreRequestHandlerExecute;
        }

        public void Dispose() { }

        // Note: remember web.config: <add type="GAM.Umbraco.HttpModules.GAMAuthorizationModule" name="GAMAuthorizationModule" /> system.webServer.modules
        
        public void application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            //return;
            DynamicNode currentNode = null;
            var ctx = HttpContext.Current;

            // ignore images, css and js requests 
            if (ctx.Response.ContentType != "text/html") return;
            // ignore error status codes 
            if (ctx.Response.StatusCode != 200) return;
            // ignore back end pages
            if (ctx.Request.RawUrl.ToLower().StartsWith("/umbraco/", StringComparison.InvariantCultureIgnoreCase)) return;
            if (ctx.Request.RawUrl.ToLower().Contains("/authorization/")) return;
            //if (ctx.Request.PhysicalPath.ToLower().Contains(@"\authorization\")) return;

            var previewMode = "" + ctx.Request.Cookies["UMB_PREVIEW"];
            if (!string.IsNullOrEmpty(previewMode)) return;

            try
            {
                currentNode = new DynamicNode(Node.GetCurrent().Id);
                if (currentNode.NiceUrl.ToLower().Contains("/authorization/")) return;

                // skip disclaimer nodes
                if (string.Compare(currentNode.NodeTypeAlias, "disclaimer", true) == 0) return;

                

                if (!currentNode.IsCompliedFor(UserFacade.CurrentComplianceGroup))
                {

                    UserFacade.PendingDeepLink = ctx.Request.RawUrl;

                    // server.TransferRequest breaks session variables *sometimes* 
                    // (not when the url is localhost, but when the full server name is used...)
                    //ctx.Server.TransferRequest("/en/authorization/deep-link.aspx?targetNode=" + currentNode.Id.ToString());

                    ctx.Response.Redirect("/en/authorization/deep-link.aspx?targetNode=" + currentNode.Id.ToString(), true);
                }

            }
            catch 
            {
            }

            //var orgTargetUrl = ctx.Request.RawUrl;


            }

    }
}