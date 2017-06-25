using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace UmbracoWorkbench.usercontrols
{
    public static class UserControlHelper
    {
        // brings in custom css for the admin controls
        public static void IncludeAdminStylesheet(Page page)
        {
            const string ADMIN_STYLESHEET = @"/css/admin.import.min.css";
            string link = String.Format("<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}\" />", ADMIN_STYLESHEET);
            page.Header.Controls.Add(new LiteralControl { Text = link });
        }
    }
}