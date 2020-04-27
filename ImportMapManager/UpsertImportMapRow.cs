using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using ImportMapManager.Contracts;
using ImportMapManager.Extensions;
using ImportMapManager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace ImportMapManager
{
    public class UpsertImportMapRow
    {
        private readonly IImportMapTableService _tableService;

        public UpsertImportMapRow(IImportMapTableService tableService)
        {
            _tableService = tableService;
        }

        [FunctionName("AddImportMapRow")]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "{environment}")]
            ImportMapRow row,
            string environment,
            ILogger log)
        {
            log.LogInformation("[AddImportMapRow]");

            if (row == null || !row.IsValid())
            {
                log.LogError("Body was empty or invalid");
                return new ContentResult
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Content = "Body should contain the following properties: \n -referenceName \n -referenceUrl"
                };
            }

            var dto = row.ToRowEntity(environment);
            await _tableService.UpsertRowAsync(dto);

            return new OkResult();
        }
    }
}