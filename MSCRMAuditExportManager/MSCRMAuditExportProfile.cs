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

using System;
using System.Collections.Generic;

namespace MSCRMToolKit
{
    /// <summary>
    /// MSCRMAuditExportProfile class extending MSCRMToolKitProfile
    /// </summary>
    public class MSCRMAuditExportProfile : MSCRMToolKitProfile
    {
        /// <summary>
        /// Gets or sets the type of the audit.
        /// </summary>
        /// <value>
        /// The type of the audit.
        /// </value>
        public string AuditType { get; set; }
        /// <summary>
        /// Gets or sets the last run at.
        /// </summary>
        /// <value>
        /// The last run at.
        /// </value>
        public string LastRunAt { get; set; }
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
        /// <summary>
        /// Gets or sets a value indicating whether [all entities selected].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [all entities selected]; otherwise, <c>false</c>.
        /// </value>
        public bool AllEntitiesSelected { get; set; }
        /// <summary>
        /// Gets or sets the selected entities.
        /// </summary>
        /// <value>
        /// The selected entities.
        /// </value>
        public List<SelectedAuditEntity> SelectedEntities { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [all users selected].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [all users selected]; otherwise, <c>false</c>.
        /// </value>
        public bool AllUsersSelected { get; set; }
        /// <summary>
        /// Gets or sets the selected users.
        /// </summary>
        /// <value>
        /// The selected users.
        /// </value>
        public List<AuditUser> SelectedUsers { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [all operations selected].
        /// </summary>
        /// <value>
        /// <c>true</c> if [all operations selected]; otherwise, <c>false</c>.
        /// </value>
        public bool AllOperationsSelected { get; set; }
        /// <summary>
        /// Gets or sets the selected operations.
        /// </summary>
        /// <value>
        /// The selected operations.
        /// </value>
        public List<int> SelectedOperations { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [all actions selected].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [all actions selected]; otherwise, <c>false</c>.
        /// </value>
        public bool AllActionsSelected { get; set; }
        /// <summary>
        /// Gets or sets the selected actions.
        /// </summary>
        /// <value>
        /// The selected actions.
        /// </value>
        public List<int> SelectedActions { get; set; }
        /// <summary>
        /// Gets or sets the audit record created on filter.
        /// </summary>
        /// <value>
        /// The audit record created on filter.
        /// </value>
        public string AuditRecordCreatedOnFilter { get; set; }
        /// <summary>
        /// Gets or sets the audit record created on filter last x.
        /// </summary>
        /// <value>
        /// The audit record created on filter last x.
        /// </value>
        public decimal AuditRecordCreatedOnFilterLastX { get; set; }
        /// <summary>
        /// Gets or sets the audit record created on filter from.
        /// </summary>
        /// <value>
        /// The audit record created on filter from.
        /// </value>
        public DateTime AuditRecordCreatedOnFilterFrom { get; set; }
        /// <summary>
        /// Gets or sets the audit record created on filter to.
        /// </summary>
        /// <value>
        /// The audit record created on filter to.
        /// </value>
        public DateTime AuditRecordCreatedOnFilterTo { get; set; }
        /// <summary>
        /// Gets the selected entity.
        /// </summary>
        /// <param name="selectedEntityName">Name of the selected entity.</param>
        /// <returns>The Selected Audit Entity</returns>
        public SelectedAuditEntity getSelectedEntity(string selectedEntityName)
        {
            foreach (SelectedAuditEntity ee in this.SelectedEntities)
            {
                if (selectedEntityName == ee.LogicalName)
                    return ee;
            }
            return null;
        }
    }

    /// <summary>
    /// Environment Audit Entity
    /// </summary>
    public class EnvAuditEntity
    {
        /// <summary>
        /// Gets or sets the entity logical name.
        /// </summary>
        /// <value>
        /// The entity logical name.
        /// </value>
        public string LogicalName { get; set; }

        /// <summary>
        /// Gets or sets the object type code.
        /// </summary>
        /// <value>
        /// The object type code.
        /// </value>
        public int ObjectTypeCode { get; set; }

        /// <summary>
        /// Gets or sets the primary name attribute.
        /// </summary>
        /// <value>
        /// The primary name attribute.
        /// </value>
        public string PrimaryNameAttribute { get; set; }

        /// <summary>
        /// Gets or sets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        public List<string> Attributes { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvAuditEntity"/> class.
        /// </summary>
        public EnvAuditEntity()
        {
            Attributes = new List<string>();
        }
    }

    /// <summary>
    /// Environment Audit Structure
    /// </summary>
    public class EnvAuditStructure
    {
        /// <summary>
        /// Gets or sets the name of the connection.
        /// </summary>
        /// <value>
        /// The name of the connection.
        /// </value>
        public string connectionName { get; set; }

        /// <summary>
        /// Gets or sets the entities.
        /// </summary>
        /// <value>
        /// The entities.
        /// </value>
        public List<EnvAuditEntity> Entities { get; set; }

        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        public List<AuditUser> Users { get; set; }

        /// <summary>
        /// Gets or sets the operations.
        /// </summary>
        /// <value>
        /// The operations.
        /// </value>
        public List<KeyValuePair<int, string>> Operations { get; set; }

        /// <summary>
        /// Gets or sets the actions.
        /// </summary>
        /// <value>
        /// The actions.
        /// </value>
        public List<KeyValuePair<int, string>> Actions { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvAuditStructure"/> class.
        /// </summary>
        public EnvAuditStructure()
        {
            Entities = new List<EnvAuditEntity>();
            Users = new List<AuditUser>();
            Operations = new List<KeyValuePair<int, string>>();
            Actions = new List<KeyValuePair<int, string>>();
        }
    }

    /// <summary>
    /// Audit User
    /// </summary>
    public class AuditUser
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        /// <value>
        /// The full name.
        /// </value>
        public string FullName { get; set; }
    }

    /// <summary>
    /// Selected Audit Entity
    /// </summary>
    public class SelectedAuditEntity
    {
        /// <summary>
        /// Gets or sets the entity logical name.
        /// </summary>
        /// <value>
        /// The entity logical name.
        /// </value>
        public string LogicalName { get; set; }

        /// <summary>
        /// Gets or sets the object type code.
        /// </summary>
        /// <value>
        /// The object type code.
        /// </value>
        public int ObjectTypeCode { get; set; }

        /// <summary>
        /// Gets or sets the filter.
        /// </summary>
        /// <value>
        /// The filter.
        /// </value>
        public string Filter { get; set; }

        /// <summary>
        /// Gets or sets the selected attributes.
        /// </summary>
        /// <value>
        /// The selected attributes.
        /// </value>
        public List<string> SelectedAttributes { get; set; }
    }

    /// <summary>
    /// Audit Detail Line class extending Microsoft.Xrm.Sdk.Entity
    /// </summary>
    public class AuditDetailLine : Microsoft.Xrm.Sdk.Entity
    {
        /// <summary>
        /// Gets or sets the createdon.
        /// </summary>
        /// <value>
        /// The createdon.
        /// </value>
        public DateTime createdon { get; set; }

        /// <summary>
        /// Gets or sets the userid.
        /// </summary>
        /// <value>
        /// The userid.
        /// </value>
        public Guid userid { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the objecttypecode.
        /// </summary>
        /// <value>
        /// The objecttypecode.
        /// </value>
        public int objecttypecode { get; set; }

        /// <summary>
        /// Gets or sets the objectid.
        /// </summary>
        /// <value>
        /// The objectid.
        /// </value>
        public Guid objectid { get; set; }

        /// <summary>
        /// Gets or sets the name of the record logical.
        /// </summary>
        /// <value>
        /// The name of the record logical.
        /// </value>
        public string RecordLogicalName { get; set; }

        /// <summary>
        /// Gets or sets the name of the record.
        /// </summary>
        /// <value>
        /// The name of the record.
        /// </value>
        public string RecordName { get; set; }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>
        /// The action.
        /// </value>
        public string action { get; set; }

        /// <summary>
        /// Gets or sets the operation.
        /// </summary>
        /// <value>
        /// The operation.
        /// </value>
        public string operation { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public string key { get; set; }

        /// <summary>
        /// Gets or sets the old value.
        /// </summary>
        /// <value>
        /// The old value.
        /// </value>
        public string oldValue { get; set; }

        /// <summary>
        /// Gets or sets the new value.
        /// </summary>
        /// <value>
        /// The new value.
        /// </value>
        public string newValue { get; set; }

        /// <summary>
        /// Gets or sets the transaction identifier.
        /// </summary>
        /// <value>
        /// The transaction identifier.
        /// </value>
        public Guid TransactionId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditDetailLine"/> class.
        /// </summary>
        public AuditDetailLine()
        {
            UserName = "";
            RecordLogicalName = "";
            RecordName = "";
            action = "";
            operation = "";
            key = "";
            oldValue = "";
            newValue = "";
            //link = "";
        }
    }
}