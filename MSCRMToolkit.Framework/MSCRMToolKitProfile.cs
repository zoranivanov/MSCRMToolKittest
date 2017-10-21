// ========================================================================================
//  This file is part of the MSCRM ToolKit project.
//  https://github.com/zoranivanov/MSCRMToolKit
//  Author:         Zoran IVANOV
//  Created:        14/04/2014
//
//  Disclaimer:
//  This software is provided "as is" with no technical support.
//  Use it at your own risk.
//  The author does not take any responsibility for any damage in whatever form or context.
// ========================================================================================

namespace MSCRMToolKit
{
    /// <summary>
    /// MSCRMToolKit generic Profile
    /// </summary>
    public class MSCRMToolKitProfile
    {
        /// <summary>
        /// The Connections Manager
        /// </summary>
        private MSCRMConnectionsManager cm = new MSCRMConnectionsManager();
        /// <summary>
        /// The source connection
        /// </summary>
        private MSCRMConnection SourceConnection = null;
        /// <summary>
        /// The target connection
        /// </summary>
        private MSCRMConnection TargetConnection = null;
        /// <summary>
        /// Gets or sets the name of the profile.
        /// </summary>
        /// <value>
        /// The name of the profile.
        /// </value>
        public string ProfileName { get; set; }
        /// <summary>
        /// Gets or sets the name of the source connection.
        /// </summary>
        /// <value>
        /// The name of the source connection.
        /// </value>
        public string SourceConnectionName { get; set; }
        /// <summary>
        /// Gets or sets the name of the target connection.
        /// </summary>
        /// <value>
        /// The name of the target connection.
        /// </value>
        public string TargetConnectionName { get; set; }
        /// <summary>
        /// Sets the source conneciton.
        /// </summary>
        /// <remarks>
        /// The SourceConnectionName must be previously set.
        /// </remarks>
        public void setSourceConneciton()
        {
            if (!string.IsNullOrEmpty(SourceConnectionName))
                SourceConnection = cm.getConnection(SourceConnectionName);
        }
        /// <summary>
        /// Gets the source conneciton.
        /// </summary>
        /// <returns>
        /// The Source MSCRMConnection.
        /// </returns>
        /// <remarks>
        /// The SourceConnectionName must be previously set.
        /// </remarks>
        public MSCRMConnection getSourceConneciton()
        {
            if (!string.IsNullOrEmpty(SourceConnectionName))
                SourceConnection = cm.getConnection(SourceConnectionName);

            return SourceConnection;
        }
        /// <summary>
        /// Sets the target conneciton.
        /// </summary>
        /// <remarks>
        /// The TargetConnectionName must be previously set.
        /// </remarks>
        public void setTargetConneciton()
        {
            if (!string.IsNullOrEmpty(TargetConnectionName))
                TargetConnection = cm.getConnection(TargetConnectionName);
        }
        /// <summary>
        /// Gets the target conneciton.
        /// </summary>
        /// <returns>
        /// The Target MSCRMConnection.
        /// </returns>
        /// <remarks>
        /// The TargetConnectionName must be previously set.
        /// </remarks>
        public MSCRMConnection getTargetConneciton()
        {
            if (!string.IsNullOrEmpty(TargetConnectionName))
                TargetConnection = cm.getConnection(TargetConnectionName);

            return TargetConnection;
        }
    }
}
