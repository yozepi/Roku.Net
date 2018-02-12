using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace yozepi.Roku
{
    public class RokuScreenSaver
    {

        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("subtype")]
        public string SubType { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("version")]
        public string Version { get; set; }

        [XmlText]
        public string Text { get; set; }
    }
}
