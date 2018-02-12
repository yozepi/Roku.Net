using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace yozepi.Roku
{
    [XmlRoot("app")]
    public class RokuApp
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("type")]
        public string AppType { get; set; }

        [XmlAttribute("version")]
        public string Version { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    /// <summary>
    /// Used my infrastructure to deserialize the application list.
    /// </summary>
    [XmlRoot("apps")]
    public class RokuAppContainer
    {
        [XmlElement("app")]
        public List<RokuApp> Apps { get; set; }
    }
}
