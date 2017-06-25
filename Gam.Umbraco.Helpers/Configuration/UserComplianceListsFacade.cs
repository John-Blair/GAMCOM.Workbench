using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using umbraco.MacroEngines;

namespace Gam.Umbraco.Helpers
{

    /// <summary>
    ///     User Compliance Lists allow users to store frequently used patterns of compliance 
    ///     that can then be applied to pages or resources. They are created in Umbraco as content,
    ///     under Settings > Compliance > Compliance Lists. 
    /// </summary>
    public static class UserComplianceListsFacade
    {

        /// <summary>
        ///     Settings > Compliance Lists Folder > .Children 
        /// </summary>
        /// <returns>
        ///     Returns a DynamicNodeList which contains the DynamicNodes that represent the user compliance lists
        ///     Each DynamicNode is a compliance list, which can be extracted using GetPropertyValue("compliance")
        /// </returns>
        private static DynamicNodeList getUserLists()
        {
            DynamicNode settingsRoot;
            DynamicNodeList complianceListFolder;
            DynamicNodeList userLists = new DynamicNodeList(); // returned

            if (NodeFacade.settingsRoot(out settingsRoot))
            {
                if (settingsRoot.HasDescendantsOfType("ComplianceListsFolder", out complianceListFolder))
                {
                    userLists = complianceListFolder.First().Children;
                }
            }
            return userLists;
        }


        /// <summary>
        /// Returns the names of all of the user compliance lists.
        /// </summary>
        /// <returns>Enumerable set of strings which represent the names of the user compliance lists</returns>
        public static IEnumerable<string> UserComplianceLists()
        {

            // first entry & default
            var userComplianceLists = new List<string>() { "None" };

            foreach (var eachList in getUserLists())
            {
                userComplianceLists.Add(eachList.Name);
            }

            return userComplianceLists;

        }

        /// <summary>
        /// Search the user compliance lists for one matching the listName.
        /// If it's found, return true and populate listContent with the compliance settings.
        /// If not found, return false and populate listContent with an empty string.
        /// </summary>
        /// <param name="listName">target list</param>
        /// <param name="listContent">output, comma sperated list of compliance group names</param>
        /// <returns></returns>
        public static bool FindUserList(string listName, out string listContent)
        {

            listContent = "";
            foreach (var eachList in getUserLists())
            {
                if (eachList.Name == listName)
                {
                    listContent = eachList.SafeProperty("compliance");
                    return true;
                }
            }
            return false;

        }


    }
}
