using Microsoft.Xrm.Sdk.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSCRMToolKit
{
    /// <summary>
    /// DiagramBuildingProperties class
    /// </summary>
    internal class DiagramBuildingProperties
    {
        /// <summary>
        /// the connection Name
        /// </summary>
        public string connectionName { get; set; }
        /// <summary>
        /// Entities label idsplay: 0:Logical Name; 1: Display Name; 2: Logical + Display Name
        /// </summary>
        public int entityLabelDisplay { get; set; }
        /// <summary>
        /// Attributes label idsplay: 0:Logical Name; 1: Display Name; 2: Logical + Display Name
        /// </summary>
        public int attributeLabelDisplay { get; set; }
        /// <summary>
        /// Show Primary Keys
        /// </summary>
        public bool showPrimaryKeys { get; set; }
        /// <summary>
        /// Show Foreign Keys
        /// </summary>
        public bool showForeignKeys { get; set; }
        /// <summary>
        /// Show Relationships names
        /// </summary>
        public bool showRelationshipsNames { get; set; }
        /// <summary>
        /// Show Ownership with colors
        /// </summary>
        public bool showOwnership { get; set; }
        /// <summary>
        /// List of entities for the diagram construction
        /// </summary>
        public List<string> entities { get; set; }
        /// <summary>
        /// The CRM Environement Metadata
        /// </summary>
        public RetrieveAllEntitiesResponse environmentStructure { get; set; }
    }
}
