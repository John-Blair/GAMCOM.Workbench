using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using umbraco.businesslogic;
using umbraco.interfaces;

namespace Gam.Umbraco.FundMedia
{

    // registers fund media app with umbraco 
    // alternative to editing the applications.config method
    [Application("fundMedia", "FundMediaApp", "FundMedia.gif", 7)]
    public class FundMediaApp : IApplication
    {
    }

}
