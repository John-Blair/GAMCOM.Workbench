using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Umbraco;
using Umbraco.Web.BaseRest;
using System.Web;

namespace Gam.Umbraco.Services
{
    [RestExtension("Manager")]
    public class Manager
    {
        [RestExtensionMethod(ReturnXml = false)]
        public static string HelloWorld()
        {
            return "hello world!";
        }

        [RestExtensionMethod(ReturnXml = false)]
        public static string Commentary(string managerName)
        {
            if (String.IsNullOrEmpty(managerName.Trim()))
            {
                return "<p>Please select a manager</p>";
            }
            return string.Format("<a href='/media/876/uuda_r.pdf'>Latest Commentary for {0}</a>", managerName);
        }


        [RestExtensionMethod(ReturnXml = false)]
        public static string Biography(string managerName)
        {
            if (String.IsNullOrEmpty(managerName.Trim()))
            {
                return "<p>Please select a manager</p>";
            }
            return string.Format("<a href='/media/876/uuda_r.pdf'>Latest Biography for {0}</a>", managerName);
        }


        [RestExtensionMethod(ReturnXml = false)]
        public static string Funds(string managerName)
        {
            if (String.IsNullOrEmpty(managerName.Trim()))
            {
                return "<p>Please select a manager</p>";
            }
            return string.Format("<p>Funds for {0}</p><a href='/media/876/uuda_r.pdf'>GAM Japan</a>", managerName);
        }

    }
}
