using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Xml2Tmx.Common;
using Xml2Tmx.Models;

namespace Xml2Tmx.Writers
{
    public class TmxWriter
    {
        private int nodeDepth;
        private int indnentSpaces = 4;

        public string WriteTmxDocument(string inputXmlFilename)
        {
            var xDocument = XDocument.Load(inputXmlFilename);
            XIncludeExtentions.ReplaceXIncludes(xDocument);

            var finalXml = xDocument.ToString();


            XmlSerializer xmlSerializer = new XmlSerializer(typeof(TmxNode));

            var result = String.Empty;

            using (TextReader textReader = new StringReader(finalXml))
            {
                TmxNode document = (TmxNode)xmlSerializer.Deserialize(textReader);

                result = WriteTmxDocument(document);

            }

            return result;
        }

        public string WriteTmxDocument(TmxNode document)
        {
            var sb = new StringBuilder();

            nodeDepth = 0;

            OutputNode(document, sb);

            return sb.ToString();
        }

        private void OutputNode(TmxNode node, StringBuilder sb)
        {
            // If the node is an include node, we don't write anything
            // but do descend into its children
            if (node.Type != null && node.Type.ToLower() == "include")
            {
                if (node.ChildNodes != null)
                {
                    foreach (var childNode in node.ChildNodes)
                    {
                        OutputNode(childNode, sb);
                    }
                }
            }
            // A normal node, so we need to output
            else
            {
                sb.AppendFormat("{0}<[{1}][{2}][{3}]", new String(' ', indnentSpaces * nodeDepth), node.Type, node.Name, node.Value);

                if (node.ChildNodes != null)
                {
                    int count = node.ChildNodes.Count;

                    // If this node has child nodes start a new line
                    if (node.ChildNodes != null && node.ChildNodes.Count > 0)
                    {
                        sb.Append(Environment.NewLine);
                    }

                    nodeDepth++;
                    foreach (var childNode in node.ChildNodes)
                    {
                        OutputNode(childNode, sb);
                    }
                }

                nodeDepth--;

                if (node.ChildNodes != null && node.ChildNodes.Count > 0)
                {
                    sb.AppendFormat("{0}>", new String(' ', indnentSpaces * nodeDepth));
                }
                else
                {
                    sb.Append(">");
                }

                // New line as we're starting a new node
                sb.Append(Environment.NewLine);
            }


        }


    }
}
