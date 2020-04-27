using System;
using System.Collections.Generic;
using System.Linq;
using ImportMapManager.Contracts;
using ImportMapManager.Dtos;

namespace ImportMapManager.Extensions
{
    public static class ImportMapRowEntityExtensions
    {
        public static ImportMap ToImportMap(this IEnumerable<ImportMapRowEntity> rows)
        {
            var imports =
                from r in rows
                where string.IsNullOrWhiteSpace(r.Scope)
                group r by r.ReferenceName into importGroups
                select importGroups.OrderByDescending(c => c.Version).FirstOrDefault();

            var scopes =
                from row in rows
                where !string.IsNullOrWhiteSpace(row.Scope)
                group row by row.Scope into scopeGroup
                select (scopeGroup.Key, from scopedRow in scopeGroup
                group scopedRow by scopedRow.ReferenceName
                into scopedImportGroup
                where scopedImportGroup.Any()
                select scopedImportGroup.OrderByDescending(s => s.Version).First());
            
            return new ImportMap
            {
                Imports = imports.ToDictionary(
                    r => r.ReferenceName, 
                    r => r.ReferenceUrl
                    ),
                Scopes = scopes.ToDictionary(
                    r => r.Key,
                    r => r.Item2.ToDictionary(
                        scopedR => scopedR.ReferenceName,
                        scopedR => scopedR.ReferenceUrl
                        )
                    )
            };
        }
    }
}