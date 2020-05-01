using Flurl.Http.Testing;
using Glasswall.CloudSdk.AWS.FileTypeDetection.Controllers;
using Glasswall.CloudSdk.Common;
using Glasswall.Core.Engine.Common.FileProcessing;
using Microsoft.Extensions.Logging;
using Moq;

namespace Glasswall.CloudSdk.AWS.FileTypeDetection.Tests.FileTypeDetectionControllerTests
{
    public class FileTypeDetectionControllerTestBase
    {
        /// <summary>
        /// For mocking URL to url rebuild GET/PUT
        /// </summary>
        protected HttpTest HttpTest;

        protected FileTypeDetectionController ClassInTest;
        protected Mock<IGlasswallVersionService> GlasswallVersionServiceMock;
        protected Mock<IFileTypeDetector> FileTypeDetectorMock;
        protected Mock<IMetricService> MetricServiceMock;
        protected Mock<ILogger<FileTypeDetectionController>> LoggerMock;

        protected void CommonSetup()
        {
            GlasswallVersionServiceMock = new Mock<IGlasswallVersionService>();
            FileTypeDetectorMock = new Mock<IFileTypeDetector>();
            MetricServiceMock = new Mock<IMetricService>();
            LoggerMock = new Mock<ILogger<FileTypeDetectionController>>();

            ClassInTest = new FileTypeDetectionController(
                GlasswallVersionServiceMock.Object,
                FileTypeDetectorMock.Object,
                MetricServiceMock.Object,
                LoggerMock.Object
            );

            HttpTest = new HttpTest();
        }
    }
}