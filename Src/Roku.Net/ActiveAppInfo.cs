using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Xml.Serialization;

namespace yozepi.Roku
{
    /// <summary>
    /// The result from the call to GetActiveAppAsync.
    /// See the Roku API documentation at https://sdkdocs.roku.com/display/sdkdoc/External+Control+API#ExternalControlAPI-query/active-app
    /// </summary>
    [XmlRoot("active-app")]
    public class ActiveAppInfo: ICommandResponse
    {
        /// <summary>
        /// The information about the currently app currently running on the Roku.
        /// </summary>
        /// <remarks>
        /// When the Roku is on the Home screen App.Id will be 0 (zero) and App.Text will be "Roku".
        /// </remarks>
        [XmlElement("app")]
        public RokuApp App { get; set; }

        /// <summary>
        /// When not null, this property contains information about the current screensaver.
        /// </summary>
        [XmlElement("screensaver")]
        public RokuScreenSaver ScreenSaver { get; set; }

        [XmlIgnore]
        public bool IsSuccess { get; internal set; }

        [XmlIgnore]
        public HttpStatusCode StatusCode { get; internal set; }

        [XmlIgnore]
        public string StatusDescription { get; internal set; }
    }
}
