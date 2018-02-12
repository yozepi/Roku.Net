using Rssdp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using Rssdp.Infrastructure;
using yozepi.Roku.Infrastructure;

namespace yozepi.Roku
{
    /// <summary>
    /// Used to discover Roku devices on your local network.
    /// See the Roku API documentation at https://sdkdocs.roku.com/display/sdkdoc/External+Control+API#ExternalControlAPI-SSDP(SimpleServiceDiscoveryProtocol)
    /// </summary>
    public class RokuDiscovery : IRokuDiscovery
    {
        internal const string RokuSSDPSearchTarget = "roku:ecp";

        private Func<ISsdpDeviceLocator> _ssdpDeviceLocatorFactory;
        private IRokuRequestFactory _requestFactory;
        private Uri[] rokuIps;

        public RokuDiscovery()
            : this(() => new SsdpDeviceLocator(), new RokuRequestFactory())
        { }

        //This constructor is used internally for unit testing.
        internal RokuDiscovery(Func<ISsdpDeviceLocator> ssdpDeviceLocatorFactory, IRokuRequestFactory requestFactory)
        {
            _ssdpDeviceLocatorFactory = ssdpDeviceLocatorFactory;
            _requestFactory = requestFactory;
        }


        /// <summary>
        /// Scans your local network and returns an IRokuRemote instance for each Roku found.
        /// </summary>
        /// <returns></returns>
        public async Task<IList<IRokuRemote>> DiscoverAsync()
        {
            Uri[] rokuUris = null;
            var deviceLocator = _ssdpDeviceLocatorFactory();
            try
            {
                var foundDevices = await deviceLocator.SearchAsync(); // Can pass search arguments here (device type, uuid). No arguments means all devices.
                rokuIps = foundDevices
                    .Where(d => d.Usn == RokuSSDPSearchTarget)
                    .Select(d => d.DescriptionLocation).ToArray();
            }

            finally
            {
                var asDisposable = deviceLocator as IDisposable;
                if (asDisposable != null)
                    asDisposable.Dispose();
            }

            var rokus = new List<IRokuRemote>();
            foreach (var rokuIp in rokuIps)
            {

                var rokuRemote = await BuildRokuDataAsync(rokuIp);
                if (rokuRemote != null)
                {
                    rokus.Add(rokuRemote);
                }
            }

            return rokus;
        }

        public async Task<IRokuRemote> DiscoverAsync(IPAddress rokuIp)
        {
            if (rokuIp == null)
                throw new ArgumentNullException("rokuIp");

            var rokuUri = new Uri($"http://{rokuIp}:8060", UriKind.Absolute);
            var rokuRemote = await BuildRokuDataAsync(rokuUri);
            return rokuRemote;
        }


        private async Task<IRokuRemote> BuildRokuDataAsync(Uri rokuUri)
        {
            var infoRequest = _requestFactory.Create();
            var appsRequest = _requestFactory.Create();

            using (var infoResponse = await infoRequest.GetResponseAsync(UrlUtils.DeviceInfoUrlFor(rokuUri), "GET"))
            using (var appsResponse = await appsRequest.GetResponseAsync(UrlUtils.AppsUrlFor(rokuUri), "GET"))
            {
                if (infoResponse.StatusCode != HttpStatusCode.OK || appsResponse.StatusCode != HttpStatusCode.OK)
                {
                    return null; //TODO: throw?
                }

                var result = new RokuRemote(
                    requestFactory: _requestFactory,
                    uri: rokuUri,
                    info: ConversionUtils.XmlToInstance<RokuInfo>(infoResponse.GetResponseStream()),
                    apps: ConversionUtils.XmlToInstance<RokuAppContainer>(appsResponse.GetResponseStream()).Apps
                );
                return result;
            }
        }
    }
}
