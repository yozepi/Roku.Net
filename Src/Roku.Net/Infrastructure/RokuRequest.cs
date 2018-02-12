using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace yozepi.Roku.Infrastructure
{
    internal class RokuRequest : IRokuRequest
    {
        public async Task<IRokuResponse> GetResponseAsync(string url, string method)
        {
            var request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = method;

            return new RokuResponse(await request.GetResponseAsync() as HttpWebResponse);

        }
    }
}
