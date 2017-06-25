using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using umbraco.cms.businesslogic.datatype;
using umbraco.MacroEngines;
using umbraco.presentation.nodeFactory;

namespace DataEditorControls
{

    public partial class ListOfValues : System.Web.UI.UserControl,
       umbraco.editorControls.userControlGrapper.IUsercontrolDataEditor
    {
        

        [DataEditorSetting("List of Values Root Node",description = "Select the root node containing the list of values",
            type = typeof(umbraco.editorControls.SettingControls.Pickers.Content))]
        public string LOVRootNode { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LOVValueTextBox.Text = _umbval;
                PopulateLOV(LOVDropDownList);
                //Debug hidden in output
                rootNodeTextBox.Text = LOVRootNode;
            }
        }
       
        private void PopulateLOV(DropDownList ddl)
        {

            umbraco.NodeFactory.Node rootNode = new umbraco.NodeFactory.Node(Convert.ToInt32(LOVRootNode));
            var lov = new List<string>();
            foreach (umbraco.NodeFactory.Node page in rootNode.Children)
            {
                lov.Add(page.Name);
            }


            //lov.Sort(); // preserve the list order
            lov.Insert(0, "--- Select One ---");

            ddl.DataSource = lov;
            ddl.DataBind();


        }
        private string _umbval;
        public object value
        {
            get { return LOVValueTextBox.Text; }
            set { _umbval = value.ToString(); }
        }

        protected void LOVDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl = sender as DropDownList;

            if (ddl.SelectedIndex == 0) return;

            // Update the user's choice of style
            // remove existing style - leaving other values if present.
            umbraco.NodeFactory.Node rootNode = new umbraco.NodeFactory.Node(Convert.ToInt32(LOVRootNode));
            var lovValue = " " + LOVValueTextBox.Text + " ";
            bool valueReplaced = false;
            foreach (umbraco.NodeFactory.Node page in rootNode.Children)
            {
                //Match whole word only
                if (lovValue.Contains(" " + page.Name + " ")) 
                {
                    //Replace it.
                    lovValue = lovValue.Replace(" " + page.Name + " ",  " " + ddl.SelectedValue + " ");
                    valueReplaced = true;
                    break;
                }
            }

            if (!valueReplaced) 
            {
                // Add selected style.
                lovValue += " " + ddl.SelectedValue;
            }

            // Bin leading spaces if no other styles present.
            LOVValueTextBox.Text = lovValue.Trim();
        }
    }
}