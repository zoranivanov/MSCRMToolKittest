// ========================================================================================
//  This file is part of the MSCRM ToolKit project.
//  https://github.com/zoranivanov/MSCRMToolKit
//  Author:         Zoran IVANOV
//  Created:        31/12/2013
//
//  Disclaimer:
//  This software is provided "as is" with no technical support.
//  Use it at your own risk.
//  The author does not take any responsibility for any damage in whatever form or context.
// ========================================================================================

using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Windows.Forms;
using System.Xml;

namespace MSCRMToolKit
{
    /// <summary>
    /// NtoNAssociationsTransporter class
    /// </summary>
    public partial class NtoNAssociationsTransporter : Form
    {
        private MSCRMConnectionsManager cm = new MSCRMConnectionsManager();
        internal NtoNAssociationsTransportProfile currentProfile;
        internal MSCRMNtoNAssociationsTransportManager man = new MSCRMNtoNAssociationsTransportManager();
        internal List<SelectedNtoNRelationship> TemporarySelectedEntityListForIgnoredAttributes = new List<SelectedNtoNRelationship>();
        internal NtoNRelationshipsStructure es = null;
        private BackgroundWorker bwExport = new BackgroundWorker();
        private BackgroundWorker bwImport = new BackgroundWorker();
        private OrganizationServiceProxy _serviceProxy;
        private string currentlyTransportedEntity = "";
        private int totalImportFailures = 0;
        private int totalImportSuccess = 0;
        private int totalTreatedRecords = 0;
        private bool transportStopped = false;
        private bool transportRunning = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="NtoNAssociationsTransporter"/> class.
        /// </summary>
        public NtoNAssociationsTransporter()
        {
            InitializeComponent();
            LogManager.WriteLog("N to N Relationships Transporter launched.");

            if (man.Profiles != null)
            {
                foreach (NtoNAssociationsTransportProfile tp in man.Profiles)
                {
                    this.comboBoxTransportationProfiles.Items.AddRange(new object[] { tp.ProfileName });
                }
            }
            else
            {
                man.Profiles = new List<NtoNAssociationsTransportProfile>();
            }

            int cpt = 0;
            foreach (MSCRMConnection connection in cm.MSCRMConnections)
            {
                this.comboBoxConnectionSource.Items.AddRange(new object[] { connection.ConnectionName });
                this.comboBoxConnectionTarget.Items.AddRange(new object[] { connection.ConnectionName });
                cpt++;
            }

            buttonLoadEntities.Enabled = false;
            comboBoxOperation.SelectedIndex = 2;
            dataGridView1.AutoGenerateColumns = false;
            bwExport.WorkerReportsProgress = true;
            bwExport.WorkerSupportsCancellation = true;
            bwExport.DoWork += new DoWorkEventHandler(bwExport_DoWork);
            bwExport.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(bwExport_ProgressChanged);
            bwExport.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwExport_RunWorkerCompleted);

            bwImport.WorkerReportsProgress = true;
            bwImport.WorkerSupportsCancellation = true;
            bwImport.DoWork += new DoWorkEventHandler(bwImport_DoWork);
            bwImport.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(bwImport_ProgressChanged);
            bwImport.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwImport_RunWorkerCompleted);
        }

        #region Transport threads

        private void bwExport_DoWork(object sender, DoWorkEventArgs e)
        {
            transportStopped = false;
            BackgroundWorker worker = sender as BackgroundWorker;
            //Export data
            ExportGUI(man.ReportFileName, worker, e);
        }

        private void bwExport_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.pgbState.Value = e.ProgressPercentage;
            toolStripStatusLabel.Text = "Exporting: " + currentlyTransportedEntity;
        }

        private void bwExport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                //Stop Transport threads if running
                bwImport.CancelAsync();
                MessageBox.Show("Error ! Detail : " + e.Error.Message);
                transportReportToolStripMenuItem.Visible = true;
                transportRunning = false;
                SetFields(true);
                transportStopped = true;
                buttonStopTransport.Visible = false;
                runProfileToolStripMenuItem.Enabled = true;
                pgbState.Visible = false;
            }
            else if (e.Cancelled)
            {
                //Stop Transport threads if running
                bwImport.CancelAsync();
                toolStripStatusLabel.Text = "Transport canceled";
                transportReportToolStripMenuItem.Visible = true;
                SetFields(true);
                transportRunning = false;
                transportStopped = true;
                buttonStopTransport.Visible = false;
                runProfileToolStripMenuItem.Enabled = true;
                pgbState.Visible = false;
            }
            else
            {
                toolStripStatusLabel.Text = "Export Finished";

                if (currentProfile.Operation == 2)
                {
                    if (!transportStopped)
                    {
                        transportRunning = true;
                        SetFields(false);
                        toolStripStatusLabel.Text = "Initializing Data Import";
                        label2.Visible = true;
                        label3.Visible = true;
                        label4.Visible = true;
                        bwImport.RunWorkerAsync();
                    }
                    else
                    {
                        bwImport.CancelAsync();
                        transportRunning = false;
                        SetFields(true);
                        buttonStopTransport.Visible = false;
                        runProfileToolStripMenuItem.Enabled = true;
                        transportReportToolStripMenuItem.Visible = true;
                        pgbState.Visible = false;
                        NtoNTransportReport report = man.ReadTransportReport(man.ReportFileName);
                        report.TransportFinishedAt = DateTime.Now.ToString();
                        report.TransportCompleted = true;
                        TimeSpan TransportTimeSpan = DateTime.Now - Convert.ToDateTime(report.TransportStartedAt);
                        report.TransportedIn = TransportTimeSpan.ToString().Substring(0, 10);
                        man.WriteTransportReport(report, man.ReportFileName);
                    }
                }
                else
                {
                    SetFields(true);
                    transportRunning = false;
                    transportReportToolStripMenuItem.Visible = true;
                    buttonStopTransport.Visible = false;
                    runProfileToolStripMenuItem.Enabled = true;
                    pgbState.Visible = false;
                    NtoNTransportReport report = man.ReadTransportReport(man.ReportFileName);
                    report.TransportFinishedAt = DateTime.Now.ToString();
                    report.TransportCompleted = true;
                    TimeSpan TransportTimeSpan = DateTime.Now - Convert.ToDateTime(report.TransportStartedAt);
                    report.TransportedIn = TransportTimeSpan.ToString().Substring(0, 10);
                    man.WriteTransportReport(report, man.ReportFileName);
                }
            }
        }

        private int ExportGUI(string transportReportFileName, BackgroundWorker worker, DoWorkEventArgs e)
        {
            NtoNTransportReport report = null;
            try
            {
                report = new NtoNTransportReport(man.ReportFileName);
                //Get Transport Report
                if (File.Exists(man.ReportFileName))
                {
                    report = man.ReadTransportReport(man.ReportFileName);
                }

                //Clean Data folder
                string dataExportFolder = man.Folder + "\\" + currentProfile.ProfileName + "\\Data";
                if (Directory.Exists(dataExportFolder))
                {
                    Directory.Delete(dataExportFolder, true);
                }
                Directory.CreateDirectory(dataExportFolder);

                MSCRMConnection connection = currentProfile.getSourceConneciton();
                NtoNRelationshipsStructure es = man.ReadEnvStructure(currentProfile.SourceConnectionName);
                man._serviceProxy = cm.connect(connection);
                IOrganizationService service = (IOrganizationService)_serviceProxy;
                List<NtoNTransportReportLine> TransportReport = new List<NtoNTransportReportLine>();
                currentProfile.TotalExportedRecords = 0;
                //Mesure export time
                DateTime exportStartDT = DateTime.Now;

                LogManager.WriteLog("Start exporting data from " + connection.ConnectionName);

                int recordCount = 0;
                if (es != null)
                {
                    int treatedEntities = 1;

                    foreach (SelectedNtoNRelationship ee in currentProfile.SelectedNtoNRelationships)
                    {
                        if (worker.CancellationPending)
                        {
                            e.Cancel = true;
                            return 0;
                        }

                        int percentage = 0;
                        if (currentProfile.SelectedNtoNRelationships.Count != 0)
                            percentage = (int)(100 * treatedEntities / currentProfile.SelectedNtoNRelationships.Count);
                        worker.ReportProgress(percentage);
                        treatedEntities++;

                        currentlyTransportedEntity = ee.RelationshipSchemaName;
                        LogManager.WriteLog("Exporting data for relationship " + ee.RelationshipSchemaName);
                        DateTime entityExportStartDT = DateTime.Now;
                        string fetchXml = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>";
                        fetchXml += "<entity name='" + ee.IntersectEntityName + "'>";
                        //Get Entity structure
                        NtoNRelationship strE = new NtoNRelationship();
                        foreach (NtoNRelationship envE in es.NtoNRelationships)
                        {
                            if (envE.IntersectEntityName == ee.IntersectEntityName)
                            {
                                strE = envE;
                                fetchXml += "<attribute name='" + strE.Entity1IntersectAttribute + "' />";
                                fetchXml += "<attribute name='" + strE.Entity2IntersectAttribute + "' />";
                                if (ee.IntersectEntityName == "listmember" || ee.IntersectEntityName == "campaignitem")
                                    fetchXml += "<attribute name='entitytype' />";
                                else if (ee.IntersectEntityName == "campaignactivityitem")
                                    fetchXml += "<attribute name='itemobjecttypecode' />";

                                break;
                            }
                        }

                        //Add Query filter
                        int objectTypeCode = 0;
                        if (ee.Entity2LogicalName == "account") objectTypeCode = 1;
                        else if (ee.Entity2LogicalName == "campaign") objectTypeCode = 4400;
                        else if (ee.Entity2LogicalName == "contact") objectTypeCode = 2;
                        else if (ee.Entity2LogicalName == "lead") objectTypeCode = 4;
                        else if (ee.Entity2LogicalName == "list") objectTypeCode = 4300;
                        else if (ee.Entity2LogicalName == "product") objectTypeCode = 1024;
                        else if (ee.Entity2LogicalName == "salesliterature") objectTypeCode = 1038;

                        if (ee.IntersectEntityName == "campaignitem")
                            fetchXml += "<filter type='and'><condition attribute='entitytype' operator='eq' value='" + objectTypeCode + "' /></filter>";
                        else if (ee.IntersectEntityName == "campaignactivityitem")
                            fetchXml += "<filter type='and'><condition attribute='itemobjecttypecode' operator='eq' value='" + objectTypeCode + "' /></filter>";

                        fetchXml += "</entity></fetch>";
                        int recordCountPerEntity = man.ExportEntity(currentProfile, fetchXml, ee.RelationshipSchemaName);
                        recordCount += recordCountPerEntity;
                        DateTime entityExportEndDT = DateTime.Now;
                        TimeSpan ts = entityExportEndDT - entityExportStartDT;
                        NtoNTransportReportLine transportReportLine = new NtoNTransportReportLine();
                        transportReportLine.RelationshipSchemaName = ee.RelationshipSchemaName;
                        transportReportLine.ExportedRecords = recordCountPerEntity;
                        report.TotalExportedRecords += recordCountPerEntity;
                        transportReportLine.ExportedIn = ts.ToString().Substring(0, 10);
                        transportReportLine.ExportStartedAt = entityExportStartDT.ToString();
                        transportReportLine.ExportFinishedAt = entityExportEndDT.ToString();
                        report.ReportLines.Add(transportReportLine);
                        man.WriteTransportReport(report, transportReportFileName);
                    }
                }

                TimeSpan exportTimeSpan = DateTime.Now - exportStartDT;
                LogManager.WriteLog("Export finished for " + currentProfile.SourceConnectionName + ". Exported " + recordCount + " records in " + exportTimeSpan.ToString().Substring(0, 10));
                return recordCount;
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                //Stop Transport threads if running
                transportStopped = true;
                e.Cancel = true;
                MessageBox.Show("Error:" + ex.Detail.Message + "\n" + ex.Detail.TraceText);
                LogManager.WriteLog("Error:" + ex.Detail.Message + "\n" + ex.Detail.TraceText);
            }
            catch (Exception ex)
            {
                //Stop Transport threads if running
                transportStopped = true;
                e.Cancel = true;
                if (ex.InnerException != null)
                {
                    MessageBox.Show("Error:" + ex.Message + "\n" + ex.InnerException.Message);
                    LogManager.WriteLog("Error:" + ex.Message + "\n" + ex.InnerException.Message);
                }
                else
                {
                    MessageBox.Show("Error:" + ex.Message);
                    LogManager.WriteLog("Error:" + ex.Message);
                }
            }

            return 0;
        }

        private void bwImport_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            //Import Data
            ImportGUI(man.ReportFileName, worker, e);
        }

        private void bwImport_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.pgbState.Value = e.ProgressPercentage;
            toolStripStatusLabel.Text = "Importing: " + currentlyTransportedEntity;
            labelImportFailures.Text = totalImportFailures.ToString(); ;
            labelImportSuccess.Text = totalImportSuccess.ToString();
        }

        private void bwImport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("Error ! Detail : " + e.Error.Message);
                toolStripStatusLabel.Text = "Transport stopped.";
            }
            else if (e.Cancelled)
            {
                toolStripStatusLabel.Text = "Transport canceled";
            }
            else
            {
                toolStripStatusLabel.Text = "Import Finished";
                NtoNTransportReport report = man.ReadTransportReport(man.ReportFileName);
                report.TransportFinishedAt = DateTime.Now.ToString();
                report.TransportCompleted = true;
                TimeSpan TransportTimeSpan = DateTime.Now - Convert.ToDateTime(report.TransportStartedAt);
                report.TransportedIn = TransportTimeSpan.ToString().Substring(0, 10);
                man.WriteTransportReport(report, man.ReportFileName);
            }

            transportRunning = false;
            buttonStopTransport.Visible = false;
            runProfileToolStripMenuItem.Enabled = true;
            pgbState.Visible = false;
            transportReportToolStripMenuItem.Visible = true;
            SetFields(true);
        }

        private int ImportGUI(string transporimportFailurestReportFileName, BackgroundWorker worker, DoWorkEventArgs e)
        {
            totalTreatedRecords = 0;
            totalImportFailures = 0;
            totalImportSuccess = 0;
            int ReconnectionRetryCount = 5;

            try
            {
                NtoNTransportReport report = new NtoNTransportReport(man.ReportFileName);
                //Get Transport Report
                if (File.Exists(man.ReportFileName))
                {
                    report = man.ReadTransportReport(man.ReportFileName);
                }

                MSCRMConnection connection = currentProfile.getTargetConneciton(); ;
                _serviceProxy = cm.connect(connection);
                IOrganizationService service = (IOrganizationService)_serviceProxy;
                LogManager.WriteLog("Start importing data in " + connection.ConnectionName);

                //Mesure import time
                DateTime importStartDT = DateTime.Now;

                //es = man.ReadEnvStructure(currentProfile.SourceConnectionName);

                foreach (SelectedNtoNRelationship ee in currentProfile.SelectedNtoNRelationships)
                {
                    //Check if there are any records to import
                    if (ee.ExportedRecords == 0)
                    {
                        continue;
                    }

                    //Mesure import time
                    DateTime entityImportStartDT = DateTime.Now;
                    currentlyTransportedEntity = ee.RelationshipSchemaName;

                    string entityFolderPath = man.Folder + "\\" + currentProfile.ProfileName + "\\Data\\" + ee.RelationshipSchemaName;
                    string[] filePaths = Directory.GetFiles(entityFolderPath, "*.xml");

                    LogManager.WriteLog("Importing " + ee.RelationshipSchemaName + " records.");
                    int treatedRecordsForEntity = 0;
                    int importedRecordsForEntity = 0;
                    int importFailuresForEntity = 0;

                    foreach (string filePath in filePaths)
                    {
                        List<Type> knownTypes = new List<Type>();
                        knownTypes.Add(typeof(Entity));
                        XmlDictionaryReaderQuotas XRQ = new XmlDictionaryReaderQuotas();
                        XRQ.MaxStringContentLength = int.MaxValue;

                        using(FileStream fs = new FileStream(filePath, FileMode.Open))
                        using (XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, XRQ))
                        {
                            DataContractSerializer ser = new DataContractSerializer(typeof(EntityCollection), knownTypes);
                            EntityCollection fromDisk = (EntityCollection)ser.ReadObject(reader, true);


                            foreach (Entity en in fromDisk.Entities)
                            {
                                if (worker.CancellationPending)
                                {
                                    e.Cancel = true;
                                    return 0;
                                }

                                EntityReference relatedEntity1 = new EntityReference();
                                EntityReference relatedEntity2 = new EntityReference();

                                try
                                {
                                    Guid relatedEntity1Id = man.getRelatedEntityGuid(en[ee.Entity1IntersectAttribute]);
                                    Guid relatedEntity2Id = man.getRelatedEntityGuid(en[ee.Entity2IntersectAttribute]);

                                    relatedEntity1 = new EntityReference { LogicalName = ee.Entity1LogicalName, Id = relatedEntity1Id };
                                    relatedEntity2 = new EntityReference { LogicalName = ee.Entity2LogicalName, Id = relatedEntity2Id };

                                    if (!man.AlreadyAssociated(_serviceProxy, ee, relatedEntity1Id, relatedEntity2Id))
                                    {
                                        if (ee.IntersectEntityName == "listmember")
                                        {
                                            Guid entity_id = Guid.Empty;
                                            Guid list_id = Guid.Empty;

                                            if (ee.Entity1LogicalName == "list")
                                            {
                                                entity_id = relatedEntity2Id;
                                                list_id = relatedEntity1Id;
                                            }
                                            else
                                            {
                                                entity_id = relatedEntity1Id;
                                                list_id = relatedEntity2Id;
                                            }

                                            AddMemberListRequest request = new AddMemberListRequest();
                                            request.EntityId = entity_id;
                                            request.ListId = list_id;
                                            AddMemberListResponse response = (AddMemberListResponse)service.Execute(request);
                                        }
                                        else if (ee.IntersectEntityName == "campaignitem")
                                        {
                                            Guid entity_id = Guid.Empty;
                                            Guid list_id = Guid.Empty;
                                            string EntityName = "";

                                            if (ee.Entity1LogicalName == "campaign")
                                            {
                                                entity_id = relatedEntity2Id;
                                                list_id = relatedEntity1Id;
                                                EntityName = (string)en["entitytype"];
                                                relatedEntity2.LogicalName = EntityName;
                                            }
                                            else
                                            {
                                                entity_id = relatedEntity1Id;
                                                list_id = relatedEntity2Id;
                                                EntityName = (string)en["entitytype"];
                                                relatedEntity1.LogicalName = EntityName;
                                            }

                                            AddItemCampaignRequest req = new AddItemCampaignRequest();
                                            req.CampaignId = relatedEntity1Id;
                                            req.EntityName = EntityName;
                                            req.EntityId = entity_id;
                                            AddItemCampaignResponse resp = (AddItemCampaignResponse)service.Execute(req);
                                        }
                                        else if (ee.IntersectEntityName == "campaignactivityitem")
                                        {
                                            Guid entity_id = Guid.Empty;
                                            Guid list_id = Guid.Empty;
                                            string EntityName = "";

                                            if (ee.Entity1LogicalName == "campaignactivity")
                                            {
                                                entity_id = relatedEntity2Id;
                                                list_id = relatedEntity1Id;
                                                EntityName = (string)en["itemobjecttypecode"];
                                                relatedEntity2.LogicalName = EntityName;
                                            }
                                            else
                                            {
                                                entity_id = relatedEntity1Id;
                                                list_id = relatedEntity2Id;
                                                EntityName = (string)en["itemobjecttypecode"];
                                                relatedEntity1.LogicalName = EntityName;
                                            }

                                            AddItemCampaignActivityRequest req = new AddItemCampaignActivityRequest();
                                            req.CampaignActivityId = relatedEntity1Id;
                                            req.EntityName = EntityName;
                                            req.ItemId = entity_id;
                                            AddItemCampaignActivityResponse resp = (AddItemCampaignActivityResponse)service.Execute(req);
                                        }
                                        else
                                        {
                                            EntityReferenceCollection relatedEntities = new EntityReferenceCollection();
                                            relatedEntities.Add(relatedEntity2);
                                            Relationship relationship = new Relationship(ee.RelationshipSchemaName);
                                            relationship.PrimaryEntityRole = EntityRole.Referencing;
                                            service.Associate(relatedEntity1.LogicalName, relatedEntity1.Id, relationship, relatedEntities);
                                        }
                                    }
                                    importedRecordsForEntity++;
                                    totalImportSuccess++;
                                }
                                catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
                                {
                                    //If "Cannot insert duplicate key." is raised ignore it
                                    totalImportFailures++;
                                    importFailuresForEntity++;
                                    NtoNRelationshipsImportFailure failure = new NtoNRelationshipsImportFailure
                                    {
                                        CreatedOn = DateTime.Now.ToString(),
                                        NtoNRelationshipName = ee.RelationshipSchemaName,
                                        Reason = ex.Detail.Message,
                                        UrlEntity1 = currentProfile.getSourceConneciton().ServerAddress + "main.aspx?pagetype=entityrecord&etn=" + relatedEntity1.LogicalName + "&id=" + relatedEntity1.Id.ToString(),
                                        UrlEntity2 = currentProfile.getSourceConneciton().ServerAddress + "main.aspx?pagetype=entityrecord&etn=" + relatedEntity2.LogicalName + "&id=" + relatedEntity2.Id.ToString()
                                    };
                                    report.TotalImportFailures += 1;
                                    //Insert the Failure line in the Failures Report
                                    man.WriteNewImportFailureLine(failure, man.importFailuresReportFileName);
                                }
                                catch (Exception ex)
                                {
                                    //Check if the authentification session is expired
                                    if (ex.InnerException != null && ex.InnerException.Message.StartsWith("ID3242"))
                                    {
                                        LogManager.WriteLog("Error:The CRM authentication session expired. Reconnection attempt n° " + ReconnectionRetryCount);
                                        ReconnectionRetryCount--;
                                        //On 5 failed reconnections exit
                                        if (ReconnectionRetryCount == 0)
                                            throw;

                                        _serviceProxy = cm.connect(connection);
                                        service = (IOrganizationService)_serviceProxy;
                                        LogManager.WriteLog("Error:The CRM authentication session expired.");
                                        totalImportFailures++;
                                        importFailuresForEntity++;
                                        NtoNRelationshipsImportFailure failure = new NtoNRelationshipsImportFailure
                                        {
                                            CreatedOn = DateTime.Now.ToString(),
                                            NtoNRelationshipName = ee.RelationshipSchemaName,
                                            Reason = ex.InnerException.Message,
                                            UrlEntity1 = currentProfile.getSourceConneciton().ServerAddress + "main.aspx?pagetype=entityrecord&etn=" + relatedEntity1.LogicalName + "&id=" + relatedEntity1.Id.ToString(),
                                            UrlEntity2 = currentProfile.getSourceConneciton().ServerAddress + "main.aspx?pagetype=entityrecord&etn=" + relatedEntity2.LogicalName + "&id=" + relatedEntity2.Id.ToString()
                                        };
                                        report.TotalImportFailures += 1;
                                        //Insert the Failure line in the Failures Report
                                        man.WriteNewImportFailureLine(failure, man.importFailuresReportFileName);
                                    }
                                    else
                                    {
                                        throw;
                                    }
                                }
                                totalTreatedRecords++;
                                treatedRecordsForEntity++;
                                man.updateTransportReport(report, ee, importedRecordsForEntity, importFailuresForEntity, entityImportStartDT);

                                int percentage = 0;
                                if (currentProfile.TotalExportedRecords != 0)
                                    percentage = totalTreatedRecords * 100 / currentProfile.TotalExportedRecords;
                                worker.ReportProgress(percentage);
                            }
                        }
                    }
                    LogManager.WriteLog("Treated " + treatedRecordsForEntity + " " + ee.RelationshipSchemaName + " records with " + importedRecordsForEntity + " successfully imported records and " + importFailuresForEntity + " failures.");
                }

                TimeSpan importTimeSpan = DateTime.Now - importStartDT;
                LogManager.WriteLog("Import finished for " + connection.ConnectionName + ". Treated " + totalTreatedRecords + " records in " + importTimeSpan.ToString().Substring(0, 10) + ". Successfuly imported " + totalImportSuccess + " records and " + totalImportFailures + " failures.");
                man.WriteTransportReport(report, man.ReportFileName);
                return 0;
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                //Stop Transport threads if running
                bwImport.CancelAsync();
                LogManager.WriteLog("Error:" + ex.Detail.Message + "\n" + ex.Detail.TraceText);
                MessageBox.Show("Error:" + ex.Detail.Message + "\n" + ex.Detail.TraceText);
            }
            catch (Exception ex)
            {
                //Stop Transport threads if running
                bwImport.CancelAsync();
                if (ex.InnerException != null)
                {
                    LogManager.WriteLog("Error:" + ex.Message + "\n" + ex.InnerException.Message);
                    MessageBox.Show("Error:" + ex.Message + "\n" + ex.InnerException.Message);
                }
                else
                {
                    LogManager.WriteLog("Error:" + ex.Message);
                    MessageBox.Show("Error:" + ex.Message);
                }
            }

            return 0;
        }

        #endregion Transport threads

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "IEXPLORE.EXE";
            startInfo.Arguments = "https://github.com/zoranivanov/MSCRMToolKit/wiki#ntonassociationstransporter";
            Process.Start(startInfo);
        }

        private void comboBoxConnectionSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            populateCheckedEntities();
            if (comboBoxConnectionSource.SelectedItem == null)
            {
                buttonLoadEntities.Enabled = false;
            }
            else
            {
                buttonLoadEntities.Enabled = true;
            }
        }

        private void buttonLoadEntities_Click(object sender, EventArgs e)
        {
            DialogResult dResTest = MessageBox.Show("Loading the structure may take some time. The application will become unresponsive during this time.\n\nAre you sure you want to load the structure from the Source?", "Confirm Structure Load", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dResTest == DialogResult.No)
            {
                return;
            }

            toolStripStatusLabel.Text = "Loading structure. Please wait...";
            Application.DoEvents();

            try
            {
                es = man.downloadEnvStructure(comboBoxConnectionSource.SelectedItem.ToString());
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                MessageBox.Show("Error:" + ex.Detail.Message + "\n" + ex.Detail.TraceText);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    MessageBox.Show("Error:" + ex.Message + "\n" + ex.InnerException.Message);
                else
                {
                    MessageBox.Show("Error:" + ex.Message);
                }
            }

            if (es == null)
                return;

            string structureFileName = man.Folder + "\\" + comboBoxConnectionSource.SelectedItem.ToString() + ".xml";
            DateTime structureRefreshedOn = File.GetLastWriteTime(structureFileName);
            labelStructureLastLoadedDate.Text = structureRefreshedOn.ToString();
            //Display Relationships
            dataGridView1.DataSource = es.NtoNRelationships;

            if (currentProfile != null && currentProfile.SelectedNtoNRelationships != null && currentProfile.SelectedNtoNRelationships.Count > 0)
            {
                foreach (DataGridViewRow row in this.dataGridView1.Rows)
                {
                    DataGridViewCheckBoxCell check = (DataGridViewCheckBoxCell)row.Cells[0];
                    string RelationshipSchemaName = (string)row.Cells[1].Value;
                    bool c = currentProfile.SelectedNtoNRelationships.FindIndex(r => r.RelationshipSchemaName == RelationshipSchemaName) > -1;
                    ((DataGridViewCheckBoxCell)row.Cells[0]).Value = c;
                }
            }

            toolStripStatusLabel.Text = "Structure successfully loaded";
        }

        private void populateCheckedEntities()
        {
            dataGridView1.DataSource = null;

            //If Structure already downloaded display entities list
            if (comboBoxConnectionSource.SelectedItem == null)
                return;

            try
            {
                es = man.ReadEnvStructure(comboBoxConnectionSource.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while reading the struction of connection " + comboBoxConnectionSource.SelectedItem.ToString() + ". The structure file may be corrupted.\r\n\r\n" + ex.Message);
                return;
            }
            labelStructureLastLoadedDate.Text = "never";
            if (es != null)
            {
                string structureFileName = man.Folder + "\\" + comboBoxConnectionSource.SelectedItem + ".xml";
                DateTime structureRefreshedOn = File.GetLastWriteTime(structureFileName);
                labelStructureLastLoadedDate.Text = structureRefreshedOn.ToString();
                SortableBindingList<NtoNRelationship> sortedNToNRelationships = new SortableBindingList<NtoNRelationship>(es.NtoNRelationships);
                dataGridView1.DataSource = sortedNToNRelationships;

                if (currentProfile != null && currentProfile.SelectedNtoNRelationships != null && currentProfile.SelectedNtoNRelationships.Count > 0)
                {
                    foreach (DataGridViewRow row in this.dataGridView1.Rows)
                    {
                        DataGridViewCheckBoxCell check = (DataGridViewCheckBoxCell)row.Cells[0];
                        string RelationshipSchemaName = (string)row.Cells[1].Value;
                        bool c = currentProfile.SelectedNtoNRelationships.FindIndex(r => r.RelationshipSchemaName == RelationshipSchemaName) > -1;
                        ((DataGridViewCheckBoxCell)row.Cells[0]).Value = c;
                    }
                }
            }
        }

        private bool SaveProfile()
        {
            bool result = true;
            //Check that all fields are provided
            if (string.IsNullOrEmpty(textBoxTransportationProfileName.Text))
            {
                MessageBox.Show("Profile Name is mandatory!");
                return false;
            }

            //Check that the name of the profile is valid
            if (textBoxTransportationProfileName.Text.Contains(" ") ||
                    textBoxTransportationProfileName.Text.Contains("\\") ||
                    textBoxTransportationProfileName.Text.Contains("/") ||
                    textBoxTransportationProfileName.Text.Contains(">") ||
                    textBoxTransportationProfileName.Text.Contains("<") ||
                    textBoxTransportationProfileName.Text.Contains("?") ||
                    textBoxTransportationProfileName.Text.Contains("*") ||
                    textBoxTransportationProfileName.Text.Contains(":") ||
                    textBoxTransportationProfileName.Text.Contains("|") ||
                    textBoxTransportationProfileName.Text.Contains("\"") ||
                    textBoxTransportationProfileName.Text.Contains("'")
                    )
            {
                MessageBox.Show("You shouldn't use spaces nor the following characters (\\/<>?*:|\"') in the Profile Name as it will be used to create folders and files.");
                return false;
            }

            if (comboBoxConnectionSource.SelectedItem == null)
            {
                MessageBox.Show("You must select a Source for the Profile");
                return false;
            }

            if (comboBoxConnectionTarget.SelectedItem == null && comboBoxOperation.SelectedItem.ToString() != "Export Data")
            {
                MessageBox.Show("You must select a Target for the Profile");
                return false;
            }

            //Check if this is a creation
            if (currentProfile == null)
            {
                //Check if a Connection having the same name exist already
                foreach (NtoNAssociationsTransportProfile tp in man.Profiles)
                {
                    if (tp.ProfileName.ToLower() == textBoxTransportationProfileName.Text.ToLower())
                    {
                        MessageBox.Show("Profile with the name " + textBoxTransportationProfileName.Text + " exist already. Please select another name");
                        return false;
                    }
                }

                NtoNAssociationsTransportProfile newProfile = new NtoNAssociationsTransportProfile();
                newProfile.ProfileName = textBoxTransportationProfileName.Text;
                //newProfile.ImportMode = comboBoxImportMode.SelectedIndex;
                newProfile.SourceConnectionName = comboBoxConnectionSource.SelectedItem.ToString();
                newProfile.setSourceConneciton();
                if (comboBoxOperation.SelectedItem.ToString() != "Export Data")
                {
                    newProfile.TargetConnectionName = comboBoxConnectionTarget.SelectedItem.ToString();
                    newProfile.setTargetConneciton();
                }
                newProfile.SelectedNtoNRelationships = new List<SelectedNtoNRelationship>();

                dataGridView1.EndEdit();
                foreach (DataGridViewRow row in this.dataGridView1.Rows)
                {
                    DataGridViewCheckBoxCell check = (DataGridViewCheckBoxCell)row.Cells[0];

                    if (check.Value != null && (bool)check.Value)
                    {
                        NtoNRelationship nnr = es.NtoNRelationships.Find(r => r.RelationshipSchemaName == (string)row.Cells[1].Value);
                        SelectedNtoNRelationship ee = new SelectedNtoNRelationship();
                        ee.RelationshipSchemaName = nnr.RelationshipSchemaName;
                        ee.IntersectEntityName = nnr.IntersectEntityName;
                        ee.Entity1IntersectAttribute = nnr.Entity1IntersectAttribute;
                        ee.Entity1LogicalName = nnr.Entity1LogicalName;
                        ee.Entity2IntersectAttribute = nnr.Entity2IntersectAttribute;
                        ee.Entity2LogicalName = nnr.Entity2LogicalName;
                        ee.ExportedRecords = 0;
                        newProfile.SelectedNtoNRelationships.Add(ee);
                    }
                }

                newProfile.Operation = comboBoxOperation.SelectedIndex;
                man.CreateProfile(newProfile);
                comboBoxTransportationProfiles.Items.AddRange(new object[] { newProfile.ProfileName });
                comboBoxTransportationProfiles.SelectedItem = newProfile.ProfileName;
                currentProfile = newProfile;
            }
            else
            {
                currentProfile.ProfileName = textBoxTransportationProfileName.Text;
                //currentProfile.ImportMode = comboBoxImportMode.SelectedIndex;
                currentProfile.SourceConnectionName = comboBoxConnectionSource.SelectedItem.ToString();
                currentProfile.setSourceConneciton();
                if (comboBoxOperation.SelectedItem.ToString() != "Export Data")
                {
                    currentProfile.TargetConnectionName = comboBoxConnectionTarget.SelectedItem.ToString();
                    currentProfile.setTargetConneciton();
                }

                //Backup Export Records numbers if existing
                List<SelectedNtoNRelationship> backupSelectedEntites = currentProfile.SelectedNtoNRelationships;

                currentProfile.SelectedNtoNRelationships = new List<SelectedNtoNRelationship>();

                NtoNAssociationsTransportProfile oldProfile = man.GetProfile(currentProfile.ProfileName);

                dataGridView1.EndEdit();
                foreach (DataGridViewRow row in this.dataGridView1.Rows)
                {
                    DataGridViewCheckBoxCell check = (DataGridViewCheckBoxCell)row.Cells[0];
                    if (check.Value != null && (bool)check.Value)
                    {
                        NtoNRelationship nnr = es.NtoNRelationships.Find(r => r.RelationshipSchemaName == (string)row.Cells[1].Value);
                        SelectedNtoNRelationship ee = new SelectedNtoNRelationship();
                        ee.RelationshipSchemaName = nnr.RelationshipSchemaName;
                        ee.IntersectEntityName = nnr.IntersectEntityName;
                        ee.Entity1IntersectAttribute = nnr.Entity1IntersectAttribute;
                        ee.Entity1LogicalName = nnr.Entity1LogicalName;
                        ee.Entity2IntersectAttribute = nnr.Entity2IntersectAttribute;
                        ee.Entity2LogicalName = nnr.Entity2LogicalName;
                        ee.ExportedRecords = 0;
                        currentProfile.SelectedNtoNRelationships.Add(ee);
                    }
                }

                currentProfile.Operation = comboBoxOperation.SelectedIndex;
                man.UpdateProfile(currentProfile);
            }

            runProfileToolStripMenuItem.Enabled = true;
            toolStripStatusLabel.Text = "Profile " + currentProfile.ProfileName + " saved.";
            LogManager.WriteLog("Profile " + currentProfile.ProfileName + " saved.");
            return result;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveProfile();
        }

        private void comboBoxTransportationProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxConnectionSource.SelectedItem = null;
            comboBoxConnectionTarget.SelectedItem = null;
            if (comboBoxTransportationProfiles.SelectedItem != null)
            {
                currentProfile = man.Profiles[comboBoxTransportationProfiles.SelectedIndex];
                textBoxTransportationProfileName.Text = currentProfile.ProfileName;
                //comboBoxImportMode.SelectedIndex = currentProfile.ImportMode;
                comboBoxConnectionSource.SelectedItem = currentProfile.SourceConnectionName;
                comboBoxConnectionTarget.SelectedItem = currentProfile.TargetConnectionName;
                TemporarySelectedEntityListForIgnoredAttributes = currentProfile.SelectedNtoNRelationships;
                deleteProfileToolStripMenuItem.Enabled = true;
                textBoxTransportationProfileName.Enabled = false;
                comboBoxOperation.SelectedIndex = currentProfile.Operation;
                runProfileToolStripMenuItem.Enabled = true;
            }
            else
            {
                currentProfile = null;
                textBoxTransportationProfileName.Text = "";
                comboBoxOperation.SelectedIndex = 2;
                //comboBoxImportMode.SelectedIndex = 1;
                deleteProfileToolStripMenuItem.Enabled = false;
                textBoxTransportationProfileName.Enabled = true;
                labelStructureLastLoadedDate.Text = "never";
                runProfileToolStripMenuItem.Enabled = false;
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentProfile = null;
            textBoxTransportationProfileName.Text = "";
            //comboBoxImportMode.SelectedIndex = 0;
            comboBoxConnectionSource.SelectedItem = null;
            comboBoxConnectionTarget.SelectedItem = null;
            comboBoxTransportationProfiles.SelectedItem = null;
            comboBoxOperation.SelectedIndex = 2;
            //comboBoxImportMode.SelectedIndex = 1;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Stop Transport threads if running
            bwExport.CancelAsync();
            bwImport.CancelAsync();
            this.Dispose();
        }

        private void deleteProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string currentTransportationProfileName = currentProfile.ProfileName;
            DialogResult dResTest;
            dResTest = MessageBox.Show("Are you sure you want to delete this Profile ?", "Confirm Profile Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dResTest == DialogResult.No)
            {
                return;
            }
            else
            {
                comboBoxTransportationProfiles.Items.Remove(currentProfile.ProfileName);
                comboBoxTransportationProfiles.SelectedItem = null;
                man.DeleteProfile(currentProfile);
                currentProfile = null;
                textBoxTransportationProfileName.Text = "";
                labelStructureLastLoadedDate.Text = "never";
                textBoxTransportationProfileName.Enabled = true;
                comboBoxConnectionSource.SelectedItem = null;
                comboBoxConnectionTarget.SelectedItem = null;
                comboBoxOperation.SelectedIndex = 2;
                //comboBoxImportMode.SelectedIndex = 1;
                toolStripStatusLabel.Text = "Profile " + currentTransportationProfileName + " deleted";
            }
        }

        private void viewLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogManager.OpenLogFile();
        }

        private void logArchiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogManager.OpenLogFolder();
        }

        private void runProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!SaveProfile())
                return;

            LogManager.WriteLog("Running Transportation Profile: " + currentProfile.ProfileName);

            //Check if there are selected entities to transport
            if (currentProfile.SelectedNtoNRelationships == null || currentProfile.SelectedNtoNRelationships.Count == 0)
            {
                MessageBox.Show("No entities selected for transport. Select the entities and then run the profile");
                LogManager.WriteLog("No entities selected for transport. Select the entities and then run the profile");
                return;
            }

            DateTime now = DateTime.Now;
            man.ReportFileName = man.Folder + "\\" + currentProfile.ProfileName + "\\ExecutionReports\\TransportReport" + now.Year + "-" + now.Month + "-" + now.Day + "-" + now.Hour + "-" + now.Minute + "-" + now.Second + ".xml";
            man.importFailuresReportFileName = man.Folder + "\\" + currentProfile.ProfileName + "\\ExecutionReports\\ImportFailuresReport" + now.Year + "-" + now.Month + "-" + now.Day + "-" + now.Hour + "-" + now.Minute + "-" + now.Second + ".xml";

            //Initialize Execution Reports folder
            string executionReportsFolder = man.Folder + "\\" + currentProfile.ProfileName + "\\ExecutionReports";
            if (!Directory.Exists(executionReportsFolder))
            {
                Directory.CreateDirectory(executionReportsFolder);
            }

            //Create Transport Report
            NtoNTransportReport tr = new NtoNTransportReport(currentProfile.ProfileName);
            man.WriteTransportReport(tr, man.ReportFileName);

            transportReportToolStripMenuItem.Visible = false;
            labelImportSuccess.Text = "";
            labelImportFailures.Text = "";
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;

            if (currentProfile.Operation == 0 || currentProfile.Operation == 2)
            {
                if (!transportRunning)
                {
                    buttonStopTransport.Visible = true;
                    runProfileToolStripMenuItem.Enabled = false;
                    pgbState.Visible = true;
                    SetFields(false);

                    toolStripStatusLabel.Text = "Initializing Data Export";
                    if (bwExport.IsBusy != true)
                        bwExport.RunWorkerAsync();
                }
                else
                {
                    MessageBox.Show("Transport already running!");
                }
            }
            else if (currentProfile.Operation == 1)
            {
                if (!transportRunning)
                {
                    buttonStopTransport.Visible = true;
                    runProfileToolStripMenuItem.Enabled = false;
                    pgbState.Visible = true;
                    SetFields(false);

                    toolStripStatusLabel.Text = "Initializing Data Import";
                    label2.Visible = true;
                    label3.Visible = true;
                    label4.Visible = true;
                    if (bwImport.IsBusy != true)
                        bwImport.RunWorkerAsync();
                }
                else
                {
                    MessageBox.Show("Transport already running!");
                }
            }
        }

        private void SetFields(bool state)
        {
            comboBoxTransportationProfiles.Enabled = state;
            comboBoxConnectionSource.Enabled = state;
            comboBoxConnectionTarget.Enabled = state;
            //comboBoxImportMode.Enabled = state;
            comboBoxOperation.Enabled = state;
            buttonLoadEntities.Enabled = state;
            deleteProfileToolStripMenuItem.Enabled = state;
            dataGridView1.Enabled = state;
            pgbState.Value = 0;
        }

        private void buttonStopTransport_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel.Text = "Stopping Transport";
            transportStopped = true;
            bwExport.CancelAsync();
            bwImport.CancelAsync();
        }

        private void transportReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Check if the file exist
            if (!File.Exists(man.ReportFileName))
            {
                MessageBox.Show("Report File not found!");
                return;
            }

            NtoNTransportReportViewer tpv = new NtoNTransportReportViewer(man.ReportFileName, man.importFailuresReportFileName);
            tpv.Show();
            return;
        }
    }
}