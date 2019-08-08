using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xml2Tmx.Models;

namespace Xml2Tmx.Parsers
{
    public class TmxParser
    {
        private string typeBuffer;
        private string nameBuffer;
        private string valueBuffer;

        private TmxNode currentNode;

        private TmxNode rootNode;

        private int nodeDepth;
        private int charIndex;

        private int nodeOpenBranceCount;

        private string tmxText;

        public TmxNode ParseTmx(string tmx)
        {

            tmxText = tmx;
            rootNode = null;

            nodeDepth = 0;

            for (charIndex = 0; charIndex < tmx.Length; charIndex++)
            {
                switch (tmx[charIndex])
                {
                    case '<':
                        StartNode();
                        break;
                    case '>':
                        EndNode();
                        break;
                    case '[':
                        StartAttribute();
                        break;
                    case ']':
                        EndAttribute();
                        break;
                }
            }

            return rootNode;
        }

        private void StartNode()
        {
            //Console.WriteLine("StartNode");

            var node = new TmxNode();
            
            // If we have a current node, this new node
            // will have it as its parent
            if (currentNode != null)
            {
                node.Parent = currentNode;
            }

            // If there is no root node, this new node is the root node
            if (rootNode == null)
            {
                rootNode = node;
                currentNode = rootNode;
            }
            // If we already have a root node, 
            else
            {
                if (currentNode.ChildNodes ==null)
                {
                    currentNode.ChildNodes = new List<TmxNode>();
                }

                currentNode.ChildNodes.Add(node);
                currentNode = node;

            }

            nodeDepth++;
            nodeOpenBranceCount = 0;
        }

        private void EndNode()
        {
            //Console.WriteLine("EndNode");

            // Move up the hierarchy
            currentNode = currentNode.Parent;

            nodeDepth--;
        }

        private void StartAttribute()
        {
            //Console.WriteLine("StartAttribute");
            nodeOpenBranceCount++;

            char currentChar = ' ';

            var sb = new StringBuilder();
            

            do
            {
                charIndex = charIndex + 1;
                currentChar = tmxText[charIndex];

                if (currentChar != ']')
                {
                    sb.Append(currentChar);
                }

            } while (currentChar != ']');


            var finalAttribute = sb.ToString().Trim();
            string replaceWith = " ";
            finalAttribute = finalAttribute.Replace("\r\n", replaceWith).Replace("\n", replaceWith).Replace("\r", replaceWith);
            finalAttribute = Regex.Replace(finalAttribute, @"\s+", " ");

            switch (nodeOpenBranceCount)
            {
                case 1:
                    currentNode.Type = finalAttribute;
                    break;
                case 2:
                    currentNode.Name = finalAttribute;
                    break;
                case 3:
                    currentNode.Value = finalAttribute;
                    break;
            }
        }

        private void EndAttribute()
        {
            //Console.WriteLine("EndAttribute");

        }
    }
}
