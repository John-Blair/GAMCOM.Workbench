using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UmbracoWorkbench.usercontrols
{

    /// <summary>
    /// 
    ///     A very simple user control which will be used by Umbraco to create a custom datatype.
    ///     The datatype is a simple checkbox, but one which defaults to TRUE if no value is set.
    /// 
    ///     This will be useful when we want an element that can support Hide In Menu, but we want the
    ///     default to be hidden, rather than shown.
    ///     
    ///     The database value for this control is INTEGER.
    ///     
    /// </summary>
    public partial class Flag_defaultTrue : System.Web.UI.UserControl, umbraco.editorControls.userControlGrapper.IUsercontrolDataEditor  
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public object value
        {
            get
            {
                return CheckBox1.Checked ? 1 : 0;
            }
            set
            {
                
                if (value.ToString() == "" || value.ToString() == "1")
                {
                    CheckBox1.Checked = true;
                }
                else
                {
                    CheckBox1.Checked = false;
                }

            }
        }
    }
}