using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gam.Umbraco.Helpers;
using Umbraco;
using Umbraco.Web.BaseRest;
using System.Web;

namespace Gam.Umbraco.Services
{
    /// <summary>
    /// url targets:
    ///     /base/Fund/FindDocument
    /// </summary>
    [RestExtension("Fund")]
    public class Fund
    {
        /// <summary>
        /// Expect a posted form.
        /// </summary>
        /// <returns></returns>
        [RestExtensionMethod(ReturnXml = false)]
        public static string FindDocument()
        {

            int filtersSet = 0;

            HttpRequest request = HttpContext.Current.Request;


            // Check at least 1 filter set.
            foreach (string name in request.Form.AllKeys)
            {
                // Process all checkboxes - excluding non-filter checkboxes.
                string value = request.Form[name];
                if (name != FundControls.ToggleFilters && name != FundControls.DocumentTextbox && name != SharedControls.CurrentLanguage)
                {
                    filtersSet++;
                }
                // Validate textboxes.
                else if (name == FundControls.DocumentTextbox && !string.IsNullOrEmpty(value))
                {
                    filtersSet++;
                }

            }


            var currentLangauge = request.Form[SharedControls.CurrentLanguage] ?? Languages.English;

            if (filtersSet == 0)
            {
                var msg = LanguageFacade.DictionaryItem("nodocumentfound", currentLangauge);

                return string.Format("<span>{0}</span>", msg);
            }
            else
            {
                
                // Setup document filters for documents results page in Session.
                Dictionary<string, string> finddocumentParams = request.Form.AllKeys.ToDictionary(key => key, key => request.Form[key]);
                HttpContext.Current.Session[SessionKeys.FindDocumentParams] = finddocumentParams;

                // TBD: response.redirect crashes the browser.
                // HttpResponse response = HttpContext.Current.Response;
                //response.Redirect("/en/funddocuments.aspx",false);
                // TBD: Seerver transfer gives an ajax error that the url does not exist. 
                //HttpContext.Current.Server.Transfer("/en/funddocuments.aspx", true);

                // Complete the Ajax call, and redirect on the client.
                return string.Format(URLs.FundDocuments,currentLangauge);
            }

        }
    }
}
