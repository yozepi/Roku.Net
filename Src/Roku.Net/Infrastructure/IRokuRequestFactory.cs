using System;
using System.Collections.Generic;
using System.Text;

namespace yozepi.Roku.Infrastructure
{
    internal interface IRokuRequestFactory
    {
        IRokuRequest Create();
    }
}
