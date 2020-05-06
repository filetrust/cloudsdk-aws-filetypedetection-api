using System;
using System.Net;
using System.Net.Http;
using Glasswall.CloudSdk.Common.Web.Models;
using NUnit.Framework;

namespace Glasswall.CloudSdk.AWS.FileTypeDetection.Tests.FileTypeDetectionControllerTests.DetermineFileTypeFromUrlMethod
{
    [TestFixture]
    public class WhenEngineThrows : FileTypeDetectionControllerTestBase
    {
        private Exception _dummyException;

        [OneTimeSetUp]
        public void OnetimeSetup()
        {
            CommonSetup();
            
            _dummyException = new Exception();

            GlasswallVersionServiceMock.Setup(s => s.GetVersion())
                .Throws(_dummyException = new Exception());

            HttpTest.ResponseQueue.Enqueue(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new ByteArrayContent(new byte[] { 0x00 })
            });
        }

        [Test]
        public void Exception_Is_Rethrown()
        {
            Assert.That(() => ClassInTest.DetermineFileTypeFromUrl(new UrlRequest
            {
                InputGetUrl = new Uri("https://www.input.com"),
            }), Throws.Exception.EqualTo(_dummyException));
        }
    }
}