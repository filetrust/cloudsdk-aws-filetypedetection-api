﻿using System;
using Glasswall.CloudSdk.Common.Web.Models;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace Glasswall.CloudSdk.AWS.FileTypeDetection.Tests.FileTypeDetectionControllerTests.DetermineFileTypeFromUrlMethod
{
    [TestFixture]
    public class WhenModelStateIsInvalid : FileTypeDetectionControllerTestBase
    {
        private IActionResult _result;
        private Uri _expectedInputUrl;

        [OneTimeSetUp]
        public void OnetimeSetup()
        {
            CommonSetup();

            _expectedInputUrl = new Uri("https://www.myfileserver.com/myfile.png");

            ClassInTest.ModelState.AddModelError("SomeError", "SomeMessage");
            _result = ClassInTest.DetermineFileTypeFromUrl(new UrlRequest
            {
                InputGetUrl = _expectedInputUrl
            });
        }

        [Test]
        public void Bad_Request_Is_Returned()
        {
            Assert.That(_result, Is.Not.Null);
            Assert.That(_result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public void Bad_Request_Contains_Errors()
        {
            var result = _result as BadRequestObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.InstanceOf<SerializableError>());
        }
        
        [Test]
        public void Error_Is_Expected()
        {
            var result = _result as BadRequestObjectResult;
            var responseBody = (SerializableError)result?.Value;
            Assert.That(responseBody, Has.One.With.Property("Key").EqualTo("SomeError"));
            Assert.That(responseBody, Has.One.With.Property("Value").Contains("SomeMessage"));
        }

        [Test]
        public void No_Engine_Actions_Are_Performed()
        {
            GlasswallVersionServiceMock.VerifyNoOtherCalls();
            FileTypeDetectorMock.VerifyNoOtherCalls();
        }

        [Test]
        public void No_Http_Calls_Were_Made()
        {
            HttpTest.ShouldNotHaveMadeACall();
        }
    }
}