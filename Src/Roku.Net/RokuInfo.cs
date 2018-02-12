using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace yozepi.Roku
{
    [XmlRoot("device-info")]
    public class RokuInfo
    {
        [XmlElement("udn")]
        public string Udn { get; set; }

        [XmlElement("serial-number")]
        public string SerialNumber { get; set; }

        [XmlElement("device-id")]
        public string Id { get; set; }

        [XmlElement("model-number")]
        public string ModelNumber { get; set; }

        [XmlElement("wifi-mac")]
        public string WifiMacAddress { get; set; }

        [XmlElement("user-device-name")]
        public string UserDeviceName { get; set; }

        [XmlElement("network-type")]
        public string NetworkType { get; set; }

        [XmlElement("software-version")]
        public string SoftwareVersion { get; set; }

        [XmlElement("software-build")]
        public string SoftwareBuild { get; set; }

        [XmlElement("secure-device")]
        public bool IsSecureDevice { get; set; }

        [XmlElement("language")]
        public string Language { get; set; }

        [XmlElement("country")]
        public string Country { get; set; }

        [XmlElement("locale")]
        public string Locale { get; set; }

        [XmlElement("time-zone")]
        public string TimeZone { get; set; }

        [XmlElement("time-zone-offset")]
        public int TimeZoneOffset { get; set; }

        [XmlElement("power-mode")]
        public string PowerMode { get; set; }

        [XmlElement("supports-suspend")]
        public bool SupportsSuspend { get; set; }

        [XmlElement("supports-find-remote")]
        public bool SupportsFindRemote { get; set; }

        [XmlElement("supports-audio-guide")]
        public bool SupportsAudioGuide { get; set; }

        [XmlElement("developer-enabled")]
        public bool IsDeveloperEnabled { get; set; }

        [XmlElement("keyed-developer-id")]
        public string KeyedDeveloperId { get; set; }

        [XmlElement("search-enabled")]
        public bool IsSearchEnabled { get; set; }

        [XmlElement("voice-search-enabled")]
        public bool IsVoiceSearchEnabled { get; set; }

        [XmlElement("notifications-enabled")]
        public bool IsNotificationsEnabled { get; set; }

        [XmlElement("notifications-first-use")]
        public bool NotificationsFirstUse { get; set; }

        [XmlElement("supports-private-listening")]
        public bool SupportsPrivateListening { get; set; }

        [XmlElement("headphones-connected")]
        public bool IsHeadphonesConnected { get; set; }

    }
}
