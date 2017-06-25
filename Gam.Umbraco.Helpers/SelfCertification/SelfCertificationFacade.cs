using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using umbraco.MacroEngines;
using umbraco.NodeFactory;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Gam.Umbraco.Helpers
{

    /// <summary>
    /// 
    ///     Supporting data types
    /// 
    /// </summary>
    public class SelfCertCountry
    {
        public string CountryName { get; set; }
        public string GermanCountryName { get; set; }
        public int CountryNodeId { get; set; }
        public string ComplianceGroup { get; set; }
        public bool IsActive { get; set; }

        public override string ToString()
        {
            return
                string.Format(
                    "CountryName: {0}, GermanCountryName: {1}, CountryNodeId:{2}, ComplianceGroup: {3}, IsActive:{4}<br/>\n",
                    CountryName, GermanCountryName, CountryNodeId, ComplianceGroup, IsActive);
        }
    }

    public class SelfCertComplianceCountryCollection
    {
        // is the compliance group a professional, or non-professional, group
        public bool IsProf { get; set; }
        public List<SelfCertCountry> Countries { get; set; }
    }
    /// <summary>
    /// 
    ///     Self Certification Facade
    ///     Provides helper methods for self certification and deep linking.
    ///     
    /// </summary>
    public static class SelfCertificationFacade
    {

        public static IEnumerable<SelfCertCountry> fetchProfSelfCertOptions(string targetNodeId)
        {
            return fetchSelfCertOptions(targetNodeId, true);
        }

        public static IEnumerable<SelfCertCountry> fetchIndividualSelfCertOptions(string targetNodeId)
        {
            return fetchSelfCertOptions(targetNodeId, false);
        }

        public static IEnumerable<SelfCertCountry> fetchProfSelfCertOptions()
        {
            return fetchSelfCertOptions(true);
        }

        public static IEnumerable<SelfCertCountry> fetchIndividualSelfCertOptions()
        {
            return fetchSelfCertOptions(false);
        }


        private static IEnumerable<SelfCertCountry> fetchSelfCertOptions(bool selectProf)
        {

            var countryList = new List<SelfCertCountry>();

            // return a list of countries, where the counties are active if the compliance matches the compliance string
            var rawList = fetchSelfCertData();
            foreach (string eachComplianceGroup in rawList.Keys)
            {
                if (rawList[eachComplianceGroup].IsProf == selectProf)
                {
                    foreach (SelfCertCountry eachCountry in rawList[eachComplianceGroup].Countries)
                    {
                        eachCountry.IsActive = true;
                        eachCountry.ComplianceGroup = eachComplianceGroup;
                        countryList.Add(eachCountry);
                    }
                }
            }

            return countryList.OrderBy(x => x.CountryName);

        }

        private static IEnumerable<SelfCertCountry> fetchSelfCertOptions(string targetNodeId, bool selectProf)
        {

            var targetNode = new DynamicNode();
            var complianceString = "";
            var countryList = new List<SelfCertCountry>();

            if (NodeFacade.NodeExists(targetNodeId, out targetNode))
            {
                if (NodeFacade.NodeHasCompliance(targetNode, out complianceString))
                {
                    // return a list of countries, where the counties are active if the compliance matches the compliance string
                    var rawList = fetchSelfCertData();
                    foreach (string eachComplianceGroup in rawList.Keys)
                    {
                        var isActive = (complianceString.Contains("," + eachComplianceGroup + ","));
                        if (rawList[eachComplianceGroup].IsProf == selectProf)
                        {
                            foreach (SelfCertCountry eachCountry in rawList[eachComplianceGroup].Countries)
                            {
                                eachCountry.IsActive = isActive;
                                eachCountry.ComplianceGroup = eachComplianceGroup;
                                countryList.Add(eachCountry);
                            }
                        }

                    }
                }
            }

            return countryList.OrderBy(x => x.CountryName);

        }


        private static Dictionary<string, SelfCertComplianceCountryCollection> fetchSelfCertData()
        {

            var data = new Dictionary<string, SelfCertComplianceCountryCollection>();
            var complianceGroups = new DynamicNodeList();
            var countryList = new DynamicNodeList();

            if (fetchComplianceGroups(out complianceGroups))
            {
                foreach (DynamicNode eachComplianceGroup in complianceGroups)
                {

                    var complianceGroup = new SelfCertComplianceCountryCollection();

                    complianceGroup.Countries = new List<SelfCertCountry>();
                    complianceGroup.IsProf = isProfessionalComplianceGroup(eachComplianceGroup);
                    var s = eachComplianceGroup.Name;

                    if (eachComplianceGroup.HasDescendantsOfType("Country", out countryList))
                    {
                        foreach (DynamicNode eachCountry in countryList)
                        {
                            if (eachCountry.SafeProperty("includeInSelf-Certification") == "1")
                            {
                                complianceGroup.Countries.Add(new SelfCertCountry { CountryName = eachCountry.Name, GermanCountryName = eachCountry.SafeProperty("de"), CountryNodeId = eachCountry.Id, IsActive = false });
                            }
                        }
                    }
                    data.Add(eachComplianceGroup.Name, complianceGroup);
                }
            }


            return data;

        }

        private static bool fetchComplianceGroups(out DynamicNodeList complianceGroups)
        {

            DynamicNode settingsRoot;
            if (NodeFacade.settingsRoot(out settingsRoot))
            {
                if (settingsRoot.HasDescendantsOfType("ComplianceGroup", out complianceGroups))
                {
                    return true;
                }
            }

            complianceGroups = null;
            return false;
        }

        private static bool isProfessionalComplianceGroup(DynamicNode n)
        {
            DynamicNodeList groupFolder;
            if (n.HasParentsOfType("ComplianceGroupFolder", out groupFolder))
            {
                if (groupFolder.Count() == 1)
                {
                    if (groupFolder.First().Name == "Professional" || groupFolder.First().Name == "Plus")
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }


}
