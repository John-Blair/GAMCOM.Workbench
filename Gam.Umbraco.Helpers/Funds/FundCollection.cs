using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gam.Umbraco.Helpers
{

    public class FundCollection
    {

        private List<Fund> _fundList = new List<Fund>();
        public string Name { get; set; }
        public IEnumerable<Fund> Funds
        {
            get
            {
                return _fundList;
            }
        }

        public void AddFund(Fund fund)
        {
            _fundList.Add(fund);
        }

    }

}
