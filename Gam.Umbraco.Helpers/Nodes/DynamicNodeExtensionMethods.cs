using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using umbraco.MacroEngines;

namespace Gam.Umbraco.Helpers
{

    /// <summary>
    /// 
    /// Note that to use these extension methods the object must be strongly typed.
    /// In other words:
    /// 
    ///     yes : foreach (DynamicNode eachTile in tiles)
    ///           {
    ///             var colour = eachTile.SafeProperty("colour");
    ///             ...
    ///           }
    ///           
    ///     no  : foreach (var eachTile in tiles)
    ///           {
    ///             var colour = eachTile.SafeProperty("colour");
    ///             ...
    ///           }
    ///     
    /// </summary>
    public static class DynamicNodeExtensionMethods
    {

        public static bool IfSafeProperty(this DynamicNode node, string propertyName, out string result)
        {

            string returnValue = "";
            if (node.HasProperty(propertyName))
            {
                returnValue = node.GetProperty(propertyName).Value as string;
            }

            result = returnValue;
            return !string.IsNullOrEmpty(returnValue);

        }

        public static bool IfSafePropertyMatch(this DynamicNode node, string propertyName, string match)
        {
            
            bool returnValue = false;
            string propertyValue = "";
            if (node.HasProperty(propertyName))
            {
                propertyValue = node.GetProperty(propertyName).Value as string;
                returnValue = (propertyValue == match);
            }

            return returnValue;

        }

        public static string SafeProperty(this DynamicNode node, string propertyName)
        {

            string returnValue = "";
            if (node.HasProperty(propertyName))
            {
                returnValue = node.GetProperty(propertyName).Value as string;
            }
            
            return returnValue;

        }

        public static bool IsPropertyPopulated(this DynamicNode node, string propertyName, out string value)
        {
            
            value = node.SafeProperty(propertyName);
            return !string.IsNullOrEmpty(value);

        }

        public static bool IsCompliedFor(this DynamicNode node, string complianceGroup)
        {

            if (string.IsNullOrEmpty(node.Compliance())) 
            {
                // if there's no compliance configured for an item, assume everyone can see it
                return true;
            }
            else
            {
                return node.Compliance().Contains("," + complianceGroup + ",");    
            }
            
        }

        public static string Compliance(this DynamicNode node)
        {

            // news article content may inherit non-empty compliance settings from their parent news article
            // but only if their own compliance is blank. this allows compliance to be only set once when it's needed
            // but also allows links to be open and content to prompt self-certification

            string inheritedCompliance = "";
            if (node != null && node.Parent != null && node.Parent.NodeTypeAlias != null)
            {
                if (node.Parent.NodeTypeAlias.ToLower() == "newsarticle")
                {
                    inheritedCompliance = node.Parent.SafeProperty("compliance");
                }
            }
            
            var nodeCompliance = node.SafeProperty("compliance");

            if (string.IsNullOrEmpty(nodeCompliance))
            {

                return inheritedCompliance;
            }
            else
            {
                return nodeCompliance;
            }

        }




        public static bool HasDescendantsOfType(this DynamicNode node, string documentType, out DynamicNodeList descendants)
        {
            
            var nodes = node.Descendants(documentType);
            descendants = new DynamicNodeList();

            if (nodes == null)
            {
                return false;
            }
            else if (nodes.Count() == 0)
            {
                return false;
            }
            else
            {
                descendants = nodes;
                return true;
            }

        }


        public static bool HasParentsOfType(this DynamicNode node, string documentType, out DynamicNodeList parents)
        {

            var nodes = node.Ancestors(documentType);
            parents = new DynamicNodeList();

            if (nodes == null)
            {
                return false;
            }
            else if (nodes.Count() == 0)
            {
                return false;
            }
            else
            {
                parents = nodes;
                return true;
            }

        }

    }   
}
