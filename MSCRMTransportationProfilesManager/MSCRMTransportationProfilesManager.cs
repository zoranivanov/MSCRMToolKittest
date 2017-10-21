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
    /// MSCRM Transportation Profiles Manager class extending MSCRMToolKitProfileManager
    /// </summary>
    public class MSCRMTransportationProfilesManager : MSCRMToolKitProfileManager
    {
        /// <summary>
        /// The Transportation Profiles
        /// </summary>
        public List<TransportationProfile> Profiles = new List<TransportationProfile>();
        /// <summary>
        /// The Workspace folder
        /// </summary>
        public string Folder = "ReferenceDataTransporter";
        /// <summary>
        /// The configuration file name
        /// </summary>
        private string ConfigurationFileName = "ReferenceDataTransporter\\TransportationProfiles.xml";
        /// <summary>
        /// The report file name
        /// </summary>
        public string ReportFileName;
        /// <summary>
        /// The import failures report file name
        /// </summary>
        public string importFailuresReportFileName;
        /// <summary>
        /// Initializes a new instance of the <see cref="MSCRMTransportationProfilesManager"/> class.
        /// </summary>
        public MSCRMTransportationProfilesManager()
        {
            if (!Directory.Exists(Folder))
                Directory.CreateDirectory(Folder);
            ReadProfiles();
        }

        #region Public Methods

        /// <summary>
        /// Creates the profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        public void CreateProfile(TransportationProfile profile)
        {
            if (!Directory.Exists(Folder + "\\" + profile.ProfileName))
                Directory.CreateDirectory(Folder + "\\" + profile.ProfileName);

            //Refresh Transportation Profiles in case some other instance is updating the profiles
            ReadProfiles();

            //Creating new Transportation Profile
            Profiles.Add(profile);
            WriteProfiles();
        }

        /// <summary>
        /// Updates the profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        public void UpdateProfile(TransportationProfile profile)
        {
            if (!Directory.Exists(Folder + "\\" + profile.ProfileName))
                Directory.CreateDirectory(Folder + "\\" + profile.ProfileName);

            //Refresh Transportation Profiles  in case some other instance is updating the profiles
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
        public void DeleteProfile(TransportationProfile profile)
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
        /// <returns></returns>
        public TransportationProfile GetProfile(string profileName)
        {
            foreach (TransportationProfile retrievedTP in Profiles)
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
        /// <returns></returns>
        public string RunProfile(TransportationProfile profile)
        {
            LogManager.WriteLog("Running Transportation Profile: " + profile.ProfileName);

            //Check if there are selected entities to transport
            if (profile.SelectedEntities == null || profile.SelectedEntities.Count == 0)
            {
                LogManager.WriteLog("No entities selected for transport. Select the entities and then run the profile");
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
            TransportReport tr = new TransportReport(profile.ProfileName);
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

            TransportReport report = ReadTransportReport(ReportFileName);
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
        /// <returns>The Environment Structure</returns>
        public EnvStructure downloadEnvStructure(string connectionName)
        {
            try
            {
                MSCRMConnection connection = cm.getConnection(connectionName);
                _serviceProxy = cm.connect(connection);
                IOrganizationService service = (IOrganizationService)_serviceProxy;
                EnvStructure es = new EnvStructure();
                es.Entities = new List<EnvEntity>();

                RetrieveAllEntitiesRequest request = new RetrieveAllEntitiesRequest()
                {
                    EntityFilters = EntityFilters.Attributes,
                    RetrieveAsIfPublished = false
                };

                // Retrieve the MetaData.
                RetrieveAllEntitiesResponse AllEntitiesResponse = (RetrieveAllEntitiesResponse)_serviceProxy.Execute(request);
                IOrderedEnumerable<EntityMetadata> EMD = AllEntitiesResponse.EntityMetadata.OrderBy(ee => ee.LogicalName);

                List<string> AdditionalEntities = new List<string> {"duplicaterule",
                                                                    "duplicaterulecondition",
                                                                    "incidentresolution",
                                                                    "kbarticlecomment",
                                                                    "opportunityclose",
                                                                    "orderclose",
                                                                    "postcomment",
                                                                    "postlike",
                                                                    "quoteclose",
                                                                    "subject",
                                                                    "uom"};

                List<string> IgnoredEntities = new List<string> { "activitypointer",
                    "asyncoperation",
                    "fieldsecurityprofile",
                    "importjob",
                    "pluginassembly",
                    "plugintype",
                    "processsession",
                    "recurringappointmentmaster",
                    "sdkmessage",
                    "sdkmessagefilter",
                    "sdkmessageprocessingstep",
                    "sdkmessageprocessingstepimage",
                    "sdkmessageprocessingstepsecureconfig",
                    "workflow",
                    "channelaccessprofile"
                };

                List<string> IgnoredAttributes = new List<string> { "importsequencenumber",
                                                                    "statuscode",
                                                                    "timezoneruleversionnumber",
                                                                    "utcconversiontimezonecode",
                                                                    "overriddencreatedon",
                                                                    "ownerid",
                                                                    "haveprivilegeschanged"
                };

                foreach (EntityMetadata currentEntity in EMD)
                {
                    if (currentEntity.LogicalName == "externalpartyitem")
                        AdditionalEntities.Add(currentEntity.LogicalName);
                    if (IgnoredEntities.IndexOf(currentEntity.LogicalName) < 0 && 
                        currentEntity.IsIntersect.Value == false &&                        
                        (currentEntity.IsValidForAdvancedFind.Value || AdditionalEntities.IndexOf(currentEntity.LogicalName) >= 0)
                       )
                    {
                        EnvEntity ee = new EnvEntity();
                        ee.EntityName = currentEntity.LogicalName;
                        ee.Attributes = new List<string>();
                        IOrderedEnumerable<AttributeMetadata> AMD = currentEntity.Attributes.OrderBy(a => a.LogicalName);

                        foreach (AttributeMetadata currentAttribute in AMD)
                        {
                            // Only write out main attributes enabled for reading and creation.
                            if ((currentAttribute.AttributeOf == null) &&
                                IgnoredAttributes.IndexOf(currentAttribute.LogicalName) < 0 &&
                                currentAttribute.IsValidForRead != null &&
                                currentAttribute.IsValidForRead.Value &&
                                currentAttribute.IsValidForCreate != null &&
                                currentAttribute.IsValidForCreate.Value)
                            {
                                ee.Attributes.Add(currentAttribute.LogicalName);
                            }
                        }
                        //Dont export entitites for which only the ID is retrieved
                        if (ee.Attributes.Count > 1)
                            es.Entities.Add(ee);
                    }
                }

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
        /// <returns>The Environment Structure</returns>
        public EnvStructure ReadEnvStructure(string connectionName)
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
                        DataContractSerializer ser = new DataContractSerializer(typeof(EnvStructure));
                        EnvStructure es = (EnvStructure)ser.ReadObject(reader, true);
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
        /// Exports the entity.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <param name="fetchXml">The fetch XML.</param>
        /// <returns></returns>
        public int ExportEntity(TransportationProfile profile, string fetchXml)
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
                    string entityFolderName = Folder + "\\" + profile.ProfileName + "\\Data\\" + returnCollection.EntityName;
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
                foreach (SelectedEntity ee in profile.SelectedEntities)
                {
                    if (entityName == ee.EntityName)
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
        /// <param name="ee">The ee.</param>
        /// <param name="importedRecordsForEntity">The imported records for entity.</param>
        /// <param name="importFailuresForEntity">The import failures for entity.</param>
        /// <param name="entityImportStartDT">The entity import start dt.</param>
        public void updateTransportReport(TransportReport report, SelectedEntity ee, int importedRecordsForEntity, int importFailuresForEntity, DateTime entityImportStartDT)
        {
            bool addNewLine = true;

            foreach (TransportReportLine reportLine in report.ReportLines)
            {
                if (reportLine.Entity == ee.EntityName)
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
                TransportReportLine currentLine = new TransportReportLine();
                currentLine.Entity = ee.EntityName;
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
        /// <param name="tr">The Transport Report.</param>
        /// <param name="transportReportFileName">Name of the transport report file.</param>
        public void WriteTransportReport(TransportReport tr, string transportReportFileName)
        {
            FileStream writer = new FileStream(transportReportFileName, FileMode.Create);
            DataContractSerializer ser = new DataContractSerializer(typeof(TransportReport));
            ser.WriteObject(writer, tr);
            writer.Close();
        }

        /// <summary>
        /// Reads the transport report.
        /// </summary>
        /// <param name="transportReportFileName">Name of the transport report file.</param>
        /// <returns>The Transport Report</returns>
        public TransportReport ReadTransportReport(string transportReportFileName)
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
                        DataContractSerializer ser = new DataContractSerializer(typeof(TransportReport));
                        TransportReport tr = (TransportReport)ser.ReadObject(reader, true);
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
        public void WriteNewImportFailureLine(ImportFailure failure, string importFailuresReportFileName)
        {
            bool reportExists = File.Exists(importFailuresReportFileName);
            List<ImportFailure> failures = new List<ImportFailure>();
            if (reportExists)
            {
                using (FileStream fs = new FileStream(importFailuresReportFileName, FileMode.OpenOrCreate))
                {
                    XmlDictionaryReaderQuotas XRQ = new XmlDictionaryReaderQuotas();
                    XRQ.MaxStringContentLength = int.MaxValue;
                    XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, XRQ);
                    DataContractSerializer ser = new DataContractSerializer(typeof(List<ImportFailure>));
                    failures = (List<ImportFailure>)ser.ReadObject(reader, true);
                    reader.Close();
                    fs.Close();

                }
            }

            using (FileStream writer = new FileStream(importFailuresReportFileName, FileMode.OpenOrCreate))
            {
                failures.Add(failure);
                DataContractSerializer ser = new DataContractSerializer(typeof(List<ImportFailure>));
                //Write updated failures report                
                ser.WriteObject(writer, failures);
                writer.Close();
            }
            
        }

        /// <summary>
        /// Reads the import failures report.
        /// </summary>
        /// <param name="importFailuresReportFileName">Name of the import failures report file.</param>
        /// <returns>The List of Import Failures</returns>
        public List<ImportFailure> ReadImportFailuresReport(string importFailuresReportFileName)
        {
            if (File.Exists(importFailuresReportFileName))
            {
                XmlDictionaryReaderQuotas XRQ = new XmlDictionaryReaderQuotas();
                XRQ.MaxStringContentLength = int.MaxValue;

                using (FileStream fs = new FileStream(importFailuresReportFileName, FileMode.Open))
                using (XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, XRQ))
                {                    
                    DataContractSerializer ser = new DataContractSerializer(typeof(List<ImportFailure>));
                    List<ImportFailure> failures = (List<ImportFailure>)ser.ReadObject(reader, true);
                    return failures;
                }                
            }
            return null;
        }

        /// <summary>
        /// Maps the records.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>The Mapped Record</returns>
        public Entity MapRecords(TransportationProfile profile, Entity entity)
        {
            if (profile.RecordMappings == null || profile.RecordMappings.Count == 0)
                return entity;

            List<KeyValuePair<string, object>> kvList = new List<KeyValuePair<string, object>>();

            foreach (KeyValuePair<string, object> p in entity.Attributes)
            {
                Type t = p.Value.GetType();
                if (t.Name == "EntityReference")
                    kvList.Add(p);
            }

            foreach (KeyValuePair<string, object> kv in kvList)
            {
                EntityReference er = (EntityReference)kv.Value;

                RecordMapping rmList = profile.RecordMappings.Find(rm => rm.EntityName == er.LogicalName && rm.SourceRecordId == er.Id);

                if (rmList == null)
                    continue;

                entity[kv.Key] = new EntityReference { Id = rmList.TargetRecordId, LogicalName = rmList.EntityName };
            }

            return entity;
        }

        #endregion Public Methods

        #region Private Methods
        /// <summary>
        /// Reads the profiles.
        /// </summary>
        private void ReadProfiles()
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(List<TransportationProfile>));
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
                Profiles = (List<TransportationProfile>)ser.ReadObject(reader, true);
            }

            foreach (TransportationProfile retrievedTP in Profiles)
            {
                retrievedTP.setSourceConneciton();
                retrievedTP.setTargetConneciton();
            }
        }

        /// <summary>
        /// Writes the profiles.
        /// </summary>
        private void WriteProfiles()
        {
            FileStream writer = new FileStream(ConfigurationFileName, FileMode.Create);
            DataContractSerializer ser = new DataContractSerializer(typeof(List<TransportationProfile>));
            ser.WriteObject(writer, Profiles);
            writer.Close();
        }

        /// <summary>
        /// Writes the environment structure.
        /// </summary>
        /// <param name="str">The Environment Structure.</param>
        private void WriteEnvStructure(EnvStructure str)
        {
            string filename = Folder + "\\" + str.connectionName + ".xml";
            FileStream writer = new FileStream(filename, FileMode.Create);
            DataContractSerializer ser = new DataContractSerializer(typeof(EnvStructure));
            ser.WriteObject(writer, str);
            writer.Close();
        }

        /// <summary>
        /// Exports the specified profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <param name="transportReportFileName">Name of the transport report file.</param>
        private void Export(TransportationProfile profile, string transportReportFileName)
        {
            try
            {
                TransportReport report = new TransportReport(transportReportFileName);
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
                EnvStructure es = ReadEnvStructure(profile.SourceConnectionName);
                _serviceProxy = cm.connect(connection);
                IOrganizationService service = (IOrganizationService)_serviceProxy;
                List<TransportReportLine> TransportReport = new List<TransportReportLine>();
                profile.TotalExportedRecords = 0;
                //Mesure export time
                DateTime exportStartDT = DateTime.Now;

                LogManager.WriteLog("Start exporting data from " + connection.ConnectionName);

                int recordCount = 0;
                if (es != null)
                {
                    //Order export according to profile's transport order
                    IOrderedEnumerable<SelectedEntity> orderedSelectedEntities = profile.SelectedEntities.OrderBy(se => se.TransportOrder);

                    foreach (SelectedEntity ee in orderedSelectedEntities)
                    {
                        LogManager.WriteLog("Exporting data for entity " + ee.EntityName);
                        DateTime entityExportStartDT = DateTime.Now;
                        string fetchXml = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>";
                        fetchXml += "<entity name='" + ee.EntityName + "'>";
                        //Get Entity structure
                        EnvEntity strE = new EnvEntity();
                        foreach (EnvEntity envE in es.Entities)
                        {
                            if (envE.EntityName == ee.EntityName)
                            {
                                strE = envE;
                                break;
                            }
                        }

                        //Create fetchXML Query
                        foreach (string ea in strE.Attributes)
                        {
                            if (ee.IgnoredAttributes == null)
                            {
                                fetchXml += "<attribute name='" + ea + "' />";
                            }
                            else if (!ee.IgnoredAttributes.Contains(ea))
                            {
                                fetchXml += "<attribute name='" + ea + "' />";
                            }
                        }

                        //Add Query filter
                        fetchXml += ee.Filter;

                        fetchXml += "</entity></fetch>";
                        int recordCountPerEntity = ExportEntity(profile, fetchXml);
                        recordCount += recordCountPerEntity;

                        DateTime entityExportEndDT = DateTime.Now;
                        TimeSpan ts = entityExportEndDT - entityExportStartDT;
                        TransportReportLine transportReportLine = new TransportReportLine();
                        transportReportLine.Entity = ee.EntityName;
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
        private void Import(TransportationProfile profile, string transportReportFileName)
        {
            int totalTreatedRecords = 0;
            int totalImportFailures = 0;
            int totalImportSuccess = 0;
            int ReconnectionRetryCount = 5;

            try
            {
                TransportReport report = new TransportReport(transportReportFileName);
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

                //Order import according to profile's import order
                IOrderedEnumerable<SelectedEntity> orderedSelectedEntities = profile.SelectedEntities.OrderBy(e => e.TransportOrder);

                foreach (SelectedEntity ee in orderedSelectedEntities)
                {
                    //Check if there are any records to import
                    if (ee.ExportedRecords == 0)
                    {
                        continue;
                    }

                    //Mesure import time
                    DateTime entityImportStartDT = DateTime.Now;

                    string entityFolderPath = Folder + "\\" + profile.ProfileName + "\\Data\\" + ee.EntityName;
                    string[] filePaths = Directory.GetFiles(entityFolderPath, "*.xml");

                    LogManager.WriteLog("Importing " + ee.EntityName + " records.");
                    int treatedRecordsForEntity = 0;
                    int importedRecordsForEntity = 0;
                    int importFailuresForEntity = 0;
                    foreach (string filePath in filePaths)
                    {
                        List<Type> knownTypes = new List<Type>();
                        knownTypes.Add(typeof(Entity));

                        XmlDictionaryReaderQuotas XRQ = new XmlDictionaryReaderQuotas();
                        XRQ.MaxStringContentLength = int.MaxValue;

                        using (FileStream fs = new FileStream(filePath, FileMode.Open))
                        using (XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, XRQ))
                        {                            
                            DataContractSerializer ser = new DataContractSerializer(typeof(EntityCollection), knownTypes);
                            EntityCollection fromDisk = (EntityCollection)ser.ReadObject(reader, true);

                            foreach (Entity e in fromDisk.Entities)
                            {
                                //Records mapping for the Lookup attributes
                                Entity entity = MapRecords(profile, e);

                                string executingOperation = "";
                                try
                                {
                                    if (profile.ImportMode == 0)
                                    {
                                        executingOperation = "Create";
                                        service.Create(entity);
                                    }
                                    else if (profile.ImportMode == 1)
                                    {
                                        try
                                        {
                                            executingOperation = "Update";
                                            service.Update(entity);
                                        }
                                        catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault>)
                                        {
                                            executingOperation = "Create";
                                            service.Create(entity);
                                        }
                                    }
                                    else if (profile.ImportMode == 2)
                                    {
                                        executingOperation = "Update";
                                        service.Update(entity);
                                    }
                                    importedRecordsForEntity++;
                                    totalImportSuccess++;
                                }
                                catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
                                {
                                    totalImportFailures++;
                                    importFailuresForEntity++;
                                    ImportFailure failure = new ImportFailure
                                    {
                                        CreatedOn = DateTime.Now.ToString(),
                                        EntityName = ee.EntityName,
                                        Operation = executingOperation,
                                        Reason = ex.Detail.Message,
                                        Url = profile.getSourceConneciton().ServerAddress + "main.aspx?pagetype=entityrecord&etn=" + ee.EntityName + "&id=" + entity.Id.ToString()
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
                                        ImportFailure failure = new ImportFailure
                                        {
                                            CreatedOn = DateTime.Now.ToString(),
                                            EntityName = ee.EntityName,
                                            Operation = executingOperation,
                                            Reason = ex.InnerException.Message,
                                            Url = profile.getSourceConneciton().ServerAddress + "main.aspx?pagetype=entityrecord&etn=" + ee.EntityName + "&id=" + entity.Id.ToString()
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
                    LogManager.WriteLog("Treated " + treatedRecordsForEntity + " " + ee.EntityName + " records with " + importedRecordsForEntity + " successfully imported records and " + importFailuresForEntity + " failures.");
                }

                TimeSpan importTimeSpan = DateTime.Now - importStartDT;
                LogManager.WriteLog("Import finished for " + connection.ConnectionName + ". Treated " + totalTreatedRecords + " records in " + importTimeSpan.ToString().Substring(0, 10) + ". Successfuly imported " + totalImportSuccess + " records and " + totalImportFailures + " failures.");
                //WriteTransportReport(report, transportReportFileName);
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