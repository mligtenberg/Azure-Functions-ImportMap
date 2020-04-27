using System;
using Microsoft.Azure.Cosmos.Table;

namespace ImportMapManager.Dtos
{
    public class ImportMapRowEntity: TableEntity
    {
        public ImportMapRowEntity() : base()
        {
        }

        public ImportMapRowEntity(string environment, string referenceName, int version, string referenceUrl, string scope = null)
        : base(environment, $"{referenceName}:{version}")
        {
            ReferenceName = referenceName;
            Version = version;
            ReferenceUrl = referenceUrl;
            Scope = scope;
        }

        public string ReferenceName { get; set; }
        public int Version { get; set; }
        public string ReferenceUrl { get; set; }
        public string Scope { get; set; }
    }
}