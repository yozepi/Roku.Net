using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Roku.Net.Tests.Unit;
using Rssdp;
using Rssdp.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using yozepi.Roku;
using yozepi.Roku.Infrastructure;
using FluentAssertions;

namespace Roku.Net.Tests.Unit.specs
{
    [TestClass]
    public class RokuDiscovery_specs : nSpecTestHarness
    {
        [TestMethod]
        public void RokuDiscoverySpecs()
        {
            this.RunMySpecs();
        }

        RokuDiscovery subject;

        Mock<ISsdpDeviceLocator> deviceLocator = null;
        Mock<IRokuRequestFactory> requestFactory = null;

        string roku1 = "http://198.1.0.1";
        string roku2 = "http://198.1.0.2";

        void DiscoverAsync_Method()
        {
            IList<IRokuRemote> results = null;

            it["should return a list of roku devices discovered on the local network"] = () =>
            {
                results.Count.Should().NotBe(0);
            };

            actAsync = async () =>
            {
                subject = new RokuDiscovery(() => deviceLocator.Object, requestFactory.Object);
                results = await subject.DiscoverAsync();
            };

            before = () =>
            {
                InitializeMocks();
            };
        }

        void InitializeMocks()
        {

            var discoveredRokus = new List<DiscoveredSsdpDevice>
                        {
                            new DiscoveredSsdpDevice
                            {
                                DescriptionLocation = new Uri(roku1),
                                Usn = RokuDiscovery.RokuSSDPSearchTarget
                            },
                            new DiscoveredSsdpDevice
                            {
                                DescriptionLocation = new Uri(roku2),
                                Usn = RokuDiscovery.RokuSSDPSearchTarget
                            }
                        };

            deviceLocator = new Mock<ISsdpDeviceLocator>();
            deviceLocator.Setup(m => m.SearchAsync(RokuDiscovery.RokuSSDPSearchTarget))
                .ReturnsAsync(() => discoveredRokus);
            deviceLocator.As<IDisposable>().Setup(m => m.Dispose());


            var rokuRequest = new Mock<IRokuRequest>();
            foreach (var roku in discoveredRokus)
            {
                rokuRequest.Setup(m => m.GetResponseAsync(UrlUtils.DeviceInfoUrlFor(roku.DescriptionLocation), "GET"))
                    .ReturnsAsync(() => TestHelper.BuildRokuResponse("DeviceInfo.xml"));

                rokuRequest.Setup(m => m.GetResponseAsync(UrlUtils.AppsUrlFor(roku.DescriptionLocation), "GET"))
                    .ReturnsAsync(() => TestHelper.BuildRokuResponse("Apps.xml"));
            }

            requestFactory = new Mock<IRokuRequestFactory>();
            requestFactory.Setup(m => m.Create())
                .Returns(() => rokuRequest.Object);
        }
    }
}
