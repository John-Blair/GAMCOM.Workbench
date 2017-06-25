using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gam.Umbraco.Helpers;

namespace Gam.Umbraco.Helpers.Test
{
    [TestClass]
    public class StructuredFundListTest
    {
        [TestMethod]
        public void TestMethod1()
        {

            var x = new StructuredFundList();

            foreach (FundCollection eachCollection in x.Funds)
            {
                Console.WriteLine(eachCollection.Name);
                foreach(Fund eachFund in eachCollection.Funds)
                {
                    Console.WriteLine("   " + eachFund.FundName);
                }
            }
            var i = 2;


        }
    }
}
