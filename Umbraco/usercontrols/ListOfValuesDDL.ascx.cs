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

    public partial class ListOfValuesDDL : System.Web.UI.UserControl,
       umbraco.editorControls.userControlGrapper.IUsercontrolDataEditor
    {
        

        [DataEditorSetting("List of Values Root Node",description = "Select the root node containing the list of values",
            type = typeof(umbraco.editorControls.SettingControls.Pickers.Content))]
        public string LOVRootNode { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                PopulateLOV(LOVDropDownList);
                LOVDropDownList.SelectedValue = _umbval;
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

            lov.Insert(0, "");

            ddl.DataSource = lov;
            ddl.DataBind();


        }
        private string _umbval;
        public object value
        {
            get { return LOVDropDownList.SelectedValue; }
            set { _umbval = value.ToString(); }
        }

        protected void LOVDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl = sender as DropDownList;

            if (ddl.SelectedIndex == 0) return;

        }
    }
}