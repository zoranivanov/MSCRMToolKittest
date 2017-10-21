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

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MSCRMToolKit
{
    /// <summary>
    /// Environment Entity
    /// </summary>
    public class EnvEntity
    {
        /// <summary>
        /// Gets or sets the name of the Environment entity.
        /// </summary>
        /// <value>
        /// The name of the Environment entity.
        /// </value>
        public string EntityName { get; set; }

        /// <summary>
        /// Gets or sets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        public List<string> Attributes { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvEntity"/> class.
        /// </summary>
        public EnvEntity()
        {
            Attributes = new List<string>();
        }
    }

    /// <summary>
    /// Environment Structure
    /// </summary>
    public class EnvStructure
    {
        /// <summary>
        /// Gets or sets the name of the connection.
        /// </summary>
        /// <value>
        /// The name of the connection.
        /// </value>
        public string connectionName { get; set; }

        /// <summary>
        /// Gets or sets the Environment entities.
        /// </summary>
        /// <value>
        /// The Environment entities.
        /// </value>
        public List<EnvEntity> Entities { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvStructure"/> class.
        /// </summary>
        public EnvStructure()
        {
            Entities = new List<EnvEntity>();
        }
    }

    /// <summary>
    /// Selected Entity for transport
    /// </summary>
    public class SelectedEntity
    {
        /// <summary>
        /// Gets or sets the name of the entity.
        /// </summary>
        /// <value>
        /// The name of the entity.
        /// </value>
        public string EntityName { get; set; }

        /// <summary>
        /// Gets or sets the exported records.
        /// </summary>
        /// <value>
        /// The exported records.
        /// </value>
        public int ExportedRecords { get; set; }

        /// <summary>
        /// Gets or sets the transport order.
        /// </summary>
        /// <value>
        /// The transport order.
        /// </value>
        public int TransportOrder { get; set; }

        /// <summary>
        /// Gets or sets the filter.
        /// </summary>
        /// <value>
        /// The filter.
        /// </value>
        public string Filter { get; set; }

        /// <summary>
        /// Gets or sets the ignored attributes.
        /// </summary>
        /// <value>
        /// The ignored attributes.
        /// </value>
        public List<string> IgnoredAttributes { get; set; }
    }

    /// <summary>
    /// Transport Report
    /// </summary>
    [DataContract]
    public class TransportReport
    {
        /// <summary>
        /// Gets or sets the name of the transport profile.
        /// </summary>
        /// <value>
        /// The name of the transport profile.
        /// </value>
        [DataMember]
        public string TransportProfileName { get; set; }

        /// <summary>
        /// Gets or sets the transport started at.
        /// </summary>
        /// <value>
        /// The transport started at.
        /// </value>
        [DataMember]
        public string TransportStartedAt { get; set; }

        /// <summary>
        /// Gets or sets the transport finished at.
        /// </summary>
        /// <value>
        /// The transport finished at.
        /// </value>
        [DataMember]
        public string TransportFinishedAt { get; set; }

        /// <summary>
        /// Gets or sets the transported in.
        /// </summary>
        /// <value>
        /// The transported in.
        /// </value>
        [DataMember]
        public string TransportedIn { get; set; }

        /// <summary>
        /// Gets or sets the total exported records.
        /// </summary>
        /// <value>
        /// The total exported records.
        /// </value>
        [DataMember]
        public int TotalExportedRecords { get; set; }

        /// <summary>
        /// Gets or sets the total imported records.
        /// </summary>
        /// <value>
        /// The total imported records.
        /// </value>
        [DataMember]
        public int TotalImportedRecords { get; set; }

        /// <summary>
        /// Gets or sets the total import failures.
        /// </summary>
        /// <value>
        /// The total import failures.
        /// </value>
        [DataMember]
        public int TotalImportFailures { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [transport completed].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [transport completed]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool TransportCompleted { get; set; }

        /// <summary>
        /// Gets or sets the report lines.
        /// </summary>
        /// <value>
        /// The report lines.
        /// </value>
        [DataMember]
        public List<TransportReportLine> ReportLines { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransportReport"/> class.
        /// </summary>
        /// <param name="TransportProfileName">Name of the transport profile.</param>
        public TransportReport(string TransportProfileName)
        {
            this.TransportProfileName = TransportProfileName;
            TransportStartedAt = DateTime.Now.ToString();
            ReportLines = new List<TransportReportLine>();
            TransportCompleted = false;
        }
    }

    /// <summary>
    /// Transport Report Line
    /// </summary>
    public class TransportReportLine
    {
        /// <summary>
        /// Gets or sets the entity.
        /// </summary>
        /// <value>
        /// The entity.
        /// </value>
        public string Entity { get; set; }

        /// <summary>
        /// Gets or sets the exported records.
        /// </summary>
        /// <value>
        /// The exported records.
        /// </value>
        public int ExportedRecords { get; set; }

        /// <summary>
        /// Gets or sets the export started at.
        /// </summary>
        /// <value>
        /// The export started at.
        /// </value>
        public string ExportStartedAt { get; set; }

        /// <summary>
        /// Gets or sets the export finished at.
        /// </summary>
        /// <value>
        /// The export finished at.
        /// </value>
        public string ExportFinishedAt { get; set; }

        /// <summary>
        /// Gets or sets the exported in.
        /// </summary>
        /// <value>
        /// The exported in.
        /// </value>
        public string ExportedIn { get; set; }

        /// <summary>
        /// Gets or sets the imported records.
        /// </summary>
        /// <value>
        /// The imported records.
        /// </value>
        public int ImportedRecords { get; set; }

        /// <summary>
        /// Gets or sets the import failures.
        /// </summary>
        /// <value>
        /// The import failures.
        /// </value>
        public int ImportFailures { get; set; }

        /// <summary>
        /// Gets or sets the import started at.
        /// </summary>
        /// <value>
        /// The import started at.
        /// </value>
        public string ImportStartedAt { get; set; }

        /// <summary>
        /// Gets or sets the import finished at.
        /// </summary>
        /// <value>
        /// The import finished at.
        /// </value>
        public string ImportFinishedAt { get; set; }

        /// <summary>
        /// Gets or sets the import time.
        /// </summary>
        /// <value>
        /// The import time.
        /// </value>
        public string ImportedIn { get; set; }
    }

    /// <summary>
    /// TransportationProfile class extending MSCRMToolKitProfile
    /// </summary>
    public class TransportationProfile : MSCRMToolKitProfile
    {
        /// <summary>
        /// Gets or sets the operation.
        /// </summary>
        /// <value>
        /// The operation.
        /// </value>
        public int Operation { get; set; }

        /// <summary>
        /// Gets or sets the import mode.
        /// </summary>
        /// <value>
        /// The import mode.
        /// </value>
        public int ImportMode { get; set; }

        /// <summary>
        /// Gets or sets the selected entities.
        /// </summary>
        /// <value>
        /// The selected entities.
        /// </value>
        public List<SelectedEntity> SelectedEntities { get; set; }

        /// <summary>
        /// Gets or sets the total exported records.
        /// </summary>
        /// <value>
        /// The total exported records.
        /// </value>
        public int TotalExportedRecords { get; set; }

        /// <summary>
        /// Gets or sets the record mappings.
        /// </summary>
        /// <value>
        /// The record mappings.
        /// </value>
        public List<RecordMapping> RecordMappings { get; set; }

        /// <summary>
        /// Gets the selected entity.
        /// </summary>
        /// <param name="selectedEntityName">Name of the selected entity.</param>
        /// <returns>The selected entity</returns>
        public SelectedEntity getSelectedEntity(string selectedEntityName)
        {
            foreach (SelectedEntity ee in this.SelectedEntities)
            {
                if (selectedEntityName == ee.EntityName)
                    return ee;
            }
            return null;
        }
    }

    /// <summary>
    /// Import Failure
    /// </summary>
    public class ImportFailure
    {
        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        /// <value>
        /// The creation date.
        /// </value>
        public string CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the reason.
        /// </summary>
        /// <value>
        /// The reason.
        /// </value>
        public string Reason { get; set; }

        /// <summary>
        /// Gets or sets the name of the entity.
        /// </summary>
        /// <value>
        /// The name of the entity.
        /// </value>
        public string EntityName { get; set; }

        /// <summary>
        /// Gets or sets the operation.
        /// </summary>
        /// <value>
        /// The operation.
        /// </value>
        public string Operation { get; set; }

        /// <summary>
        /// Gets or sets the URL of the Source record.
        /// </summary>
        /// <value>
        /// The URL of the Source record.
        /// </value>
        public string Url { get; set; }
    }

    /// <summary>
    /// Records Mapping 
    /// </summary>
    public class RecordMapping
    {
        /// <summary>
        /// Gets or sets the name of the entity.
        /// </summary>
        /// <value>
        /// The name of the entity.
        /// </value>
        public string EntityName { get; set; }

        /// <summary>
        /// Gets or sets the source record identifier.
        /// </summary>
        /// <value>
        /// The source record identifier.
        /// </value>
        public Guid SourceRecordId { get; set; }

        /// <summary>
        /// Gets or sets the target record identifier.
        /// </summary>
        /// <value>
        /// The target record identifier.
        /// </value>
        public Guid TargetRecordId { get; set; }
    }
}