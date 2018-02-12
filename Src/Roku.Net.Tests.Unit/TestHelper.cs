using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using yozepi.Roku.Infrastructure;

namespace Roku.Net.Tests.Unit
{
    internal class TestHelper
    {

        public static IRokuResponse BuildRokuResponse(string fileName = null, HttpStatusCode statusCode = HttpStatusCode.OK, string contentType = "text/xml")
        {
            Stream resourceStream = null;
            if (fileName != null)
            {
                var resourceName = $"Roku.Net.Tests.Unit.TestData.{fileName}";
                resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
                var assNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            }
            var response = new Mock<IRokuResponse>();
            response.Setup(m => m.StatusCode).Returns(statusCode);
            response.Setup(m => m.StatusDescription).Returns(statusCode.ToString);
            response.Setup(m => m.ContentType).Returns(contentType);
            response.Setup(m => m.GetResponseStream()).Returns(resourceStream);
            response.As<IDisposable>().Setup(m => m.Dispose());
            return response.Object;
        }

    }
}
