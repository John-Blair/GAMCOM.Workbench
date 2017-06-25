using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Gam.Umbraco.Helpers
{
    public static class DBFacade
    {

        public static string Echo(string msg)
        {
            return DBServices.ExecuteScalar<string>("echo",new SqlParameter("@msg", msg));
        }
    }
}
