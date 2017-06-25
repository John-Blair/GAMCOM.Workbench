using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gam.Umbraco.Plugins.FundMedia.Pages
{
    public partial class editFundMedia : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void testButton_Click(object sender, EventArgs e)
        {
            testLabel.Text = "yeah yeah";
        }
    }
}