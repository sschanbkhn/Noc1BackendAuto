using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Network.Core.Helpers
{
    public static class XmlHelpers
    {
        public static string[] GetValuesOfAtribute(string xmlString, string xPath, string atribute)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);
            XmlNodeList nodes = doc.DocumentElement.SelectNodes(xPath);
            string[] rs = new string[nodes.Count];
            for(int i = 0;i < nodes.Count;i++)
            {
                rs[i] = nodes[i].Attributes[atribute].Value;
            }
            return rs;
        }
        public static string[] GetInnerTextsOfTag(string xmlString, string xPath)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);
            XmlNodeList nodes = doc.DocumentElement.SelectNodes(xPath);
            string[] rs = new string[nodes.Count];
            for (int i = 0; i < nodes.Count; i++)
            {
                rs[i] = nodes[i].InnerText;
            }
            return rs;
        }
    }
}
