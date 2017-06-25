using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Umbraco.Core;
using Umbraco.Core.Persistence;

namespace Gam.Umbraco.Helpers
{
    public class TaxonomyDAO
    {

        // eg FetchNodesByValue("FUND", "IE0003010158") 
        // Fetch the nodes related to GAM Star Japan Equity EUR B Acc (ISIN code = IE0003010158)
        public static List<TaxonomyDTO> FetchNodesByValue(string TaxonomyCode, string TaxonomyValue)
        {
            var db = ApplicationContext.Current.DatabaseContext.Database;
            var sql = new Sql()
                      .Select("tax.TaxonomyId, tax.NodeId, tax.Value")
                      .From("gamTaxonomy tax, gamTaxonomyType taxtype")
                      .Where("tax.TaxonomyId = taxtype.TaxonomyId AND taxtype.TaxonomyCode = @0 AND tax.Value = @1", TaxonomyCode, TaxonomyValue);

            return db.Fetch<TaxonomyDTO>(sql);
        }

        // eg FetchValuesByNode("FUND", 666)
        // Fetch all of the values associated with the given NodeId
        public static List<TaxonomyDTO> FetchValuesByNode(string TaxonomyCode, int NodeId)
        {
            var db = ApplicationContext.Current.DatabaseContext.Database;
            var sql = new Sql()
                      .Select("tax.TaxonomyId, tax.NodeId, tax.Value")
                      .From("gamTaxonomy tax, gamTaxonomyType taxtype")
                      .Where("tax.TaxonomyId = taxtype.TaxonomyId AND taxtype.TaxonomyCode = @0 AND tax.NodeId = @1", TaxonomyCode, NodeId);

            return db.Fetch<TaxonomyDTO>(sql);
        }

        // eg "FUND" --> 1
        public static int ConvertTaxCodeToTaxId(string TaxonomyCode)
        {
            var db = ApplicationContext.Current.DatabaseContext.Database;
            var sql = new Sql().Select("TaxonomyId").From("gamTaxonomyType").Where("TaxonomyCode = @0", TaxonomyCode);
            return db.Fetch<int>(sql).First();
        }

        public static void DeleteByNodeId(int nodeId)
        {
            var db = ApplicationContext.Current.DatabaseContext.Database;
            db.Delete("gamTaxonomy", "NodeId", null, nodeId);
        }

        // public insert
        public static void Insert(IEnumerable<TaxonomyDTO> taxonomyRecords)
        {
            // the underlying saveToDB method has a natural limit to the number of records it can save
            // chunkSave breaks the records into sublists which don't break that limit
            chunkSave(taxonomyRecords, 50, TaxonomyDAO.saveToDB);
        }


        private static void chunkSave(IEnumerable<TaxonomyDTO> records, int chunkSize, Action<IEnumerable<TaxonomyDTO>> dbSave)
        {
            while (records.Any())
            {
                var chunk = records.Take(chunkSize);
                records = records.Skip(chunkSize);
                dbSave(chunk);
            }
        }

        // bulk insert of taxonomy records
        // note: this technique will fail if too many records are passed at once
        private static void saveToDB(IEnumerable<TaxonomyDTO> taxonomyRecords)
        {

            var db = ApplicationContext.Current.DatabaseContext.Database;
            var sql = new Sql("insert into gamTaxonomy (TaxonomyId, NodeId, Value) values");

            var lastRecord = taxonomyRecords.Last();
            foreach (TaxonomyDTO eachTaxonomyRecord in taxonomyRecords)
            {
                sql.Append("(@0, @1, @2)", eachTaxonomyRecord.TaxonomyId, eachTaxonomyRecord.NodeId, eachTaxonomyRecord.Value);
                if (eachTaxonomyRecord != lastRecord) sql.Append(", "); 

            }
            
            db.Execute(sql);

        }

    }
}
