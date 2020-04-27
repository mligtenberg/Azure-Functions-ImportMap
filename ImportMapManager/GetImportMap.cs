using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImportMapManager.Extensions;
using ImportMapManager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ImportMapManager
{
    public class GetImportMap
    {
        private readonly IImportMapTableService _tableService;
        public GetImportMap(IImportMapTableService tableService)
        {
            _tableService = tableService;
        }
        
        [FunctionName("GetImportMapWithoutVersion")]
        public Task<IActionResult> WithoutVersionAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "{environment}")]
            HttpRequest req, string environment, ILogger log)
        {
            return WithVersionAsync(req, environment, 0, log);
        }
        
        [FunctionName("GetImportMapWithVersion")]
        public async Task<IActionResult> WithVersionAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "{environment}/{version}")]
            HttpRequest req, string environment, int? version, ILogger log)
        {
            var items = (await _tableService.GetRowsAsync(environment, version))?.ToList();
            if (items == null || !items.Any())
            {
                return new NotFoundResult();
            }
            
            return new OkObjectResult(items.ToImportMap());
        }
    }
}