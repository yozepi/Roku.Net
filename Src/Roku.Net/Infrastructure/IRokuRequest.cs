using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace yozepi.Roku.Infrastructure    
{
    internal interface IRokuRequest
    {
        Task<IRokuResponse> GetResponseAsync(string url, string method);
    }
}
