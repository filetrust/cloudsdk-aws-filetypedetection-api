using System;
using System.Net;
using System.Net.Http;
using Glasswall.CloudSdk.Common;
using Glasswall.CloudSdk.Common.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Glasswall.CloudSdk.AWS.FileTypeDetection.Tests.FileTypeDetectionControllerTests.DetermineFileTypeFromUrlMethod
{
    [TestFixture]
    public class WhenInputUrlCannotBeDownloaded : FileTypeDetectionControllerTestBase
    {
        private IActionResult _result;
        private Uri _expectedInputUrl;

        [OneTimeSetUp]
        public void OnetimeSetup()
        {
            CommonSetup();
            
            _expectedInputUrl = new Uri("https://www.myfileserver.com/myfile.png");

            HttpTest.ResponseQueue.Enqueue(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            });

            _result = ClassInTest.DetermineFileTypeFromUrl(new UrlRequest
            {
                InputGetUrl = _expectedInputUrl
            });
        }

        [OneTimeTearDown]
        public void OnetimeTeardown()
        {
            HttpTest?.Dispose();
        }

        [Test]
        public void Bad_Request_Is_Returned()
        {

            Assert.That(_result, Is.Not.Null);
            Assert.That(_result, Is.TypeOf<BadRequestObjectResult>()
                .With.Property(nameof(BadRequestObjectResult.Value))
                .EqualTo("Input file could not be downloaded."));
        }
        
        [Test]
        public void Metrics_Are_Recorded()
        {
            MetricServiceMock.Verify(s =>
                    s.Record(
                        It.Is<string>(x => x == Metric.DownloadTime),
                        It.Is<TimeSpan>(x => x > TimeSpan.Zero)),
                Times.Once);

            MetricServiceMock.Verify(s =>
                    s.Record(
                        It.Is<string>(x => x == Metric.FileSize),
                        It.Is<int>(x => x == 0)),
                Times.Once);

            MetricServiceMock.VerifyNoOtherCalls();
        }

        [Test]
        public void No_Engine_Actions_Are_Performed()
        {
            GlasswallVersionServiceMock.VerifyNoOtherCalls();
            FileTypeDetectorMock.VerifyNoOtherCalls();
        }

        [Test]
        public void File_Download_Was_Attempted()
        {
            HttpTest.ShouldHaveMadeACall().Times(1);
            HttpTest.ShouldHaveCalled(_expectedInputUrl.ToString())
                .With(s => s.HttpStatus == HttpStatusCode.InternalServerError)
                .With(s => s.Request.Method == HttpMethod.Get)
                .Times(1);
        }
    }
}