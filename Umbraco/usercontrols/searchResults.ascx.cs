using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gam.Umbraco.Helpers;
using Gam.Umbraco.Services;

namespace UmbracoWorkbench.usercontrols
{
    public partial class searchResults : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            var sql = umbraco.BusinessLogic.Application.SqlHelper;



            if (!IsPostBack)
            {
                Dictionary<string, string> findDocumentParams = (Dictionary<string, string>)Session[SessionKeys.FindDocumentParams] ?? new Dictionary<string, string>();

                String postedValues = "Posted Values:<br/>";
                // Check at least 1 filter set.
                foreach (string name in findDocumentParams.Keys)
                {
                    string value = findDocumentParams[name];
                    postedValues += string.Format("Name:{0} Value:{1}<br/>", name, value);

                }

                string currentUserLanguage = string.Format("<br/> Current User's Language: {0}",
                                                           LanguageFacade.CurrentLanguage);
                searchParams.Text = postedValues + currentUserLanguage;

                var msg = DBFacade.Echo(DateTime.Now.ToString("s"));

                searchParams.Text += "<br/>Stored Proc Echo:" + msg;
            }
        }
    }
}