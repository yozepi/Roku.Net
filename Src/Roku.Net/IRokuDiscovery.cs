using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace yozepi.Roku
{
    /// <summary>
    /// Used to discover Roku devices on your local network.
    /// See the Roku API documentation at https://sdkdocs.roku.com/display/sdkdoc/External+Control+API#ExternalControlAPI-SSDP(SimpleServiceDiscoveryProtocol)
    /// </summary>
    public interface IRokuDiscovery
    {
        /// <summary>
        /// Scans your local network and returns an IRokuRemote instance for each Roku found.
        /// </summary>
        /// <returns></returns>
        Task<IList<IRokuRemote>> DiscoverAsync();

        Task<IRokuRemote> DiscoverAsync(IPAddress rokuIp) ;

    }
}
