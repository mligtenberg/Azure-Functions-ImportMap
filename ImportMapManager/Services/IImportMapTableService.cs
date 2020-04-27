using System.Collections.Generic;
using System.Threading.Tasks;
using ImportMapManager.Dtos;

namespace ImportMapManager.Services
{
    public interface IImportMapTableService
    {
        Task<IEnumerable<ImportMapRowEntity>> GetRowsAsync(string environment, int? version);
        Task<bool> UpsertRowAsync(ImportMapRowEntity row);
    }
}