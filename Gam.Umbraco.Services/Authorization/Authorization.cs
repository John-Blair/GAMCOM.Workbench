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
    [RestExtension("Authorization")]
    public class Authorization
    {
        [RestExtensionMethod(ReturnXml = false)]
        public static bool CertifyAs(string complianceGroup)
        {
            UserFacade.CurrentComplianceGroup = complianceGroup;
            return true;
        }


        [RestExtensionMethod(ReturnXml = false)]
        public static bool CertifyAsProfile(string complianceGroup, string country, string investorType)
        {
            UserFacade.CurrentComplianceGroup = complianceGroup;
            UserFacade.Country = country;
            UserFacade.InvestorType = investorType;
            return true;
        }

        
        [RestExtensionMethod(ReturnXml = false)]
        public static bool StorePendingCertValues(string complianceGroup, string country, string investorType)
        {
            var info = new PendingAuthorizationInfo { ComplianceGroup = complianceGroup, Country = country, InvestorType = investorType };
            UserFacade.PendingAuthorization = info;
            return true;
        }

    }
}
