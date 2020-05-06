using System;
using System.Linq;
using Glasswall.CloudSdk.Common;
using Glasswall.CloudSdk.Common.Web.Models;
using Glasswall.Core.Engine.Messaging;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Glasswall.CloudSdk.AWS.FileTypeDetection.Tests.FileTypeDetectionControllerTests.DetermineFileTypeFromBase64Method
{
    [TestFixture]
    public class WhenFileTypeIsDetected : FileTypeDetectionControllerTestBase
    {
        private const string Version = "Some Version";
        private static readonly byte[] ExpectedDecoded = { 116, 101, 115, 116 };

        private IActionResult _result;
        private FileTypeDetectionResponse _expectedFileType;

        [OneTimeSetUp]
        public void OnetimeSetup()
        {
            CommonSetup();

            GlasswallVersionServiceMock.Setup(s => s.GetVersion())
                .Returns(Version);

            FileTypeDetectorMock.Setup(s => s.DetermineFileType(It.IsAny<byte[]>()))
                .Returns(_expectedFileType = new FileTypeDetectionResponse(FileType.Unknown));

            _result = ClassInTest.DetermineFileTypeFromBase64(new Base64Request
            {
                Base64 = "dGVzdA=="
            });
        }

        [Test]
        public void Ok_Is_Returned()
        {
            Assert.That(_result, Is.Not.Null);
            Assert.That(_result, Is.TypeOf<OkObjectResult>()
                .With.Property(nameof(OkObjectResult.Value))
                .EqualTo(_expectedFileType));
        }

        [Test]
        public void Metrics_Are_Recorded()
        {
            MetricServiceMock.Verify(s =>
                    s.Record(
                        It.Is<string>(x => x == Metric.Base64DecodeTime),
                        It.Is<TimeSpan>(x => x > TimeSpan.Zero)),
                Times.Once);

            MetricServiceMock.Verify(s =>
                    s.Record(
                        It.Is<string>(x => x == Metric.FileSize),
                        It.Is<int>(x => x == ExpectedDecoded.Length)),
                Times.Once);

            MetricServiceMock.Verify(s =>
                    s.Record(
                        It.Is<string>(x => x == Metric.Version),
                        It.Is<string>(x => x == Version)),
                Times.Once);

            MetricServiceMock.Verify(s =>
                    s.Record(
                        It.Is<string>(x => x == Metric.DetectFileTypeTime),
                        It.Is<TimeSpan>(x => x > TimeSpan.Zero)),
                Times.Once);

            MetricServiceMock.VerifyNoOtherCalls();
        }

        [Test]
        public void Engine_Version_Is_Retrieved()
        {
            GlasswallVersionServiceMock.Verify(s => s.GetVersion(), Times.Once);
            GlasswallVersionServiceMock.VerifyNoOtherCalls();
        }

        [Test]
        public void FileTypeDetection_Is_Retrieved()
        {
            FileTypeDetectorMock.Verify(s => s.DetermineFileType(It.Is<byte[]>(x => x.SequenceEqual(ExpectedDecoded))), Times.Once);
            FileTypeDetectorMock.VerifyNoOtherCalls();
        }
    }
}