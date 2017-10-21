// ========================================================================================
//  This file is part of the MSCRM ToolKit project.
//  https://github.com/zoranivanov/MSCRMToolKit
//  Author:         Zoran IVANOV
//  Created:        18/10/2012
//
//  Disclaimer:
//  This software is provided "as is" with no technical support.
//  Use it at your own risk.
//  The author does not take any responsibility for any damage in whatever form or context.
// ========================================================================================

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security;
using System.ServiceModel;
using System.Text;
using System.Xml;

namespace MSCRMToolKit
{
    /// <summary>
    /// MSCRM Data Export Manager class extending MSCRMToolKitProfileManager
    /// </summary>
    public class MSCRMDataExportManager : MSCRMToolKitProfileManager
    {
        /// <summary>
        /// The profiles
        /// </summary>
        public List<MSCRMDataExportProfile> Profiles = new List<MSCRMDataExportProfile>();
        /// <summary>
        /// The Workspace folder
        /// </summary>
        public string Folder = "DataExportManager";
        /// <summary>
        /// The configuration file name
        /// </summary>
        private string ConfigurationFileName = "DataExportManager\\DataExportProfiles.xml";
        /// <summary>
        /// The report file name
        /// </summary>
        public string ReportFileName;
        /// <summary>
        /// The exported data file name
        /// </summary>
        public string ExportedDataFileName;
        /// <summary>
        /// The exported records number
        /// </summary>
        public int ExportedRecordsNumber;
        /// <summary>
        /// The option set values
        /// </summary>
        private List<StringMap> OptionSetValues = new List<StringMap>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MSCRMDataExportManager"/> class.
        /// </summary>
        public MSCRMDataExportManager()
        {
            if (!Directory.Exists(Folder))
                Directory.CreateDirectory(Folder);
            ReadProfiles();
        }

        /// <summary>
        /// Creates the profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        public void CreateProfile(MSCRMDataExportProfile profile)
        {
            if (!Directory.Exists(Folder + "\\" + profile.ProfileName))
                Directory.CreateDirectory(Folder + "\\" + profile.ProfileName);

            //Creating new Profile
            Profiles.Add(profile);
            WriteProfiles();
        }

        /// <summary>
        /// Updates the profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <exception cref="System.Exception">Data Export Profile Update failed. The Data Export Profile  + profile.ProfileName +  was not found in the configuration file.</exception>
        public void UpdateProfile(MSCRMDataExportProfile profile)
        {
            if (!Directory.Exists(Folder + "\\" + profile.ProfileName))
                Directory.CreateDirectory(Folder + "\\" + profile.ProfileName);

            int index = Profiles.FindIndex(d => d.ProfileName == profile.ProfileName);
            if (index > -1)
            {
                Profiles[index] = profile;
            }
            else
            {
                LogManager.WriteLog("Data Export Profile Update failed. The Data Export Profile " + profile.ProfileName + " was not found in the configuration file.");
                throw new Exception("Data Export Profile Update failed. The Data Export Profile " + profile.ProfileName + " was not found in the configuration file.");
            }

            WriteProfiles();
        }

        /// <summary>
        /// Deletes the profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <exception cref="System.Exception">Data Export Profile deletion failed. The Data Export Profile  + profile.ProfileName +  was not found in the configuration file.</exception>
        public void DeleteProfile(MSCRMDataExportProfile profile)
        {
            int index = Profiles.FindIndex(d => d.ProfileName == profile.ProfileName);
            if (index > -1)
            {
                Profiles.RemoveAt(index);
            }
            else
            {
                LogManager.WriteLog("Data Export Profile deletion failed. The Data Export Profile " + profile.ProfileName + " was not found in the configuration file.");
                throw new Exception("Data Export Profile deletion failed. The Data Export Profile " + profile.ProfileName + " was not found in the configuration file.");
            }

            //Delete Profile folder
            try
            {
                if (Directory.Exists(Folder + "\\" + profile.ProfileName))
                    Directory.Delete(Folder + "\\" + profile.ProfileName, true);
            }
            catch (Exception)
            {
                throw;
            }

            //Save Profiles
            WriteProfiles();
        }

        /// <summary>
        /// Gets the profile.
        /// </summary>
        /// <param name="profileName">Name of the profile.</param>
        /// <returns>The MSCRM Data Export Profile</returns>
        public MSCRMDataExportProfile GetProfile(string profileName)
        {
            return Profiles.Find(d => d.ProfileName == profileName);
        }

        /// <summary>
        /// Runs the profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        public void RunProfile(MSCRMDataExportProfile profile)
        {
            LogManager.WriteLog("Running Data Export Profile: " + profile.ProfileName);
            DateTime now = DateTime.Now;
            ReportFileName = Folder + "\\" + profile.ProfileName + "\\ExecutionReports\\DataExportReport" + now.Year + "-" + now.Month + "-" + now.Day + "-" + now.Hour + "-" + now.Minute + "-" + now.Second + ".xml";

            //Initialize Execution Reports folder
            string executionReportsFolder = Folder + "\\" + profile.ProfileName + "\\ExecutionReports";
            if (!Directory.Exists(executionReportsFolder))
            {
                Directory.CreateDirectory(executionReportsFolder);
            }

            ExportedRecordsNumber = 0;

            //Create Data Export Report
            DataExportReport der = new DataExportReport(profile.ProfileName);
            WriteReport(der, ReportFileName);

            //Export data
            Export(profile, ReportFileName);

            DataExportReport report = ReadReport(ReportFileName);
            report.DataExportFinishedAt = DateTime.Now.ToString();
            report.DataExportCompleted = true;
            TimeSpan ExportTimeSpan = DateTime.Now - Convert.ToDateTime(report.DataExportStartedAt);
            report.DataExportedIn = ExportTimeSpan.ToString().Substring(0, 10);
            WriteReport(report, ReportFileName);
        }

        /// <summary>
        /// Reads the profiles.
        /// </summary>
        private void ReadProfiles()
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(List<MSCRMDataExportProfile>));
            if (!File.Exists(ConfigurationFileName))
            {
                FileStream writer = new FileStream(ConfigurationFileName, FileMode.Create);
                ser.WriteObject(writer, Profiles);
                writer.Close();
            }

            XmlDictionaryReaderQuotas XRQ = new XmlDictionaryReaderQuotas();
            XRQ.MaxStringContentLength = int.MaxValue;
            
            using (FileStream fs = new FileStream(ConfigurationFileName, FileMode.Open))
            using (XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, XRQ))
            {
                Profiles = (List<MSCRMDataExportProfile>)ser.ReadObject(reader, true);
            }

            foreach (MSCRMDataExportProfile profile in Profiles)
            {
                profile.setSourceConneciton();
            }
        }

        /// <summary>
        /// Writes the profiles.
        /// </summary>
        private void WriteProfiles()
        {
            FileStream writer = new FileStream(ConfigurationFileName, FileMode.Create);
            DataContractSerializer ser = new DataContractSerializer(typeof(List<MSCRMDataExportProfile>));
            ser.WriteObject(writer, Profiles);
            writer.Close();
        }

        /// <summary>
        /// Exports the specified profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <param name="DataExportReportFileName">Name of the data export report file.</param>
        private void Export(MSCRMDataExportProfile profile, string DataExportReportFileName)
        {
            try
            {
                DataExportReport report = new DataExportReport(DataExportReportFileName);
                //Get Data Export Report
                if (File.Exists(DataExportReportFileName))
                {
                    report = ReadReport(DataExportReportFileName);
                }

                //Set Data export folder
                string dataExportFolder = Folder + "\\" + profile.ProfileName + "\\Data";
                if (!Directory.Exists(dataExportFolder))
                {
                    Directory.CreateDirectory(dataExportFolder);
                }

                MSCRMConnection connection = profile.getSourceConneciton();
                _serviceProxy = cm.connect(connection);
                IOrganizationService service = (IOrganizationService)_serviceProxy;
                profile.TotalExportedRecords = 0;
                //Mesure export time
                DateTime exportStartDT = DateTime.Now;

                LogManager.WriteLog("Start exporting data from " + connection.ConnectionName);

                //Set the number of records per page to retrieve.
                //This value should not be bigger than 5000 as this is the limit of records provided by the CRM
                int fetchCount = 5000;
                // Initialize the file number.
                int fileNumber = 1;
                // Initialize the number of records.
                int recordsCount = 0;
                // Specify the current paging cookie. For retrieving the first page, pagingCookie should be null.
                string pagingCookie = null;
                string entityName = "";

                DateTime now = DateTime.Now;
                string fileName = Folder + "\\" + profile.ProfileName + "\\Data\\ExportedData";
                string fileExtension = profile.ExportFormat.ToLower();
                if (profile.ExportFormat.ToLower() == "xml spreadsheet 2003")
                    fileExtension = "xml";

                fileName += now.Year + "-" + now.Month + "-" + now.Day + "-" + now.Hour + "-" + now.Minute + "-" + now.Second + "." + fileExtension;
                this.ExportedDataFileName = fileName;

                while (true)
                {
                    // Build fetchXml string with the placeholders.
                    string xml = CreateXml(profile.FetchXMLQuery, pagingCookie, fileNumber, fetchCount);

                    StringReader stringReader = new StringReader(profile.FetchXMLQuery);
                    XmlTextReader reader = new XmlTextReader(stringReader);
                    // Load document
                    XmlDocument doc = new XmlDocument();
                    doc.Load(reader);
                    XmlNodeList xnl = doc.ChildNodes[0].ChildNodes[0].ChildNodes;

                    List<string> columns = new List<string>();
                    List<string> DisplayedColumns = new List<string>();
                    foreach (XmlNode sm in xnl)
                    {
                        if (sm.Name == "attribute")
                        {
                            columns.Add(sm.Attributes[0].Value);
                            if (profile.ExportFormat.ToLower() == "csv")
                                DisplayedColumns.Add(profile.DataSeparator + sm.Attributes[0].Value + profile.DataSeparator);
                            else if (profile.ExportFormat.ToLower() == "xml")
                                DisplayedColumns.Add(sm.Attributes[0].Value);
                            else if (profile.ExportFormat.ToLower() == "xml spreadsheet 2003")
                                DisplayedColumns.Add(sm.Attributes[0].Value);
                        }
                        else if (sm.Name == "link-entity")
                        {
                            //Linked entity
                            string linkedEntityAlias = sm.Attributes.GetNamedItem("alias").Value;
                            string linkedAttributeyName = sm.Attributes.GetNamedItem("to").Value;
                            XmlNodeList xnlLinkedEntity = sm.ChildNodes;
                            foreach (XmlNode linkedAttribute in xnlLinkedEntity)
                            {
                                //Check if this is not a filter
                                if (linkedAttribute.Name == "filter")
                                    continue;

                                columns.Add(linkedEntityAlias + "." + linkedAttribute.Attributes[0].Value);
                                if (profile.ExportFormat.ToLower() == "csv")
                                    DisplayedColumns.Add(profile.DataSeparator + linkedAttributeyName + "_" + linkedAttribute.Attributes[0].Value + profile.DataSeparator);
                                else if (profile.ExportFormat.ToLower() == "xml")
                                    DisplayedColumns.Add(linkedAttributeyName + "_" + linkedAttribute.Attributes[0].Value);
                                else if (profile.ExportFormat.ToLower() == "xml spreadsheet 2003")
                                    DisplayedColumns.Add(linkedAttributeyName + "_" + linkedAttribute.Attributes[0].Value);
                            }
                        }
                    }

                    // Execute the fetch query and get the xml result.
                    RetrieveMultipleRequest fetchRequest = new RetrieveMultipleRequest
                    {
                        Query = new FetchExpression(xml)
                    };

                    EntityCollection returnCollection = ((RetrieveMultipleResponse)_serviceProxy.Execute(fetchRequest)).EntityCollection;
                    recordsCount += returnCollection.Entities.Count;
                    if (recordsCount > 0)
                    {
                        if (profile.ExportFormat.ToLower() == "csv")
                            WriteCSV(returnCollection, fileName, columns, DisplayedColumns, profile);
                        else if (profile.ExportFormat.ToLower() == "xml")
                            WriteXML(returnCollection, fileName, columns, DisplayedColumns, profile);
                        else if (profile.ExportFormat.ToLower() == "xml spreadsheet 2003")
                            WriteXMLSpreadsheet2003(returnCollection, fileName, columns, DisplayedColumns, profile);
                    }
                    // Check for more records, if it returns 1.
                    if (returnCollection.MoreRecords)
                    {
                        // Increment the page number to retrieve the next page.
                        fileNumber++;
                        pagingCookie = returnCollection.PagingCookie;
                    }
                    else
                    {
                        // If no more records in the result nodes, exit the loop.
                        break;
                    }
                }

                Encoding encoding = GetEncoding(profile.Encoding);
                
                if (profile.ExportFormat.ToLower() == "xml")
                {
                    using (var writer = new StreamWriter(fileName, true, encoding))
                    {
                        writer.WriteLine("</Records>");
                        writer.Flush();
                    }
                }
                else if (profile.ExportFormat.ToLower() == "xml spreadsheet 2003")
                {
                    using (var writer = new StreamWriter(fileName, true, encoding))
                    {
                        writer.WriteLine("</Table></Worksheet></Workbook>\n");
                        writer.Flush();
                    }
                }

                LogManager.WriteLog("Exported " + recordsCount + " " + entityName + " records.");

                report.TotalExportedRecords = recordsCount;
                ExportedRecordsNumber = recordsCount;
                //Delete file if  no record found
                if (recordsCount < 1)
                    File.Delete(fileName);

                WriteReport(report, DataExportReportFileName);

                TimeSpan exportTimeSpan = DateTime.Now - exportStartDT;
                LogManager.WriteLog("Export finished for " + profile.ProfileName + ". Exported " + recordsCount + " records in " + exportTimeSpan.ToString().Substring(0, 10));
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                LogManager.WriteLog("Error:" + ex.Detail.Message + "\n" + ex.Detail.TraceText);
                throw;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    LogManager.WriteLog("Error:" + ex.Message + "\n" + ex.InnerException.Message);
                else
                    LogManager.WriteLog("Error:" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Writes the report.
        /// </summary>
        /// <param name="der">The der.</param>
        /// <param name="DataExportReportFileName">Name of the data export report file.</param>
        public void WriteReport(DataExportReport der, string DataExportReportFileName)
        {
            FileStream writer = new FileStream(DataExportReportFileName, FileMode.Create);
            DataContractSerializer ser = new DataContractSerializer(typeof(DataExportReport));
            ser.WriteObject(writer, der);
            writer.Close();
        }

        /// <summary>
        /// Reads the report.
        /// </summary>
        /// <param name="DataExportReportFileName">Name of the data export report file.</param>
        /// <returns></returns>
        public DataExportReport ReadReport(string DataExportReportFileName)
        {
            if (File.Exists(DataExportReportFileName))
            {
                XmlDictionaryReaderQuotas XRQ = new XmlDictionaryReaderQuotas();
                XRQ.MaxStringContentLength = int.MaxValue;

                using (FileStream fs = new FileStream(DataExportReportFileName, FileMode.Open))
                using (XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, XRQ))
                {
                    DataContractSerializer ser = new DataContractSerializer(typeof(DataExportReport));
                    DataExportReport der = (DataExportReport)ser.ReadObject(reader, true);
                    return der;
                }                
            }
            return null;
        }

        /// <summary>
        /// Writes the CSV.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="path">The path.</param>
        /// <param name="columns">The columns.</param>
        /// <param name="DisplayedColumns">The displayed columns.</param>
        /// <param name="profile">The profile.</param>
        public void WriteCSV(EntityCollection items, string path, List<string> columns, List<string> DisplayedColumns, MSCRMDataExportProfile profile)
        {
            bool writeColumns = false;
            if (!File.Exists(path))
                writeColumns = true;

            Encoding encoding = System.Text.Encoding.Default;
            if (profile.Encoding == "UTF8")
                encoding = System.Text.Encoding.UTF8;
            else if (profile.Encoding == "Unicode")
                encoding = System.Text.Encoding.Unicode;
            else if (profile.Encoding == "ASCII")
                encoding = System.Text.Encoding.ASCII;
            else if (profile.Encoding == "BigEndianUnicode")
                encoding = System.Text.Encoding.BigEndianUnicode;

            using (var writer = new StreamWriter(path, true, encoding))
            {
                if (writeColumns)
                    writer.WriteLine(string.Join(profile.FieldSeparator, DisplayedColumns));

                foreach (Entity e in items.Entities)
                {
                    List<string> values = new List<string>();
                    foreach (string column in columns)
                    {
                        string value = getFormattedValue(e, column);

                        if (value != null && value != "")
                        {
                            //Escape Data
                            if (profile.DataSeparator != "")
                                value = value.Replace(profile.DataSeparator, profile.DataSeparator + profile.DataSeparator);
                            else if (value.Contains(profile.FieldSeparator))
                                value = "\"" + value.Replace("\"", "\"\"") + "\"";

                            values.Add(profile.DataSeparator + value + profile.DataSeparator);
                        }
                        else
                        {
                            values.Add("");
                        }
                    }

                    writer.WriteLine(string.Join(profile.FieldSeparator, values));
                }
                writer.Flush();
            }
        }

        /// <summary>
        /// Writes the XML.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="path">The path.</param>
        /// <param name="columns">The columns.</param>
        /// <param name="DisplayedColumns">The displayed columns.</param>
        /// <param name="profile">The profile.</param>
        public void WriteXML(EntityCollection items, string path, List<string> columns, List<string> DisplayedColumns, MSCRMDataExportProfile profile)
        {
            bool writeColumns = false;
            if (!File.Exists(path))
                writeColumns = true;

            Encoding encoding = GetEncoding(profile.Encoding);

            using (var writer = new StreamWriter(path, true, encoding))
            {
                if (writeColumns)
                {
                    writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    writer.WriteLine("<?mso-application progid=\"Excel.Sheet\"?>");
                    writer.WriteLine("<Records>");
                }

                foreach (Entity e in items.Entities)
                {
                    string row = "<row>";
                    int cCpt = 0;
                    List<string> values = new List<string>();
                    foreach (string column in columns)
                    {
                        string value = getFormattedValue(e, column);
                        row += "<" + DisplayedColumns[cCpt] + ">" + SecurityElement.Escape(value) + "</" + DisplayedColumns[cCpt] + ">";
                        cCpt++;
                    }
                    row += "</row>";
                    writer.WriteLine(row);
                }
                writer.Flush();
            }
        }

        /// <summary>
        /// Writes the XML spreadsheet 2003.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="path">The path.</param>
        /// <param name="columns">The columns.</param>
        /// <param name="DisplayedColumns">The displayed columns.</param>
        /// <param name="profile">The profile.</param>
        public void WriteXMLSpreadsheet2003(EntityCollection items, string path, List<string> columns, List<string> DisplayedColumns, MSCRMDataExportProfile profile)
        {
            bool writeColumns = false;
            if (!File.Exists(path))
                writeColumns = true;

            Encoding encoding = GetEncoding(profile.Encoding);

            using (var writer = new StreamWriter(path, true, encoding))
            {
                if (writeColumns)
                {
                    writer.WriteLine("<?xml version=\"1.0\"?>\n");
                    writer.WriteLine("<?mso-application progid=\"Excel.Sheet\"?>\n");
                    writer.WriteLine("<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\" ");
                    writer.WriteLine("xmlns:o=\"urn:schemas-microsoft-com:office:office\" ");
                    writer.WriteLine("xmlns:x=\"urn:schemas-microsoft-com:office:excel\" ");
                    writer.WriteLine("xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\" ");
                    writer.WriteLine("xmlns:html=\"http://www.w3.org/TR/REC-html40\">\n");
                    writer.WriteLine("<DocumentProperties xmlns=\"urn:schemas-microsoft-com:office:office\">");
                    writer.WriteLine("<Author>MSCRMToolkit</Author>");
                    writer.WriteLine("</DocumentProperties>");
                    writer.WriteLine("<ExcelWorkbook xmlns=\"urn:schemas-microsoft-com:office:excel\">\n");
                    writer.WriteLine("<ProtectStructure>False</ProtectStructure>\n");
                    writer.WriteLine("<ProtectWindows>False</ProtectWindows>\n");
                    writer.WriteLine("</ExcelWorkbook>\n");
                    writer.WriteLine("<Worksheet ss:Name=\"WorkSheet1\">");
                    writer.WriteLine("<Table>");
                    string header = "<Row>";
                    foreach (string displayedColumn in DisplayedColumns)
                    {
                        header += "<Cell><Data ss:Type=\"String\">" + SecurityElement.Escape(displayedColumn) + "</Data></Cell>";
                    }
                    header += "</Row>";
                    writer.WriteLine(header);
                }

                foreach (Entity e in items.Entities)
                {
                    string row = "<Row>";
                    int cCpt = 0;
                    List<string> values = new List<string>();
                    foreach (string column in columns)
                    {
                        string value = getFormattedValue(e, column);
                        row += "<Cell><Data ss:Type=\"String\">" + SecurityElement.Escape(value) + "</Data></Cell>";
                        cCpt++;
                    }
                    row += "</Row>";
                    writer.WriteLine(row);
                }
                writer.Flush();
            }
        }

        /// <summary>
        /// Gets the formatted value.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns></returns>
        private string getFormattedValue(Entity entity, string attributeName)
        {
            KeyValuePair<string, object> kvp = new KeyValuePair<string, object>();
            KeyValuePair<string, string> formattedValue = new KeyValuePair<string, string>();
            string value = "";
            if (entity.FormattedValues.Keys.Contains(attributeName))
            {
                formattedValue = entity.FormattedValues.First(k => k.Key == attributeName);
                value = formattedValue.Value;
            }
            else if (entity.Attributes.Keys.Contains(attributeName))
            {
                kvp = entity.Attributes.First(k => k.Key == attributeName);

                Type t = kvp.Value.GetType();
                if (t.Name == "EntityReference")
                {
                    EntityReference er = (EntityReference)kvp.Value;
                    if (er.Name != null)
                        value = er.Name;
                }
                else if (t.Name == "AliasedValue")
                {
                    AliasedValue av = (AliasedValue)kvp.Value;
                    Type t2 = av.Value.GetType();
                    if (t2.Name == "EntityReference")
                    {
                        EntityReference er2 = (EntityReference)av.Value;
                        if (er2.Name != null)
                            value = er2.Name;
                    }
                    else
                        value = av.Value.ToString();
                }
                else if (t.Name == "OptionSetValue")
                {
                    OptionSetValue osv = (OptionSetValue)kvp.Value;
                    value = osv.Value.ToString();
                }
                else if (attributeName == "entityimage" && t.Name == "Byte[]")
                {
                    byte[] binaryData = (byte[])kvp.Value;
                    value = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);
                }
                else
                {
                    value = kvp.Value.ToString();
                }
            }
            return value;
        }

        /// <summary>
        /// Get encoding from a string
        /// </summary>
        /// <param name="encodingString">Encoding string</param>
        /// <returns>Encoding</returns>
        private Encoding GetEncoding(string encodingString)
        {
            Encoding encoding = System.Text.Encoding.Default;
            if (encodingString == "UTF8")
                encoding = System.Text.Encoding.UTF8;
            else if (encodingString == "Unicode")
                encoding = System.Text.Encoding.Unicode;
            else if (encodingString == "ASCII")
                encoding = System.Text.Encoding.ASCII;
            else if (encodingString == "BigEndianUnicode")
                encoding = System.Text.Encoding.BigEndianUnicode;
            return encoding;
        }
    }
}