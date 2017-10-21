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
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Xml;

namespace MSCRMToolKit
{
    /// <summary>
    /// MSCRM N to N Associations Transport Manager class extending MSCRMToolKitProfileManager
    /// </summary>
    public class MSCRMNtoNAssociationsTransportManager : MSCRMToolKitProfileManager
    {
        /// <summary>
        /// The profiles
        /// </summary>
        public List<NtoNAssociationsTransportProfile> Profiles = new List<NtoNAssociationsTransportProfile>();
        /// <summary>
        /// The Workspace folder
        /// </summary>
        public string Folder = "NtoNAssociationsTransporter";
        /// <summary>
        /// The configuration file name
        /// </summary>
        private string ConfigurationFileName = "NtoNAssociationsTransporter\\NtoNAssociationsTransporter.xml";
        /// <summary>
        /// The report file name
        /// </summary>
        public string ReportFileName;
        /// <summary>
        /// The import failures report file name
        /// </summary>
        public string importFailuresReportFileName;

        #region Public Methods
        /// <summary>
        /// Initializes a new instance of the <see cref="MSCRMNtoNAssociationsTransportManager"/> class.
        /// </summary>
        public MSCRMNtoNAssociationsTransportManager()
        {
            if (!Directory.Exists(Folder))
                Directory.CreateDirectory(Folder);
            ReadProfiles();
        }

        /// <summary>
        /// Creates the profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        public void CreateProfile(NtoNAssociationsTransportProfile profile)
        {
            if (!Directory.Exists(Folder + "\\" + profile.ProfileName))
                Directory.CreateDirectory(Folder + "\\" + profile.ProfileName);

            //Refresh Profiles in case some other instance is updating the profiles
            ReadProfiles();

            //Creating new Profile
            Profiles.Add(profile);
            WriteProfiles();
        }

        /// <summary>
        /// Updates the profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        public void UpdateProfile(NtoNAssociationsTransportProfile profile)
        {
            if (!Directory.Exists(Folder + "\\" + profile.ProfileName))
                Directory.CreateDirectory(Folder + "\\" + profile.ProfileName);

            //Refresh Profiles  in case some other instance is updating the profiles
            ReadProfiles();

            for (int i = 0; i < Profiles.Count; i++)
            {
                if (Profiles[i].ProfileName == profile.ProfileName)
                {
                    Profiles[i] = profile;
                    break;
                }
            }
            WriteProfiles();
        }

        /// <summary>
        /// Deletes the profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        public void DeleteProfile(NtoNAssociationsTransportProfile profile)
        {
            //Refresh Profiles in case some other instance is updating the profiles
            ReadProfiles();

            for (int i = 0; i < Profiles.Count; i++)
            {
                if (Profiles[i].ProfileName == profile.ProfileName)
                {
                    Profiles.RemoveAt(i);
                }
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
        /// <returns>The N to N Associations Transport Profile</returns>
        public NtoNAssociationsTransportProfile GetProfile(string profileName)
        {
            foreach (NtoNAssociationsTransportProfile retrievedTP in Profiles)
            {
                if (profileName == retrievedTP.ProfileName)
                {
                    return retrievedTP;
                }
            }
            return null;
        }

        /// <summary>
        /// Runs the profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns>The Execution Report File Name</returns>
        public string RunProfile(NtoNAssociationsTransportProfile profile)
        {
            LogManager.WriteLog("Running N*N Transportation Profile: " + profile.ProfileName);

            //Check if there are selected Relationships to transport
            if (profile.SelectedNtoNRelationships == null || profile.SelectedNtoNRelationships.Count == 0)
            {
                LogManager.WriteLog("No Relationships selected for transport. Select the Relationships and then run the profile");
                return "";
            }

            DateTime now = DateTime.Now;
            ReportFileName = Folder + "\\" + profile.ProfileName + "\\ExecutionReports\\TransportReport" + now.Year + "-" + now.Month + "-" + now.Day + "-" + now.Hour + "-" + now.Minute + "-" + now.Second + ".xml";
            importFailuresReportFileName = Folder + "\\" + profile.ProfileName + "\\ExecutionReports\\ImportFailuresReport" + now.Year + "-" + now.Month + "-" + now.Day + "-" + now.Hour + "-" + now.Minute + "-" + now.Second + ".xml";

            //Initialize Execution Reports folder
            string executionReportsFolder = Folder + "\\" + profile.ProfileName + "\\ExecutionReports";
            if (!Directory.Exists(executionReportsFolder))
            {
                Directory.CreateDirectory(executionReportsFolder);
            }

            //Create Transport Report
            NtoNTransportReport tr = new NtoNTransportReport(profile.ProfileName);
            WriteTransportReport(tr, ReportFileName);

            //Check for the Operation to execute
            if (profile.Operation == 0)
            {
                //Export data
                Export(profile, ReportFileName);
            }
            else if (profile.Operation == 1)
            {
                //Import Data
                Import(profile, ReportFileName);
            }
            else if (profile.Operation == 2)
            {
                //Transport data => Export + Import
                Export(profile, ReportFileName);
                Import(profile, ReportFileName);
            }

            NtoNTransportReport report = ReadTransportReport(ReportFileName);
            report.TransportFinishedAt = DateTime.Now.ToString();
            report.TransportCompleted = true;
            TimeSpan TransportTimeSpan = DateTime.Now - Convert.ToDateTime(report.TransportStartedAt);
            report.TransportedIn = TransportTimeSpan.ToString().Substring(0, 10);
            WriteTransportReport(report, ReportFileName);

            return ReportFileName;
        }

        /// <summary>
        /// Downloads the env structure.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns>The N to N Relationships Structure</returns>
        public NtoNRelationshipsStructure downloadEnvStructure(string connectionName)
        {
            try
            {
                MSCRMConnection connection = cm.getConnection(connectionName);
                _serviceProxy = cm.connect(connection);
                IOrganizationService service = (IOrganizationService)_serviceProxy;
                NtoNRelationshipsStructure es = new NtoNRelationshipsStructure();
                es.NtoNRelationships = new List<NtoNRelationship>();

                RetrieveAllEntitiesRequest request = new RetrieveAllEntitiesRequest()
                {
                    EntityFilters = EntityFilters.Relationships,
                    RetrieveAsIfPublished = false
                };

                // Retrieve the MetaData.
                RetrieveAllEntitiesResponse AllEntitiesResponse = (RetrieveAllEntitiesResponse)_serviceProxy.Execute(request);
                List<string> TreatedRelationships = new List<string>();

                foreach (EntityMetadata currentEntity in AllEntitiesResponse.EntityMetadata)
                {
                    List<ManyToManyRelationshipMetadata> ManyToManyRelationships = currentEntity.ManyToManyRelationships.ToList();
                    foreach (ManyToManyRelationshipMetadata relationship in ManyToManyRelationships)
                    {
                        if (!relationship.IsValidForAdvancedFind.Value)
                            continue;

                        if (relationship.IntersectEntityName == "subscriptionmanuallytrackedobject")
                            continue;

                        if (TreatedRelationships.Find(r => r == relationship.SchemaName) != null)
                            continue;

                        NtoNRelationship ee = new NtoNRelationship();
                        // Start Entity Node
                        ee.RelationshipSchemaName = relationship.SchemaName;
                        ee.IntersectEntityName = relationship.IntersectEntityName;
                        ee.Entity1LogicalName = relationship.Entity1LogicalName;
                        ee.Entity1IntersectAttribute = relationship.Entity1IntersectAttribute;
                        ee.Entity2LogicalName = relationship.Entity2LogicalName;
                        ee.Entity2IntersectAttribute = relationship.Entity2IntersectAttribute;

                        // End Entity Node
                        TreatedRelationships.Add(relationship.SchemaName);
                        es.NtoNRelationships.Add(ee);
                    }
                }

                //Order by SchemaName
                IOrderedEnumerable<NtoNRelationship> orderedNtoNRelationships = es.NtoNRelationships.OrderBy(se => se.RelationshipSchemaName);
                es.NtoNRelationships = orderedNtoNRelationships.ToList<NtoNRelationship>();

                es.connectionName = connectionName;
                WriteEnvStructure(es);

                return es;
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault>)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Reads the env structure.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns>The N to N Relationships Structure</returns>
        public NtoNRelationshipsStructure ReadEnvStructure(string connectionName)
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
                        DataContractSerializer ser = new DataContractSerializer(typeof(NtoNRelationshipsStructure));
                        NtoNRelationshipsStructure es = (NtoNRelationshipsStructure)ser.ReadObject(reader, true);
                        return es;
                    }                    
                }
                catch (Exception)
                {
                    LogManager.WriteLog("Error while reading the structure of connection" + connectionName + ". The structure file may be corrupted.");
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// Exports the entity.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <param name="fetchXml">The fetch fetchXML query.</param>
        /// <param name="RelationshipSchemaName">Name of the relationship schema.</param>
        /// <returns>The number of exported record for the Entity</returns>
        public int ExportEntity(NtoNAssociationsTransportProfile profile, string fetchXml, string RelationshipSchemaName)
        {
            //Set the number of records per page to retrieve.
            //This value should not be bigger than 5000 as this is the limit of records provided by the CRM
            int fetchCount = 5000;
            // Initialize the file number.
            int fileNumber = 1;
            // Initialize the number of records.
            int recordCount = 0;
            // Specify the current paging cookie. For retrieving the first page, pagingCookie should be null.
            string pagingCookie = null;
            string entityName = "";

            while (true)
            {
                // Build fetchXml string with the placeholders.
                string xml = CreateXml(fetchXml, pagingCookie, fileNumber, fetchCount);

                // Execute the fetch query and get the xml result.

                RetrieveMultipleRequest fetchRequest = new RetrieveMultipleRequest
                {
                    Query = new FetchExpression(xml)
                };

                EntityCollection returnCollection = ((RetrieveMultipleResponse)_serviceProxy.Execute(fetchRequest)).EntityCollection;
                recordCount += returnCollection.Entities.Count;
                if (recordCount > 0)
                {
                    string entityFolderName = Folder + "\\" + profile.ProfileName + "\\Data\\" + RelationshipSchemaName;
                    entityName = returnCollection.EntityName;
                    if (!Directory.Exists(entityFolderName))
                        Directory.CreateDirectory(entityFolderName);
                    List<Type> knownTypes = new List<Type>();
                    knownTypes.Add(typeof(Entity));
                    int fileCpt = 1000000 + fileNumber;
                    string filename = entityFolderName + "\\" + fileCpt + ".xml";
                    FileStream writer = new FileStream(filename, FileMode.Create);
                    DataContractSerializer ser = new DataContractSerializer(typeof(EntityCollection), knownTypes);
                    ser.WriteObject(writer, returnCollection);
                    writer.Close();
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

            if (recordCount > 0)
            {
                //Save Exported Entites number
                foreach (SelectedNtoNRelationship ee in profile.SelectedNtoNRelationships)
                {
                    if (RelationshipSchemaName == ee.RelationshipSchemaName)
                    {
                        ee.ExportedRecords = recordCount;
                    }
                }

                profile.TotalExportedRecords += recordCount;
                UpdateProfile(profile);
            }
            LogManager.WriteLog("Exported " + recordCount + " " + entityName + " records.");

            return recordCount;
        }

        /// <summary>
        /// Updates the transport report.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="ee">The Selected N to N Relationship.</param>
        /// <param name="importedRecordsForEntity">The imported records for entity.</param>
        /// <param name="importFailuresForEntity">The import failures for entity.</param>
        /// <param name="entityImportStartDT">The entity import start dt.</param>
        public void updateTransportReport(NtoNTransportReport report, SelectedNtoNRelationship ee, int importedRecordsForEntity, int importFailuresForEntity, DateTime entityImportStartDT)
        {
            bool addNewLine = true;

            foreach (NtoNTransportReportLine reportLine in report.ReportLines)
            {
                if (reportLine.RelationshipSchemaName == ee.RelationshipSchemaName)
                {
                    reportLine.ImportedRecords = importedRecordsForEntity;
                    report.TotalImportedRecords += importedRecordsForEntity;
                    DateTime entityImportEndDT = DateTime.Now;
                    TimeSpan ts = entityImportEndDT - entityImportStartDT;
                    reportLine.ImportStartedAt = entityImportStartDT.ToString();
                    reportLine.ImportFinishedAt = entityImportEndDT.ToString();
                    reportLine.ImportedIn = ts.ToString().Substring(0, 10);
                    reportLine.ImportFailures = importFailuresForEntity;
                    addNewLine = false;
                    break;
                }
            }

            if (addNewLine)
            {
                NtoNTransportReportLine currentLine = new NtoNTransportReportLine();
                currentLine.RelationshipSchemaName = ee.RelationshipSchemaName;
                currentLine.ImportedRecords = importedRecordsForEntity;
                report.TotalImportedRecords += importedRecordsForEntity;
                DateTime entityImportEndDT = DateTime.Now;
                TimeSpan ts = entityImportEndDT - entityImportStartDT;
                currentLine.ImportStartedAt = entityImportStartDT.ToString();
                currentLine.ImportFinishedAt = entityImportEndDT.ToString();
                currentLine.ImportedIn = ts.ToString().Substring(0, 10);
                currentLine.ImportFailures = importFailuresForEntity;
                report.ReportLines.Add(currentLine);
            }
            WriteTransportReport(report, ReportFileName);
        }

        /// <summary>
        /// Writes the transport report.
        /// </summary>
        /// <param name="tr">The N to N Transport Report.</param>
        /// <param name="transportReportFileName">Name of the transport report file.</param>
        public void WriteTransportReport(NtoNTransportReport tr, string transportReportFileName)
        {
            FileStream writer = new FileStream(transportReportFileName, FileMode.Create);
            DataContractSerializer ser = new DataContractSerializer(typeof(NtoNTransportReport));
            ser.WriteObject(writer, tr);
            writer.Close();
        }

        /// <summary>
        /// Reads the transport report.
        /// </summary>
        /// <param name="transportReportFileName">Name of the transport report file.</param>
        /// <returns>The N to N Transport Report.</returns>
        public NtoNTransportReport ReadTransportReport(string transportReportFileName)
        {
            if (File.Exists(transportReportFileName))
            {
                try
                {
                    XmlDictionaryReaderQuotas XRQ = new XmlDictionaryReaderQuotas();
                    XRQ.MaxStringContentLength = int.MaxValue;

                    using (FileStream fs = new FileStream(transportReportFileName, FileMode.Open))
                    using (XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, XRQ))
                    {                       
                        DataContractSerializer ser = new DataContractSerializer(typeof(NtoNTransportReport));
                        NtoNTransportReport tr = (NtoNTransportReport)ser.ReadObject(reader, true);
                        return tr;
                    }                    
                }
                catch (SerializationException)
                {
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// Writes the new import failure line.
        /// </summary>
        /// <param name="failure">The failure.</param>
        /// <param name="importFailuresReportFileName">Name of the import failures report file.</param>
        public void WriteNewImportFailureLine(NtoNRelationshipsImportFailure failure, string importFailuresReportFileName)
        {
            bool reportExists = File.Exists(importFailuresReportFileName);

            using (FileStream fs = new FileStream(importFailuresReportFileName, FileMode.OpenOrCreate))
            using (FileStream writer = new FileStream(importFailuresReportFileName, FileMode.Create))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(List<NtoNRelationshipsImportFailure>));
                List<NtoNRelationshipsImportFailure> failures = new List<NtoNRelationshipsImportFailure>();
                if (!reportExists)
                {
                    failures = new List<NtoNRelationshipsImportFailure>();
                }
                else
                {
                    XmlDictionaryReaderQuotas XRQ = new XmlDictionaryReaderQuotas();
                    XRQ.MaxStringContentLength = int.MaxValue;
                    XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, XRQ);
                    failures = (List<NtoNRelationshipsImportFailure>)ser.ReadObject(reader, true);
                    reader.Close();
                }
                failures.Add(failure);

                //Write updated failures report
                
                ser.WriteObject(writer, failures);
            }            
        }

        /// <summary>
        /// Reads the import failures report.
        /// </summary>
        /// <param name="importFailuresReportFileName">Name of the import failures report file.</param>
        /// <returns>List of N to N Relationships Import Failures.</returns>
        public List<NtoNRelationshipsImportFailure> ReadImportFailuresReport(string importFailuresReportFileName)
        {
            if (File.Exists(importFailuresReportFileName))
            {
                XmlDictionaryReaderQuotas XRQ = new XmlDictionaryReaderQuotas();
                XRQ.MaxStringContentLength = int.MaxValue;
                using (FileStream fs = new FileStream(importFailuresReportFileName, FileMode.Open))
                using (XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, XRQ))
                {                    
                    DataContractSerializer ser = new DataContractSerializer(typeof(List<NtoNRelationshipsImportFailure>));
                    List<NtoNRelationshipsImportFailure> failures = (List<NtoNRelationshipsImportFailure>)ser.ReadObject(reader, true);

                    return failures;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the related entity unique identifier.
        /// </summary>
        /// <param name="RelatedEntity">The related entity.</param>
        /// <returns>Guid of the Related Entity</returns>
        /// <exception cref="System.Exception">Unknown type</exception>
        public Guid getRelatedEntityGuid(object RelatedEntity)
        {
            Guid relatedEntity1Id = Guid.Empty;
            Type t = RelatedEntity.GetType();
            if (t.Name == "EntityReference")
            {
                EntityReference er = (EntityReference)RelatedEntity;
                relatedEntity1Id = er.Id;
            }
            else if (t.Name == "Guid")
            {
                relatedEntity1Id = (Guid)RelatedEntity;
            }
            else
                throw new Exception("Unknown type");

            return relatedEntity1Id;
        }

        /// <summary>
        /// Alreadies the associated.
        /// </summary>
        /// <param name="osp">The Organization Service Proxy.</param>
        /// <param name="ee">The Selected N to N Relationship.</param>
        /// <param name="Record1">The record 1 Id.</param>
        /// <param name="Record2">The record 2 Id.</param>
        /// <returns>True or False if the two records are already associated</returns>
        public bool AlreadyAssociated(OrganizationServiceProxy osp, SelectedNtoNRelationship ee, Guid Record1, Guid Record2)
        {
            string fetchXML = string.Format(@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' count='1'>
                                      <entity name='{0}'>
                                        <attribute name='{1}' />
                                        <attribute name='{2}' />
                                        <order attribute='{1}' descending='false' />
                                        <filter type='and'>
                                           <condition attribute='{1}' operator='eq' uitype='account' value='{3}' />
                                           <condition attribute='{2}' operator='eq' uitype='contact' value='{4}' />
                                         </filter>
                                      </entity>
                                    </fetch> ", ee.IntersectEntityName, ee.Entity1IntersectAttribute, ee.Entity2IntersectAttribute, Record1, Record2);

            EntityCollection result = osp.RetrieveMultiple(new FetchExpression(fetchXML));
            if (result != null && result.Entities.Count > 0 && result.Entities[0] != null)
                return true;
            else
                return false;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Reads the profiles.
        /// </summary>
        private void ReadProfiles()
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(List<NtoNAssociationsTransportProfile>));
            if (!File.Exists(ConfigurationFileName))
            {
                FileStream writer = new FileStream(ConfigurationFileName, FileMode.Create);
                ser.WriteObject(writer, Profiles);
                writer.Close();
            }

            XmlDictionaryReaderQuotas XRQ = new XmlDictionaryReaderQuotas();
            XRQ.MaxStringContentLength = int.MaxValue;

            using(FileStream fs = new FileStream(ConfigurationFileName, FileMode.Open))
            using (XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, XRQ))
            {
                Profiles = (List<NtoNAssociationsTransportProfile>)ser.ReadObject(reader, true);

                foreach (NtoNAssociationsTransportProfile retrievedTP in Profiles)
                {
                    retrievedTP.setSourceConneciton();
                    retrievedTP.setTargetConneciton();
                }
            }
        }

        /// <summary>
        /// Writes the profiles.
        /// </summary>
        private void WriteProfiles()
        {
            FileStream writer = new FileStream(ConfigurationFileName, FileMode.Create);
            DataContractSerializer ser = new DataContractSerializer(typeof(List<NtoNAssociationsTransportProfile>));
            ser.WriteObject(writer, Profiles);
            writer.Close();
        }

        /// <summary>
        /// Writes the env structure.
        /// </summary>
        /// <param name="str">The string.</param>
        private void WriteEnvStructure(NtoNRelationshipsStructure str)
        {
            string filename = Folder + "\\" + str.connectionName + ".xml";
            FileStream writer = new FileStream(filename, FileMode.Create);
            DataContractSerializer ser = new DataContractSerializer(typeof(NtoNRelationshipsStructure));
            ser.WriteObject(writer, str);
            writer.Close();
        }

        /// <summary>
        /// Exports the specified profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <param name="transportReportFileName">Name of the transport report file.</param>
        private void Export(NtoNAssociationsTransportProfile profile, string transportReportFileName)
        {
            NtoNTransportReport report = null;
            try
            {
                report = new NtoNTransportReport(transportReportFileName);
                //Get Transport Report
                if (File.Exists(transportReportFileName))
                {
                    report = ReadTransportReport(transportReportFileName);
                }

                //Clean Data folder
                string dataExportFolder = Folder + "\\" + profile.ProfileName + "\\Data";
                if (Directory.Exists(dataExportFolder))
                {
                    Directory.Delete(dataExportFolder, true);
                }
                Directory.CreateDirectory(dataExportFolder);

                MSCRMConnection connection = profile.getSourceConneciton();
                NtoNRelationshipsStructure es = ReadEnvStructure(profile.SourceConnectionName);
                _serviceProxy = cm.connect(connection);
                IOrganizationService service = (IOrganizationService)_serviceProxy;
                List<NtoNTransportReportLine> TransportReport = new List<NtoNTransportReportLine>();
                profile.TotalExportedRecords = 0;
                //Mesure export time
                DateTime exportStartDT = DateTime.Now;

                LogManager.WriteLog("Start exporting data from " + connection.ConnectionName);

                int recordCount = 0;
                if (es != null)
                {
                    foreach (SelectedNtoNRelationship ee in profile.SelectedNtoNRelationships)
                    {
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
                        int recordCountPerEntity = ExportEntity(profile, fetchXml, ee.RelationshipSchemaName);
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
                        WriteTransportReport(report, transportReportFileName);
                    }
                }

                TimeSpan exportTimeSpan = DateTime.Now - exportStartDT;
                LogManager.WriteLog("Export finished for " + profile.SourceConnectionName + ". Exported " + recordCount + " records in " + exportTimeSpan.ToString().Substring(0, 10));
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                LogManager.WriteLog("Error:" + ex.Detail.Message + "\n" + ex.Detail.TraceText);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    LogManager.WriteLog("Error:" + ex.Message + "\n" + ex.InnerException.Message);
                }
                else
                {
                    LogManager.WriteLog("Error:" + ex.Message);
                }
            }
        }

        /// <summary>
        /// Imports the specified profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <param name="transportReportFileName">Name of the transport report file.</param>
        private void Import(NtoNAssociationsTransportProfile profile, string transportReportFileName)
        {
            int totalTreatedRecords = 0;
            int totalImportFailures = 0;
            int totalImportSuccess = 0;
            int ReconnectionRetryCount = 5;

            try
            {
                NtoNTransportReport report = new NtoNTransportReport(transportReportFileName);
                //Get Transport Report
                if (File.Exists(transportReportFileName))
                {
                    report = ReadTransportReport(transportReportFileName);
                }

                MSCRMConnection connection = profile.getTargetConneciton(); ;
                _serviceProxy = cm.connect(connection);
                IOrganizationService service = (IOrganizationService)_serviceProxy;
                LogManager.WriteLog("Start importing data in " + connection.ConnectionName);

                //Mesure import time
                DateTime importStartDT = DateTime.Now;

                //es = ReadEnvStructure(profile.SourceConnectionName);

                foreach (SelectedNtoNRelationship ee in profile.SelectedNtoNRelationships)
                {
                    //Check if there are any records to import
                    if (ee.ExportedRecords == 0)
                    {
                        continue;
                    }

                    //Mesure import time
                    DateTime entityImportStartDT = DateTime.Now;

                    string entityFolderPath = Folder + "\\" + profile.ProfileName + "\\Data\\" + ee.RelationshipSchemaName;
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
                                EntityReference relatedEntity1 = new EntityReference();
                                EntityReference relatedEntity2 = new EntityReference();

                                try
                                {
                                    Guid relatedEntity1Id = getRelatedEntityGuid(en[ee.Entity1IntersectAttribute]);
                                    Guid relatedEntity2Id = getRelatedEntityGuid(en[ee.Entity2IntersectAttribute]);

                                    relatedEntity1 = new EntityReference { LogicalName = ee.Entity1LogicalName, Id = relatedEntity1Id };
                                    relatedEntity2 = new EntityReference { LogicalName = ee.Entity2LogicalName, Id = relatedEntity2Id };

                                    if (!AlreadyAssociated(_serviceProxy, ee, relatedEntity1Id, relatedEntity2Id))
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
                                    totalImportFailures++;
                                    importFailuresForEntity++;
                                    NtoNRelationshipsImportFailure failure = new NtoNRelationshipsImportFailure
                                    {
                                        CreatedOn = DateTime.Now.ToString(),
                                        NtoNRelationshipName = ee.RelationshipSchemaName,
                                        Reason = ex.Detail.Message,
                                        UrlEntity1 = profile.getSourceConneciton().ServerAddress + "main.aspx?pagetype=entityrecord&etn=" + relatedEntity1.LogicalName + "&id=" + relatedEntity1.Id.ToString(),
                                        UrlEntity2 = profile.getSourceConneciton().ServerAddress + "main.aspx?pagetype=entityrecord&etn=" + relatedEntity2.LogicalName + "&id=" + relatedEntity2.Id.ToString()
                                    };
                                    report.TotalImportFailures += 1;
                                    //Insert the Failure line in the Failures Report
                                    WriteNewImportFailureLine(failure, importFailuresReportFileName);
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
                                            UrlEntity1 = profile.getSourceConneciton().ServerAddress + "main.aspx?pagetype=entityrecord&etn=" + relatedEntity1.LogicalName + "&id=" + relatedEntity1.Id.ToString(),
                                            UrlEntity2 = profile.getSourceConneciton().ServerAddress + "main.aspx?pagetype=entityrecord&etn=" + relatedEntity2.LogicalName + "&id=" + relatedEntity2.Id.ToString()
                                        };
                                        report.TotalImportFailures += 1;
                                        //Insert the Failure line in the Failures Report
                                        WriteNewImportFailureLine(failure, importFailuresReportFileName);
                                    }
                                    else
                                    {
                                        throw;
                                    }
                                }
                                totalTreatedRecords++;
                                treatedRecordsForEntity++;
                                updateTransportReport(report, ee, importedRecordsForEntity, importFailuresForEntity, entityImportStartDT);
                            }
                        }
                    }
                    LogManager.WriteLog("Treated " + treatedRecordsForEntity + " " + ee.RelationshipSchemaName + " records with " + importedRecordsForEntity + " successfully imported records and " + importFailuresForEntity + " failures.");
                }

                TimeSpan importTimeSpan = DateTime.Now - importStartDT;
                LogManager.WriteLog("Import finished for " + connection.ConnectionName + ". Treated " + totalTreatedRecords + " records in " + importTimeSpan.ToString().Substring(0, 10) + ". Successfuly imported " + totalImportSuccess + " records and " + totalImportFailures + " failures.");
                WriteTransportReport(report, transportReportFileName);
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                LogManager.WriteLog("Error:" + ex.Detail.Message + "\n" + ex.Detail.TraceText);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    LogManager.WriteLog("Error:" + ex.Message + "\n" + ex.InnerException.Message);
                else
                {
                    LogManager.WriteLog("Error:" + ex.Message);
                }
            }
        }

        #endregion Private Methods
    }
}