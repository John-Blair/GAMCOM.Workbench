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
    [RestExtension("User")]
    public class User
    {
        [RestExtensionMethod(ReturnXml = false)]
        public static bool SetProfile(string country, string investorType)
        {
            UserFacade.Country = country;
            UserFacade.InvestorType = investorType;

            return true;
        }

    }
}
