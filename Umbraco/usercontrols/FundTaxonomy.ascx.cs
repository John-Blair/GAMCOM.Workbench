using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using Gam.Umbraco.Helpers;
using umbraco.NodeFactory;
using Umbraco.Core;
using Umbraco.Core.Persistence;
using Umbraco.Core.Models;

namespace UmbracoWorkbench.usercontrols
{

    public partial class FundTaxonomy : System.Web.UI.UserControl, umbraco.editorControls.userControlGrapper.IUsercontrolDataEditor
    {

        // tracks the fund collection name to its matching checkbox
        private Dictionary<string, CheckBox> _masterFunds = new Dictionary<string, CheckBox>();

        // tracks the fund isin to its matching checkbox
        private Dictionary<string, CheckBox> _fundClasses = new Dictionary<string, CheckBox>();

        private StructuredFundList _structuredFundList;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            UserControlHelper.IncludeAdminStylesheet(this.Page);
            _structuredFundList = new StructuredFundList();
            addControl(createFundCollectionControl(_structuredFundList));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        // abstracts exactly where the child controls are being added on the user control
        private void addControl(Control childControl)
        {
            //fundHolder.Controls.Add(childControl);
            updatePanel.ContentTemplateContainer.Controls.Add(childControl);
        }


        private Table createFundCollectionControl(StructuredFundList fundMasterList)
        {

            var t = new Table();
            t.CssClass = "fundTaxonomyTable";

            foreach (FundCollection eachFundCollection in fundMasterList.Funds)
            {
                var tRow = new TableRow();

                TableCell tCell = new TableCell(); 
                tCell.Width = 280;
                tCell.Controls.Add( newMasterFundCheckbox(eachFundCollection) );
                tRow.Cells.Add(tCell);

                tCell = new TableCell();
                foreach (Fund eachFundClass in eachFundCollection.Funds)
                {
                    tCell.Controls.Add(newFundClassCheckbox(eachFundCollection, eachFundClass));
                }
                tRow.Cells.Add(tCell);
                t.Rows.Add(tRow);
            }

            return t;

        }


        private CheckBox newMasterFundCheckbox(FundCollection fundCollection)
        {

            var cbox = new CheckBox() { Text = fundCollection.Name, AutoPostBack = true };

            // name of master fund
            cbox.Attributes.Add("fundName", fundCollection.Name);

            // related fund classes: csv list, eg "A,B,C"
            // from this, note _fundClasses["A"] gives the cbox representing fundClass A
            string relatedFundClasses = string.Join(",", fundCollection.Funds.Select(f => f.ISIN));
            cbox.Attributes.Add("relatedClasses", relatedFundClasses);

            cbox.CheckedChanged += masterFundCheckbox_CheckChanged;
            _masterFunds.Add(fundCollection.Name, cbox);

            return cbox;
        }

        void masterFundCheckbox_CheckChanged(object sender, EventArgs e)
        {
            var cb = sender as CheckBox;
            if (cb != null)
            {
                // update the master fund checkbox 
                var fundName = cb.Attributes["fundName"];
                setMasterFundCheckboxValue(_masterFunds[fundName], cb.Checked);

                //update the related classes to match the master fund
                var relatedClasses = cb.Attributes["relatedClasses"].Split(',');
                foreach (string eachRelatedClass in relatedClasses)
                {
                    if (!string.IsNullOrEmpty(eachRelatedClass))
                    {
                        setFundClassCheckboxValue(_fundClasses[eachRelatedClass], cb.Checked);
                    }
                }

            }
        }

        private CheckBox newFundClassCheckbox(FundCollection fundCollection, Fund fund)
        {

            var cbox = new CheckBox() { Text = fund.ClassDescription, AutoPostBack = true, Width = 170 };

            cbox.Attributes.Add("fundName", fund.FundName);
            cbox.Attributes.Add("masterFund", fundCollection.Name);
            cbox.Attributes.Add("isin", fund.ISIN);
            cbox.CheckedChanged += fundClassCheckbox_CheckChanged;
            
            _fundClasses.Add(fund.ISIN, cbox);

            return cbox;
        }


        void fundClassCheckbox_CheckChanged(object sender, EventArgs e)
        {
            var cb = sender as CheckBox;
            if (cb != null)
            {
                var key = cb.Attributes["isin"];
                setFundClassCheckboxValue(_fundClasses[key], cb.Checked);

                // find the master fund for this fund class
                // then check all related classes to update the status of the master fund checkbox
                bool masterChecked = true;
                var masterFund = _masterFunds[cb.Attributes["masterFund"]];
                var relatedClasses = masterFund.Attributes["relatedClasses"].Split(',');
                foreach (string eachRelatedClass in relatedClasses)
                {
                    if (!string.IsNullOrEmpty(eachRelatedClass))
                    {
                        masterChecked = masterChecked && _fundClasses[eachRelatedClass].Checked;
                    }
                }

                setMasterFundCheckboxValue(masterFund, masterChecked);
            }
        }

        // sets the checkbox value, handles formatting
        private void setMasterFundCheckboxValue(CheckBox cb, bool checkedValue)
        {

            if (checkedValue)
            {
                cb.ForeColor = Color.FromName("RoyalBlue");
                cb.Font.Bold = true;
            }
            else
            {
                cb.ForeColor = Color.FromName("Black");
                cb.Font.Bold = false;
            }
            cb.Checked = checkedValue;
        }

        // sets the checkbox value, handles formatting
        private void setFundClassCheckboxValue(CheckBox cb, bool checkedValue)
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

        // IUsercontrolDataEditor property 
        // umbraco property 
        public object value
        {
            // umbraco triggers a get when it wants to save the current control state
            get
            {
                // identify the nodeid of the current document/media element
                var currentNodeId = int.Parse(Request.QueryString["id"]);
                var state = extractStateFromControl(currentNodeId);
                // save to umbraco db
                TaxonomyFacade.Save("FUND", currentNodeId, state);
                // return blank, we don't need to save anything again
                return "";
            }

            // umbraco triggers a set when it wants to populate the control state with a serialised representation
            set
            {
                // identify the nodeid of the current document/media element
                var currentNodeId = int.Parse(Request.QueryString["id"]);
                // ignore the persistance value and fetch values from the umbraco db instead
                var nodeList = TaxonomyFacade.FetchValuesByNode("FUND", currentNodeId);
                var nodeCSV = string.Join(",", nodeList.Select(n => n.Value));
                populateControlWithState(nodeCSV);
            }
        }

        private string extractStateFromControl(int nodeId)
        {
            var state = "," + _fundClasses.Values
              .Where(x => x.Checked)
              .Aggregate(new StringBuilder(), (ag, n) => ag.Append(n.Attributes["isin"]).Append(","))
              .ToString();

            // let's not leak the implementation of the leading comma
            // if we did, the check for no compliance is "," rather than string.IsNullOrEmpty
            return (state == ",") ? "" : state;
        }
        
        private void populateControlWithState(string serializedState)
        {

            var state = serializedState.Split(',');
            foreach (string eachFundClass in state)
            {
                if (_fundClasses.ContainsKey(eachFundClass.Trim()))
                {
                    setFundClassCheckboxValue(_fundClasses[eachFundClass.Trim()], true);
                    fundClassCheckbox_CheckChanged(_fundClasses[eachFundClass.Trim()], null);
                }
            }
            
        }


        protected void filtertext_TextChanged(object sender, EventArgs e)
        {
            
            string filterText = FilterText.Text.ToLower();

            foreach (string eachKey in _masterFunds.Keys)
            {
                if (eachKey.ToLower().Contains(filterText))
                {
                    _masterFunds[eachKey].Parent.Parent.Visible = true;
                }
                else
                {
                    _masterFunds[eachKey].Parent.Parent.Visible = false;
                }
            }

        }

        // clear all filters
        protected void clear_Click(object sender, EventArgs e)
        {
            FilterText.Text = "";
            filtertext_TextChanged(null, null);
        }


        // only show funds which have at least one class selected
        protected void showSelected_Click(object sender, EventArgs e)
        {
            foreach (string eachKey in _masterFunds.Keys)
            {
                _masterFunds[eachKey].Parent.Parent.Visible = anySelected(_masterFunds[eachKey]);
            }
        }

        private bool anySelected(CheckBox masterCheckBox)
        {

            bool returnValue;
            returnValue = masterCheckBox.Checked;

            if (!returnValue)
            {

                var relatedClasses = masterCheckBox.Attributes["relatedClasses"].Split(',');
                foreach (string eachRelatedClass in relatedClasses)
                {

                    if (!string.IsNullOrEmpty(eachRelatedClass) && _fundClasses[eachRelatedClass].Checked)
                    {
                        returnValue = true;
                        break;
                    }
                }
            }

            return returnValue;

        }

    }
}