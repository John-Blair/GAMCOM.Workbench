using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using umbraco.MacroEngines;

namespace Gam.Umbraco.Helpers
{

    /// <summary>
    /// 
    ///     Content Facade
    ///     Provides helper methods for manipulating Umbraco content.
    ///     
    /// </summary>
    public static class ContentFacade
    {

        /// <summary>
        /// Takes a content string, finds and executes any embedded macros, 
        /// replaces the embedded macro string with the result of executing the macro.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        /// <remarks>
        /// 
        /// An embedded macro looks something like this 
        ///     <?UMBRACO_MACRO picked=\"1316\" macroAlias=\"snippetBySelection\" />
        ///     
        /// The trick to expanding the macro is the call to umbraco.library.RenderMacroContent.
        /// It's not quite clear what the pageid parameter should be, although nominally it is used
        /// to provide an executuion context for the macro, we don't currently have any macros that rely on this.
        /// 
        /// A few links that were used to write this code
        /// http://our.umbraco.org/forum/developers/api-questions/16387-call-and-render-macro-from-base?p=0#comment61064
        /// http://our.umbraco.org/forum/templating/templates-and-document-types/33095-RenderMacroContent
        /// http://our.umbraco.org/wiki/reference/umbracolibrary/rendermacrocontent
        /// 
        /// </remarks>
        public static string ExpandEmbeddedMacros(string content, int contextNodeId)
        {

            // regexp to match an embedded macro
            var regex = new Regex(@"<\?UMBRACO_MACRO.+?>");

            // a context for the macro to run in
            // not entirely clear what this should be, but the current node seems a good place to start!
            string contextOverride = string.Format("contextidoverride=\"{0}\"", contextNodeId);

            // everywhere we match an embedded macro, we execute the macro 
            // then replace the embedded macro string with the result of running that macro
            foreach (Match eachMatch in regex.Matches(content))
            {
                if (eachMatch.Success)
                {
                    string originalEmbeddedMacro = eachMatch.Value;

                    string embeddedMacro = eachMatch.Value.Replace("contextidoverride=\"\"", contextOverride);
                    string renderedMacro = umbraco.library.RenderMacroContent(embeddedMacro, contextNodeId);
                    content = content.Replace(originalEmbeddedMacro, renderedMacro);
                }
            }

            return content;
        }

    }
}
