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
using System.Runtime.Serialization;

namespace MSCRMToolKit
{
    /// <summary>
    /// MSCRMDataExportProfile class extending MSCRMToolKitProfile
    /// </summary>
    public class MSCRMDataExportProfile : MSCRMToolKitProfile
    {
        /// <summary>
        /// Gets or sets the total exported records.
        /// </summary>
        /// <value>
        /// The total exported records.
        /// </value>
        public int TotalExportedRecords { get; set; }
        /// <summary>
        /// Gets or sets the fetch XML query.
        /// </summary>
        /// <value>
        /// The fetch XML query.
        /// </value>
        public string FetchXMLQuery { get; set; }
        /// <summary>
        /// Gets or sets the last run at.
        /// </summary>
        /// <value>
        /// The last run at.
        /// </value>
        public string LastRunAt { get; set; }
        /// <summary>
        /// Gets or sets the field separator.
        /// </summary>
        /// <value>
        /// The field separator.
        /// </value>
        public string FieldSeparator { get; set; }
        /// <summary>
        /// Gets or sets the data separator.
        /// </summary>
        /// <value>
        /// The data separator.
        /// </value>
        public string DataSeparator { get; set; }
        /// <summary>
        /// Gets or sets the export format.
        /// </summary>
        /// <value>
        /// The export format.
        /// </value>
        public string ExportFormat { get; set; }
        /// <summary>
        /// Gets or sets the encoding.
        /// </summary>
        /// <value>
        /// The encoding.
        /// </value>
        public string Encoding { get; set; }
    }

    /// <summary>
    /// Data Export Report
    /// </summary>
    [DataContract]
    public class DataExportReport
    {
        /// <summary>
        /// Gets or sets the name of the data export profile.
        /// </summary>
        /// <value>
        /// The name of the data export profile.
        /// </value>
        [DataMember]
        public string DataExportProfileName { get; set; }

        /// <summary>
        /// Gets or sets the data export started at.
        /// </summary>
        /// <value>
        /// The data export started at.
        /// </value>
        [DataMember]
        public string DataExportStartedAt { get; set; }

        /// <summary>
        /// Gets or sets the data export finished at.
        /// </summary>
        /// <value>
        /// The data export finished at.
        /// </value>
        [DataMember]
        public string DataExportFinishedAt { get; set; }

        /// <summary>
        /// Gets or sets the data exported in.
        /// </summary>
        /// <value>
        /// The data exported in.
        /// </value>
        [DataMember]
        public string DataExportedIn { get; set; }

        /// <summary>
        /// Gets or sets the total exported records.
        /// </summary>
        /// <value>
        /// The total exported records.
        /// </value>
        [DataMember]
        public int TotalExportedRecords { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [data export completed].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [data export completed]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool DataExportCompleted { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataExportReport"/> class.
        /// </summary>
        /// <param name="DataExportProfileName">Name of the data export profile.</param>
        public DataExportReport(string DataExportProfileName)
        {
            this.DataExportProfileName = DataExportProfileName;
            DataExportStartedAt = DateTime.Now.ToString();
            DataExportCompleted = false;
        }
    }

    /// <summary>
    /// String Map
    /// </summary>
    public class StringMap
    {
        /// <summary>
        /// Gets or sets the entity.
        /// </summary>
        /// <value>
        /// The entity.
        /// </value>
        public string Entity { get; set; }

        /// <summary>
        /// Gets or sets the attribute.
        /// </summary>
        /// <value>
        /// The attribute.
        /// </value>
        public string Attribute { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the lcid.
        /// </summary>
        /// <value>
        /// The lcid.
        /// </value>
        public string LCID { get; set; }
    }
}