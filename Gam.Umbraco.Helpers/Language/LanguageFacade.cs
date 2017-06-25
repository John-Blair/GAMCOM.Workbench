using System;
using System.Collections.Generic;
using System.Web;
using umbraco.MacroEngines;

namespace Gam.Umbraco.Helpers
{
    public static class LanguageFacade
    {

        public const string WebDirSeparator = @"/";
        public const string HomePageLink = @"/";

        public const string SelectedLanguageClass = "currentItem";

        /// <summary>
        /// Umbraco cmsLanguageText table identifies the language ids for English and German.
        /// </summary>
        private static Dictionary<string, int> _languageIds = new Dictionary<string,int>()
            {
                {Languages.English, 3},
                {Languages.German, 4}
            };

        private static dynamic model
        {
            get
            {
                return new umbraco.MacroEngines.DynamicNode(umbraco.NodeFactory.Node.GetCurrent());
            }

        }


        public static string CurrentLanguage
        {
            get
            {
                string currentLanguage;
                try
                {
                    // Determine current language from page path i.e.
                    // check if EN or DE is in the path.
                    // Internally always use lowercase for a language.
                    var currentUrl = model.Url.ToLower();

                    var index = currentUrl.IndexOf(WebDirSeparator + Languages.English + WebDirSeparator);
                    if (index != -1)
                    {
                        currentLanguage = Languages.English;
                    }
                    else
                    {
                        currentLanguage = Languages.German;
                    }
                }
                catch
                {
                    // Default to English
                    currentLanguage = Languages.English;
                }

                return currentLanguage;
            }
        }

        public static string EnglishUrl
        {
            get
            {
                string enUrl;

                if (CurrentLanguage == Languages.English)
                {
                    // In English, so English link is home page.
                    enUrl = HomePageLink;
                }
                else
                {
                    // Current language is German.
                    // English link is the equivalent English page of the current German page.
                    enUrl = CurrentPageInLanguage(Languages.English);
                }

                return enUrl;
            }
        }


        public static string GermanUrl
        {
            get
            {
                string deUrl;

                if (CurrentLanguage == Languages.English)
                {
                    // German link is the equivalent german page of the current English page.
                    deUrl = CurrentPageInLanguage(Languages.German);
                }
                else
                {
                    // Current language is German, so German link is home page.
                    deUrl = HomePageLink;

                }

                return deUrl;
            }
        }

        public static string CurrentPageUrl
        {
            get
            {
                // Accommodate preview mode where the language element is not in the HttpContext.Current.Request
                string currentPageUrl = model.Url;

                // Add on parameters.
                currentPageUrl += HttpContext.Current.Request.Url.Query;
                    
                return currentPageUrl;
            }
        }

        public static string CurrentPageUrlNoQueryString
        {
            get
            {
                // Accommodate preview mode where the language element is not in the HttpContext.Current.Request
                return model.Url;
            }
        }

        public static string SelectEnglishIfCurrent
        {
            get
            {
               if (CurrentLanguage == Languages.English)
               {
                   return SelectedLanguageClass;
               }
               return string.Empty;
            }
        }

        public static string SelectGermanIfCurrent
        {
            get
            {
                if (CurrentLanguage == Languages.German)
                {
                    return SelectedLanguageClass;
                }
                return string.Empty;
            }
        }

        



        public static string CurrentPageInLanguage(string language)
        {

            string currentPageUrl = CurrentPageUrl;

            // Avoid injecting script into the rendered page.
            if (ScriptElementPresent( currentPageUrl))
            {
                currentPageUrl = CurrentPageUrlNoQueryString;
            }

            string currentLanguage = CurrentLanguage;

            // Replace the language which may be upper or lower case but do not change the case of the rest of the path.
            string requestedPage =
                currentPageUrl.Replace(
                WebDirSeparator + currentLanguage + WebDirSeparator,
                WebDirSeparator + language + WebDirSeparator);

            // Handle no substitution of uppercase.
            requestedPage =
                requestedPage.Replace(
                WebDirSeparator + currentLanguage.ToLower() + WebDirSeparator,
                WebDirSeparator + language.ToLower() + WebDirSeparator);

            return requestedPage;

        }


        public static bool ScriptElementPresent(string url)
        {

            var urlLower = url.ToLower();
            var urlDecoded = HttpContext.Current.Server.UrlDecode(url);
            if (    urlLower.Contains("<script>")
                || urlLower.Contains("</script>")
                || urlDecoded.Contains("<script>")
                || urlDecoded.Contains("</script>")
                )
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        /// <summary>
        /// Lookup a dictionary item for the given language.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string DictionaryItem(string item, string language)
        {

            // Avoid casing issues with lookup.
            language = language.ToLower();

            string translation;
            try
            {
                var languageId = _languageIds[language];

                var dictionaryItem = new umbraco.cms.businesslogic.Dictionary.DictionaryItem(item);
                translation = dictionaryItem.Value(languageId);
                if (string.IsNullOrEmpty(translation) && language != Languages.English)
                {
                    // Default to English.
                    var englishLanguageId = _languageIds[Languages.English];
                    translation = dictionaryItem.Value(englishLanguageId);
                }

                if (string.IsNullOrEmpty(translation))
                {
                    // Not avaialable in English either.
                    // Display the dictionary item that needs updating.
                    translation = item;
                }
            }
            catch (Exception)
            {
                translation = string.Empty;
            }
            return translation;
        }


        public static string CountryName(string countryNodeId, string language)
        {
            string countryName = string.Empty;

            DynamicNode countryNode;

            if  (NodeFacade.NodeExists(countryNodeId, out countryNode))
            {
                countryName = countryNode.Name;

                if (language == Languages.German)
                {
                    // Override the English if there is a german name present.
                    var germanCountryName = countryNode.SafeProperty(Languages.German);

                    if (!string.IsNullOrEmpty(germanCountryName))
                    {
                        countryName = germanCountryName;
                    }

                }
            }

            return countryName;
        }

    }
}
