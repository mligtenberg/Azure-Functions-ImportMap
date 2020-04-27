using System;

namespace ImportMapManager.Contracts
{
    public class ImportMapRow
    {
        public string ReferenceName { get; set; }
        public int Version { get; set; }
        public string ReferenceUrl { get; set; }
        public string Scope { get; set; }
    }
}