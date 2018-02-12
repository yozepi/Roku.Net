using System;
using System.Collections.Generic;
using System.Text;

namespace yozepi.Roku.Infrastructure
{
    internal class RokuRequestFactory : IRokuRequestFactory
    {
        public IRokuRequest Create()
        {
            return new RokuRequest();
        }
    }
}
