using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using yozepi.Roku.Infrastructure;

namespace yozepi.Roku
{
    /// <summary>
    /// The result from the call to GetAppIconAsync.
    /// See the Roku API documentation at https://sdkdocs.roku.com/display/sdkdoc/External+Control+API#ExternalControlAPI-query/iconExample
    /// 
    /// </summary>
    public class AppIcon: ICommandResponse
    {
        internal AppIcon(int appId, string contentType, byte[] image, IRokuResponse rokuResponse)
            :this(rokuResponse)
        {
            this.AppId = appId;
            this.ContentType = contentType;
            this.Image = image;
        }

        internal AppIcon(IRokuResponse rokuResponse)
        {
            IsSuccess = rokuResponse.StatusCode == HttpStatusCode.OK;
            StatusCode = rokuResponse.StatusCode;
            StatusDescription = rokuResponse.StatusDescription;
        }

        /// <summary>
        /// The Roku application Id for the application.
        /// </summary>
        public int AppId { get; private set; }

        /// <summary>
        /// The Content type of the returned image.
        /// </summary>
        public string ContentType { get; private set; }

        /// <summary>
        /// A byte array containing the image data.
        /// </summary>
        public byte[] Image { get; private set; }

        public bool IsSuccess { get; internal set; }

        public HttpStatusCode StatusCode { get; internal set; }

        public string StatusDescription { get; internal set; }
    }
}
