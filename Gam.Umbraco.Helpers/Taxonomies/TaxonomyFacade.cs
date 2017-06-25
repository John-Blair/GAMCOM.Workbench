using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gam.Umbraco.Helpers
{
    public static class TaxonomyFacade
    {


        public static List<TaxonomyDTO> FetchNodesByValue(string TaxonomyCode, string TaxonomyValue)
        {
            return TaxonomyDAO.FetchNodesByValue(TaxonomyCode, TaxonomyValue);
        }

        public static List<TaxonomyDTO> FetchValuesByNode(string TaxonomyCode, int NodeId)
        {
            return TaxonomyDAO.FetchValuesByNode(TaxonomyCode, NodeId);
        }

        public static void Save (string TaxonomyCode, int NodeId, string valuesCSV)
        {

            // convert TaxonomyCode to TaxonomyID
            var taxId = TaxonomyDAO.ConvertTaxCodeToTaxId(TaxonomyCode);

            // create as a list of taxonomy records
            var values = valuesCSV.Split(',');
            var taxRecords = new List<TaxonomyDTO>();

            foreach (string eachValue in values)
            {
                if (!string.IsNullOrEmpty(eachValue))
                {
                    taxRecords.Add(new TaxonomyDTO() { TaxonomyId = taxId, NodeId = NodeId, Value = eachValue });
                }
            }

            // clear out any previous records relating to this nodeId
            // then insert all the taxRecords into database
            TaxonomyDAO.DeleteByNodeId(NodeId);
            TaxonomyDAO.Insert(taxRecords);
            
        }


    }
}
