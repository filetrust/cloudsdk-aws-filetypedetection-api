using System;
using Glasswall.CloudSdk.Common.Web.Models;
using NUnit.Framework;

namespace Glasswall.CloudSdk.AWS.FileTypeDetection.Tests.FileTypeDetectionControllerTests.DetermineFileTypeFromBase64Method
{
    [TestFixture]
    public class WhenRebuildThrows : FileTypeDetectionControllerTestBase
    {
        private Exception _dummyException;

        [OneTimeSetUp]
        public void OnetimeSetup()
        {
            CommonSetup();
            
            GlasswallVersionServiceMock.Setup(s => s.GetVersion())
                .Throws(_dummyException = new Exception());
        }

        [Test]
        public void Exception_Is_Rethrown()
        {
            Assert.That(() => ClassInTest.DetermineFileTypeFromBase64(new Base64Request
            {
                Base64 = "dGVzdA=="
            }), Throws.Exception.EqualTo(_dummyException));
        }
    }
}