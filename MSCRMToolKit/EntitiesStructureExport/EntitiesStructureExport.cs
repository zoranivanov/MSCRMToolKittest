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

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace MSCRMToolKit
{
    /// <summary>
    /// EntitiesStructureExport class
    /// </summary>
    public partial class EntitiesStructureExport : Form
    {
        private OrganizationServiceProxy _serviceProxy = null;
        private MSCRMConnectionsManager cm = new MSCRMConnectionsManager();
        private RetrieveAllEntitiesResponse environmentStructure = null;
        private List<string> checkedEntities = new List<string>();
        private string EntitesStructureFolderName = "EntitesStructure";
        private string tempExportedFilePath = "";
        private string exportedFilePath = "";
        private List<ControlEnabled> cList = new List<ControlEnabled>();

        // Diagram generation variables****************************************
        private BackgroundWorker bwDiagramsGenerator = new BackgroundWorker();
        private List<string> DGentities = new List<string>();
        private string DGconnectionName = "";
        private bool DiagramGeneratedSuccesfully = false;
        private int selectedDiagramsEntityLabelIndex = 0;
        private int selectedDiagramsAttributeLabelIndex = 0;
        private bool showForeignKeys = true;
        private bool showPrimaryKeys = true;
        private bool showRelationshipsNames = false;
        private bool showOwnership = false;
        // Diagram generation variables****************************************

        /// <summary>
        /// Initializes a new instance of the <see cref="EntitiesStructureExport"/> class.
        /// </summary>
        public EntitiesStructureExport()
        {
            InitializeComponent();

            int cpt = 0;
            foreach (MSCRMConnection connection in cm.MSCRMConnections)
            {
                this.comboBoxSource.Items.AddRange(new object[] { connection.ConnectionName });
                cpt++;
            }

            bwDiagramsGenerator.WorkerReportsProgress = true;
            bwDiagramsGenerator.WorkerSupportsCancellation = true;
            bwDiagramsGenerator.DoWork += new DoWorkEventHandler(bwGenerateGiagrams_DoWork);
            bwDiagramsGenerator.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(bwGenerateGiagrams_ProgressChanged);
            bwDiagramsGenerator.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwGenerateGiagrams_RunWorkerCompleted);
            comboBoxDiagramEntitiesLabels.SelectedIndex = 0;
            comboBoxDiagramAttributesLabels.SelectedIndex = 0;
            checkBoxDiagramShowOwnership.Checked = false;
            checkBoxShowForeignKeys.Checked = true;
            checkBoxShowPrimaryKeys.Checked = true;
        }

        private void comboBoxSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkedListBoxEntities.Items.Clear();
            string structureFileName = EntitesStructureFolderName + "\\" + comboBoxSource.SelectedItem + ".xml";
            if (File.Exists(structureFileName))
            {
                toolStripStatusLabel1.Text = "Loading MetaData from disc. Please wait...";
                Application.DoEvents();
                environmentStructure = ReadMetadata(structureFileName);
                DateTime structureRefreshedOn = File.GetLastWriteTime(structureFileName);
                labelStructureLoadedDate.Text = structureRefreshedOn.ToString();
                IOrderedEnumerable<EntityMetadata> EMD = environmentStructure.EntityMetadata.OrderBy(em => em.LogicalName);
                foreach (EntityMetadata currentEntity in EMD)
                {
                    if (currentEntity.IsIntersect.Value == false)
                    {
                        checkedListBoxEntities.Items.AddRange(new object[] { currentEntity.LogicalName });
                    }
                }
                toolStripStatusLabel1.Text = "MetaData loaded from disc.";
            }
            else
            {
                labelStructureLoadedDate.Text = "never";
            }
            buttonLoadEntitiesFromSource.Enabled = true;
            buttonExportStructure.Enabled = true;
            buttonExportRelationships.Enabled = true;
            buttonGenerateDiagrams.Enabled = true;
            buttonSolutionComponents.Enabled = true;
            buttonFieldsOnForms.Enabled = true;
            comboBoxFilter.Enabled = true;
        }

        private static IEnumerable<Control> GetAllChildrens(Control control)
        {
            var controls = control.Controls.Cast<Control>();
            return controls.SelectMany(c => GetAllChildrens(c))
              .Concat(controls);
        }

        private void setStateAllControls(bool newState, List<string> ignoredControls = null)
        {
            List<Type> ignoredTypes = new List<Type>();
            ignoredTypes.Add(typeof(Panel));
            ignoredTypes.Add(typeof(System.Windows.Forms.Label));

            if (newState)
            {
                foreach (ControlEnabled ce in cList)
                {
                    ce.Control.Enabled = ce.Enabled;
                }
            }
            else
            {
                cList = new List<ControlEnabled>();
                IEnumerable<Control> allControls = GetAllChildrens(tableLayoutPanel1);
                foreach (Control c in allControls)
                {
                    if (ignoredTypes.Contains(c.GetType()))
                        continue;
                    if (ignoredControls != null && ignoredControls.Contains(c.Name))
                        continue;
                    //if (typeof(Panel) == c.GetType())
                    //    continue;
                    //if (typeof(System.Windows.Forms.Label) == c.GetType())
                    //    continue;
                    //if (c.Name == buttonStopDiagramGeneration.Name)
                    //    continue;
                    cList.Add(new ControlEnabled { Control = c, Enabled = c.Enabled });
                    c.Enabled = false;
                }
            }
        }

        private void buttonLoadEntitiesFromSource_Click(object sender, EventArgs e)
        {
            DialogResult dResTest = MessageBox.Show("Loading the structure may take some time. The application will become unresponsive during this time.\n\nAre you sure you want to load the structure from the Source?", "Confirm Structure Load", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dResTest == DialogResult.No)
            {
                return;
            }

            setStateAllControls(false);
            try
            {
                toolStripStatusLabel1.Text = "Loading MetaData from source. Please wait...";
                Application.DoEvents();
                MSCRMConnection connection = cm.MSCRMConnections[comboBoxSource.SelectedIndex];
                if (_serviceProxy == null)
                    _serviceProxy = cm.connect(connection);

                RetrieveAllEntitiesRequest request = new RetrieveAllEntitiesRequest()
                {
                    EntityFilters = EntityFilters.Entity | EntityFilters.Attributes | EntityFilters.Relationships,
                    RetrieveAsIfPublished = false
                };

                //Make sure the EntityExport folder exists
                if (!Directory.Exists(EntitesStructureFolderName))
                    Directory.CreateDirectory(EntitesStructureFolderName);

                // Retrieve the MetaData.
                environmentStructure = (RetrieveAllEntitiesResponse)_serviceProxy.Execute(request);
                string structureFileName = EntitesStructureFolderName + "\\" + comboBoxSource.SelectedItem + ".xml";
                WriteMetadata(structureFileName, environmentStructure);
                DateTime structureRefreshedOn = File.GetLastWriteTime(structureFileName);
                labelStructureLoadedDate.Text = structureRefreshedOn.ToString();
                IOrderedEnumerable<EntityMetadata> EMD = environmentStructure.EntityMetadata.OrderBy(em => em.LogicalName);
                foreach (EntityMetadata currentEntity in EMD)
                {
                    if (currentEntity.IsIntersect.Value == false)
                    {
                        checkedListBoxEntities.Items.AddRange(new object[] { currentEntity.LogicalName });
                    }
                }
                toolStripStatusLabel1.Text = "MetaData loaded from source.";
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                toolStripStatusLabel1.Text = "Error.";
                MessageBox.Show("Error:" + ex.Detail.Message + "\n" + ex.Detail.TraceText, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = "Error.";
                if (ex.InnerException != null)
                    MessageBox.Show("Error:" + ex.Message + "\n" + ex.InnerException.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("Error:" + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            setStateAllControls(true);
            buttonOpenInExcel.Visible = false;
        }

        private void WriteMetadata(string filename, RetrieveAllEntitiesResponse RAER)
        {
            List<Type> knownTypes = new List<Type>();
            knownTypes.Add(typeof(EntityMetadata));
            FileStream writer = new FileStream(filename, FileMode.Create);
            DataContractSerializer ser = new DataContractSerializer(typeof(RetrieveAllEntitiesResponse), knownTypes);
            ser.WriteObject(writer, RAER);
            writer.Close();
        }

        private RetrieveAllEntitiesResponse ReadMetadata(string filename)
        {
            XmlDictionaryReaderQuotas XRQ = new XmlDictionaryReaderQuotas();
            XRQ.MaxStringContentLength = int.MaxValue;

            using(FileStream fs = new FileStream(filename, FileMode.Open))
            using(XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, XRQ))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(RetrieveAllEntitiesResponse));
                RetrieveAllEntitiesResponse RAER = (RetrieveAllEntitiesResponse)ser.ReadObject(reader, true);            
                return RAER;
            }
        }

        private bool AtLeastOneMetadataColumnChecked(TreeNode tn)
        {
            if (tn.Checked)
                return true;
            else if (tn.Nodes.Count > 0)
            {
                foreach (TreeNode tnChild in tn.Nodes)
                {
                    return AtLeastOneMetadataColumnChecked(tnChild);
                }
            }
            return false;
        }

        private void buttonExportStructure_Click(object sender, EventArgs e)
        {
            if (environmentStructure == null)
            {
                MessageBox.Show("You must first load the Entities Structure before Exporting!");
                return;
            }

            if (checkedListBoxEntities.CheckedItems.Count == 0)
            {
                MessageBox.Show("You must check at least 1 Entity for export!");
                return;
            }

            bool atLeastOneColumnchecked = false;
            foreach (TreeNode tn in treeViewExport.Nodes)
            {
                if (AtLeastOneMetadataColumnChecked(tn))
                {
                    atLeastOneColumnchecked = true;
                    break;
                }
            }

            if (!atLeastOneColumnchecked)
            {
                MessageBox.Show("You must check at least 1 Metadata Column for export!");
                return;
            }

            setStateAllControls(false);
            try
            {
                //Set the Exe path as current path
                String workingDirectory = Path.GetDirectoryName(Assembly.GetAssembly(typeof(ReferenceDataTransporter)).Location);
                Directory.SetCurrentDirectory(workingDirectory);
                //Make sure the EntityExport folder exists
                if (!Directory.Exists(EntitesStructureFolderName))
                    Directory.CreateDirectory(EntitesStructureFolderName);

                DateTime now = DateTime.Now;
                tempExportedFilePath = String.Concat(EntitesStructureFolderName + "\\" + comboBoxSource.SelectedItem + "_" + now.Year + "-" + now.Month + "-" + now.Day + "-" + now.Hour + "-" + now.Minute + "-" + now.Second + ".xml");

                MemoryStream ms = new MemoryStream();

                using (StreamWriter sw = new StreamWriter(tempExportedFilePath))
                {
                    // Create Xml Writer.
                    XmlTextWriter metadataWriter = new XmlTextWriter(sw);

                    // Start Xml File.
                    metadataWriter.WriteStartDocument();

                    // <?mso-application progid="Excel.Sheet"?>
                    metadataWriter.WriteProcessingInstruction("mso-application", "progid=\"Excel.Sheet\"");

                    // Metadata Xml Node.
                    metadataWriter.WriteStartElement("Metadata");

                    IOrderedEnumerable<EntityMetadata> EMD = environmentStructure.EntityMetadata.OrderBy(em => em.LogicalName);

                    foreach (EntityMetadata currentEntity in EMD)
                    {
                        if ((currentEntity.IsIntersect.Value == false) && (checkedListBoxEntities.CheckedItems.Contains(currentEntity.LogicalName)))
                        {
                            // Start Entity Node
                            metadataWriter.WriteStartElement("Entity");
                            if (treeViewExport.Nodes["NodeExportEntitesMetaData"].Checked)
                            {
                                if (treeViewExport.Nodes["NodeExportEntitesMetaData"].Nodes["NodeEntityLogicalName"].Checked)
                                    metadataWriter.WriteElementString("EntityLogicalName", currentEntity.LogicalName);
                                if (treeViewExport.Nodes["NodeExportEntitesMetaData"].Nodes["NodeEntityDisplayName"].Checked)
                                {
                                    if (currentEntity.DisplayName.UserLocalizedLabel != null)
                                        metadataWriter.WriteElementString("EntityDisplayName", currentEntity.DisplayName.UserLocalizedLabel.Label);
                                }

                                if (treeViewExport.Nodes["NodeExportEntitesMetaData"].Nodes["NodeEntitySchemaName"].Checked)
                                    metadataWriter.WriteElementString("EntitySchemaName", currentEntity.SchemaName);
                                if (treeViewExport.Nodes["NodeExportEntitesMetaData"].Nodes["NodeEntityObjectTypeCode"].Checked)
                                    metadataWriter.WriteElementString("EntityObjectTypeCode", currentEntity.ObjectTypeCode.ToString());
                                if (treeViewExport.Nodes["NodeExportEntitesMetaData"].Nodes["NodeEntityIsCustom"].Checked)
                                    metadataWriter.WriteElementString("EntityIsCustom", currentEntity.IsCustomEntity.Value.ToString());
                                if (treeViewExport.Nodes["NodeExportEntitesMetaData"].Nodes["NodeEntityIsCustomizable"].Checked)
                                    metadataWriter.WriteElementString("EntityIsCustomizable", currentEntity.IsCustomizable.Value.ToString());
                                if (treeViewExport.Nodes["NodeExportEntitesMetaData"].Nodes["NodeEntityDescription"].Checked)
                                {
                                    if (currentEntity.Description.UserLocalizedLabel != null)
                                        metadataWriter.WriteElementString("EntityDescription", currentEntity.Description.UserLocalizedLabel.Label);
                                }
                                if (treeViewExport.Nodes["NodeExportEntitesMetaData"].Nodes["NodeEntityIsAuditEnabled"].Checked)
                                    metadataWriter.WriteElementString("EntityIsAuditEnabled", currentEntity.IsAuditEnabled.Value.ToString());



                                if (treeViewExport.Nodes["NodeExportEntitesMetaData"].Nodes["NodeEntityIsValidForAdvancedFind"].Checked)
                                    metadataWriter.WriteElementString("EntityIsValidForAdvancedFind", currentEntity.IsValidForAdvancedFind.Value.ToString());
                                if (treeViewExport.Nodes["NodeExportEntitesMetaData"].Nodes["NodeEntityIsActivity"].Checked)
                                    metadataWriter.WriteElementString("EntityIsActivity", currentEntity.IsActivity.Value.ToString());
                                if (treeViewExport.Nodes["NodeExportEntitesMetaData"].Nodes["NodeEntityIsAvailableOffline"].Checked)
                                    metadataWriter.WriteElementString("EntityIsAvailableOffline", currentEntity.IsAvailableOffline.Value.ToString());
                                if (treeViewExport.Nodes["NodeExportEntitesMetaData"].Nodes["NodeEntityIsDocumentManagementEnabled"].Checked)
                                    metadataWriter.WriteElementString("EntityIsDocumentManagementEnabled", currentEntity.IsDocumentManagementEnabled.Value.ToString());
                                if (treeViewExport.Nodes["NodeExportEntitesMetaData"].Nodes["NodeEntityIsMailMergeEnabled"].Checked)
                                    metadataWriter.WriteElementString("EntityIsMailMergeEnabled", currentEntity.IsMailMergeEnabled.Value.ToString());
                                if (treeViewExport.Nodes["NodeExportEntitesMetaData"].Nodes["NodeEntityIsManaged"].Checked)
                                    metadataWriter.WriteElementString("EntityIsManaged", currentEntity.IsManaged.Value.ToString());
                                if (treeViewExport.Nodes["NodeExportEntitesMetaData"].Nodes["NodeEntityIsVisibleInMobile"].Checked)
                                    metadataWriter.WriteElementString("EntityIsVisibleInMobile", currentEntity.IsVisibleInMobile.Value.ToString());
                            }

                            List<OneToManyRelationshipMetadata> ManyToOneRelationships = currentEntity.ManyToOneRelationships.ToList();

                            #region Attributes

                            if (treeViewExport.Nodes["NodeExportAttributesMetaData"].Checked)
                            {
                                // Write Entity's Attributes.
                                metadataWriter.WriteStartElement("Attributes");

                                foreach (AttributeMetadata currentAttribute in currentEntity.Attributes)
                                {
                                    // Only write out main attributes
                                    if (currentAttribute.AttributeOf == null)
                                    {
                                        // Start Attribute Node
                                        metadataWriter.WriteStartElement("Attribute");

                                        if (treeViewExport.Nodes["NodeExportAttributesMetaData"].Nodes["NodeAttributeLogicalName"].Checked)
                                            metadataWriter.WriteElementString("AttributeLogicalName", currentAttribute.LogicalName);
                                        if (treeViewExport.Nodes["NodeExportAttributesMetaData"].Nodes["NodeAttributeType"].Checked)
                                            metadataWriter.WriteElementString("AttributeType", currentAttribute.AttributeType.Value.ToString());
                                        if (treeViewExport.Nodes["NodeExportAttributesMetaData"].Nodes["NodeAttributeDisplayName"].Checked)
                                        {
                                            if (currentAttribute.DisplayName.UserLocalizedLabel != null)
                                                metadataWriter.WriteElementString("AttributeDisplayName", currentAttribute.DisplayName.UserLocalizedLabel.Label.ToString());
                                        }
                                        if (treeViewExport.Nodes["NodeExportAttributesMetaData"].Nodes["NodeAttributeDescription"].Checked)
                                        {
                                            if (currentAttribute.Description.UserLocalizedLabel != null)
                                            {
                                                metadataWriter.WriteElementString("AttributeDescription", currentAttribute.Description.UserLocalizedLabel.Label.ToString());
                                            }
                                        }
                                        if (treeViewExport.Nodes["NodeExportAttributesMetaData"].Nodes["NodeAttributeSchemaName"].Checked)
                                        {
                                            if (currentAttribute.SchemaName != null)
                                                metadataWriter.WriteElementString("AttributeSchemaName", currentAttribute.SchemaName.ToString());
                                        }

                                        if (treeViewExport.Nodes["NodeExportAttributesMetaData"].Nodes["NodeLookupReferencedEntity"].Checked)
                                        {
                                            if (currentAttribute.AttributeType.Value == AttributeTypeCode.Lookup)
                                            {
                                                var LookupAttribute = (LookupAttributeMetadata)currentAttribute;
                                                metadataWriter.WriteElementString("LookupReferencedEntity", LookupAttribute.Targets.Aggregate("", (current, entity) => current + ("" + entity)));
                                            }
                                            else
                                            {
                                                metadataWriter.WriteElementString("LookupReferencedEntity", "");
                                            }
                                        }
                                        if (treeViewExport.Nodes["NodeExportAttributesMetaData"].Nodes["NodeAttributeIsCustom"].Checked)
                                            metadataWriter.WriteElementString("AttributeIsCustom", currentAttribute.IsCustomAttribute.Value.ToString());
                                        if (treeViewExport.Nodes["NodeExportAttributesMetaData"].Nodes["NodeAttributeIsCustomizable"].Checked)
                                            metadataWriter.WriteElementString("AttributeIsCustomizable", currentAttribute.IsCustomizable.Value.ToString());
                                        if (treeViewExport.Nodes["NodeExportAttributesMetaData"].Nodes["NodeAttributeRequiredLevel"].Checked && currentAttribute.RequiredLevel != null)
                                            metadataWriter.WriteElementString("AttributeRequiredLevel", currentAttribute.RequiredLevel.Value.ToString());
                                        if (treeViewExport.Nodes["NodeExportAttributesMetaData"].Nodes["NodeAttributeIsValidForCreate"].Checked && currentAttribute.IsValidForCreate != null)
                                            metadataWriter.WriteElementString("AttributeIsValidForCreate", currentAttribute.IsValidForCreate.Value.ToString());
                                        if (treeViewExport.Nodes["NodeExportAttributesMetaData"].Nodes["NodeAttributeIsValidForRead"].Checked)
                                            metadataWriter.WriteElementString("AttributeIsValidForRead", currentAttribute.IsValidForRead.Value.ToString());
                                        if (treeViewExport.Nodes["NodeExportAttributesMetaData"].Nodes["NodeAttributeIsValidForUpdate"].Checked && currentAttribute.IsValidForUpdate != null)
                                            metadataWriter.WriteElementString("AttributeIsValidForUpdate", currentAttribute.IsValidForUpdate.Value.ToString());
                                        if (treeViewExport.Nodes["NodeExportAttributesMetaData"].Nodes["NodeAttributeIsAuditEnabled"].Checked)
                                            metadataWriter.WriteElementString("AttributeIsAuditEnabled", currentAttribute.IsAuditEnabled.Value.ToString());
                                        if (treeViewExport.Nodes["NodeExportAttributesMetaData"].Nodes["NodeAttributeIsManaged"].Checked)
                                            metadataWriter.WriteElementString("AttributeIsManaged", currentAttribute.IsManaged.Value.ToString());
                                        if (treeViewExport.Nodes["NodeExportAttributesMetaData"].Nodes["NodeAttributeIsPrimaryId"].Checked)
                                            metadataWriter.WriteElementString("AttributeIsPrimaryId", currentAttribute.IsPrimaryId.Value.ToString());
                                        if (treeViewExport.Nodes["NodeExportAttributesMetaData"].Nodes["NodeAttributeIsPrimaryName"].Checked && currentAttribute.IsPrimaryName != null)
                                            metadataWriter.WriteElementString("AttributeIsPrimaryName", currentAttribute.IsPrimaryName.Value.ToString());
                                        if (treeViewExport.Nodes["NodeExportAttributesMetaData"].Nodes["NodeAttributeIsRenameable"].Checked)
                                            metadataWriter.WriteElementString("AttributeIsRenameable", currentAttribute.IsRenameable.Value.ToString());
                                        if (treeViewExport.Nodes["NodeExportAttributesMetaData"].Nodes["NodeAttributeIsValidForAdvancedFind"].Checked && currentAttribute.IsValidForAdvancedFind != null)
                                            metadataWriter.WriteElementString("AttributeIsValidForAdvancedFind", currentAttribute.IsValidForAdvancedFind.Value.ToString());

                                        string MinimumValue = "";
                                        string MaximumValue = "";
                                        string DefaultValue = "";
                                        string Precision = "";
                                        string Format = "";
                                        string MaxLength = "";
                                        string GlobalOptionSet = "";
                                        string Options = "";
                                        if (currentAttribute.AttributeType != null)
                                        {
                                            if (currentAttribute.AttributeType.Value == AttributeTypeCode.Integer)
                                            {
                                                var typedAttribute = (IntegerAttributeMetadata)currentAttribute;
                                                MinimumValue = typedAttribute.MinValue.HasValue ? typedAttribute.MinValue.Value.ToString(CultureInfo.InvariantCulture) : "";
                                                MaximumValue = typedAttribute.MaxValue.HasValue ? typedAttribute.MaxValue.Value.ToString(CultureInfo.InvariantCulture) : "";
                                            }
                                            else if (currentAttribute.AttributeType.Value == AttributeTypeCode.BigInt)
                                            {
                                                var typedAttribute = (BigIntAttributeMetadata)currentAttribute;
                                                MinimumValue = typedAttribute.MinValue.HasValue ? typedAttribute.MinValue.Value.ToString(CultureInfo.InvariantCulture) : "";
                                                MaximumValue = typedAttribute.MaxValue.HasValue ? typedAttribute.MaxValue.Value.ToString(CultureInfo.InvariantCulture) : "";
                                            }
                                            else if (currentAttribute.AttributeType.Value == AttributeTypeCode.Decimal)
                                            {
                                                var typedAttribute = (DecimalAttributeMetadata)currentAttribute;
                                                MinimumValue = typedAttribute.MinValue.HasValue ? typedAttribute.MinValue.Value.ToString(CultureInfo.InvariantCulture) : "";
                                                MaximumValue = typedAttribute.MaxValue.HasValue ? typedAttribute.MaxValue.Value.ToString(CultureInfo.InvariantCulture) : "";
                                                Precision = typedAttribute.Precision.HasValue ? typedAttribute.Precision.Value.ToString(CultureInfo.InvariantCulture) : "";
                                            }
                                            else if (currentAttribute.AttributeType.Value == AttributeTypeCode.Double)
                                            {
                                                var typedAttribute = (DoubleAttributeMetadata)currentAttribute;
                                                MinimumValue = typedAttribute.MinValue.HasValue ? typedAttribute.MinValue.Value.ToString(CultureInfo.InvariantCulture) : "";
                                                MaximumValue = typedAttribute.MaxValue.HasValue ? typedAttribute.MaxValue.Value.ToString(CultureInfo.InvariantCulture) : "";
                                                Precision = typedAttribute.Precision.HasValue ? typedAttribute.Precision.Value.ToString(CultureInfo.InvariantCulture) : "";
                                            }
                                            else if (currentAttribute.AttributeType.Value == AttributeTypeCode.String)
                                            {
                                                var typedAttribute = (StringAttributeMetadata)currentAttribute;
                                                Format = typedAttribute.Format.HasValue ? typedAttribute.Format.Value.ToString() : "";
                                                MaxLength = typedAttribute.MaxLength.HasValue ? typedAttribute.MaxLength.Value.ToString(CultureInfo.InvariantCulture) : "";
                                            }
                                            else if (currentAttribute.AttributeType.Value == AttributeTypeCode.Memo)
                                            {
                                                var typedAttribute = (MemoAttributeMetadata)currentAttribute;
                                                Format = typedAttribute.Format.HasValue ? typedAttribute.Format.Value.ToString() : "";
                                                MaxLength = typedAttribute.MaxLength.HasValue ? typedAttribute.MaxLength.Value.ToString(CultureInfo.InvariantCulture) : "";
                                            }
                                            else if (currentAttribute.AttributeType.Value == AttributeTypeCode.Boolean)
                                            {
                                                var typedAttribute = (BooleanAttributeMetadata)currentAttribute;
                                                DefaultValue = typedAttribute.DefaultValue.HasValue ? typedAttribute.DefaultValue.Value.ToString(CultureInfo.InvariantCulture) : "";
                                                Options += "True: " + typedAttribute.OptionSet.TrueOption.Label.UserLocalizedLabel.Label;
                                                Options += "\r\nFalse: " + typedAttribute.OptionSet.FalseOption.Label.UserLocalizedLabel.Label;
                                            }
                                            else if (currentAttribute.AttributeType.Value == AttributeTypeCode.DateTime)
                                            {
                                                var typedAttribute = (DateTimeAttributeMetadata)currentAttribute;
                                                Format = typedAttribute.Format.HasValue ? typedAttribute.Format.Value.ToString() : "";
                                            }
                                            else if (currentAttribute.AttributeType.Value == AttributeTypeCode.Money)
                                            {
                                                var typedAttribute = (MoneyAttributeMetadata)currentAttribute;
                                                MinimumValue = typedAttribute.MinValue.HasValue ? typedAttribute.MinValue.Value.ToString(CultureInfo.InvariantCulture) : "";
                                                MaximumValue = typedAttribute.MaxValue.HasValue ? typedAttribute.MaxValue.Value.ToString(CultureInfo.InvariantCulture) : "";
                                                Precision = typedAttribute.Precision.HasValue ? typedAttribute.Precision.Value.ToString(CultureInfo.InvariantCulture) : "";
                                            }
                                            else if (currentAttribute.AttributeType.Value == AttributeTypeCode.Picklist)
                                            {
                                                var typedAttribute = (PicklistAttributeMetadata)currentAttribute;
                                                if (typedAttribute.OptionSet.IsGlobal.Value)
                                                    GlobalOptionSet = typedAttribute.OptionSet.Name;
                                                DefaultValue = typedAttribute.DefaultFormValue.HasValue ? typedAttribute.DefaultFormValue.Value.ToString(CultureInfo.InvariantCulture) : "";
                                                foreach (var option in typedAttribute.OptionSet.Options)
                                                {
                                                    Options += string.Format("\r\n{0}: {1}", option.Value, option.Label.UserLocalizedLabel.Label);
                                                }
                                            }
                                            else if (currentAttribute.AttributeType.Value == AttributeTypeCode.State)
                                            {
                                                var typedAttribute = (StateAttributeMetadata)currentAttribute;
                                                foreach (var option in typedAttribute.OptionSet.Options)
                                                {
                                                    Options += string.Format("\r\n{0}: {1}", option.Value, option.Label.UserLocalizedLabel.Label);
                                                }
                                            }
                                            else if (currentAttribute.AttributeType.Value == AttributeTypeCode.Status)
                                            {
                                                var typedAttribute = (StatusAttributeMetadata)currentAttribute;
                                                foreach (OptionMetadata option in typedAttribute.OptionSet.Options)
                                                {
                                                    Options += string.Format("\r\n{0}: {1}", option.Value, option.Label.UserLocalizedLabel.Label);
                                                }
                                            }
                                        }

                                        if (treeViewExport.Nodes["NodeExportAttributesMetaData"].Nodes["NodeAttributeMinimumValue"].Checked)
                                            metadataWriter.WriteElementString("AttributeMinimumValue", MinimumValue);
                                        if (treeViewExport.Nodes["NodeExportAttributesMetaData"].Nodes["NodeAttributeMaximumValue"].Checked)
                                            metadataWriter.WriteElementString("AttributeMaximumValue", MaximumValue);
                                        if (treeViewExport.Nodes["NodeExportAttributesMetaData"].Nodes["NodeAttributeDefaultValue"].Checked)
                                            metadataWriter.WriteElementString("AttributeDefaultValue", DefaultValue);
                                        if (treeViewExport.Nodes["NodeExportAttributesMetaData"].Nodes["NodeAttributePrecision"].Checked)
                                            metadataWriter.WriteElementString("AttributePrecision", Precision);
                                        if (treeViewExport.Nodes["NodeExportAttributesMetaData"].Nodes["NodeAttributeFormat"].Checked)
                                            metadataWriter.WriteElementString("AttributeFormat", Format);
                                        if (treeViewExport.Nodes["NodeExportAttributesMetaData"].Nodes["NodeAttributeMaxLength"].Checked)
                                            metadataWriter.WriteElementString("AttributeMaxLength", MaxLength);
                                        if (treeViewExport.Nodes["NodeExportAttributesMetaData"].Nodes["NodeGlobalOptionSet"].Checked)
                                            metadataWriter.WriteElementString("GlobalOptionSet", GlobalOptionSet);
                                        if (treeViewExport.Nodes["NodeExportAttributesMetaData"].Nodes["NodeAttributeOptions"].Checked)
                                            metadataWriter.WriteElementString("AttributeOptions", Options);

                                        // End Attribute Node
                                        metadataWriter.WriteEndElement();
                                    }
                                }
                                // End Attributes Node
                                metadataWriter.WriteEndElement();
                            }

                            #endregion Attributes

                            // End Entity Node
                            metadataWriter.WriteEndElement();
                        }
                    }

                    // End Metadata Xml Node
                    metadataWriter.WriteEndElement();
                    metadataWriter.WriteEndDocument();

                    // Close xml writer.
                    metadataWriter.Close();
                }

                //Propose the file for saving
                XmlDocument doc = new XmlDocument();
                doc.Load(tempExportedFilePath);
                SaveFileDialog saveXMLDialog = new SaveFileDialog();
                saveXMLDialog.Filter = "XML Files | *.xml";
                saveXMLDialog.DefaultExt = "xml";
                saveXMLDialog.FileName = comboBoxSource.SelectedItem.ToString();
                DialogResult savRes = saveXMLDialog.ShowDialog();
                if (savRes == DialogResult.OK)
                {
                    doc.Save(saveXMLDialog.FileName);

                    exportedFilePath = saveXMLDialog.FileName;

                    //Delete temporary file
                    File.Delete(tempExportedFilePath);
                    //Display Open in Excel Button
                    buttonOpenInExcel.Text = "Open in Excel";
                    buttonOpenInExcel.Visible = true;
                    toolStripStatusLabel1.Text = "Structure exported.";
                }
                else
                {
                    //Delete temporary file
                    File.Delete(tempExportedFilePath);
                    buttonOpenInExcel.Visible = false;
                    toolStripStatusLabel1.Text = "Export cancelled.";
                }
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                toolStripStatusLabel1.Text = "Error.";
                MessageBox.Show("Error:" + ex.Detail.Message + "\n" + ex.Detail.TraceText, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = "Error.";
                if (ex.InnerException != null)
                    MessageBox.Show("Error:" + ex.Message + "\n" + ex.InnerException.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("Error:" + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            setStateAllControls(true);
        }

        private void buttonOpenInExcel_Click(object sender, EventArgs e)
        {
            // These are the Win32 error code for file not found or access denied.
            const int ERROR_FILE_NOT_FOUND = 2;
            const int ERROR_ACCESS_DENIED = 5;
            string programName = "";
            ProcessStartInfo startInfo = new ProcessStartInfo();
            
            try
            {
                if (exportedFilePath.EndsWith("vsd"))
                {
                    startInfo.FileName = "VISIO.EXE";
                    programName = "Excel";
                }
                else
                {
                    startInfo.FileName = "EXCEL.EXE";
                    programName = "Visio";
                }

                startInfo.Arguments = exportedFilePath;
                Process.Start(startInfo);
            }
            catch (Win32Exception ex)
            {
                if (ex.NativeErrorCode == ERROR_FILE_NOT_FOUND)
                {
                    MessageBox.Show(ex.Message + ". You may not have " + programName + " installed on this computer. Try opening this file on a computer with " + programName + " installed");
                }
                else if (ex.NativeErrorCode == ERROR_ACCESS_DENIED)
                {
                    MessageBox.Show(ex.Message + ". You do not have permission to access this file.");
                }
            }


        }

        private void comboBoxFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxFilter.SelectedIndex == 0 || comboBoxFilter.SelectedIndex == 1)
            {
                bool checkAll = (comboBoxFilter.SelectedIndex == 0);
                for (int i = 0; i < checkedListBoxEntities.Items.Count; i++)
                {
                    checkedListBoxEntities.SetItemChecked(i, checkAll);
                }
            }
            else if (comboBoxFilter.SelectedIndex == 2 || comboBoxFilter.SelectedIndex == 3)
            {
                List<EntityMetadata> EMD = environmentStructure.EntityMetadata.ToList<EntityMetadata>();
                bool checkEntity = (comboBoxFilter.SelectedIndex == 2);
                for (int i = 0; i < checkedListBoxEntities.Items.Count; i++)
                {
                    EntityMetadata emdata = EMD.Find(em => em.LogicalName == checkedListBoxEntities.Items[i].ToString());
                    if (emdata.IsCustomEntity.Value)
                        checkedListBoxEntities.SetItemChecked(i, !checkEntity);
                    else
                        checkedListBoxEntities.SetItemChecked(i, checkEntity);
                }
            }
        }

        // Updates all child tree nodes recursively.
        private void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
        {
            foreach (TreeNode node in treeNode.Nodes)
            {
                node.Checked = nodeChecked;
                if (node.Nodes.Count > 0)
                {
                    // If the current node has child nodes, call the CheckAllChildsNodes method recursively.
                    this.CheckAllChildNodes(node, nodeChecked);
                }
            }
        }

        // NOTE   This code can be added to the BeforeCheck event handler instead of the AfterCheck event.
        // After a tree node's Checked property is changed, all its child nodes are updated to the same value.
        private void treeViewExport_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // The code only executes if the user caused the checked state to change.
            if (e.Action != TreeViewAction.Unknown)
            {
                if (e.Node.Nodes.Count > 0)
                {
                    /* Calls the CheckAllChildNodes method, passing in the current
                    Checked value of the TreeNode whose checked state changed. */
                    this.CheckAllChildNodes(e.Node, e.Node.Checked);
                }

                if (e.Node.Parent != null && e.Node.Parent.Nodes.Count > 0)
                {
                    bool uncheckParent = false;
                    foreach (TreeNode tn in e.Node.Parent.Nodes)
                    {
                        if (tn.Checked)
                            uncheckParent = true;
                    }
                    e.Node.Parent.Checked = uncheckParent;
                }
            }
        }

        private void buttonExportRelationships_Click(object sender, EventArgs e)
        {
            if (environmentStructure == null)
            {
                MessageBox.Show("You must first load the Entities Structure before Exporting!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (checkedListBoxEntities.CheckedItems.Count == 0)
            {
                MessageBox.Show("You must check at least 1 Entity for export!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            setStateAllControls(false);
            try
            {
                //Set the Exe path as current path
                String workingDirectory = Path.GetDirectoryName(Assembly.GetAssembly(typeof(ReferenceDataTransporter)).Location);
                Directory.SetCurrentDirectory(workingDirectory);
                //Make sure the EntityExport folder exists
                if (!Directory.Exists(EntitesStructureFolderName))
                    Directory.CreateDirectory(EntitesStructureFolderName);

                DateTime now = DateTime.Now;
                tempExportedFilePath = String.Concat(EntitesStructureFolderName + "\\" + comboBoxSource.SelectedItem + "_" + now.Year + "-" + now.Month + "-" + now.Day + "-" + now.Hour + "-" + now.Minute + "-" + now.Second + ".xml");

                MemoryStream ms = new MemoryStream();

                using (StreamWriter sw = new StreamWriter(tempExportedFilePath))
                {
                    // Create Xml Writer.
                    XmlTextWriter metadataWriter = new XmlTextWriter(sw);

                    // Start Xml File.
                    metadataWriter.WriteStartDocument();

                    // <?mso-application progid="Excel.Sheet"?>
                    metadataWriter.WriteProcessingInstruction("mso-application", "progid=\"Excel.Sheet\"");

                    // Metadata Xml Node.
                    metadataWriter.WriteStartElement("Metadata");

                    IOrderedEnumerable<EntityMetadata> EMD = environmentStructure.EntityMetadata.OrderBy(em => em.LogicalName);
                    List<string> TreatedRelationships = new List<string>();
                    foreach (EntityMetadata currentEntity in EMD)
                    {
                        if ((currentEntity.IsIntersect.Value == false) && (checkedListBoxEntities.CheckedItems.Contains(currentEntity.LogicalName)))
                        {
                            List<OneToManyRelationshipMetadata> OneToManyRelationships = currentEntity.OneToManyRelationships.ToList();
                            foreach (OneToManyRelationshipMetadata relationship in OneToManyRelationships)
                            {
                                // Start Entity Node
                                metadataWriter.WriteStartElement("Relationship");
                                metadataWriter.WriteElementString("RelationshipType", "1*N");
                                metadataWriter.WriteElementString("RelationshipSchemaName", relationship.SchemaName);

                                metadataWriter.WriteElementString("ReferencedAttribute", relationship.ReferencedAttribute);
                                metadataWriter.WriteElementString("ReferencedEntity", relationship.ReferencedEntity);
                                metadataWriter.WriteElementString("ReferencingAttribute", relationship.ReferencingAttribute);
                                metadataWriter.WriteElementString("ReferencingEntity", relationship.ReferencingEntity);

                                metadataWriter.WriteElementString("IntersectEntityName", "");
                                metadataWriter.WriteElementString("Entity1LogicalName", "");
                                metadataWriter.WriteElementString("Entity1IntersectAttribute", "");
                                metadataWriter.WriteElementString("Entity2LogicalName", "");
                                metadataWriter.WriteElementString("Entity2IntersectAttribute", "");

                                metadataWriter.WriteElementString("RelationshipIsCustomizable", relationship.IsCustomizable.Value.ToString());
                                metadataWriter.WriteElementString("RelationshipIsCustomRelationship", relationship.IsCustomRelationship.Value.ToString());
                                metadataWriter.WriteElementString("RelationshipIsValidForAdvancedFind", relationship.IsValidForAdvancedFind.Value.ToString());

                                // End Entity Node
                                metadataWriter.WriteEndElement();
                            }

                            List<ManyToManyRelationshipMetadata> ManyToManyRelationships = currentEntity.ManyToManyRelationships.ToList();
                            foreach (ManyToManyRelationshipMetadata relationship in ManyToManyRelationships)
                            {
                                if (TreatedRelationships.Find(r => r == relationship.SchemaName) != null)
                                    continue;
                                // Start Entity Node
                                metadataWriter.WriteStartElement("Relationship");
                                metadataWriter.WriteElementString("RelationshipType", "N*N");
                                metadataWriter.WriteElementString("RelationshipSchemaName", relationship.SchemaName);

                                metadataWriter.WriteElementString("ReferencedAttribute", "");
                                metadataWriter.WriteElementString("ReferencedEntity", "");
                                metadataWriter.WriteElementString("ReferencingAttribute", "");
                                metadataWriter.WriteElementString("ReferencingEntity", "");

                                metadataWriter.WriteElementString("IntersectEntityName", relationship.IntersectEntityName);
                                metadataWriter.WriteElementString("Entity1LogicalName", relationship.Entity1LogicalName);
                                metadataWriter.WriteElementString("Entity1IntersectAttribute", relationship.Entity1IntersectAttribute);
                                metadataWriter.WriteElementString("Entity2LogicalName", relationship.Entity2LogicalName);
                                metadataWriter.WriteElementString("Entity2IntersectAttribute", relationship.Entity2IntersectAttribute);

                                metadataWriter.WriteElementString("RelationshipIsCustomizable", relationship.IsCustomizable.Value.ToString());
                                metadataWriter.WriteElementString("RelationshipIsCustomRelationship", relationship.IsCustomRelationship.Value.ToString());
                                metadataWriter.WriteElementString("RelationshipIsValidForAdvancedFind", relationship.IsValidForAdvancedFind.Value.ToString());

                                // End Entity Node
                                metadataWriter.WriteEndElement();
                                TreatedRelationships.Add(relationship.SchemaName);
                            }
                        }
                    }

                    // End Metadata Xml Node
                    metadataWriter.WriteEndElement();
                    metadataWriter.WriteEndDocument();

                    // Close xml writer.
                    metadataWriter.Close();
                }

                //Propose the file for saving
                XmlDocument doc = new XmlDocument();
                doc.Load(tempExportedFilePath);
                SaveFileDialog saveXMLDialog = new SaveFileDialog();
                saveXMLDialog.Filter = "XML Files | *.xml";
                saveXMLDialog.DefaultExt = "xml";
                saveXMLDialog.FileName = comboBoxSource.SelectedItem.ToString() + "_Relationships";
                DialogResult savRes = saveXMLDialog.ShowDialog();
                if (savRes == DialogResult.OK)
                {
                    doc.Save(saveXMLDialog.FileName);

                    exportedFilePath = saveXMLDialog.FileName;

                    //Delete temporary file
                    File.Delete(tempExportedFilePath);
                    //Display Open in Excel Button
                    buttonOpenInExcel.Text = "Open in Excel";
                    buttonOpenInExcel.Visible = true;
                    toolStripStatusLabel1.Text = "Relationships exported.";
                }
                else
                {
                    //Delete temporary file
                    File.Delete(tempExportedFilePath);
                    buttonOpenInExcel.Visible = false;
                    toolStripStatusLabel1.Text = "Export cancelled.";
                }
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                toolStripStatusLabel1.Text = "Error.";
                MessageBox.Show("Error:" + ex.Detail.Message + "\n" + ex.Detail.TraceText, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = "Error.";
                if (ex.InnerException != null)
                    MessageBox.Show("Error:" + ex.Message + "\n" + ex.InnerException.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("Error:" + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            setStateAllControls(true);
        }

        private void buttonGenerateDiagrams_Click(object sender, EventArgs e)
        {
            if (environmentStructure == null)
            {
                MessageBox.Show("You must first load the Entities Structure before diagram generation!");
                return;
            }

            if (checkedListBoxEntities.CheckedItems.Count == 0)
            {
                MessageBox.Show("You must check at least 1 Entity for diagram generation!");
                return;
            }

            //If the bg worker is running stop it
            if (bwDiagramsGenerator.IsBusy)
            {
                bwDiagramsGenerator.CancelAsync();
                toolStripStatusLabel1.Text = "Stopping Diagram generation. Please wait...";
                return;
            }

            setStateAllControls(false, new List<string> { buttonGenerateDiagrams.Name });
            DiagramGeneratedSuccesfully = false;
            buttonGenerateDiagrams.Text = "Stop generation";
            toolStripStatusLabel1.Text = "Exporting entites diagrams. Please wait...";
            DateTime now = DateTime.Now;
            tempExportedFilePath = "EntitesStructure\\Diagrams.vsd";
            foreach (string ci in checkedListBoxEntities.CheckedItems)
            {
                DGentities.Add(ci);
            }
            DGconnectionName = comboBoxSource.SelectedItem.ToString();
            this.toolStripProgressBar1.Visible = true;

            selectedDiagramsEntityLabelIndex = comboBoxDiagramEntitiesLabels.SelectedIndex;
            selectedDiagramsAttributeLabelIndex = comboBoxDiagramAttributesLabels.SelectedIndex;
            showForeignKeys = checkBoxShowForeignKeys.Checked;
            showPrimaryKeys = checkBoxShowPrimaryKeys.Checked;
            showRelationshipsNames = checkBoxShowRelationshipsNames.Checked;
            showOwnership = checkBoxDiagramShowOwnership.Checked;

            try
            {
                if (bwDiagramsGenerator.IsBusy != true)
                    bwDiagramsGenerator.RunWorkerAsync();
                else
                    bwDiagramsGenerator.CancelAsync();
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                toolStripStatusLabel1.Text = "Error.";
                MessageBox.Show("Error:" + ex.Detail.Message + "\n" + ex.Detail.TraceText, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = "Error.";
                if (ex.InnerException != null)
                    MessageBox.Show("Error:" + ex.Message + "\n" + ex.InnerException.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    //Personalise error message if Visio not present on the machine
                    if (ex.Message != null && ex.Message.Contains("REGDB_E_CLASSNOTREG"))
                    {
                        string errorMessage = "Error:" + ex.Message + "\n\n" + "You're probably seen this message because Visio 2010 is not found on this machine. \nYou have to install Visio 2010 before generating diagrams!";
                        MessageBox.Show(errorMessage, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                        MessageBox.Show("Error:" + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void bwGenerateGiagrams_DoWork(object sender, DoWorkEventArgs e)
        {
            //transportStopped = false;
            BackgroundWorker worker = sender as BackgroundWorker;          

            //Generate Diagrams
            DiagramBuilder db = new DiagramBuilder();
            try
            {
                DiagramBuildingProperties dbp = new DiagramBuildingProperties();
                dbp.connectionName = DGconnectionName;
                dbp.entities = DGentities;
                dbp.environmentStructure = environmentStructure;
                dbp.entityLabelDisplay = selectedDiagramsEntityLabelIndex;
                dbp.attributeLabelDisplay = selectedDiagramsAttributeLabelIndex;
                dbp.showForeignKeys = showForeignKeys;
                dbp.showPrimaryKeys = showPrimaryKeys;
                dbp.showRelationshipsNames = showRelationshipsNames;
                dbp.showOwnership = showOwnership;

                string filename = db.GenerateDiagram(dbp, worker, e);
                DiagramGeneratedSuccesfully = true;
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                DiagramGeneratedSuccesfully = false;
                toolStripStatusLabel1.Text = "Error.";
                MessageBox.Show("Error:" + ex.Detail.Message + "\n" + ex.Detail.TraceText, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                DiagramGeneratedSuccesfully = false;
                toolStripStatusLabel1.Text = "Error.";
                if (ex.InnerException != null)
                    MessageBox.Show("Error:" + ex.Message + "\n" + ex.InnerException.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    //Personalise error message if Visio not present on the machine
                    if (ex.Message != null && ex.Message.Contains("REGDB_E_CLASSNOTREG"))
                    {
                        string errorMessage = "Error:" + ex.Message + "\n\n" + "You're probably seen this message because Visio 2010 is not found on this machine. \nYou have to install Visio 2010 before generating diagrams!";
                        MessageBox.Show(errorMessage, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                        MessageBox.Show("Error:" + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void bwGenerateGiagrams_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int percentage = e.ProgressPercentage * 100 / DGentities.Count;
            this.toolStripProgressBar1.Value = percentage;
            toolStripStatusLabel1.Text = "Generating diagram for entity: " + DGentities[e.ProgressPercentage - 1];
        }

        private void bwGenerateGiagrams_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.toolStripProgressBar1.Visible = false;
            buttonGenerateDiagrams.Text = "Generate Diagrams";
            DGentities = new List<string>();
            DGconnectionName = "";
            if (!DiagramGeneratedSuccesfully)
            {
                bwDiagramsGenerator.CancelAsync();
                toolStripStatusLabel1.Text = "Error...";
                buttonOpenInExcel.Visible = false;
            }
            else if (e.Error != null)
            {
                bwDiagramsGenerator.CancelAsync();
                MessageBox.Show("Error ! Detail : " + e.Error.Message);
                toolStripStatusLabel1.Text = "Error...";
                buttonOpenInExcel.Visible = false;
            }
            else if (e.Cancelled)
            {
                bwDiagramsGenerator.CancelAsync();
                toolStripStatusLabel1.Text = "Diagram Generation cancelled by user.";
                buttonOpenInExcel.Visible = false;
            }
            else
            {
                //Propose the file for saving
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Visio Files | *.vsd";
                saveDialog.DefaultExt = "vsd";
                saveDialog.FileName = comboBoxSource.SelectedItem.ToString() + "_Diagrams";
                DialogResult savRes = saveDialog.ShowDialog();
                if (savRes == DialogResult.OK)
                {
                    if (File.Exists(saveDialog.FileName))
                        File.Delete(saveDialog.FileName);
                    File.Move("EntitesStructure\\Diagrams.vsd", saveDialog.FileName);
                    //Display Open in Excel Button
                    exportedFilePath = saveDialog.FileName;
                    buttonOpenInExcel.Text = "Open in Visio";
                    buttonOpenInExcel.Visible = true;
                    toolStripStatusLabel1.Text = "Diagram exported.";
                }
                else
                {
                    if (File.Exists(saveDialog.FileName))
                        File.Delete(saveDialog.FileName);
                    buttonOpenInExcel.Visible = false;
                    toolStripStatusLabel1.Text = "Export cancelled.";
                }
            }
            setStateAllControls(true);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void viewLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogManager.OpenLogFile();
        }

        private void logArchiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogManager.OpenLogFolder();
        }

        private void buttonFieldsOnForms_Click(object sender, EventArgs e)
        {
            if (environmentStructure == null)
            {
                MessageBox.Show("You must first load the Entities Structure before Exporting!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (checkedListBoxEntities.CheckedItems.Count != 1)
            {
                MessageBox.Show("You must check exactly 1 Entity for export!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            toolStripStatusLabel1.Text = "Exporting Fields on Forms. Please wait...";
            Application.DoEvents();

            setStateAllControls(false);
            try
            {
                //Set the Exe path as current path
                String workingDirectory = Path.GetDirectoryName(Assembly.GetAssembly(typeof(ReferenceDataTransporter)).Location);
                Directory.SetCurrentDirectory(workingDirectory);
                //Make sure the EntityExport folder exists
                if (!Directory.Exists(EntitesStructureFolderName))
                    Directory.CreateDirectory(EntitesStructureFolderName);

                DateTime now = DateTime.Now;
                tempExportedFilePath = String.Concat(EntitesStructureFolderName + "\\" + comboBoxSource.SelectedItem + "_" + now.Year + "-" + now.Month + "-" + now.Day + "-" + now.Hour + "-" + now.Minute + "-" + now.Second + ".xml");

                MemoryStream ms = new MemoryStream();

                //Connect to the CRM
                MSCRMConnection connection = cm.MSCRMConnections[comboBoxSource.SelectedIndex];
                if (_serviceProxy == null)
                    _serviceProxy = cm.connect(connection);

                using (StreamWriter sw = new StreamWriter(tempExportedFilePath))
                {
                    // Create Xml Writer.
                    XmlTextWriter metadataWriter = new XmlTextWriter(sw);

                    // Start Xml File.
                    metadataWriter.WriteStartDocument();

                    // <?mso-application progid="Excel.Sheet"?>
                    metadataWriter.WriteProcessingInstruction("mso-application", "progid=\"Excel.Sheet\"");

                    // Metadata Xml Node.
                    metadataWriter.WriteStartElement("Metadata");

                    IOrderedEnumerable<EntityMetadata> EMD = environmentStructure.EntityMetadata.OrderBy(em => em.LogicalName);
                    List<string> TreatedRelationships = new List<string>();
                    foreach (EntityMetadata currentEntity in EMD)
                    {
                        if ((currentEntity.IsIntersect.Value == false) && (checkedListBoxEntities.CheckedItems.Contains(currentEntity.LogicalName)))
                        {
                            //Retrieve Entity FormsXML
                            QueryByAttribute query = new QueryByAttribute("systemform");
                            query.ColumnSet = new ColumnSet("formxml", "objecttypecode", "name", "type");
                            query.Attributes.AddRange("objecttypecode", "type");
                            query.Values.AddRange(currentEntity.LogicalName, 2);
                            EntityCollection formsCollection = _serviceProxy.RetrieveMultiple(query);

                            List<FormAndFields> fieldsAndForms = new List<FormAndFields>();
                            foreach (var form in formsCollection.Entities)
                            {
                                IEnumerable<AttributeMetadata> attributes = new List<AttributeMetadata>();
                                if (((OptionSetValue)form["type"]).Value != 2)
                                    continue;
                                XmlDocument FormXML = new XmlDocument();
                                FormXML.LoadXml(form["formxml"].ToString());

                                attributes = currentEntity.Attributes.Where(attr => FormXML.SelectSingleNode("//control[@datafieldname='" + attr.LogicalName + "']") != null);

                                fieldsAndForms.Add(new FormAndFields() { formName = XmlConvert.EncodeName(form["name"].ToString()), fields = attributes.ToList() });
                            }

                            // Start Entity Node
                            metadataWriter.WriteStartElement("Entity");
                            metadataWriter.WriteElementString("EntityName", currentEntity.LogicalName);

                            // Start Attributes Node
                            metadataWriter.WriteStartElement("Attributes");
                            IOrderedEnumerable<AttributeMetadata> AD = currentEntity.Attributes.OrderBy(a => a.LogicalName);
                            foreach (AttributeMetadata attribute in AD)
                            {
                                //Write only main attributes
                                if (attribute.AttributeOf != null)
                                    continue;

                                // Start Attribute Node
                                metadataWriter.WriteStartElement("Attribute");
                                metadataWriter.WriteElementString("AttributeName", attribute.LogicalName);
                                metadataWriter.WriteStartElement("Forms");

                                foreach (FormAndFields faf in fieldsAndForms)
                                {
                                    string onForm;
                                    if (faf.fields.Find(a => a.LogicalName == attribute.LogicalName) == null)
                                        onForm = "false";
                                    else
                                        onForm = "true";

                                    metadataWriter.WriteElementString(faf.formName, onForm);
                                }

                                // End Forms Node
                                metadataWriter.WriteEndElement();
                                // End Attribute Node
                                metadataWriter.WriteEndElement();
                            }

                            // End Attributes Node
                            metadataWriter.WriteEndElement();

                            // End Entity Node
                            metadataWriter.WriteEndElement();
                        }
                    }

                    // End Metadata Xml Node
                    metadataWriter.WriteEndElement();
                    metadataWriter.WriteEndDocument();

                    // Close xml writer.
                    metadataWriter.Close();
                }

                //Propose the file for saving
                XmlDocument doc = new XmlDocument();
                doc.Load(tempExportedFilePath);
                SaveFileDialog saveXMLDialog = new SaveFileDialog();
                saveXMLDialog.Filter = "XML Files | *.xml";
                saveXMLDialog.DefaultExt = "xml";
                saveXMLDialog.FileName = comboBoxSource.SelectedItem.ToString() + "_FieldsOnForms";
                DialogResult savRes = saveXMLDialog.ShowDialog();
                if (savRes == DialogResult.OK)
                {
                    doc.Save(saveXMLDialog.FileName);

                    exportedFilePath = saveXMLDialog.FileName;

                    //Delete temporary file
                    File.Delete(tempExportedFilePath);
                    //Display Open in Excel Button
                    buttonOpenInExcel.Text = "Open in Excel";
                    buttonOpenInExcel.Visible = true;
                    toolStripStatusLabel1.Text = "Fields on Forms exported.";
                }
                else
                {
                    //Delete temporary file
                    File.Delete(tempExportedFilePath);
                    buttonOpenInExcel.Visible = false;
                    toolStripStatusLabel1.Text = "Export cancelled.";
                }
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                toolStripStatusLabel1.Text = "Error.";
                MessageBox.Show("Error:" + ex.Detail.Message + "\n" + ex.Detail.TraceText, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = "Error.";
                if (ex.InnerException != null)
                    MessageBox.Show("Error:" + ex.Message + "\n" + ex.InnerException.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("Error:" + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            setStateAllControls(true);
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "IEXPLORE.EXE";
            startInfo.Arguments = "https://github.com/zoranivanov/MSCRMToolKit/wiki#exportentitesstructure";
            Process.Start(startInfo);
        }

        private string CreateXml(string xml, string cookie, int page, int count)
        {
            StringReader stringReader = new StringReader(xml);
            XmlTextReader reader = new XmlTextReader(stringReader);

            // Load document
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);

            XmlAttributeCollection attrs = doc.DocumentElement.Attributes;

            if (cookie != null)
            {
                XmlAttribute pagingAttr = doc.CreateAttribute("paging-cookie");
                pagingAttr.Value = cookie;
                attrs.Append(pagingAttr);
            }

            XmlAttribute pageAttr = doc.CreateAttribute("page");
            pageAttr.Value = System.Convert.ToString(page);
            attrs.Append(pageAttr);

            XmlAttribute countAttr = doc.CreateAttribute("count");
            countAttr.Value = System.Convert.ToString(count);
            attrs.Append(countAttr);

            StringBuilder sb = new StringBuilder(1024);
            StringWriter stringWriter = new StringWriter(sb);

            XmlTextWriter writer = new XmlTextWriter(stringWriter);
            doc.WriteTo(writer);
            writer.Close();

            return sb.ToString();
        }

        private void buttonSolutionComponents_Click(object sender, EventArgs e)
        {
            if (environmentStructure == null)
            {
                MessageBox.Show("You must first load the Entities Structure before Exporting!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            setStateAllControls(false);
            try
            {
                //Set the Exe path as current path
                String workingDirectory = Path.GetDirectoryName(Assembly.GetAssembly(typeof(ReferenceDataTransporter)).Location);
                Directory.SetCurrentDirectory(workingDirectory);
                //Make sure the EntityExport folder exists
                if (!Directory.Exists(EntitesStructureFolderName))
                    Directory.CreateDirectory(EntitesStructureFolderName);

                DateTime now = DateTime.Now;
                tempExportedFilePath = String.Concat(EntitesStructureFolderName + "\\" + comboBoxSource.SelectedItem + "_" + now.Year + "-" + now.Month + "-" + now.Day + "-" + now.Hour + "-" + now.Minute + "-" + now.Second + ".xml");

                MemoryStream ms = new MemoryStream();

                using (StreamWriter sw = new StreamWriter(tempExportedFilePath))
                {
                    // Create Xml Writer.
                    XmlTextWriter metadataWriter = new XmlTextWriter(sw);

                    // Start Xml File.
                    metadataWriter.WriteStartDocument();
                    // <?mso-application progid="Excel.Sheet"?>
                    metadataWriter.WriteProcessingInstruction("mso-application", "progid=\"Excel.Sheet\"");
                    // Metadata Xml Node.
                    metadataWriter.WriteStartElement("Metadata");

                    List<Entity> scList = getSolutionComponentsList();

                    foreach (Entity currentEntity in scList)
                    {                            
                        // Start Entity Node
                        metadataWriter.WriteStartElement("Entity");
                        metadataWriter.WriteElementString("solutioncomponentid", currentEntity["solutioncomponentid"].ToString());
                        if (currentEntity.Contains("rootcomponentbehavior") && currentEntity["rootcomponentbehavior"] != null)
                            metadataWriter.WriteElementString("rootcomponentbehavior", getFormattedValue(currentEntity, "rootcomponentbehavior"));
                        if (currentEntity.Contains("objectid") && currentEntity["objectid"] != null)
                            metadataWriter.WriteElementString("objectid", currentEntity["objectid"].ToString());
                        if (currentEntity.Contains("createdon") && currentEntity["createdon"] != null)
                            metadataWriter.WriteElementString("createdon", currentEntity["createdon"].ToString());
                        if (currentEntity.Contains("rootsolutioncomponentid") && currentEntity["rootsolutioncomponentid"] != null)
                            metadataWriter.WriteElementString("rootsolutioncomponentid", currentEntity["rootsolutioncomponentid"].ToString());
                        if (currentEntity.Contains("modifiedon") && currentEntity["modifiedon"] != null)
                            metadataWriter.WriteElementString("modifiedon", currentEntity["modifiedon"].ToString());
                        if (currentEntity.Contains("solutionid") && currentEntity["solutionid"] != null)
                            metadataWriter.WriteElementString("solutionid", ((EntityReference)currentEntity["solutionid"]).Id.ToString());
                        if (currentEntity.Contains("ismetadata") && currentEntity["ismetadata"] != null)
                            metadataWriter.WriteElementString("ismetadata", currentEntity["ismetadata"].ToString());
                        if (currentEntity.Contains("componenttype") && currentEntity["componenttype"] != null)
                            metadataWriter.WriteElementString("componenttype", getFormattedValue(currentEntity, "componenttype"));
                        if (currentEntity.Contains("a_createdby.fullname") && currentEntity["a_createdby.fullname"] != null)
                            metadataWriter.WriteElementString("createdby_fullname", ((AliasedValue)currentEntity["a_createdby.fullname"]).Value.ToString());
                        if (currentEntity.Contains("a_modifiedby.fullname") && currentEntity["a_modifiedby.fullname"] != null)
                            metadataWriter.WriteElementString("modifiedby_fullname", ((AliasedValue)currentEntity["a_modifiedby.fullname"]).Value.ToString());
                        if (currentEntity.Contains("a_uniquename.uniquename") && currentEntity["a_uniquename.uniquename"] != null)
                            metadataWriter.WriteElementString("solution_uniquename", ((AliasedValue)currentEntity["a_uniquename.uniquename"]).Value.ToString());
                        if (currentEntity.Contains("a_friendlyname.friendlyname") && currentEntity["a_friendlyname.friendlyname"] != null)
                            metadataWriter.WriteElementString("solution_friendlyname", ((AliasedValue)currentEntity["a_friendlyname.friendlyname"]).Value.ToString());
                        if (currentEntity.Contains("version") && currentEntity["a_version.version"] != null)
                            metadataWriter.WriteElementString("solution_version", ((AliasedValue)currentEntity["a_version.version"]).Value.ToString());
                        if (currentEntity.Contains("EntityLogicalName") && currentEntity["EntityLogicalName"] != null)
                            metadataWriter.WriteElementString("EntityLogicalName", (string)currentEntity["EntityLogicalName"]);

                        // End Entity Node
                        metadataWriter.WriteEndElement();                           
                    }

                    // End Metadata Xml Node
                    metadataWriter.WriteEndElement();
                    metadataWriter.WriteEndDocument();

                    // Close xml writer.
                    metadataWriter.Close();
                }

                //Propose the file for saving
                XmlDocument doc = new XmlDocument();
                doc.Load(tempExportedFilePath);
                SaveFileDialog saveXMLDialog = new SaveFileDialog();
                saveXMLDialog.Filter = "XML Files | *.xml";
                saveXMLDialog.DefaultExt = "xml";
                saveXMLDialog.FileName = comboBoxSource.SelectedItem.ToString() + "_SolutionComponents";
                DialogResult savRes = saveXMLDialog.ShowDialog();
                if (savRes == DialogResult.OK)
                {
                    doc.Save(saveXMLDialog.FileName);

                    exportedFilePath = saveXMLDialog.FileName;

                    //Delete temporary file
                    File.Delete(tempExportedFilePath);
                    //Display Open in Excel Button
                    buttonOpenInExcel.Text = "Open in Excel";
                    buttonOpenInExcel.Visible = true;
                    toolStripStatusLabel1.Text = "Solution Components exported.";
                }
                else
                {
                    //Delete temporary file
                    File.Delete(tempExportedFilePath);
                    buttonOpenInExcel.Visible = false;
                    toolStripStatusLabel1.Text = "Export cancelled.";
                }
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                toolStripStatusLabel1.Text = "Error.";
                MessageBox.Show("Error:" + ex.Detail.Message + "\n" + ex.Detail.TraceText, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = "Error.";
                if (ex.InnerException != null)
                    MessageBox.Show("Error:" + ex.Message + "\n" + ex.InnerException.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("Error:" + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            setStateAllControls(true);
        }

        /// <summary>
        /// Gets the formatted value.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <returns></returns>
        private string getFormattedValue(Entity entity, string attributeName)
        {
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
                    if (er.Name != null)
                        value = er.Name;
                }
                else if (t.Name == "AliasedValue")
                {
                    AliasedValue av = (AliasedValue)kvp.Value;
                    Type t2 = av.Value.GetType();
                    if (t2.Name == "EntityReference")
                    {
                        EntityReference er2 = (EntityReference)av.Value;
                        if (er2.Name != null)
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

        private List<Entity> getSolutionComponentsList()
        {
            List<Entity> solutionComponentsList = new List<Entity>();
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
	                                <entity name='solutioncomponent'>
		                                <attribute name='solutioncomponentid' />
		                                <attribute name='rootcomponentbehavior' />
		                                <attribute name='objectid' />
		                                <attribute name='createdon' />
		                                <attribute name='rootsolutioncomponentid' />
		                                <attribute name='modifiedon' />
		                                <attribute name='solutionid' />
		                                <attribute name='ismetadata' />
		                                <attribute name='componenttype' />
		                                <order attribute='createdon' descending='true' />
		                                <link-entity name='systemuser' from='systemuserid' to='createdby' visible='false' link-type='outer' alias='a_createdby'>
			                                <attribute name='fullname' />
		                                </link-entity>
		                                <link-entity name='systemuser' from='systemuserid' to='modifiedby' visible='false' link-type='outer' alias='a_modifiedby'>
			                                <attribute name='fullname' />
		                                </link-entity>
		                                <link-entity name='solution' from='solutionid' to='solutionid' visible='false' link-type='outer' alias='a_uniquename'>
			                                <attribute name='uniquename' />
		                                </link-entity>
		                                <link-entity name='solution' from='solutionid' to='solutionid' visible='false' link-type='outer' alias='a_friendlyname'>
			                                <attribute name='friendlyname' />
		                                </link-entity>
		                                <link-entity name='solution' from='solutionid' to='solutionid' visible='false' link-type='outer' alias='a_version'>
			                                <attribute name='version' />
		                                </link-entity>
	                                </entity>
                                </fetch>";

            string pagingCookie = null;
            int fileNumber = 1;
            int fetchCount = 5000;
            int recordsCount = 0;

            //Connect to the CRM
            MSCRMConnection connection = cm.MSCRMConnections[comboBoxSource.SelectedIndex];
            if (_serviceProxy == null)
                _serviceProxy = cm.connect(connection);

            while (true)
            {
                // Build fetchXml string with the placeholders.
                string xml = CreateXml(fetchXml, pagingCookie, fileNumber, fetchCount);

                // Execute the fetch query and get the xml result.
                RetrieveMultipleRequest fetchRequest = new RetrieveMultipleRequest
                {
                    Query = new FetchExpression(xml)
                };

                EntityCollection returnCollection = ((RetrieveMultipleResponse)_serviceProxy.Execute(fetchRequest)).EntityCollection;
                recordsCount += returnCollection.Entities.Count;
                if (recordsCount > 0)
                {
                    solutionComponentsList.AddRange(returnCollection.Entities);
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

            List<EntityMetadata> emList = environmentStructure.EntityMetadata.ToList<EntityMetadata>();
            List<Entity> solutionComponentsList1 = solutionComponentsList;

            for (int i = 0; i < solutionComponentsList.Count; i++)
            {
                EntityMetadata em = emList.Find(x => x.MetadataId == (Guid)solutionComponentsList[i]["objectid"]);
                if (em != null)
                    solutionComponentsList1[i]["EntityLogicalName"] = em.LogicalName;
            }

            return solutionComponentsList1;
        }
    }

    

    internal class FormAndFields
    {
        public string formName { get; set; }

        public List<AttributeMetadata> fields { get; set; }
    }
}