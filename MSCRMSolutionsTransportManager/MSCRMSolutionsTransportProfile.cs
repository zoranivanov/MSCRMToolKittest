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

using System.Collections.Generic;

namespace MSCRMToolKit
{
    /// <summary>
    /// MSCRMSolutionsTransportProfile class extending MSCRMToolKitProfile
    /// </summary>
    public class MSCRMSolutionsTransportProfile : MSCRMToolKitProfile
    {
        /// <summary>
        /// Gets or sets the operation.
        /// </summary>
        /// <value>
        /// The operation.
        /// </value>
        public int Operation { get; set; }
        /// <summary>
        /// Gets or sets the solution export folder.
        /// </summary>
        /// <value>
        /// The solution export folder.
        /// </value>
        public string SolutionExportFolder { get; set; }
        /// <summary>
        /// Gets or sets the selected solutions names.
        /// </summary>
        /// <value>
        /// The selected solutions names.
        /// </value>
        public List<string> SelectedSolutionsNames { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [export as managed].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [export as managed]; otherwise, <c>false</c>.
        /// </value>
        public bool ExportAsManaged { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [publish all customizations source].
        /// </summary>
        /// <value>
        /// <c>true</c> if [publish all customizations source]; otherwise, <c>false</c>.
        /// </value>
        public bool PublishAllCustomizationsSource { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [export automatic numbering settings].
        /// </summary>
        /// <value>
        /// <c>true</c> if [export automatic numbering settings]; otherwise, <c>false</c>.
        /// </value>
        public bool ExportAutoNumberingSettings { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [export calendar settings].
        /// </summary>
        /// <value>
        /// <c>true</c> if [export calendar settings]; otherwise, <c>false</c>.
        /// </value>
        public bool ExportCalendarSettings { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [export customization settings].
        /// </summary>
        /// <value>
        /// <c>true</c> if [export customization settings]; otherwise, <c>false</c>.
        /// </value>
        public bool ExportCustomizationSettings { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [export email tracking settings].
        /// </summary>
        /// <value>
        /// <c>true</c> if [export email tracking settings]; otherwise, <c>false</c>.
        /// </value>
        public bool ExportEmailTrackingSettings { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [export general settings].
        /// </summary>
        /// <value>
        /// <c>true</c> if [export general settings]; otherwise, <c>false</c>.
        /// </value>
        public bool ExportGeneralSettings { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [export isv configuration].
        /// </summary>
        /// <value>
        /// <c>true</c> if [export isv configuration]; otherwise, <c>false</c>.
        /// </value>
        public bool ExportIsvConfig { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [export marketing settings].
        /// </summary>
        /// <value>
        /// <c>true</c> if [export marketing settings]; otherwise, <c>false</c>.
        /// </value>
        public bool ExportMarketingSettings { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [export outlook synchronization settings].
        /// </summary>
        /// <value>
        /// <c>true</c> if [export outlook synchronization settings]; otherwise, <c>false</c>.
        /// </value>
        public bool ExportOutlookSynchronizationSettings { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [export relationship roles].
        /// </summary>
        /// <value>
        /// <c>true</c> if [export relationship roles]; otherwise, <c>false</c>.
        /// </value>
        public bool ExportRelationshipRoles { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [publish all customizations target].
        /// </summary>
        /// <value>
        /// <c>true</c> if [publish all customizations target]; otherwise, <c>false</c>.
        /// </value>
        public bool PublishAllCustomizationsTarget { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [overwrite unmanaged customizations].
        /// </summary>
        /// <value>
        /// <c>true</c> if [overwrite unmanaged customizations]; otherwise, <c>false</c>.
        /// </value>
        public bool OverwriteUnmanagedCustomizations { get; set; }
        /// <summary>
        /// Gets or sets the solutions to import.
        /// </summary>
        /// <value>
        /// The solutions to import.
        /// </value>
        public string SolutionsToImport { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [publish workflows].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [publish workflows]; otherwise, <c>false</c>.
        /// </value>
        public bool PublishWorkflows { get; set; }
    }
}