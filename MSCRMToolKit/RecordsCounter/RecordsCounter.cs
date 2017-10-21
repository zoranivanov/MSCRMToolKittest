// ========================================================================================
//  This file is part of the MSCRM ToolKit project.
//  https://github.com/zoranivanov/MSCRMToolKit
//  Author:         Zoran IVANOV
//  Created:        01/07/2012
//
//  Disclaimer:
//  This software is provided "as is" with no technical support.
//  Use it at your own risk.
//  The author does not take any responsibility for any damage in whatever form or context.
// ========================================================================================

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace MSCRMToolKit
{
    /// <summary>
    /// Records Counter class
    /// </summary>
    public partial class RecordsCounter : Form
    {
        /// <summary>
        /// The CRM service proxy
        /// </summary>
        private OrganizationServiceProxy _serviceProxy = null;
        /// <summary>
        /// The Connections Manager
        /// </summary>
        private MSCRMConnectionsManager cm = new MSCRMConnectionsManager();
        /// <summary>
        /// The entities names
        /// </summary>
        private List<List<string>> entitiesNames = new List<List<string>>();
        /// <summary>
        /// All entities
        /// </summary>
        private List<RecordLine> allEntities = new List<RecordLine>();
        /// <summary>
        /// The selected entities
        /// </summary>
        public List<RecordLine> selectedEntities = new List<RecordLine>();
        /// <summary>
        /// The exported file path
        /// </summary>
        private string exportedFilePath = "";
        /// <summary>
        /// The Workspace folder
        /// </summary>
        public string Folder = "RecordsCounter";

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordsCounter"/> class.
        /// </summary>
        public RecordsCounter()
        {
            InitializeComponent();
            if (!Directory.Exists(Folder))
                Directory.CreateDirectory(Folder);
            LogManager.WriteLog("Records Counter launched.");

            foreach (MSCRMConnection connection in cm.MSCRMConnections)
            {
                this.comboBoxConnectionSource.Items.AddRange(new object[] { connection.ConnectionName });
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the comboBoxConnectionSource control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void comboBoxConnectionSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            openInExcelToolStripMenuItem.Visible = false;
            if (comboBoxConnectionSource.SelectedItem == null)
            {
                return;
            }

            string filename = Folder + "\\" + comboBoxConnectionSource.SelectedItem.ToString() + ".xml";
            if (File.Exists(filename))
            {
                this.entitiesNames = ReadEnvStructure(comboBoxConnectionSource.SelectedItem.ToString());
                foreach (List<string> entity in this.entitiesNames)
                {
                    allEntities.Add(new RecordLine { Select = false, Entity = entity[0], Filter = "", AttribuiteIdName = entity[1] });
                }

                foreach (RecordLine re in allEntities)
                {
                    checkedListBoxEntities.Items.AddRange(new object[] { re.Entity });
                }

                toolStripStatusLabel1.Text = "Entities loaded from disc.";
            }
            else
            {
                toolStripStatusLabel1.Text = "This source's structure was never loaded. You must load the entities.";
            }
        }

        /// <summary>
        /// Downloads the env structure.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns></returns>
        private List<List<string>> downloadEnvStructure(string connectionName)
        {
            try
            {
                toolStripStatusLabel1.Text = "Loading entities from source. Please wait...";
                Application.DoEvents();
                this.entitiesNames = new List<List<string>>();
                MSCRMConnection connection = cm.getConnection(connectionName);
                _serviceProxy = cm.connect(connection);
                IOrganizationService service = (IOrganizationService)_serviceProxy;
                RetrieveAllEntitiesRequest request = new RetrieveAllEntitiesRequest()
                {
                    EntityFilters = EntityFilters.Entity,
                    RetrieveAsIfPublished = false
                };

                // Retrieve the MetaData.
                RetrieveAllEntitiesResponse AllEntitiesResponse = (RetrieveAllEntitiesResponse)_serviceProxy.Execute(request);
                IOrderedEnumerable<EntityMetadata> EMD = AllEntitiesResponse.EntityMetadata.OrderBy(ee => ee.LogicalName);

                foreach (EntityMetadata currentEntity in EMD)
                {
                    if (currentEntity.IsIntersect.Value == false && currentEntity.IsValidForAdvancedFind.Value)
                    {
                        entitiesNames.Add(new List<string> { currentEntity.LogicalName, currentEntity.PrimaryIdAttribute });
                    }
                }

                WriteEnvStructure(connectionName, entitiesNames);
                toolStripStatusLabel1.Text = "Entities loaded from source.";
                return entitiesNames;
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                toolStripStatusLabel1.Text = "Error.";
                MessageBox.Show("Error:" + ex.Detail.Message + "\n" + ex.Detail.TraceText, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = "Error.";
                if (ex.InnerException != null)
                    MessageBox.Show("Error:" + ex.Message + "\n" + ex.InnerException.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("Error:" + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }

        /// <summary>
        /// Reads the env structure.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns></returns>
        private List<List<string>> ReadEnvStructure(string connectionName)
        {
            string filename = Folder + "\\" + connectionName + ".xml";
            if (File.Exists(filename))
            {
                try
                {
                    XmlDictionaryReaderQuotas XRQ = new XmlDictionaryReaderQuotas();
                    XRQ.MaxStringContentLength = int.MaxValue;

                    using (FileStream fs = new FileStream(filename, FileMode.Open))
                    using (XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, XRQ))
                    {
                        DataContractSerializer ser = new DataContractSerializer(typeof(List<List<string>>));
                        List<List<string>> es = (List<List<string>>)ser.ReadObject(reader, true);
                        return es;
                    }
                }
                catch (Exception)
                {
                    LogManager.WriteLog("Error while reading the struction of connection" + connectionName + ". The structure file may be corrupted.");
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// Writes the env structure.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        /// <param name="data">The data.</param>
        private void WriteEnvStructure(string connectionName, List<List<string>> data)
        {
            string filename = Folder + "\\" + connectionName + ".xml";
            FileStream writer = new FileStream(filename, FileMode.Create);
            DataContractSerializer ser = new DataContractSerializer(typeof(List<List<string>>));
            ser.WriteObject(writer, data);
            writer.Close();
        }

        /// <summary>
        /// Handles the Click event of the buttonDownloadEntitiesFromSource control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void buttonDownloadEntitiesFromSource_Click(object sender, EventArgs e)
        {
            openInExcelToolStripMenuItem.Visible = false;
            if (comboBoxConnectionSource.SelectedItem == null)
            {
                MessageBox.Show("You must select a connection before loading entities.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.entitiesNames = downloadEnvStructure(comboBoxConnectionSource.SelectedItem.ToString());
            this.entitiesNames = ReadEnvStructure(comboBoxConnectionSource.SelectedItem.ToString());
            foreach (List<string> entity in this.entitiesNames)
            {
                allEntities.Add(new RecordLine { Select = false, Entity = entity[0], Filter = "", AttribuiteIdName = entity[1] });
            }

            foreach (RecordLine re in allEntities)
            {
                checkedListBoxEntities.Items.AddRange(new object[] { re.Entity });
            }
        }

        /// <summary>
        /// Handles the Click event of the quitterToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void quitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        /// <summary>
        /// Handles the Click event of the getRecordsNumberToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void getRecordsNumberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (comboBoxConnectionSource.SelectedItem == null)
            {
                MessageBox.Show("You must first select a source before counting the records!");
                return;
            }

            if (allEntities.Count == 0)
            {
                MessageBox.Show("You must load the entities from the source before counting the records!");
                return;
            }

            if (selectedEntities.Count == 0)
            {
                MessageBox.Show("You must check at least 1 Entity for records counting!");
                return;
            }

            openInExcelToolStripMenuItem.Visible = false;
            toolStripStatusLabel1.Text = "Retrieving records number. Please wait...";
            Application.DoEvents();

            try
            {
                DateTime now = DateTime.Now;
                string tempExportedFilePath = String.Concat(Folder + "\\" + comboBoxConnectionSource.SelectedItem + "_" + now.Year + "-" + now.Month + "-" + now.Day + "-" + now.Hour + "-" + now.Minute + "-" + now.Second + ".xml");

                MemoryStream ms = new MemoryStream();

                MSCRMConnection connection = cm.MSCRMConnections[comboBoxConnectionSource.SelectedIndex];

                using (StreamWriter sw = new StreamWriter(tempExportedFilePath))
                using (_serviceProxy = cm.connect(connection))
                {
                    // Create Xml Writer.
                    XmlTextWriter metadataWriter = new XmlTextWriter(sw);

                    // Start Xml File.
                    metadataWriter.WriteStartDocument();

                    // <?mso-application progid="Excel.Sheet"?>
                    metadataWriter.WriteProcessingInstruction("mso-application", "progid=\"Excel.Sheet\"");

                    // Metadata Xml Node.
                    metadataWriter.WriteStartElement("Entities");

                    foreach (RecordLine selectedEntity in selectedEntities)
                    {
                        int entityRecordsNumber = 0;
                        int fetchCount = 5000;
                        int pageNumber = 1;
                        string pagingCookie = null;

                        string query_count = "<fetch>";
                        query_count += "<entity name='" + selectedEntity.Entity + "'>";
                        query_count += "<attribute name='" + selectedEntity.AttribuiteIdName + "'/>";
                        query_count += selectedEntity.Filter;
                        query_count += "</entity>";
                        query_count += "</fetch>";

                        while (true)
                        {
                            
                            string xml = CreateXml(query_count, pagingCookie, pageNumber, fetchCount);
                            RetrieveMultipleRequest request = new RetrieveMultipleRequest();
                            request.Query = new FetchExpression(xml);
                            EntityCollection query_count_result = ((RetrieveMultipleResponse)_serviceProxy.Execute(request)).EntityCollection;

                            entityRecordsNumber = entityRecordsNumber + query_count_result.Entities.Count;

                            // Check for morerecords, if it returns 1.
                            if (query_count_result.MoreRecords)
                            {
                                // Increment the page number to retrieve the next page.
                                pageNumber++;

                                // Set the paging cookie to the paging cookie returned from current results.                            
                                pagingCookie = query_count_result.PagingCookie;
                            }
                            else
                            {
                                // If no more records in the result nodes, exit the loop.
                                break;
                            }
                        }

                        // Start Entity Node
                        metadataWriter.WriteStartElement("Entity");
                        metadataWriter.WriteElementString("Entity", selectedEntity.Entity);
                        metadataWriter.WriteElementString("Records", entityRecordsNumber.ToString());

                        // End Entity Node
                        metadataWriter.WriteEndElement();
                    }

                    // End Metadata Xml Node
                    metadataWriter.WriteEndElement();
                    metadataWriter.WriteEndDocument();

                    // Close xml writer.
                    metadataWriter.Close();
                }

                //Propose the file for saving
                XmlDocument doc = new XmlDocument();
                doc.Load(tempExportedFilePath);
                SaveFileDialog saveXMLDialog = new SaveFileDialog();
                saveXMLDialog.Filter = "XML Files | *.xml";
                saveXMLDialog.DefaultExt = "xml";
                saveXMLDialog.FileName = comboBoxConnectionSource.SelectedItem.ToString() + "_Records_Number";
                DialogResult savRes = saveXMLDialog.ShowDialog();
                if (savRes == DialogResult.OK)
                    doc.Save(saveXMLDialog.FileName);

                exportedFilePath = saveXMLDialog.FileName;

                //Delete temporary file
                File.Delete(tempExportedFilePath);
                //Display Open in Excel Button
                openInExcelToolStripMenuItem.Visible = true;
                toolStripStatusLabel1.Text = "Records number generated.";
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                toolStripStatusLabel1.Text = "Error.";
                MessageBox.Show("Error:" + ex.Detail.Message + "\n" + ex.Detail.TraceText, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = "Error.";
                if (ex.InnerException != null)
                    MessageBox.Show("Error:" + ex.Message + "\n" + ex.InnerException.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("Error:" + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Creates the FetchXML Query.
        /// </summary>
        /// <param name="xml">The FetchXMLQuery.</param>
        /// <param name="cookie">The paging cookie.</param>
        /// <param name="page">The page number.</param>
        /// <param name="count">The records per page count.</param>
        /// <returns>Formatted FechXML Query</returns>
        private string CreateXml(string xml, string cookie, int page, int count)
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

        /// <summary>
        /// Handles the Click event of the openInExcelToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void openInExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // These are the Win32 error code for file not found or access denied.
            const int ERROR_FILE_NOT_FOUND = 2;
            const int ERROR_ACCESS_DENIED = 5;
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "EXCEL.EXE";
                startInfo.Arguments = exportedFilePath;
                Process.Start(startInfo);
            }
            catch (Win32Exception ex)
            {
                if (ex.NativeErrorCode == ERROR_FILE_NOT_FOUND)
                {
                    MessageBox.Show("Excel is not installed on this computer!");
                }

                else if (ex.NativeErrorCode == ERROR_ACCESS_DENIED)
                {
                    Console.WriteLine(ex.Message + ". You do not have permission to access this file.");
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the helpToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "IEXPLORE.EXE";
            startInfo.Arguments = "https://github.com/zoranivanov/MSCRMToolKit/wiki#recordscounter";
            Process.Start(startInfo);
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the checkedListBoxEntities control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void checkedListBoxEntities_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Remove all the combobox and labels
            ArrayList list = new ArrayList(panelSelectedEntities.Controls);
            foreach (Control c in list)
            {
                panelSelectedEntities.Controls.Remove(c);
                selectedEntities = new List<RecordLine>();
            }

            if (checkedListBoxEntities.Items.Count == 0)
                return;

            int transportOrderYLocation = 7;
            int orderCpt = 1;
            List<string> treatedEntities = new List<string>();

            foreach (string checkedEntity in checkedListBoxEntities.CheckedItems)
            {
                RecordLine temp = allEntities.Find(t => t.Entity == checkedEntity);
                selectedEntities.Add(temp);
                if (treatedEntities.Contains(checkedEntity))
                    continue;
                System.Windows.Forms.Label label = new System.Windows.Forms.Label();
                label.Name = "entity_" + checkedEntity;
                label.Text = checkedEntity;
                label.Size = new System.Drawing.Size(248, 23);
                label.Location = new System.Drawing.Point(0, transportOrderYLocation + 4);
                panelSelectedEntities.Controls.Add(label);

                Button setupIgnoredAttributes = new Button();
                setupIgnoredAttributes.Name = "button_" + checkedEntity;
                setupIgnoredAttributes.Text = "...";
                setupIgnoredAttributes.Tag = checkedEntity;
                setupIgnoredAttributes.Size = new System.Drawing.Size(25, 23);
                setupIgnoredAttributes.Location = new System.Drawing.Point(250, transportOrderYLocation - 1);
                setupIgnoredAttributes.Click += new System.EventHandler(showFilterSetupWizard);
                panelSelectedEntities.Controls.Add(setupIgnoredAttributes);

                orderCpt++;
                transportOrderYLocation += 25;
            }
        }

        /// <summary>
        /// Shows the filter setup wizard.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void showFilterSetupWizard(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            string entityName = b.Name.Replace("button_", "");
            RecordsCounterFilterWizard rcfw = new RecordsCounterFilterWizard(this, entityName);
            rcfw.Show();
        }
    }

    /// <summary>
    /// Record Line class
    /// </summary>
    public class RecordLine
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RecordLine"/> is select.
        /// </summary>
        /// <value>
        ///   <c>true</c> if select; otherwise, <c>false</c>.
        /// </value>
        public bool Select { get; set; }

        /// <summary>
        /// Gets or sets the entity.
        /// </summary>
        /// <value>
        /// The entity.
        /// </value>
        public string Entity { get; set; }

        /// <summary>
        /// Gets or sets the filter.
        /// </summary>
        /// <value>
        /// The filter.
        /// </value>
        public string Filter { get; set; }

        /// <summary>
        /// Gets or sets the name of the attribuite identifier.
        /// </summary>
        /// <value>
        /// The name of the attribuite identifier.
        /// </value>
        public string AttribuiteIdName { get; set; }
    }
}