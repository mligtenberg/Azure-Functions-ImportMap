using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImportMapManager.Dtos;
using Microsoft.Azure.Cosmos.Table;

namespace ImportMapManager.Services
{
    public class ImportMapTableService: IImportMapTableService
    {
        private readonly CloudTable _table;
        private readonly bool _referencePreviousVersions;
        public ImportMapTableService(CloudTable table, bool referencePreviousVersions)
        {
            _table = table;
            _referencePreviousVersions = referencePreviousVersions;
        }
        
        public async Task<IEnumerable<ImportMapRowEntity>> GetRowsAsync(string environment, int? version)
        {
            _ = environment ?? throw new ArgumentNullException(nameof(environment));

            var query = _table.CreateQuery<ImportMapRowEntity>();
            if (version.HasValue)
            {
                query.FilterString = TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("partitionKey", QueryComparisons.Equal, environment),
                    TableOperators.And,
                    TableQuery.GenerateFilterConditionForInt("Version",
                        _referencePreviousVersions ? QueryComparisons.LessThanOrEqual : QueryComparisons.Equal, version.Value)
                );
            }
            else
            {
                query.FilterString =
                    TableQuery.GenerateFilterCondition("partitionKey", QueryComparisons.Equal, environment);
            }

            TableContinuationToken token = null;
            var result = new List<ImportMapRowEntity>();
            do
            { 
                var partialResult = await _table.ExecuteQuerySegmentedAsync(query, token);
                token = partialResult.ContinuationToken;
                if (partialResult.Results != null)
                {
                    result.AddRange(partialResult.Results);
                }
            } while (token != null);

            return result;
        }

        public async Task<bool> UpsertRowAsync(ImportMapRowEntity row)
        {
            _ = row ?? throw new ArgumentNullException(nameof(row));
            
            var result = await _table.ExecuteAsync(TableOperation.InsertOrReplace(row));

            return result.HttpStatusCode >= 200 && result.HttpStatusCode <= 299;
        }
    }
}