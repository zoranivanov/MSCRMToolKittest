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

namespace MSCRMToolKit
{
    /// <summary>
    /// MSCRMWorkflowExecutionProfile class extending MSCRMToolKitProfile
    /// </summary>
    public class MSCRMWorkflowExecutionProfile : MSCRMToolKitProfile
    {
        /// <summary>
        /// Gets or sets the workflow identifier.
        /// </summary>
        /// <value>
        /// The workflow identifier.
        /// </value>
        public Guid WorkflowId { get; set; }
        /// <summary>
        /// Gets or sets the name of the workflow.
        /// </summary>
        /// <value>
        /// The name of the workflow.
        /// </value>
        public string WorkflowName { get; set; }
        /// <summary>
        /// Gets or sets the fetch XML query.
        /// </summary>
        /// <value>
        /// The fetch XML query.
        /// </value>
        public string FetchXMLQuery { get; set; }
    }
}