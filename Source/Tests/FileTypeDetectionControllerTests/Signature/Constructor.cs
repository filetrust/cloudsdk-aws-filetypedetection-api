using System;
using Glasswall.CloudSdk.AWS.FileTypeDetection.Controllers;
using Glasswall.CloudSdk.Common;
using Glasswall.Core.Engine.Common.FileProcessing;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Glasswall.CloudSdk.AWS.FileTypeDetection.Tests.FileTypeDetectionControllerTests.Signature
{
    [TestFixture]
    public class Constructor
    {
        [Test]
        public void Valid_Arguments_Should_Construct()
        {
            var controller = new FileTypeDetectionController(
                Mock.Of<IGlasswallVersionService>(),
                Mock.Of<IFileTypeDetector>(),
                Mock.Of<IMetricService>(),
                Mock.Of<ILogger<FileTypeDetectionController>>());

            Assert.That(controller, Is.Not.Null);
        }

        [Test]
        public void Null_VersionService_Should_Throw()
        {
            Assert.That(() => new FileTypeDetectionController(
                    null,
                    Mock.Of<IFileTypeDetector>(),
                    Mock.Of<IMetricService>(),
                    Mock.Of<ILogger<FileTypeDetectionController>>()),
                Throws.ArgumentNullException.With.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("glasswallVersionService"));
        }

        [Test]
        public void Null_Detector_Should_Throw()
        {
            Assert.That(() => new FileTypeDetectionController(
                    Mock.Of<IGlasswallVersionService>(),
                    null,
                    Mock.Of<IMetricService>(),
                    Mock.Of<ILogger<FileTypeDetectionController>>()),
                Throws.ArgumentNullException.With.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("fileTypeDetector"));
        }
        
        [Test]
        public void Null_MetricService_Should_Throw()
        {
            Assert.That(() => new FileTypeDetectionController(
                    Mock.Of<IGlasswallVersionService>(),
                    Mock.Of<IFileTypeDetector>(),
                    null,
                    Mock.Of<ILogger<FileTypeDetectionController>>()),
                Throws.ArgumentNullException.With.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("metricService"));
        }

        [Test]
        public void Null_Logger_Should_Throw()
        {
            Assert.That(() => new FileTypeDetectionController(
                    Mock.Of<IGlasswallVersionService>(),
                    Mock.Of<IFileTypeDetector>(),
                    Mock.Of<IMetricService>(),
                    null),
                Throws.ArgumentNullException.With.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("logger"));
        }
    }
}