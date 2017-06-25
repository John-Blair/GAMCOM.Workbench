using System;
using System.Collections.Generic;
using System.Web;
using umbraco.MacroEngines;
using System.Linq;


namespace Gam.Umbraco.Helpers
{
    public static class TranslatedListsFacade
    {

        public static Dictionary<string, string> GetList(string listName, string language) 
        {
            language = language.ToUpper();

            Dictionary<string,string> list = new Dictionary<string,string>();
            DynamicNode listRootNode;

            if (NodeFacade.TranslatedListRoot(listName, language, out listRootNode)) 
            {

                foreach (DynamicNode listItem in listRootNode.Children) 
                { 
                    list.Add(listItem.Name, listItem.SafeProperty("value"));
                }
            }

            return list;
        }

        public static string GetListAsHTMLOptions(string listName, string language)
        {
            var optionFormat = @"<option value=""{0}"">{1}</option>";
            var result = string.Empty;

            var list = GetList(listName, language);

            foreach (var  listItem in list)
            {
                result += string.Format(optionFormat, listItem.Value, listItem.Key);
            }

            return result;
        }


        public static string GetListAsHTMLOptions(string listName, string language, string selectedValue)
        {
            var optionFormat = @"<option value=""{0}"">{1}</option>";
            var selectedOptionFormat = @"<option value=""{0}"" selected>{1}</option>";
            var result = string.Empty;

            var list = GetList(listName, language);

            foreach (var listItem in list)
            {
                if (listItem.Value == selectedValue) 
                {
                    result += string.Format(selectedOptionFormat, listItem.Value, listItem.Key);
                }
                else 
                {
                    result += string.Format(optionFormat, listItem.Value, listItem.Key);
                }
            }

            return result;
        }


         public static string GetSelfCertListAsHTMLOptions(string isProf, string language)
        {
            var optionFormat = @"<option value=""{0},{1}"">{2}</option>";
            var result = string.Empty;
             language = language.ToLower();

             // Get prof or individual self-cert list.
             IEnumerable<SelfCertCountry> list = isProf == "1" ? SelfCertificationFacade.fetchProfSelfCertOptions() : SelfCertificationFacade.fetchIndividualSelfCertOptions();

             // Sort by language.
             list = language == Languages.English ? list.OrderBy(x => x.CountryName) : list.OrderBy(x => x.GermanCountryName);


            foreach (var listItem in list)
            {
                result += string.Format(optionFormat,
                    listItem.ComplianceGroup, 
                    listItem.CountryNodeId, 
                    language == Languages.English ? listItem.CountryName : listItem.GermanCountryName);
            }

            return result;
        }


         public static string GetListMessages(string listName)
         {
             // Escape { as {{ when its not used as a substitution parameter.
             const string scriptTemplateHeader = @"
var gam = gam || {{}};
gam.form = gam.form || {{}};
gam.form.messages = gam.form.messages || {{}};
gam.form.messages.{0} = gam.form.messages.{0} || {{}};
";

             // Namespace the messages for the given list.
             var result = string.Format(scriptTemplateHeader, listName);

             // Messages for the list {0} and the language {1}
             const string scriptTemplateHeaderLanguageSection = @"gam.form.messages.{0}.{1} = gam.form.messages.{0}.{1} || {{}};
";

             // Language specific messages - key {2} and value {3}.
             const string scriptTemplateHeaderLanguageSectionMessage = @"gam.form.messages.{0}.{1}.{2} = ""{3}"";
";

            // Index messages by all available languages.
            foreach (var language in new[] { Languages.English, Languages.German }) 
            {
                result += string.Format(scriptTemplateHeaderLanguageSection, listName, language) ;
                var list = GetList(listName, language);

                foreach (var listItem in list)
                {
                    result += string.Format(scriptTemplateHeaderLanguageSectionMessage, listName, language, listItem.Key, listItem.Value);
                }
            }

             return string.Format("<script>{0}</script>", result);
         }

    }
}
