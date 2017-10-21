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

using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security;
using System.Text;
using System.Xml;

namespace MSCRMToolKit
{
    /// <summary>
    /// MSCRM Audit Export Manager class extending MSCRMToolKitProfileManager
    /// </summary>
    public class MSCRMAuditExportManager : MSCRMToolKitProfileManager
    {
        /// <summary>
        /// The profiles
        /// </summary>
        public List<MSCRMAuditExportProfile> Profiles = new List<MSCRMAuditExportProfile>();
        /// <summary>
        /// The Workspace folder
        /// </summary>
        public string Folder = "AuditExportManager";
        /// <summary>
        /// The configuration file name
        /// </summary>
        private string ConfigurationFileName = "AuditExportManager\\Profiles.xml";
        /// <summary>
        /// The report file name
        /// </summary>
        public string ReportFileName;
        /// <summary>
        /// The file name
        /// </summary>
        public string fileName = "";
        /// <summary>
        /// The encoding
        /// </summary>
        private Encoding encoding = System.Text.Encoding.Default;
        /// <summary>
        /// The Environment Audit Structure
        /// </summary>
        private EnvAuditStructure es = new EnvAuditStructure();
        /// <summary>
        /// Initializes a new instance of the <see cref="MSCRMAuditExportManager"/> class.
        /// </summary>
        public MSCRMAuditExportManager()
        {
            if (!Directory.Exists(Folder))
                Directory.CreateDirectory(Folder);
            ReadProfiles();
        }

        /// <summary>
        /// Creates the profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        public void CreateProfile(MSCRMAuditExportProfile profile)
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
        /// <exception cref="System.Exception">Audit Export Profile Update failed. The Audit Export Profile  + profile.ProfileName +  was not found in the configuration file.</exception>
        public void UpdateProfile(MSCRMAuditExportProfile profile)
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
                LogManager.WriteLog("Audit Export Profile Update failed. The Audit Export Profile " + profile.ProfileName + " was not found in the configuration file.");
                throw new Exception("Audit Export Profile Update failed. The Audit Export Profile " + profile.ProfileName + " was not found in the configuration file.");
            }

            WriteProfiles();
        }

        /// <summary>
        /// Deletes the profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <exception cref="System.Exception">Audit Export Profile deletion failed. The Audit Export Profile  + profile.ProfileName +  was not found in the configuration file.</exception>
        public void DeleteProfile(MSCRMAuditExportProfile profile)
        {
            int index = Profiles.FindIndex(d => d.ProfileName == profile.ProfileName);
            if (index > -1)
            {
                Profiles.RemoveAt(index);
            }
            else
            {
                LogManager.WriteLog("Audit Export Profile deletion failed. The Audit Export Profile " + profile.ProfileName + " was not found in the configuration file.");
                throw new Exception("Audit Export Profile deletion failed. The Audit Export Profile " + profile.ProfileName + " was not found in the configuration file.");
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
        public MSCRMAuditExportProfile GetProfile(string profileName)
        {
            return Profiles.Find(d => d.ProfileName == profileName);
        }

        /// <summary>
        /// Runs the profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        public void RunProfile(MSCRMAuditExportProfile profile)
        {
            LogManager.WriteLog("Running Audit Export Profile: " + profile.ProfileName);
            DateTime now = DateTime.Now;
            ReportFileName = Folder + "\\" + profile.ProfileName + "\\ExecutionReports\\AuditExportReport" + now.Year + "-" + now.Month + "-" + now.Day + "-" + now.Hour + "-" + now.Minute + "-" + now.Second + ".xml";

            //Initialize Execution Reports folder
            string executionReportsFolder = Folder + "\\" + profile.ProfileName + "\\ExecutionReports";
            if (!Directory.Exists(executionReportsFolder))
            {
                Directory.CreateDirectory(executionReportsFolder);
            }

            //Set Encoding
            if (profile.Encoding == "UTF8")
                encoding = System.Text.Encoding.UTF8;
            else if (profile.Encoding == "Unicode")
                encoding = System.Text.Encoding.Unicode;
            else if (profile.Encoding == "ASCII")
                encoding = System.Text.Encoding.ASCII;
            else if (profile.Encoding == "BigEndianUnicode")
                encoding = System.Text.Encoding.BigEndianUnicode;

            string fileExtension = profile.ExportFormat.ToLower();
            if (profile.ExportFormat.ToLower() == "xml spreadsheet 2003")
                fileExtension = "xml";

            fileName = Folder + "\\" + profile.ProfileName + "\\AuditExport_" + now.Year + "-" + now.Month + "-" + now.Day + "-" + now.Hour + "-" + now.Minute + "-" + now.Second + "." + fileExtension;

            //Create Audit Export Report
            //DataExportReport der = new DataExportReport(profile.ProfileName);
            //WriteReport(der, ReportFileName);

            //Export Audit
            Export(profile);

            //DataExportReport report = ReadReport(ReportFileName);
            //report.DataExportFinishedAt = DateTime.Now.ToString();
            //report.DataExportCompleted = true;
            //TimeSpan ExportTimeSpan = DateTime.Now - Convert.ToDateTime(report.DataExportStartedAt);
            //report.DataExportedIn = ExportTimeSpan.ToString().Substring(0, 10);
            //WriteReport(report, ReportFileName);
        }

        /// <summary>
        /// Reads the profiles.
        /// </summary>
        private void ReadProfiles()
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(List<MSCRMAuditExportProfile>));
            if (!File.Exists(ConfigurationFileName))
            {
                using (FileStream writer = new FileStream(ConfigurationFileName, FileMode.Create))
                {
                    ser.WriteObject(writer, Profiles);
                }
            }
            
            using (FileStream fs = new FileStream(ConfigurationFileName, FileMode.Open))
            using (XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas()))
            {
                Profiles = (List<MSCRMAuditExportProfile>)ser.ReadObject(reader, true);
            }

            foreach (MSCRMAuditExportProfile profile in Profiles)
            {
                profile.setSourceConneciton();
            }
        }

        /// <summary>
        /// Writes the profiles.
        /// </summary>
        private void WriteProfiles()
        {
            using (FileStream writer = new FileStream(ConfigurationFileName, FileMode.Create))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(List<MSCRMAuditExportProfile>));
                ser.WriteObject(writer, Profiles);
            }
        }
        /// <summary>
        /// Exports the specified profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        private void Export(MSCRMAuditExportProfile profile)
        {
            MSCRMConnection connection = profile.getSourceConneciton();
            _serviceProxy = cm.connect(connection);

            IOrganizationService service = (IOrganizationService)_serviceProxy;
            es = ReadEnvStructure(profile.SourceConnectionName);

            List<string> columns = new List<string> { "transactionid", "createdon", "userid", "objecttypecode", "objectid", "action", "operation" };
            List<string> DisplayedColumns = new List<string> { "TransactionId", "Date", "User", "Entity", "Record", "Action", "Operation" };

            if (!(profile.AuditType == "User Acces Audit") && !(profile.AuditType == "Audit Summary View"))
            {
                columns.Add("key");
                columns.Add("oldValue");
                columns.Add("newValue");
                DisplayedColumns.Add("Attribute");
                DisplayedColumns.Add("Old_Value");
                DisplayedColumns.Add("New_Value");
            }

            if (profile.AuditType == "User Acces Audit" || profile.AuditType == "Audit Summary View")
            {
                LogManager.WriteLog("Exporting " + profile.AuditType);
                string fetchXml = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>";
                fetchXml += "<entity name='audit'>";
                fetchXml += "<attribute name='transactionid' />";
                fetchXml += "<attribute name='attributemask' />";
                fetchXml += "<attribute name='action' />";
                fetchXml += "<attribute name='createdon' />";
                fetchXml += "<attribute name='auditid' />";
                fetchXml += "<attribute name='userid' />";
                fetchXml += "<attribute name='operation' />";
                fetchXml += "<attribute name='objectid' />";
                fetchXml += "<order attribute='createdon' descending='true' />";
                fetchXml += "<filter type='and'>";
                if (!profile.AllUsersSelected && profile.SelectedUsers.Count > 0)
                {
                    //When a User Access Audit type is selected the User is in the Object
                    if (profile.AuditType == "User Acces Audit")
                        fetchXml += "<condition attribute='objectid' operator='in'>";
                    else
                        fetchXml += "<condition attribute='userid' operator='in'>";

                    foreach (AuditUser u in profile.SelectedUsers)
                    {
                        fetchXml += "<value uitype='systemuser'>" + u.Id.ToString() + "</value>";
                    }
                    fetchXml += "</condition>";
                }
                if (!profile.AllActionsSelected && profile.SelectedActions.Count > 0)
                {
                    fetchXml += "<condition attribute='action' operator='in'>";
                    foreach (int i in profile.SelectedActions)
                    {
                        fetchXml += "<value>" + i + "</value>";
                    }
                    fetchXml += "</condition>";
                }
                if (!profile.AllOperationsSelected && profile.SelectedOperations.Count > 0)
                {
                    fetchXml += "<condition attribute='operation' operator='in'>";
                    foreach (int i in profile.SelectedOperations)
                    {
                        fetchXml += "<value>" + i + "</value>";
                    }
                    fetchXml += "</condition>";
                }
                if (profile.AuditType != "User Acces Audit" && !profile.AllEntitiesSelected && profile.SelectedEntities.Count > 0)
                {
                    fetchXml += "<condition attribute='objecttypecode' operator='in'>";
                    foreach (SelectedAuditEntity i in profile.SelectedEntities)
                    {
                        fetchXml += "<value>" + i.ObjectTypeCode + "</value>";
                    }
                    fetchXml += "</condition>";
                }

                if (profile.AuditRecordCreatedOnFilter == "Last X Days")
                    fetchXml += "<condition attribute='createdon' operator='last-x-days' value='" + profile.AuditRecordCreatedOnFilterLastX + "' />";
                else if (profile.AuditRecordCreatedOnFilter == "Last X Months")
                    fetchXml += "<condition attribute='createdon' operator='last-x-months' value='" + profile.AuditRecordCreatedOnFilterLastX + "' />";
                else if (profile.AuditRecordCreatedOnFilter == "Last X Years")
                    fetchXml += "<condition attribute='createdon' operator='last-x-years' value='" + profile.AuditRecordCreatedOnFilterLastX + "' />";
                else if (profile.AuditRecordCreatedOnFilter == "Between Dates")
                {
                    fetchXml += "<condition attribute='createdon' operator='on-or-after' value='" + String.Format("{0:yyyy-MM-dd}", profile.AuditRecordCreatedOnFilterFrom) + "' />";
                    fetchXml += "<condition attribute='createdon' operator='on-or-before' value='" + String.Format("{0:yyyy-MM-dd}", profile.AuditRecordCreatedOnFilterTo) + "' />";
                }

                fetchXml += "</filter>";
                fetchXml += "</entity></fetch>";
                int recordCountPerEntity = ExportEntity(profile, fetchXml, "", columns, DisplayedColumns, null);
            }
            else
            {
                foreach (SelectedAuditEntity sae in profile.SelectedEntities)
                {
                    //Get entity PrimaryNameAttribute
                    String PrimaryNameAttribute = es.Entities.Find(e => e.LogicalName == sae.LogicalName).PrimaryNameAttribute;
                    LogManager.WriteLog("Exporting audit data for entity " + sae.LogicalName);
                    string fetchXml = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>";
                    fetchXml += "<entity name='" + sae.LogicalName + "'>";
                    fetchXml += "<attribute name='" + PrimaryNameAttribute + "' />";
                    fetchXml += sae.Filter;
                    fetchXml += "</entity></fetch>";
                    int recordCountPerEntity = ExportEntity(profile, fetchXml, PrimaryNameAttribute, columns, DisplayedColumns, sae);
                }
            }
        }

        /// <summary>
        /// Exports the entity.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <param name="fetchXml">The fetch XML query.</param>
        /// <param name="PrimaryNameAttribute">The primary name attribute.</param>
        /// <param name="columns">The columns.</param>
        /// <param name="DisplayedColumns">The displayed columns.</param>
        /// <param name="sae">The sae.</param>
        /// <returns>The number of exported records</returns>
        public int ExportEntity(MSCRMAuditExportProfile profile, string fetchXml, string PrimaryNameAttribute, List<string> columns, List<string> DisplayedColumns, SelectedAuditEntity sae)
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

                RetrieveMultipleRequest fetchRequest = new RetrieveMultipleRequest
                {
                    Query = new FetchExpression(xml)
                };

                EntityCollection returnCollection = ((RetrieveMultipleResponse)_serviceProxy.Execute(fetchRequest)).EntityCollection;
                recordCount += returnCollection.Entities.Count;
                if (recordCount > 0)
                {
                    if (profile.AuditType == "User Acces Audit" || profile.AuditType == "Audit Summary View")
                    {
                        if (profile.ExportFormat.ToLower() == "csv")
                            WriteCSV(returnCollection, fileName, columns, DisplayedColumns);
                        else if (profile.ExportFormat.ToLower() == "xml")
                            WriteXML(returnCollection, fileName, columns, DisplayedColumns);
                        else if (profile.ExportFormat.ToLower() == "xml spreadsheet 2003")
                            WriteXMLSpreadsheet2003(returnCollection, fileName, columns, DisplayedColumns);
                    }
                    else
                    {
                        foreach (Entity e in returnCollection.Entities)
                        {
                            AuditDetailCollection details = new AuditDetailCollection();
                            if (profile.AuditType == "Record Change History")
                            {
                                RetrieveRecordChangeHistoryRequest changeRequest = new RetrieveRecordChangeHistoryRequest();
                                changeRequest.Target = new EntityReference(returnCollection.EntityName, e.Id);
                                RetrieveRecordChangeHistoryResponse changeResponse = (RetrieveRecordChangeHistoryResponse)_serviceProxy.Execute(changeRequest);
                                details = changeResponse.AuditDetailCollection;
                            }
                            else
                            {
                                //Attribute change history
                                var attributeChangeHistoryRequest = new RetrieveAttributeChangeHistoryRequest
                                {
                                    Target = new EntityReference(e.LogicalName, e.Id),
                                    AttributeLogicalName = sae.SelectedAttributes[0]
                                };

                                var attributeChangeHistoryResponse = (RetrieveAttributeChangeHistoryResponse)_serviceProxy.Execute(attributeChangeHistoryRequest);
                                details = attributeChangeHistoryResponse.AuditDetailCollection;
                            }

                            foreach (AuditDetail detail in details.AuditDetails)
                            {
                                DisplayAuditDetails(detail, profile, (string)e[PrimaryNameAttribute], fileName, columns, DisplayedColumns);
                            }
                        }
                    }
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

            if (profile.ExportFormat.ToLower() == "xml" && File.Exists(fileName))
            {
                using (var writer = new StreamWriter(fileName, true, encoding))
                {
                    writer.WriteLine("</Records>");
                    writer.Flush();
                }
            }
            else if (profile.ExportFormat.ToLower() == "xml spreadsheet 2003" && File.Exists(fileName))
            {
                using (var writer = new StreamWriter(fileName, true, encoding))
                {
                    writer.WriteLine("</Table></Worksheet></Workbook>\n");
                    writer.Flush();
                }
            }

            LogManager.WriteLog("Exported " + recordCount + " " + entityName + " records.");

            return recordCount;
        }

        /// <summary>
        /// Displays the audit details.
        /// </summary>
        /// <param name="detail">The detail.</param>
        /// <param name="profile">The profile.</param>
        /// <param name="ObjectName">Name of the object.</param>
        /// <param name="path">The path.</param>
        /// <param name="columns">The columns.</param>
        /// <param name="DisplayedColumns">The displayed columns.</param>
        private void DisplayAuditDetails(AuditDetail detail, MSCRMAuditExportProfile profile, string ObjectName, string path, List<string> columns, List<string> DisplayedColumns)
        {
            Audit record = (Audit)detail.AuditRecord;

            if (profile.AuditRecordCreatedOnFilter == "Last X Days")
            {
                DateTime CreatedOnBottomLimit = DateTime.Today.AddDays(-(double)profile.AuditRecordCreatedOnFilterLastX);
                if (CreatedOnBottomLimit > record.CreatedOn)
                    return;
            }
            else if (profile.AuditRecordCreatedOnFilter == "Last X Months")
            {
                DateTime CreatedOnBottomLimit = DateTime.Today.AddMonths(-(int)profile.AuditRecordCreatedOnFilterLastX);
                if (CreatedOnBottomLimit > record.CreatedOn)
                    return;
            }
            else if (profile.AuditRecordCreatedOnFilter == "Last X Years")
            {
                DateTime CreatedOnBottomLimit = DateTime.Today.AddYears(-(int)profile.AuditRecordCreatedOnFilterLastX);
                if (CreatedOnBottomLimit > record.CreatedOn)
                    return;
            }
            else if (profile.AuditRecordCreatedOnFilter == "Between Dates")
            {
                if (profile.AuditRecordCreatedOnFilterFrom > record.CreatedOn || profile.AuditRecordCreatedOnFilterTo < record.CreatedOn)
                    return;
            }

            List<AuditDetailLine> AuditDetailLines = new List<AuditDetailLine>();
            // Show additional details for certain AuditDetail sub-types.
            var detailType = detail.GetType();
            if (detailType == typeof(AttributeAuditDetail))
            {
                var attributeDetail = (AttributeAuditDetail)detail;
                AuditDetailLine adl = new AuditDetailLine();

                if (attributeDetail.NewValue != null)
                {
                    // Display the old and new attribute values.
                    foreach (KeyValuePair<String, object> attribute in attributeDetail.NewValue.Attributes)
                    {
                        String oldValue = "(no value)", newValue = "(no value)";

                        //format values
                        oldValue = getFormattedValue(attributeDetail.OldValue, attribute.Key);
                        newValue = getFormattedValue(attributeDetail.NewValue, attribute.Key);

                        adl = new AuditDetailLine();
                        adl.TransactionId = record.Id;
                        adl.createdon = record.CreatedOn.Value.ToLocalTime();
                        adl.UserName = record.UserId.Name;
                        adl.RecordLogicalName = record.ObjectId.LogicalName;
                        adl.RecordName = ObjectName;
                        adl.action = record.FormattedValues["action"];
                        adl.operation = record.FormattedValues["operation"];
                        adl.key = attribute.Key;
                        adl.oldValue = oldValue;
                        adl.newValue = newValue;

                        AuditDetailLines.Add(adl);
                    }
                }

                if (attributeDetail.OldValue != null)
                {
                    foreach (KeyValuePair<String, object> attribute in attributeDetail.OldValue.Attributes)
                    {
                        if (!attributeDetail.NewValue.Contains(attribute.Key))
                        {
                            String newValue = "(no value)";

                            //format values
                            String oldValue = getFormattedValue(attributeDetail.OldValue, attribute.Key);

                            adl = new AuditDetailLine();
                            adl.TransactionId = record.Id;
                            adl.createdon = record.CreatedOn.Value.ToLocalTime();
                            adl.UserName = record.UserId.Name;
                            adl.RecordLogicalName = record.ObjectId.LogicalName;
                            adl.RecordName = ObjectName;
                            adl.action = record.FormattedValues["action"];
                            adl.operation = record.FormattedValues["operation"];
                            adl.key = attribute.Key;
                            adl.oldValue = oldValue;
                            adl.newValue = newValue;

                            AuditDetailLines.Add(adl);
                        }
                    }
                }
            }
            else
            {
                AuditDetailLine adl = new AuditDetailLine();
                adl.TransactionId = record.Id;
                adl.createdon = record.CreatedOn.Value.ToLocalTime();
                adl.UserName = record.UserId.Name;
                adl.RecordLogicalName = record.ObjectId.LogicalName;
                adl.RecordName = ObjectName;
                adl.action = record.FormattedValues["action"];
                adl.operation = record.FormattedValues["operation"];

                AuditDetailLines.Add(adl);
            }

            if (profile.ExportFormat.ToLower() == "csv")
                WriteCSVAuditDetail(AuditDetailLines, fileName, columns, DisplayedColumns);
            else if (profile.ExportFormat.ToLower() == "xml")
                WriteXMLAuditDetail(AuditDetailLines, fileName, columns, DisplayedColumns);
            else if (profile.ExportFormat.ToLower() == "xml spreadsheet 2003")
                WriteXMLSpreadsheet2003AuditDetail(AuditDetailLines, fileName, columns, DisplayedColumns);
        }

        /// <summary>
        /// Downloads the env audit structure.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns>The Environment Audit Structure.</returns>
        public EnvAuditStructure downloadEnvAuditStructure(string connectionName)
        {
            try
            {
                MSCRMConnection connection = cm.getConnection(connectionName);
                _serviceProxy = cm.connect(connection);
                IOrganizationService service = (IOrganizationService)_serviceProxy;
                EnvAuditStructure es = new EnvAuditStructure();
                es.Entities = new List<EnvAuditEntity>();
                es.Users = new List<AuditUser>();

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
                    if (!currentEntity.IsAuditEnabled.Value && currentEntity.LogicalName != "audit")
                        continue;

                    EnvAuditEntity ee = new EnvAuditEntity();
                    ee.LogicalName = currentEntity.LogicalName;
                    ee.ObjectTypeCode = currentEntity.ObjectTypeCode.Value;
                    ee.PrimaryNameAttribute = currentEntity.PrimaryNameAttribute;
                    ee.Attributes = new List<string>();
                    IOrderedEnumerable<AttributeMetadata> AMD = currentEntity.Attributes.OrderBy(a => a.LogicalName);

                    if (currentEntity.LogicalName == "audit")
                    {
                        es.Operations = new List<KeyValuePair<int, string>>();
                    }
                    foreach (AttributeMetadata currentAttribute in AMD)
                    {
                        if (!currentAttribute.IsAuditEnabled.Value && currentEntity.LogicalName != "audit")
                            continue;

                        // Only write out main attributes enabled for reading and creation.
                        if ((currentAttribute.AttributeOf == null) &&
                            IgnoredAttributes.IndexOf(currentAttribute.LogicalName) < 0 &&
                            currentAttribute.IsValidForRead.Value &&
                            currentAttribute.IsValidForCreate.Value
                            )
                        {
                            ee.Attributes.Add(currentAttribute.LogicalName);
                        }

                        if (currentEntity.LogicalName == "audit")
                        {
                            if (currentAttribute.LogicalName == "operation")
                            {
                                var typedAttribute = (PicklistAttributeMetadata)currentAttribute;
                                foreach (var option in typedAttribute.OptionSet.Options)
                                {
                                    es.Operations.Add(new KeyValuePair<int, string>(option.Value.Value, option.Label.UserLocalizedLabel.Label));
                                }
                            }
                            else if (currentAttribute.LogicalName == "action")
                            {
                                var typedAttribute = (PicklistAttributeMetadata)currentAttribute;
                                foreach (var option in typedAttribute.OptionSet.Options)
                                {
                                    es.Actions.Add(new KeyValuePair<int, string>(option.Value.Value, option.Label.UserLocalizedLabel.Label));
                                }
                            }
                        }
                    }
                    //Dont export entitites for which only the ID is retrieved
                    if (ee.Attributes.Count > 1)
                        es.Entities.Add(ee);
                }

                //Get users
                string usersQuery = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'><entity name='systemuser'>";
                usersQuery += "<attribute name='fullname' /><attribute name='systemuserid' /><order attribute='fullname' descending='false' />";
                usersQuery += "</entity></fetch>";

                RetrieveMultipleRequest UsersRequest = new RetrieveMultipleRequest()
                {
                    Query = new FetchExpression(usersQuery)
                };

                EntityCollection Users = ((RetrieveMultipleResponse)_serviceProxy.Execute(UsersRequest)).EntityCollection;

                foreach (Entity User in Users.Entities)
                {
                    es.Users.Add(new AuditUser { Id = User.Id, FullName = (string)User["fullname"] });
                }

                es.connectionName = connectionName;
                WriteEnvStructure(es);
                return es;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Reads the env structure.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns>The Environment Audit Structure.</returns>
        public EnvAuditStructure ReadEnvStructure(string connectionName)
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
                        DataContractSerializer ser = new DataContractSerializer(typeof(EnvAuditStructure));
                        EnvAuditStructure es = (EnvAuditStructure)ser.ReadObject(reader, true);
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
        /// <param name="str">The Environment Audit Structure.</param>
        private void WriteEnvStructure(EnvAuditStructure str)
        {
            string filename = Folder + "\\" + str.connectionName + ".xml";
            FileStream writer = new FileStream(filename, FileMode.Create);
            DataContractSerializer ser = new DataContractSerializer(typeof(EnvAuditStructure));
            ser.WriteObject(writer, str);
            writer.Close();
        }

        /// <summary>
        /// Writes the CSV.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="path">The path.</param>
        /// <param name="columns">The columns.</param>
        /// <param name="DisplayedColumns">The displayed columns.</param>
        public void WriteCSV(EntityCollection items, string path, List<string> columns, List<string> DisplayedColumns)
        {
            bool writeColumns = false;
            if (!File.Exists(path))
                writeColumns = true;

            using (var writer = new StreamWriter(path, true, encoding))
            {
                if (writeColumns)
                    writer.WriteLine(string.Join(";", DisplayedColumns));

                foreach (Entity e in items.Entities)
                {
                    List<string> values = new List<string>();
                    foreach (string column in columns)
                    {
                        string value = getFormattedValue(e, column);

                        if (value != null && value != "")
                        {
                            //Escape Data
                            value = value.Replace("\"", "\"\"");
                            values.Add("\"" + value + "\"");
                        }
                        else
                        {
                            values.Add("");
                        }
                    }

                    writer.WriteLine(string.Join(";", values));
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
        public void WriteXML(EntityCollection items, string path, List<string> columns, List<string> DisplayedColumns)
        {
            bool writeColumns = false;
            if (!File.Exists(path))
                writeColumns = true;

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
        /// Writes the XML spreadsheet2003.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="path">The path.</param>
        /// <param name="columns">The columns.</param>
        /// <param name="DisplayedColumns">The displayed columns.</param>
        public void WriteXMLSpreadsheet2003(EntityCollection items, string path, List<string> columns, List<string> DisplayedColumns)
        {
            bool writeColumns = false;
            if (!File.Exists(path))
                writeColumns = true;

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
        /// Writes the CSV audit detail.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="path">The path.</param>
        /// <param name="columns">The columns.</param>
        /// <param name="DisplayedColumns">The displayed columns.</param>
        public void WriteCSVAuditDetail(List<AuditDetailLine> items, string path, List<string> columns, List<string> DisplayedColumns)
        {
            bool writeColumns = false;
            if (!File.Exists(path))
                writeColumns = true;

            using (var writer = new StreamWriter(path, true, encoding))
            {
                if (writeColumns)
                    writer.WriteLine(string.Join(";", DisplayedColumns));

                foreach (AuditDetailLine e in items)
                {
                    List<string> values = new List<string>();
                    values.Add(e.TransactionId.ToString());
                    values.Add(e.createdon.ToString());
                    values.Add(e.UserName);
                    values.Add(e.RecordLogicalName);
                    values.Add(e.RecordName);
                    values.Add(e.action);
                    values.Add(e.operation);
                    values.Add(e.key);
                    values.Add(e.oldValue);
                    values.Add(e.newValue);

                    for (int i = 0; i < values.Count; i++)
                    {
                        if (values[i] != null && values[i] != "") //Escape Data
                            values[i] = "\"" + values[i].Replace("\"", "\"\"") + "\"";
                    }

                    writer.WriteLine(string.Join(";", values));
                }
                writer.Flush();
            }
        }

        /// <summary>
        /// Writes the XML audit detail.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="path">The path.</param>
        /// <param name="columns">The columns.</param>
        /// <param name="DisplayedColumns">The displayed columns.</param>
        public void WriteXMLAuditDetail(List<AuditDetailLine> items, string path, List<string> columns, List<string> DisplayedColumns)
        {
            bool writeColumns = false;
            if (!File.Exists(path))
                writeColumns = true;

            using (var writer = new StreamWriter(path, true, encoding))
            {
                if (writeColumns)
                {
                    writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    writer.WriteLine("<?mso-application progid=\"Excel.Sheet\"?>");
                    writer.WriteLine("<Records>");
                }

                foreach (AuditDetailLine e in items)
                {
                    string row = "<row>";
                    List<string> values = new List<string>();
                    values.Add(e.TransactionId.ToString());
                    values.Add(e.createdon.ToString());
                    values.Add(e.UserName);
                    values.Add(e.RecordLogicalName);
                    values.Add(e.RecordName);
                    values.Add(e.action);
                    values.Add(e.operation);
                    values.Add(e.key);
                    values.Add(e.oldValue);
                    values.Add(e.newValue);

                    for (int i = 0; i < values.Count; i++)
                    {
                        row += "<" + DisplayedColumns[i] + ">" + SecurityElement.Escape(values[i]) + "</" + DisplayedColumns[i] + ">";
                    }
                    row += "</row>";
                    writer.WriteLine(row);
                }
                writer.Flush();
            }
        }

        /// <summary>
        /// Writes the XML spreadsheet2003 audit detail.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="path">The path.</param>
        /// <param name="columns">The columns.</param>
        /// <param name="DisplayedColumns">The displayed columns.</param>
        public void WriteXMLSpreadsheet2003AuditDetail(List<AuditDetailLine> items, string path, List<string> columns, List<string> DisplayedColumns)
        {
            bool writeColumns = false;
            if (!File.Exists(path))
                writeColumns = true;

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

                foreach (AuditDetailLine e in items)
                {
                    string row = "<Row>";
                    List<string> values = new List<string>();
                    values.Add(e.TransactionId.ToString());
                    values.Add(e.createdon.ToString());
                    values.Add(e.UserName);
                    values.Add(e.RecordLogicalName);
                    values.Add(e.RecordName);
                    values.Add(e.action);
                    values.Add(e.operation);
                    values.Add(e.key);
                    values.Add(e.oldValue);
                    values.Add(e.newValue);

                    for (int i = 0; i < values.Count; i++)
                    {
                        row += "<Cell><Data ss:Type=\"String\">" + SecurityElement.Escape(values[i]) + "</Data></Cell>";
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
        /// <returns>The formatted value.</returns>
        private string getFormattedValue(Entity entity, string attributeName)
        {
            if (attributeName == "transactionid")
                return entity.Id.ToString();

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
                    value = er.Name;
                }
                else if (t.Name == "AliasedValue")
                {
                    AliasedValue av = (AliasedValue)kvp.Value;
                    Type t2 = av.Value.GetType();
                    if (t2.Name == "EntityReference")
                    {
                        EntityReference er2 = (EntityReference)av.Value;
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
        /// Gets the operation label.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="Value">The value.</param>
        /// <returns>The operation label.</returns>
        private string getOperationLabel(EnvAuditStructure str, int Value)
        {
            return str.Operations.Find(o => o.Key == Value).Value;
        }

        /// <summary>
        /// Gets the action label.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="Value">The value.</param>
        /// <returns>The action label.</returns>
        private string getActionLabel(EnvAuditStructure str, int Value)
        {
            return str.Actions.Find(o => o.Key == Value).Value;
        }
    }
}