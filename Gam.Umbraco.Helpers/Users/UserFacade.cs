using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Gam.Umbraco.Helpers
{


    public enum UserAuthenticationStatus
    {
          SelfCertified
        , SignedIn
    }

    // 
    public class PendingAuthorizationInfo
    {
        public string Country { get; set; }
        public string ComplianceGroup { get; set; }
        public string InvestorType { get; set; }
    }

    /// <summary>
    /// 
    ///     UserFacacde
    ///     encapsulates information about the user, and the user's session
    ///     
    /// </summary>
    public static class UserFacade 
    {

        /// <summary>
        ///     if there's no current compliance group set, you're a guest
        /// </summary>
        public static string CurrentComplianceGroup
        {
            get 
            {
                return UserFacade.GetSessionVariable("ComplianceGroup") ?? "-Guest";
            }

            set
            {
                UserFacade.SetSessionVariable("ComplianceGroup", value);
            }
        }

        /// <summary>
        /// Actually a CountryNodeId
        /// </summary>
        public static string Country
        {
            get
            {
                return UserFacade.GetSessionVariable("Country") ?? string.Empty;
            }

            set
            {
                UserFacade.SetSessionVariable("Country", value);
            }
        }



        public static string InvestorType
        {
            get
            {
                return UserFacade.GetSessionVariable("InvestorType") ?? string.Empty;
            }

            set
            {
                UserFacade.SetSessionVariable("InvestorType", value);
            }
        }

        /// <summary>
        ///     a pending deep link can be set by a user wanting to access a page, but finding they need to sign in before seeing it
        ///     once the user has signed in, this page will be redirected to
        /// </summary>
        public static string PendingDeepLink
        {
            get
            {
                return UserFacade.GetSessionVariable("PendingDeepLink") ?? "";
            }

            set
            {
                UserFacade.SetSessionVariable("PendingDeepLink", value);
            }
        }

        public static PendingAuthorizationInfo PendingAuthorization
        {
            get
            {
                return GetSessionVariable<PendingAuthorizationInfo>("userfacade.PendingAuthorization") ?? new PendingAuthorizationInfo();
            }

            set
            {
                SetSessionVariable<PendingAuthorizationInfo>("userfacade.PendingAuthorization", value);
            }
        }


        public static string GetSessionVariable(string key)
        {
            string ret = null;

            if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session[key] != null)
            {
                ret = HttpContext.Current.Session[key].ToString();
            }
            return ret;
        }

        public static void SetSessionVariable(string key, string value)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session[key] = value;
            }
            
        }


        public static T GetSessionVariable<T>(string key)
        {
            T ret = default(T);

            if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session[key] != null)
            {
                ret = (T) HttpContext.Current.Session[key];
            }
            return ret;
        }

        public static void SetSessionVariable<T>(string key, T value)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session[key] = value;
            }

        }

    }
}
