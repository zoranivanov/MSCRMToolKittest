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

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MSCRMToolKit
{
    /// <summary>
    /// NtoNAssociationsTransportProfile class extending MSCRMToolKitProfile
    /// </summary>
    public class NtoNAssociationsTransportProfile : MSCRMToolKitProfile
    {
        /// <summary>
        /// Gets or sets the operation.
        /// </summary>
        /// <value>
        /// The operation.
        /// </value>
        public int Operation { get; set; }
        /// <summary>
        /// Gets or sets the selected n to n relationships.
        /// </summary>
        /// <value>
        /// The selected n to n relationships.
        /// </value>
        public List<SelectedNtoNRelationship> SelectedNtoNRelationships { get; set; }
        /// <summary>
        /// Gets or sets the total exported records.
        /// </summary>
        /// <value>
        /// The total exported records.
        /// </value>
        public int TotalExportedRecords { get; set; }
        /// <summary>
        /// Gets the selected n to n relationship.
        /// </summary>
        /// <param name="selectedNtoNRelationshipName">Name of the selected nto n relationship.</param>
        /// <returns>The retrieved n to n relationship</returns>
        public SelectedNtoNRelationship getSelectedNtoNRelationship(string selectedNtoNRelationshipName)
        {
            foreach (SelectedNtoNRelationship ee in this.SelectedNtoNRelationships)
            {
                if (selectedNtoNRelationshipName == ee.RelationshipSchemaName)
                    return ee;
            }
            return null;
        }
    }

    /// <summary>
    /// N to N Relationship
    /// </summary>
    public class NtoNRelationship
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="NtoNRelationship"/> is selected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if selected; otherwise, <c>false</c>.
        /// </value>
        public bool Selected { get; set; }
        /// <summary>
        /// Gets or sets the name of the relationship schema.
        /// </summary>
        /// <value>
        /// The name of the relationship schema.
        /// </value>
        public string RelationshipSchemaName { get; set; }
        /// <summary>
        /// Gets or sets the name of the intersect entity.
        /// </summary>
        /// <value>
        /// The name of the intersect entity.
        /// </value>
        public string IntersectEntityName { get; set; }
        /// <summary>
        /// Gets or sets the logical name of the entity 1.
        /// </summary>
        /// <value>
        /// The logical name of the entity 1 .
        /// </value>
        public string Entity1LogicalName { get; set; }
        /// <summary>
        /// Gets or sets the entity 1 intersect attribute.
        /// </summary>
        /// <value>
        /// The entity 1 intersect attribute.
        /// </value>
        public string Entity1IntersectAttribute { get; set; }
        /// <summary>
        /// Gets or sets the logical name of the entity 2 .
        /// </summary>
        /// <value>
        /// The logical name of the entity 2 .
        /// </value>
        public string Entity2LogicalName { get; set; }
        /// <summary>
        /// Gets or sets the entity 2 intersect attribute.
        /// </summary>
        /// <value>
        /// The entity 2 intersect attribute.
        /// </value>
        public string Entity2IntersectAttribute { get; set; }
    }

    /// <summary>
    /// N to N Relationships Structure
    /// </summary>
    public class NtoNRelationshipsStructure
    {
        /// <summary>
        /// Gets or sets the name of the connection.
        /// </summary>
        /// <value>
        /// The name of the connection.
        /// </value>
        public string connectionName { get; set; }

        /// <summary>
        /// Gets or sets the n to n relationships.
        /// </summary>
        /// <value>
        /// The n to n relationships.
        /// </value>
        public List<NtoNRelationship> NtoNRelationships { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NtoNRelationshipsStructure"/> class.
        /// </summary>
        public NtoNRelationshipsStructure()
        {
            NtoNRelationships = new List<NtoNRelationship>();
        }
    }

    /// <summary>
    /// Selected N to N Relationship
    /// </summary>
    public class SelectedNtoNRelationship
    {
        /// <summary>
        /// Gets or sets the name of the relationship schema.
        /// </summary>
        /// <value>
        /// The name of the relationship schema.
        /// </value>
        public string RelationshipSchemaName { get; set; }

        /// <summary>
        /// Gets or sets the name of the intersect entity.
        /// </summary>
        /// <value>
        /// The name of the intersect entity.
        /// </value>
        public string IntersectEntityName { get; set; }

        /// <summary>
        /// Gets or sets the logical name of the entity 1.
        /// </summary>
        /// <value>
        /// The name of the logical entity 1.
        /// </value>
        public string Entity1LogicalName { get; set; }

        /// <summary>
        /// Gets or sets the entity 1 intersect attribute.
        /// </summary>
        /// <value>
        /// The entity 1 intersect attribute.
        /// </value>
        public string Entity1IntersectAttribute { get; set; }

        /// <summary>
        /// Gets or sets the logical name of the entity 2.
        /// </summary>
        /// <value>
        /// The logical name of the entity 2.
        /// </value>
        public string Entity2LogicalName { get; set; }

        /// <summary>
        /// Gets or sets the entity 2 intersect attribute.
        /// </summary>
        /// <value>
        /// The entity 2 intersect attribute.
        /// </value>
        public string Entity2IntersectAttribute { get; set; }

        /// <summary>
        /// Gets or sets the exported records number.
        /// </summary>
        /// <value>
        /// The exported records number.
        /// </value>
        public int ExportedRecords { get; set; }
    }

    /// <summary>
    /// N to N Transport Report
    /// </summary>
    [DataContract(Namespace = "http://mscrmtoolkit.codeplex.com")]
    public class NtoNTransportReport
    {
        /// <summary>
        /// Gets or sets the name of the transport profile.
        /// </summary>
        /// <value>
        /// The name of the transport profile.
        /// </value>
        [DataMember(Order = 1)]
        public string TransportProfileName { get; set; }

        /// <summary>
        /// Gets or sets the transport started at.
        /// </summary>
        /// <value>
        /// The transport started at.
        /// </value>
        [DataMember(Order = 2)]
        public string TransportStartedAt { get; set; }

        /// <summary>
        /// Gets or sets the transport finished at.
        /// </summary>
        /// <value>
        /// The transport finished at.
        /// </value>
        [DataMember(Order = 3)]
        public string TransportFinishedAt { get; set; }

        /// <summary>
        /// Gets or sets the transported in.
        /// </summary>
        /// <value>
        /// The transported in.
        /// </value>
        [DataMember(Order = 4)]
        public string TransportedIn { get; set; }

        /// <summary>
        /// Gets or sets the total exported records.
        /// </summary>
        /// <value>
        /// The total exported records.
        /// </value>
        [DataMember(Order = 5)]
        public int TotalExportedRecords { get; set; }

        /// <summary>
        /// Gets or sets the total imported records.
        /// </summary>
        /// <value>
        /// The total imported records.
        /// </value>
        [DataMember(Order = 6)]
        public int TotalImportedRecords { get; set; }

        /// <summary>
        /// Gets or sets the total import failures.
        /// </summary>
        /// <value>
        /// The total import failures.
        /// </value>
        [DataMember(Order = 7)]
        public int TotalImportFailures { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [transport completed].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [transport completed]; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 8)]
        public bool TransportCompleted { get; set; }

        /// <summary>
        /// Gets or sets the report lines.
        /// </summary>
        /// <value>
        /// The report lines.
        /// </value>
        [DataMember(Order = 9)]
        public List<NtoNTransportReportLine> ReportLines { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NtoNTransportReport"/> class.
        /// </summary>
        /// <param name="TransportProfileName">Name of the transport profile.</param>
        public NtoNTransportReport(string TransportProfileName)
        {
            this.TransportProfileName = TransportProfileName;
            TransportStartedAt = DateTime.Now.ToString();
            ReportLines = new List<NtoNTransportReportLine>();
            TransportCompleted = false;
        }
    }

    /// <summary>
    /// N to N Transport Report Line
    /// </summary>
    public class NtoNTransportReportLine
    {
        /// <summary>
        /// Gets or sets the name of the relationship schema.
        /// </summary>
        /// <value>
        /// The name of the relationship schema.
        /// </value>
        public string RelationshipSchemaName { get; set; }

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
        /// Gets or sets the imported in.
        /// </summary>
        /// <value>
        /// The imported in.
        /// </value>
        public string ImportedIn { get; set; }
    }

    /// <summary>
    /// N to N Relationships Import Failure
    /// </summary>
    public class NtoNRelationshipsImportFailure
    {
        /// <summary>
        /// Gets or sets the created on.
        /// </summary>
        /// <value>
        /// The created on.
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
        /// Gets or sets the name of the nto n relationship.
        /// </summary>
        /// <value>
        /// The name of the nto n relationship.
        /// </value>
        public string NtoNRelationshipName { get; set; }

        //public string Operation { get; set; }
        /// <summary>
        /// Gets or sets the URL of entity 1.
        /// </summary>
        /// <value>
        /// The URL of entity 1.
        /// </value>
        public string UrlEntity1 { get; set; }

        /// <summary>
        /// Gets or sets the URL of entity 2.
        /// </summary>
        /// <value>
        /// The URL of entity 2.
        /// </value>
        public string UrlEntity2 { get; set; }
    }
}