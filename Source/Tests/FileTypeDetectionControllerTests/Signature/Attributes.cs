using System.Linq;
using System.Reflection;
using Glasswall.CloudSdk.AWS.FileTypeDetection.Controllers;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace Glasswall.CloudSdk.AWS.FileTypeDetection.Tests.FileTypeDetectionControllerTests.Signature
{
    [TestFixture]
    public class Attributes
    {
        [Test]
        public void Valid_Arguments_Should_Construct()
        {
            var attributes = typeof(FileTypeDetectionController).GetCustomAttributes().ToArray();

            Assert.That(attributes, Has.Exactly(2).Items);

            Assert.That(attributes,
                Has.Exactly(1)
                    .InstanceOf<RouteAttribute>()
                    .With
                    .Property(nameof(RouteAttribute.Template))
                    .EqualTo("api/[controller]"));

            Assert.That(attributes,
                Has.Exactly(1)
                    .InstanceOf<ControllerAttribute>());
        }
    }
}