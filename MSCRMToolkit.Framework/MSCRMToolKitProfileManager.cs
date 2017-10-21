using System.IO;
using System.Text;
using System.Xml;
using Microsoft.Xrm.Sdk.Client;

namespace MSCRMToolKit
{
    /// <summary>
    /// MSCRMToolKitProfileManager class
    /// </summary>
    public class MSCRMToolKitProfileManager
    {
        /// <summary>
        /// The Connections Manager
        /// </summary>
        protected MSCRMConnectionsManager cm = new MSCRMConnectionsManager();
        /// <summary>
        /// The CRM service proxy
        /// </summary>
        public OrganizationServiceProxy _serviceProxy;
        /// <summary>
        /// Creates the FetchXML Query.
        /// </summary>
        /// <param name="xml">The FetchXMLQuery.</param>
        /// <param name="cookie">The paging cookie.</param>
        /// <param name="page">The page number.</param>
        /// <param name="count">The records per page count.</param>
        /// <returns>Formatted FechXML Query</returns>
        protected string CreateXml(string xml, string cookie, int page, int count)
        {
            StringReader stringReader = new StringReader(xml);
            XmlTextReader reader = new XmlTextReader(stringReader);

            // Load document
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);

            XmlAttributeCollection attrs = doc.DocumentElement.Attributes;

            if (cookie != null)
            {
                XmlAttribute pagingAttr = doc.CreateAttribute("paging-cookie");
                pagingAttr.Value = cookie;
                attrs.Append(pagingAttr);
            }

            XmlAttribute pageAttr = doc.CreateAttribute("page");
            pageAttr.Value = System.Convert.ToString(page);
            attrs.Append(pageAttr);

            XmlAttribute countAttr = doc.CreateAttribute("count");
            countAttr.Value = System.Convert.ToString(count);
            attrs.Append(countAttr);

            StringBuilder sb = new StringBuilder(1024);
            StringWriter stringWriter = new StringWriter(sb);

            XmlTextWriter writer = new XmlTextWriter(stringWriter);
            doc.WriteTo(writer);
            writer.Close();

            return sb.ToString();
        }
    }
}
