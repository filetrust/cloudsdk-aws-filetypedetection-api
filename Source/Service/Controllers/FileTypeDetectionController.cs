using System;
using System.Diagnostics;
using Glasswall.CloudSdk.Common.Web.Abstraction;
using Glasswall.CloudSdk.Common.Web.Models;
using Glasswall.Core.Engine.Common.FileProcessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Glasswall.CloudSdk.AWS.FileTypeDetection.Controllers
{
    public class FileTypeDetectionController : CloudSdkController<FileTypeDetectionController>
    {
        private readonly IFileTypeDetector _fileTypeDetector;

        public FileTypeDetectionController(
            IFileTypeDetector fileTypeDetector,
            ILogger<FileTypeDetectionController> logger) : base(logger)
        {
            _fileTypeDetector = fileTypeDetector ?? throw new ArgumentNullException(nameof(fileTypeDetector));
        }

        [HttpPost("base64")]
        public IActionResult DetermineFileTypeFromBase64([FromBody]Base64Request request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                Logger.LogInformation("{0} method invoked", nameof(DetermineFileTypeFromBase64));

                return TryGetBase64File(request.Base64, out var bytes) 
                    ? DetermineFromBytes(request.FileName, bytes)
                    : new BadRequestObjectResult("Could not parse base 64 file.");
            }
            catch (Exception e)
            {
                Logger.LogError(e, $"Exception occured processing file: {e.Message}");
                throw;
            }
        }

        [HttpPost("sas")]
        public IActionResult DetermineFileTypeFromSas([FromBody] SasRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                Logger.LogInformation("{0} method invoked", nameof(DetermineFileTypeFromSas));

                var fileName = GetFileNameFromUrl(request.SasUrl.AbsolutePath);

                return TryGetFile(request.SasUrl, out var bytes)
                    ? DetermineFromBytes(fileName, bytes)
                    : new BadRequestObjectResult($"Could not download file from SAS: {request.SasUrl}.");
            }
            catch (Exception e)
            {
                Logger.LogError(e, $"Exception occured processing file: {e.Message}");
                throw;
            }
        }

        private IActionResult DetermineFromBytes(string fileName, byte[] bytes)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var fileType = _fileTypeDetector.DetermineFileType(bytes);
            stopwatch.Stop();

            Logger.Log(LogLevel.Information, $"File '{fileName}' DetermineFileType call took {stopwatch.Elapsed:c}");
            return new OkObjectResult(JsonConvert.SerializeObject(fileType));
        }
    }
}
