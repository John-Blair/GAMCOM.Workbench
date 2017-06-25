using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Umbraco;
using Umbraco.Web.BaseRest;
using System.Web;
using Gam.Umbraco.Helpers;

namespace Gam.Umbraco.Services
{
    [RestExtension("Test")]
    public class Test
    {
        [RestExtensionMethod(ReturnXml = false)]
        public static string GetInvestorTypeEN()
        {
            return TranslatedListsFacade.GetListAsHTMLOptions("InvestorType", "EN");
        }


        [RestExtensionMethod(ReturnXml = false)]
        public static string GetListAsHTMLOptions(string listName, string language)
        {
            return TranslatedListsFacade.GetListAsHTMLOptions(listName, language);
        }

        [RestExtensionMethod(ReturnXml = false)]
        public static string GetListAsHTMLOptionsWithSelectedValue(string listName, string language,
                                                                   string selectedValue)
        {
            return TranslatedListsFacade.GetListAsHTMLOptions(listName, language, selectedValue);
        }


        [RestExtensionMethod(ReturnXml = false)]
        public static string FetchProfSelfCertOptions()
        {
            return string.Join<SelfCertCountry>(",", SelfCertificationFacade.fetchProfSelfCertOptions().ToArray());
        }


        [RestExtensionMethod(ReturnXml = false)]
        public static string GetSelfCertListAsHTMLOptions(string isProf, string language)
        {
            return TranslatedListsFacade.GetSelfCertListAsHTMLOptions(isProf, language);
        }


        [RestExtensionMethod(ReturnXml = false)]
        public static string GetListMessages()
        {
            return TranslatedListsFacade.GetListMessages("chooseProfileMessages");
        }


    }
}
