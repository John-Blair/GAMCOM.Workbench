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
    [RestExtension("TestSelfCert")]
    public class TestSelfCert
    {
        [RestExtensionMethod(ReturnXml = false)]
        public static string GetOptions()
        {

            var countries = SelfCertificationFacade.fetchIndividualSelfCertOptions();
            return string.Join(", ", countries.Select(x=> x.CountryName).ToArray());

        }



    }
}
