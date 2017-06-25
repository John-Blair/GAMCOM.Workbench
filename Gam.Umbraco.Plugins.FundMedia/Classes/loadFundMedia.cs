using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using umbraco.businesslogic;
using umbraco.cms.presentation.Trees;

namespace Gam.Umbraco.Plugins.FundMedia
{
    // loads custom tree
    [Tree("fundMedia","fundMediaTree", "Fund Media Tree")]
    public class FundMediaTree : BaseTree
    {
        public FundMediaTree(string application)
            : base(application)
        {
        }

        protected override void CreateRootNode(ref XmlTreeNode rootNode)
        {
            rootNode.Icon = FolderIcon;
            rootNode.OpenIcon = FolderIconOpen;
            rootNode.NodeType = TreeAlias;
            rootNode.NodeID = "init";
        }

        // js executed when you click a node in the custom tree
        public override void RenderJS(ref System.Text.StringBuilder Javascript)
        {
            Javascript.Append(
                @"
                        function openFundMedia(id)
                        {
                           parent.right.document.location.href = 'plugins/FundMedia/editFundMedia.aspx?id=' + id;
                        }
                ");
        }

        // adds nodes to tree
        public override void Render(ref XmlTree tree)
        {
            List<string> nodes = new List<string>() { "hello", "dolly" };

            int i = 0;
            foreach(var s in nodes)
            {
                XmlTreeNode xNode = XmlTreeNode.Create(this);
                xNode.NodeID = i.ToString();
                xNode.Text = s;
                xNode.Icon = "fundmedia.png";
                xNode.Action = "javascript:openFundMedia(" + i.ToString() + ")";
                tree.Add(xNode);

            }

        }

    }
}
