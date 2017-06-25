using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using umbraco.interfaces;

namespace Gam.Umbraco.Plugins.CustomContentMenu
{


    // ** STARTED TO LOOK AT CUSTOM MENUS, THEY SEEM AWFUL TO WORK WITH **

    public class CreateMediaFolderAction : IAction
    {
        
        //create singleton
        private static readonly CreateMediaFolderAction instance = new CreateMediaFolderAction();
        private CreateMediaFolderAction() { }
        public static CreateMediaFolderAction Instance
        {
            get { return instance; }
        }

        #region IAction Members

        /// <summary>
        /// Each IAction needs to be assigned a letter and they all need to be unique.
        /// </summary>
        public char Letter
        {
            get { return '2'; }
        }

        public bool ShowInNotifier
        {
            get { return false; }
        }

        /// <summary>
        /// Set this to true if the menu item can be permission assignable. If set to false,
        /// the menu item will show up for all users.
        /// </summary>
        public bool CanBePermissionAssigned
        {
            get { return false; }
        }


        public string Icon
        {
            get { return "editor/help.gif"; }
        }

        public string Alias
        {
            get { return "Create Media Folders"; }
        }

        /// <summary>
        /// This is a path to a JavaScript file that contains the custom JavaScript to call.
        /// </summary>
        public string JsSource
        {
            get { return "/test.js"; }
        }

        /// <summary>
        /// This is the method to call in the custom JavaScript file. The JavaScript file will 
        /// be rendered out in the parent frame
        /// so the call to the method needs to be prefixed with "parent." Since the JavaScript 
        /// file is rendered out in the parent frame
        /// the script will have access to the properties of the umbracoDefault.js script    
        /// which contains properties such as nodeID (which is
        /// the id of the node that is use has right clicked).
        /// </summary>
        public string JsFunctionName
        {
            get { return "parent.ShowHelp()"; }
        }

        #endregion
    }
}
