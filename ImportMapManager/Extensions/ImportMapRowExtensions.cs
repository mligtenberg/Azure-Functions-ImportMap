using ImportMapManager.Contracts;
using ImportMapManager.Dtos;

namespace ImportMapManager.Extensions
{
    public static class ImportMapRowExtensions
    {
        public static ImportMapRowEntity ToRowEntity(this ImportMapRow row, string environment)
        {
            return new ImportMapRowEntity(environment, row.ReferenceName, row.Version, row.ReferenceUrl, row.Scope);
        }

        public static bool IsValid(this ImportMapRow row)
        {
            return row.ReferenceName != null && row.ReferenceUrl != null;
        }
    }
}