using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using yozepi.Roku.Infrastructure;

namespace yozepi.Roku
{
    public class CommandResponse : ICommandResponse
    {
        internal CommandResponse(IRokuResponse rokuResponse)
        {
            IsSuccess = rokuResponse.StatusCode == HttpStatusCode.OK;
            StatusCode = rokuResponse.StatusCode;
            StatusDescription = rokuResponse.StatusDescription;
        }

        public bool IsSuccess { get; }
        public HttpStatusCode StatusCode { get; }
        public string StatusDescription { get; }

    }
}
