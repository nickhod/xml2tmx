using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Xml2Tmx.Models
{
    [XmlRoot(ElementName = "Node")]
    public class TmxNode
    {
        public TmxNode()
        {
        }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }

        [XmlElement("Node")]
        public List<TmxNode> ChildNodes { get; set; }

        [XmlIgnore]
        public TmxNode Parent { get; set; }
    }
}
