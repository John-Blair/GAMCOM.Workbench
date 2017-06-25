using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using Gam.Umbraco.Helpers;

namespace UmbracoWorkbench.usercontrols
{

    /// <summary>
    /// 
    ///     This compliance user control will be used by Umbraco to create a custom datatype.
    ///     Note that in this version the sources for the regions and companies are hard-coded. 
    ///     
    ///     Compliance groups are shown in two sets of checkboxes, regions and companies.
    ///     Any group is given permission by checking the appropriate checkbox.
    ///     
    ///     Some helper controls are also given
    ///         [.] buttons to do bulk set and reset
    ///         [.] user compliance lists, which allow frequently used compliance patterns to be saved and assigned to pages
    ///         
    /// </summary>
    /// 
    public partial class Compliance : System.Web.UI.UserControl, umbraco.editorControls.userControlGrapper.IUsercontrolDataEditor
    {

        // todo: get regions and companies from AD
        private const string regions = "AllFunds,Austria,Austria-Plus,Austria-Prof,Austria-ProfTemp,BankofIrelandTemp,Belgium,Euro-Prof,Euro-ProfTemp,France,Germany,Germany-Plus,Germany-Prof,Germany-ProfTemp,Hong Kong,Hong Kong-Plus,Hong Kong-Prof,Hong Kong-ProfTemp,Ireland-Plus,Ireland-Prof,Ireland-ProfTemp,Italy,Italy-Plus,Italy-Prof,Italy-ProfTemp,Japan-Prof,Luxembourg,Monaco,Netherlands,NoFunds,ROW-Prof,ROW-ProfTemp,Scandinavia,Scandinavia-Plus,Singapore-Prof,Spain,Sweden,Sweden-Plus,Switzerland,Switzerland-Plus,Switzerland-Prof,Switzerland-ProfTemp,UK,UK-Plus,UK-Prof,UK-ProfTemp,UK-ProfUsers,US,US-Prof,US-ProfTemp";
        private const string companies = "-Charity,-DD GAM Online,-DD GAMfolio,-Dexia,-DLJ,-DM Dubai,-FCA,-GAM,-Guest,-HSBC,-Julius Baer,-ML,-MLRDR,-Old Mutual,-Oppenheimer,-UBS Intranet,-UBS Investment Bank,-UBSI,-Wachovia,-WebTeam";

        // tracks the name of a compliance group to its matching checkbox
        // hence _complianceCheckBoxes["-Dexia"] yields the checkbox for the Dexia compliance group
        // relies on not being passed compliance groups with duplicate names
        private Dictionary<string, CheckBox> _complianceCheckBoxes = new Dictionary<string, CheckBox>();

        // control, list of user compliance groups
        private DropDownList _complianceGroupList;


        protected override void OnInit(EventArgs e)
        {

            base.OnInit(e);

            addControl(
                createLabelPanel("Regions")
                );

            addControl(
                CreateTableFromComplianceList(regions.Split(','), "region")
                );

            addControl(
                createLabelPanel("Companies")
                );

            addControl(
                CreateTableFromComplianceList(companies.Split(','), "company")
                );

            addControl(
                createHelperPanel()
                );

        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        // abstracts exactly where the child controls are being added on the user control
        private void addControl(Control childControl)
        {
            updatePanel.ContentTemplateContainer.Controls.Add(childControl);
        }

        // helper panel contains all of the user's helper controls
        // user compliance lists, set all, reset all, etc
        private Panel createHelperPanel()
        {

            var helperPanel = new Panel();

            helperPanel.Controls.Add(
                new LiteralControl("<br />")
                );

            helperPanel.Controls.Add(
                new LiteralControl("<hr />")
                );

            helperPanel.Controls.Add(
                new LiteralControl("<br />")
                );

            helperPanel.Controls.Add(
                createUserComplianceListSelector()
                );

            helperPanel.Controls.Add(
                createButton_Apply()
                );

            helperPanel.Controls.Add(
                createButton_SetRegion()
                );

            helperPanel.Controls.Add(
                createButton_SetCompanies()
                );

            helperPanel.Controls.Add(
                createButton_SetAll()
                );

            helperPanel.Controls.Add(
                createButton_ResetAll()
                );


            return helperPanel;

        }

        // BUTTON: apply selected user list
        private Button createButton_Apply()
        {
            var btnApply = new Button();
            btnApply.Text = "Apply";
            btnApply.Click += btnApply_Click;
            return btnApply;
        }

        // when the apply button is pressed the current user list is applied to the compliance set
        // then, onece the user list has been applied, it is reset to None
        void btnApply_Click(object sender, EventArgs e)
        {

            var selectedUserListName = _complianceGroupList.SelectedItem.Text;

            string userListContent;
            if (UserComplianceListsFacade.FindUserList(selectedUserListName, out userListContent))
            {
                setComplianceState(userListContent);
            }

            _complianceGroupList.SelectedIndex = -1;
        }

        // BUTTON: set all regions
        private Button createButton_SetRegion()
        {
            var btnSetAll = new Button();
            btnSetAll.Text = "Set Regions";
            btnSetAll.Click += btnSetRegion_Click;
            return btnSetAll;
        }

        // set all regions 
        void btnSetRegion_Click(object sender, EventArgs e)
        {
            foreach (var eachGroup in _complianceCheckBoxes.Values)
            {
                if (eachGroup.Attributes["tag"] == "region")
                { 
                    setCheckboxValue(eachGroup, true); 
                }
            }
        }

        // BUTTON: set all comapnies
        private Button createButton_SetCompanies()
        {
            var btnSetAll = new Button();
            btnSetAll.Text = "Set Companies";
            btnSetAll.Click += btnSetCompanies_Click;
            return btnSetAll;
        }

        // set all companies
        void btnSetCompanies_Click(object sender, EventArgs e)
        {
            foreach (var eachGroup in _complianceCheckBoxes.Values)
            {
                if (eachGroup.Attributes["tag"] == "company")
                {
                    setCheckboxValue(eachGroup, true);
                }
            }
        }

        private Button createButton_SetAll()
        {
            var btnSetAll = new Button();
            btnSetAll.Text = "Set All";
            btnSetAll.Click += btnSetAll_Click;
            return btnSetAll;
        }

        // set everything
        void btnSetAll_Click(object sender, EventArgs e)
        {

            foreach (var eachGroup in _complianceCheckBoxes.Values)
            {
                setCheckboxValue(eachGroup, true);
            }

        }


        private Button createButton_ResetAll()
        {
            var btnSetAll = new Button();
            btnSetAll.Text = "Reset All";
            btnSetAll.Click += btnResetAll_Click;
            return btnSetAll;
        }

        // turn everything off
        void btnResetAll_Click(object sender, EventArgs e)
        {

            foreach (var eachGroup in _complianceCheckBoxes.Values)
            {
                setCheckboxValue(eachGroup, false);
            }

        }

        // a drop down list of all the user compliance lists
        // these are managed from within umbraco
        private DropDownList createUserComplianceListSelector()
        {
            
            var cplList = new DropDownList();
            cplList.ID = "ddlist_compliancegrouplist";
            cplList.Width = 200;

            foreach (string eachComplianceList in UserComplianceListsFacade.UserComplianceLists())
            {
                cplList.Items.Add(eachComplianceList);
            }

            _complianceGroupList = cplList;
            return cplList;
        }

        // creates a checkbox
        //  tagged with the compliance type (region or company)
        //  with the change event correctly wired
        //  puts the reference to the new checkbox into the _complianceCheckBoxes dictionary
        private CheckBox createComplianceCheckBox(string complianceGroup, string tag)
        {
            var cb = new CheckBox();
            cb.ID = "cbox_compliance_" + complianceGroup;
            cb.Text = complianceGroup;
            cb.AutoPostBack = true;
            cb.Attributes.Add("tag", tag);
            cb.ForeColor = Color.FromName("Gray");

            cb.CheckedChanged += complianceCheckbox_CheckChanged;

            _complianceCheckBoxes.Add(complianceGroup, cb);
            return cb;
        }

        void complianceCheckbox_CheckChanged(object sender, EventArgs e)
        {
            var cb = sender as CheckBox;
            if (cb != null)
            {
                setCheckboxValue(_complianceCheckBoxes[cb.Text.Trim()], cb.Checked);
            }
        }

        // sets the checkbox value, handles formatting
        private void setCheckboxValue(CheckBox cb, bool checkedValue)
        {

            if (checkedValue)
            {
                cb.ForeColor = Color.FromName("RoyalBlue");
                cb.Font.Bold = true;
            }
            else
            {
                cb.ForeColor = Color.FromName("Gray");
                cb.Font.Bold = false;
            }
            cb.Checked = checkedValue;
        }

        private static Panel createLabelPanel(string labelText)
        {
            var p = new Panel();
            p.Height = 25;

            p.Controls.Add(new LiteralControl("<br />"));

            var l = new Label { Text = labelText };
            l.Font.Bold = true;
            l.Font.Underline = true;
            p.Controls.Add(l);
            return p;
        }


        // create a table of checkboxes
        private Table CreateTableFromComplianceList(string[] complianceGroups, string tag)
        {
            var columnCount = 0;
            const int columnMax = 3;

            var t = new Table();
            var tRow = new TableRow();

            for (int i = 0; i < complianceGroups.Length; i++)
            {

                TableCell tCell = new TableCell();
                tCell.Width = 240;

                var cb = createComplianceCheckBox(complianceGroups[i], tag);

                tCell.Controls.Add(cb);
                tRow.Cells.Add(tCell);

                if (columnCount == columnMax)
                {
                    t.Rows.Add(tRow);
                    tRow = new TableRow();
                    columnCount = 0;
                }
                else
                {
                    columnCount++;
                }

            }

            if (tRow.Controls.Count > 0)
            {
                t.Rows.Add(tRow);
            }
            return t;
        }

        // umbraco property 
        public object value
        {
            get
            {
                return getComplianceState();
            }
            set
            {
                setComplianceState(value as string);
            }
        }

        // umbraco property GET
        private string getComplianceState()
        {
            var compliance = "," + _complianceCheckBoxes.Values
              .Where(x => x.Checked)
              .Aggregate(new StringBuilder(), (ag, n) => ag.Append(n.Text).Append(","))
              .ToString();

            // let's not leak the implementation of the leading comma
            // if we did, the check for no compliance is "," rather than string.IsNullOrEmpty

            return (compliance == ",") ? "" : compliance;
        }

        // umbraco property SET
        private void setComplianceState(string serializedState)
        {
            var groupsToSet = serializedState.Split(',');
            foreach (string eachGroup in groupsToSet)
            {
                if (_complianceCheckBoxes.ContainsKey(eachGroup.Trim()))
                {
                    setCheckboxValue(_complianceCheckBoxes[eachGroup.Trim()], true);
                }
            }
        }

    }
}