using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gam.Umbraco.Helpers
{
    public class StructuredFundList: IEnumerable
    {

        private int fundCount = 0;
        List<FundCollection> _structuredFundList;

        public StructuredFundList()
        {

            const string FUNDFILE = @"C:\web\GAMCOM.Workbench\Gam.Umbraco.Helpers\Funds\funds_grouped.csv";

            _structuredFundList = new List<FundCollection>();
            string[] lines = System.IO.File.ReadAllLines(FUNDFILE);
            loadFunds(lines, 0, "", "");

        }

        public IEnumerator GetEnumerator()
        {

            foreach (FundCollection eachFundCollection in _structuredFundList)
            {
                yield return eachFundCollection;
            }
                
        }

        public List<FundCollection> Funds
        {
            get
            {
                return _structuredFundList;
            }
        }

        private void loadFunds(string[] lines, int lineNumber, string group, string accruedFunds)
        {

            if (lineNumber >= lines.Length)
            {
                if (!string.IsNullOrEmpty(group))
                {
                    _structuredFundList.Add(loadFundCollection(group, accruedFunds));
                }
                return;
            }

            var csv = lines[lineNumber].Split(',');

            if (string.IsNullOrEmpty(group))
            {
                group = csv[0];
                accruedFunds = csv[1];
                loadFunds(lines, ++lineNumber, group, accruedFunds);
                return;
            }

            if (group != csv[0])
            {
                _structuredFundList.Add(loadFundCollection(group, accruedFunds));
                group = csv[0];
                accruedFunds = csv[1];
                loadFunds(lines, ++lineNumber, group, accruedFunds);
                return;
            }

            accruedFunds = accruedFunds + "," + csv[1];
            loadFunds(lines, ++lineNumber, group, accruedFunds);

        }



        private FundCollection loadFundCollection(string collectionName, string csvFunds)
        {

            var funds = csvFunds.Split(',');
            var newCollection = new FundCollection();
            newCollection.Name = collectionName;
            
            foreach (string eachFund in funds)
            {
                if (!string.IsNullOrEmpty(eachFund))
                {
                    string fakeIsin = "ISIN" + fundCount.ToString();
                    newCollection.AddFund(new Fund { FundName = collectionName + " " + eachFund, ClassDescription = eachFund, ISIN = fakeIsin });
                    fundCount++;
                }
            }

            return newCollection;

        }

    }





}
