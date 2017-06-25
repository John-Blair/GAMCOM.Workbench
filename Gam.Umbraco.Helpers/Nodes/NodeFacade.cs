using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using umbraco.MacroEngines;
using umbraco.NodeFactory;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Gam.Umbraco.Helpers
{

    /// <summary>
    /// 
    ///     Node Facade
    ///     Provides helper methods for acting on the Umbraco content nodes.
    ///     
    /// </summary>
    public static class NodeFacade
    {

        // scan the top level root nodes of Umbraco for the node of a partiulcar type
        // where Umbraco presents a multirooted tree, some work has to be done to find these nodes
        public static bool findRootByType(string contentTypeName, out DynamicNode n)
        {

            var allRoots = ApplicationContext.Current.Services.ContentService.GetRootContent();
            IContent foundContent = null;
            foreach (var eachRoot in allRoots)
            {
                if (eachRoot.ContentType.Name == contentTypeName)
                {
                    foundContent = eachRoot;
                    break;
                }
            }

            if (foundContent == null)
            {
                n = new DynamicNode();
                return false;
            }
            else
            {
                n = new DynamicNode(foundContent.Id);
                return true;
            }

        }

        public static int GetCurrentNodeId()
        {
            var x = Node.GetCurrent();
            return x.Id;
        }

        // returns the root node which holds the settings
        public static bool settingsRoot(out DynamicNode n)
        {
            return NodeFacade.findRootByType("Settings", out n);
        }

        public static bool NodeExists(string nodeId, out DynamicNode n)
        { 
            n = null;
            if (!string.IsNullOrEmpty(nodeId))
            {
                n = new DynamicNode(nodeId);
            }
            return (n != null);
        
        }

        public static bool NodeHasCompliance(DynamicNode targetNode, out string complianceString)
        {
            complianceString = targetNode.SafeProperty("compliance");
            return (!string.IsNullOrEmpty(complianceString));
        }




        /// <summary>
        ///   Document Type aliases:
        ///   Settings > TranslatedLists > TranslationLanguage > TranslatedList
        ///   
        ///   
        ///   
        ///   
        ///   
        /// </summary>
        public static bool TranslatedListRoot(string listName, string language, out DynamicNode translatedListRoot)
        {
            DynamicNode settings;

            //Default result if list not found.
            translatedListRoot= new DynamicNode();

            if (!settingsRoot(out settings))
            {
                return false;
            }

            try 
            {
                // Get the translated list for the requested language.
                translatedListRoot = settings.Descendants("TranslatedLists").First()
                    .Descendants("TranslationLanguage").First(lang => lang.Name == language)
                    .Descendants("TranslatedList").First(list => list.Name == listName);
                return true;
            }
            catch 
            {
                return false;
            }
        }

    }

}
