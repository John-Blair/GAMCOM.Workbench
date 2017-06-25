using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using umbraco.cms.presentation.Trees;
using umbraco.BusinessLogic;
using umbraco.BusinessLogic.Actions;
using umbraco.cms.businesslogic.web;
using umbraco.interfaces;

namespace Gam.Umbraco.Plugins.CustomContentMenu
{

    // ** STARTED TO LOOK AT CUSTOM MENUS, THEY SEEM AWFUL TO WORK WITH **

    //public class CustomContextMenu : ApplicationBase
    //{


    //    public CustomContextMenu()
    //    {
    //        BaseTree.BeforeNodeRender += new BaseTree.BeforeNodeRenderEventHandler(this.BaseTree_BeforeNodeRender);
    //    }

    //    private void BaseTree_BeforeNodeRender(ref XmlTree sender, ref XmlTreeNode node, EventArgs e)
    //    {
    //        // The node.NodeType will return "content" or "media", etc.,
    //        // ...and if node.Menu is null, then there is no right-click menu to customize (such as in a Content Picker data type)
    //        if (node.NodeType == "content" && node.Menu != null)
    //        {
    //            // Check type of node, currently we have access to the icon, which could be used as an identifier!
    //            // Alternativily use the node.NodeID (note, NodeId here is returned as a string) to calculate the doctype 
    //            if (node.Icon == "world.png")
    //            {
    //                //node.Menu.Remove(ActionDelete.Instance); // Removes the delete option

    //                node.Menu.Insert(0, CreateMediaFolderAction.Instance);
    //                node.Menu.Insert(1, umbraco.BusinessLogic.Actions.ContextMenuSeperator.Instance);

    //            }
    //        }
    //    }


    //}

}
