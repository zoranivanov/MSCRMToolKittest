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
    /// Midcorosft Dynamics CRM Workflow
    /// </summary>
    public class MSCRMWorkflow
    {
        /// <summary>
        /// Gets or sets the name of the Worklfow.
        /// </summary>
        /// <value>
        /// The Workflow name.
        /// </value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the identifier of the Workflow.
        /// </summary>
        /// <value>
        /// The Workflow identifier.
        /// </value>
        public Guid Id { get; set; }
    }
}