using System;
using ImportMapManager;
using ImportMapManager.Services;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(DependencyInjection))]
namespace ImportMapManager
{
    public class DependencyInjection: FunctionsStartup
    {
        private const string TableName = "importmap";
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var connectionString = Environment.GetEnvironmentVariable("ImportMapTableStorage")
                                   ?? Environment.GetEnvironmentVariable("AzureWebJobsStorage");

            var referencePreviousVersions = Environment.GetEnvironmentVariable("ReferencePreviousVersions") == "true";
            
            builder.Services.AddScoped<IImportMapTableService>((s) => new ImportMapTableService(SetupCloudTable(connectionString), referencePreviousVersions));
        }

        private static CloudTable SetupCloudTable(string connectionString)
        {
            var account = CloudStorageAccount.Parse(connectionString);
            var cloudTableClient = account.CreateCloudTableClient();
            var table = cloudTableClient.GetTableReference(TableName);
            table.CreateIfNotExists();
            return table;
        }
    }
}