using System;
using System.IO;
using System.Net;

namespace yozepi.Roku.Infrastructure
{
    internal interface IRokuResponse :IDisposable
    {
        HttpStatusCode StatusCode { get; }
        string StatusDescription { get; }
        string ContentType { get; }
        Stream GetResponseStream();
    }
}