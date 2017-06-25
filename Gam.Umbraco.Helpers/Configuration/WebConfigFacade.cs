using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using umbraco.MacroEngines;

namespace Gam.Umbraco.Helpers
{

    /// <summary>
    ///     Website configuration facade.
    ///     Used to access web config settings.
    ///     under Settings > WebConfig  
    /// </summary>
    public static class WebConfigFacade
    {

        /// <summary>
        /// Get the header theme to be used for the page.
        /// There is a site setting  Settings > WebConfig.headerTheme , which can be overriden at the page level.
        /// </summary>
        public static string HeaderTheme
        {
            get
            {
                return headerTheme();
            }
        }


        /// <summary>
        /// Get the default menu for the Header "Product" button. 
        /// </summary>
        public static string DefaultProductMenuCollection
        {
            get
            {
                return defaultProductMenuCollection();
            }
        }

        /// <summary>
        /// Get the header theme to be used for the page.
        /// There is a site setting  Settings > WebConfig.headerTheme , which can be overriden at the page level.
        /// </summary>
        /// <returns>header theme (CSS class name) to be used in a "header" element</returns>
        private static string headerTheme()
        {
            DynamicNode settingsRoot;
            DynamicNodeList webConfigSettings;

            if (NodeFacade.settingsRoot(out settingsRoot))
            {
                if (settingsRoot.HasDescendantsOfType("WebConfig", out webConfigSettings))
                {
                    return webConfigSettings.First().SafeProperty("headerTheme") ?? string.Empty;
                }
            }
            return string.Empty;
        }

        

        private static string defaultProductMenuCollection()
        {
            DynamicNode settingsRoot;
            DynamicNodeList webConfigSettings;

            if (NodeFacade.settingsRoot(out settingsRoot))
            {
                if (settingsRoot.HasDescendantsOfType("WebConfig", out webConfigSettings))
                {
                    return webConfigSettings.First().SafeProperty("defaultProductMenuCollection") ?? string.Empty;
                }
            }
            return string.Empty;
        }
    }

    

}
