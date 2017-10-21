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

using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.ServiceModel;
using VisioApi = Microsoft.Office.Interop.Visio;

namespace MSCRMToolKit
{
    internal class DiagramBuilder
    {
        // Specify which language code to use in the sample. If you are using a language
        // other than US English, you will need to modify this value accordingly.
        // See http://msdn.microsoft.com/en-us/library/0h88fahh.aspx
        public const int _languageCode = 1033;

        private VisioApi.Application _application;
        private VisioApi.Document _document;
        private RetrieveAllEntitiesResponse _metadataResponse;
        private ArrayList _processedRelationships;
        private List<string> selectedEntitiesNames;
        private List<ProcessedEntity> _processedEntities = new List<ProcessedEntity>();
        private DiagramBuildingProperties dbp = new DiagramBuildingProperties();

        private const double X_POS1 = 0;
        private const double Y_POS1 = 0;
        private const double X_POS2 = 3.2;
        private const double Y_POS2 = 0.6;

        private const double SHDW_PATTERN = 0;
        private const double BEGIN_ARROW_MANY = 29;
        private const double BEGIN_ARROW = 0;
        private const double END_ARROW = 29;
        private const double LINE_COLOR_MANY = 10;
        private const double LINE_COLOR = 8;
        private const double LINE_PATTERN_MANY = 2;
        private const double LINE_PATTERN = 1;
        private const string LINE_WEIGHT = "2pt";
        private const double ROUNDING = 0.0625;
        private const double HEIGHT = 0.25;
        private const short NAME_CHARACTER_SIZE = 12;
        const short FONT_STYLE = 225;  
        private const short VISIO_SECTION_OJBECT_INDEX = 1;
        String VersionName;

        public DiagramBuilder()
        {
            _processedRelationships = new ArrayList(128);
        }

        /// <summary>
        /// Main entry point for the application.
        /// </summary>
        /// <param name="dbp">The Diagram Buildin g Properties.</param>
        /// <param name="worker">The worker.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
        /// <returns></returns>
        //public string GenerateDiagram(string connectionName, List<string> entities, RetrieveAllEntitiesResponse response, int selectedDiagramEntityLabelIndex, BackgroundWorker worker, DoWorkEventArgs e)
        public string GenerateDiagram(DiagramBuildingProperties dbp, BackgroundWorker worker, DoWorkEventArgs e)
        {
            String filename = String.Empty;
            VisioApi.Application application;
            VisioApi.Document document;
            DiagramBuilder builder = new DiagramBuilder();

            try
            {
                // Load Visio and create a new document.
                application = new VisioApi.Application();
                application.Visible = false; // Not showing the UI increases rendering speed
                builder.VersionName = application.Version;
                document = application.Documents.Add(String.Empty);

                builder._application = application;
                builder._document = document;

                builder._metadataResponse = dbp.environmentStructure;
                builder.selectedEntitiesNames = dbp.entities;
                builder.dbp = dbp;

                // Diagram all entities if given no command-line parameters, otherwise diagram
                // those entered as command-line parameters.
                builder.BuildDiagram(dbp.entities, worker, e);
                filename = "EntitesStructure\\Diagrams.vsd";

                // Save the diagram in the current directory using the name of the first
                // entity argument or "AllEntities" if none were given. Close the Visio application.
                document.SaveAs(Directory.GetCurrentDirectory() + "\\" + filename);
                application.Quit();
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault>)
            {
                throw;
            }
            catch (System.Exception)
            {
                throw;
            }

            return filename;
        }

        /// <summary>
        /// Create a new page in a Visio file showing all the direct entity relationships participated in
        /// by the passed-in array of entities.
        /// </summary>
        /// <param name="entities">Core entities for the diagram</param>
        /// <param name="worker">The worker.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
        private void BuildDiagram(List<string> entities, BackgroundWorker worker, DoWorkEventArgs e)
        {
            // Get the default page of our new document
            VisioApi.Page page = _document.Pages[1];
            page.Name = "MSCRMToolKit generated diagram";
            int cptEntititesTreated = 0;

            // Get the metadata for each passed-in entity, draw it, and draw its relationships.
            foreach (string entityName in entities)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                cptEntititesTreated++;
                worker.ReportProgress(cptEntititesTreated);
                EntityMetadata entity = GetEntityMetadata(entityName);

                // Create a Visio rectangle shape.
                VisioApi.Shape rect;

                try
                {
                    // There is no "Get Try", so we have to rely on an exception to tell us it does not exists
                    // We have to skip some entities because they may have already been added by relationships of another entity
                    rect = page.Shapes.get_ItemU(entity.LogicalName);
                }
                catch (System.Runtime.InteropServices.COMException)
                {
                    rect = DrawEntityRectangle(page, entity);
                }

                // Draw all relationships TO this entity.
                DrawRelationships(entity, rect, entity.ManyToManyRelationships, false, worker, e);
                DrawRelationships(entity, rect, entity.ManyToOneRelationships, false, worker, e);

                // Draw all relationshipos FROM this entity
                DrawRelationships(entity, rect, entity.OneToManyRelationships, true, worker, e);
            }
            
            // Arrange the shapes to fit the page.
            page.Layout();
            page.ResizeToFitContents();
        }

        /// <summary>
        /// Draw on a Visio page the entity relationships defined in the passed-in relationship collection.
        /// </summary>
        /// <param name="entity">Core entity</param>
        /// <param name="rect">Shape representing the core entity</param>
        /// <param name="relationshipCollection">Collection of entity relationships to draw</param>
        /// <param name="areReferencingRelationships">Whether or not the core entity is the referencing entity in the relationship</param>
        /// <param name="worker">The worker.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
        private void DrawRelationships(EntityMetadata entity, VisioApi.Shape rect, RelationshipMetadataBase[] relationshipCollection, bool areReferencingRelationships, BackgroundWorker worker, DoWorkEventArgs e)
        {
            ManyToManyRelationshipMetadata currentManyToManyRelationship = null;
            OneToManyRelationshipMetadata currentOneToManyRelationship = null;
            EntityMetadata entity2 = null;
            AttributeMetadata attribute2 = null;
            AttributeMetadata attribute = null;
            Guid metadataID = Guid.NewGuid();
            bool isManyToMany = false;

            // Draw each relationship in the relationship collection.
            foreach (RelationshipMetadataBase entityRelationship in relationshipCollection)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                entity2 = null;

                if (entityRelationship is ManyToManyRelationshipMetadata)
                {
                    isManyToMany = true;
                    currentManyToManyRelationship = entityRelationship as ManyToManyRelationshipMetadata;
                    // The entity passed in is not necessarily the originator of this relationship.
                    if (String.Compare(entity.LogicalName, currentManyToManyRelationship.Entity1LogicalName, true) != 0)
                    {
                        entity2 = GetEntityMetadata(currentManyToManyRelationship.Entity1LogicalName);
                    }
                    else
                    {
                        entity2 = GetEntityMetadata(currentManyToManyRelationship.Entity2LogicalName);
                    }
                    attribute2 = GetAttributeMetadata(entity2, entity2.PrimaryIdAttribute);
                    attribute = GetAttributeMetadata(entity, entity.PrimaryIdAttribute);
                    metadataID = currentManyToManyRelationship.MetadataId.Value;
                }
                else if (entityRelationship is OneToManyRelationshipMetadata)
                {
                    isManyToMany = false;
                    currentOneToManyRelationship = entityRelationship as OneToManyRelationshipMetadata;
                    entity2 = GetEntityMetadata(areReferencingRelationships ? currentOneToManyRelationship.ReferencingEntity : currentOneToManyRelationship.ReferencedEntity);
                    attribute2 = GetAttributeMetadata(entity2, areReferencingRelationships ? currentOneToManyRelationship.ReferencingAttribute : currentOneToManyRelationship.ReferencedAttribute);
                    attribute = GetAttributeMetadata(entity, areReferencingRelationships ? currentOneToManyRelationship.ReferencedAttribute : currentOneToManyRelationship.ReferencingAttribute);
                    metadataID = currentOneToManyRelationship.MetadataId.Value;
                }
                // Verify relationship is either ManyToManyMetadata or OneToManyMetadata
                if (entity2 != null)
                {
                    if (_processedRelationships.Contains(metadataID))
                    {
                        // Skip relationships we have already drawn
                        continue;
                    }
                    else
                    {
                        // Record we are drawing this relationship
                        _processedRelationships.Add(metadataID);

                        // Define convenience variables based upon the direction of referencing with respect to the core entity.
                        VisioApi.Shape rect2;

                        // Do not draw relationships involving the entity itself, SystemUser, BusinessUnit,
                        // or those that are intentionally excluded.
                        string selectedEntityFound = selectedEntitiesNames.Find(en => en == entity2.LogicalName);
                        if (String.Compare(entity2.LogicalName, "systemuser", true) != 0 &&
                            String.Compare(entity2.LogicalName, "businessunit", true) != 0 &&
                            String.Compare(entity2.LogicalName, rect.Name, true) != 0 &&
                            (selectedEntityFound != null) &&
                            String.Compare(entity.LogicalName, "systemuser", true) != 0 &&
                            String.Compare(entity.LogicalName, "businessunit", true) != 0)
                        {
                            // Either find or create a shape that represents this secondary entity, and add the name of
                            // the involved attribute to the shape's text.                            
                            try
                            {
                                rect2 = rect.ContainingPage.Shapes.get_ItemU(entity2.LogicalName);
                                int processedEntity2Index = _processedEntities.FindIndex(pe => pe.entityName == entity2.LogicalName);
                                if (!_processedEntities[processedEntity2Index].attributes.Exists(a => a == attribute2.LogicalName) && dbp.showForeignKeys)
                                {
                                    rect2.get_CellsSRC(VISIO_SECTION_OJBECT_INDEX, (short)VisioApi.VisRowIndices.visRowXFormOut, (short)VisioApi.VisCellIndices.visXFormHeight).ResultIU += 0.25;
                                    rect2.Text += getAttributeValue(attribute2);
                                    _processedEntities[processedEntity2Index].attributes.Add(attribute2.LogicalName);

                                }
                            }
                            catch (System.Runtime.InteropServices.COMException)
                            {
                                rect2 = DrawEntityRectangle(rect.ContainingPage, entity2);
                                int processedEntity2Index = _processedEntities.FindIndex(pe => pe.entityName == entity2.LogicalName);
                                if (!_processedEntities[processedEntity2Index].attributes.Exists(a => a == attribute2.LogicalName) && dbp.showForeignKeys)
                                {
                                    rect2.Text += getAttributeValue(attribute2);                                    
                                    _processedEntities[processedEntity2Index].attributes.Add(attribute2.LogicalName);
                                }
                            }

                            // Add the name of the involved attribute to the core entity's text, if not already present.
                            int processedEntityIndex = _processedEntities.FindIndex(pe => pe.entityName == entity.LogicalName);
                            if (!_processedEntities[processedEntityIndex].attributes.Exists(a => a == attribute.LogicalName) && dbp.showForeignKeys)
                            {
                                rect.get_CellsSRC(VISIO_SECTION_OJBECT_INDEX, (short)VisioApi.VisRowIndices.visRowXFormOut, (short)VisioApi.VisCellIndices.visXFormHeight).ResultIU += HEIGHT;
                                rect.Text += getAttributeValue(attribute);
                                _processedEntities[processedEntityIndex].attributes.Add(attribute.LogicalName);
                            }
                            
                            // Draw the directional, dynamic connector between the two entity shapes.
                            if (areReferencingRelationships)
                            {
                                DrawDirectionalDynamicConnector(rect, rect2, isManyToMany, entityRelationship.SchemaName);
                            }
                            else
                            {
                                DrawDirectionalDynamicConnector(rect2, rect, isManyToMany, entityRelationship.SchemaName);
                            }
                        }
                    }
                }
            }
        }

        private string getAttributeValue(AttributeMetadata attribute)
        {
            string displayName = "";
            if (attribute.DisplayName != null && attribute.DisplayName.UserLocalizedLabel != null)
                displayName = attribute.DisplayName.UserLocalizedLabel.Label;
            if (dbp.attributeLabelDisplay == 1)
                return "\n" + displayName;
            else if (dbp.attributeLabelDisplay == 2)
                return "\n" + displayName + " [" + attribute.LogicalName + "]";
            else
                return "\n" + attribute.LogicalName;
        }

        /// <summary>
        /// Draw an "Entity" Rectangle
        /// </summary>
        /// <param name="page">The Page on which to draw</param>
        /// <param name="entity">The entity to draw</param>
        /// <returns>The newly drawn rectangle</returns>
        private VisioApi.Shape DrawEntityRectangle(VisioApi.Page page, EntityMetadata entity)
        {
            VisioApi.Shape rect = page.DrawRectangle(X_POS1, Y_POS1, X_POS2, Y_POS2);
            rect.Name = entity.LogicalName;
            if (dbp.entityLabelDisplay == 1)
                rect.Text = entity.DisplayName.UserLocalizedLabel.Label + " ";
            else if (dbp.entityLabelDisplay == 2)
                rect.Text = entity.DisplayName.UserLocalizedLabel.Label + " [" + entity.LogicalName + "] ";
            else
                rect.Text = entity.LogicalName + " ";
            

            // Determine the shape fill color based on entity ownership.
            string fillColor;

            switch (entity.OwnershipType.Value)
            {
                case OwnershipTypes.BusinessOwned:
                    fillColor = "RGB(255,202,176)"; // Light orange
                    break;
                case OwnershipTypes.OrganizationOwned:
                    fillColor = "RGB(255,255,176)"; // Light yellow
                    break;
                case OwnershipTypes.UserOwned:
                    fillColor = "RGB(204,255,204)"; // Light green
                    break;
                default:
                    fillColor = "RGB(255,255,255)"; // White
                    break;
            }

            // Set the fill color, placement properties, and line weight of the shape.
            rect.get_CellsSRC(VISIO_SECTION_OJBECT_INDEX, (short)VisioApi.VisRowIndices.visRowMisc, (short)VisioApi.VisCellIndices.visLOFlags).Formula = ((int)VisioApi.VisCellVals.visLOFlagsPlacable).ToString();
            if(dbp.showOwnership)
                rect.get_CellsSRC(VISIO_SECTION_OJBECT_INDEX, (short)VisioApi.VisRowIndices.visRowFill, (short)VisioApi.VisCellIndices.visFillForegnd).FormulaU = fillColor;

            // Update the style of the entity name
            VisioApi.Characters characters = rect.Characters;
            characters.Begin = 0;
            characters.End = entity.LogicalName.Length;
            characters.set_CharProps((short)VisioApi.VisCellIndices.visCharacterStyle, (short)VisioApi.VisCellVals.visBold);
            characters.set_CharProps((short)VisioApi.VisCellIndices.visCharacterColor, (short)VisioApi.VisDefaultColors.visDarkBlue);
            characters.set_CharProps((short)VisioApi.VisCellIndices.visCharacterSize, NAME_CHARACTER_SIZE);
            //set the font family of the text to segoe for the visio 2013.
            if (VersionName == "15.0")
                characters.set_CharProps((short)VisioApi.VisCellIndices.visCharacterFont, (short)FONT_STYLE);

            _processedEntities.Add(new ProcessedEntity { entityName = entity.LogicalName, attributes = new List<string>{entity.PrimaryIdAttribute} });

            //Add Primary Key attribute
            if(dbp.showPrimaryKeys)
            {
                rect.get_CellsSRC(VISIO_SECTION_OJBECT_INDEX, (short)VisioApi.VisRowIndices.visRowXFormOut, (short)VisioApi.VisCellIndices.visXFormHeight).ResultIU += HEIGHT;
                AttributeMetadata attribute = GetAttributeMetadata(entity, entity.PrimaryIdAttribute);
                rect.Text += getAttributeValue(attribute) + "  [PK]";
            }

            return rect;
        }

        /// <summary>
        /// Draw a directional, dynamic connector between two entities, representing an entity relationship.
        /// </summary>
        /// <param name="shapeFrom">Shape initiating the relationship</param>
        /// <param name="shapeTo">Shape referenced by the relationship</param>
        /// <param name="isManyToMany">Whether or not it is a many-to-many entity relationship</param>
        /// <param name="SchemaName">The relationship Shcema Name</param>
        private void DrawDirectionalDynamicConnector(VisioApi.Shape shapeFrom, VisioApi.Shape shapeTo, bool isManyToMany, string SchemaName)
        {
            // Add a dynamic connector to the page.
            VisioApi.Shape connectorShape = shapeFrom.ContainingPage.Drop(_application.ConnectorToolDataObject, 0.0, 0.0);
            
            if(dbp.showRelationshipsNames)
                connectorShape.Text = SchemaName;            

            // Set the connector properties, using different arrows, colors, and patterns for many-to-many relationships.
            connectorShape.get_CellsSRC(VISIO_SECTION_OJBECT_INDEX, (short)VisioApi.VisRowIndices.visRowFill, (short)VisioApi.VisCellIndices.visFillShdwPattern).ResultIU = SHDW_PATTERN;
            connectorShape.get_CellsSRC(VISIO_SECTION_OJBECT_INDEX, (short)VisioApi.VisRowIndices.visRowLine, (short)VisioApi.VisCellIndices.visLineBeginArrow).ResultIU = isManyToMany ? BEGIN_ARROW_MANY : BEGIN_ARROW;
            connectorShape.get_CellsSRC(VISIO_SECTION_OJBECT_INDEX, (short)VisioApi.VisRowIndices.visRowLine, (short)VisioApi.VisCellIndices.visLineEndArrow).ResultIU = END_ARROW;
            connectorShape.get_CellsSRC(VISIO_SECTION_OJBECT_INDEX, (short)VisioApi.VisRowIndices.visRowLine, (short)VisioApi.VisCellIndices.visLineColor).ResultIU = isManyToMany ? LINE_COLOR_MANY : LINE_COLOR;
            connectorShape.get_CellsSRC(VISIO_SECTION_OJBECT_INDEX, (short)VisioApi.VisRowIndices.visRowLine, (short)VisioApi.VisCellIndices.visLinePattern).ResultIU = isManyToMany ? LINE_PATTERN : LINE_PATTERN;
            connectorShape.get_CellsSRC(VISIO_SECTION_OJBECT_INDEX, (short)VisioApi.VisRowIndices.visRowFill, (short)VisioApi.VisCellIndices.visLineRounding).ResultIU = ROUNDING;

            // Connect the starting point.
            VisioApi.Cell cellBeginX = connectorShape.get_CellsSRC(VISIO_SECTION_OJBECT_INDEX, (short)VisioApi.VisRowIndices.visRowXForm1D, (short)VisioApi.VisCellIndices.vis1DBeginX);
            cellBeginX.GlueTo(shapeFrom.get_CellsSRC(VISIO_SECTION_OJBECT_INDEX, (short)VisioApi.VisRowIndices.visRowXFormOut, (short)VisioApi.VisCellIndices.visXFormPinX));

            // Connect the ending point.
            VisioApi.Cell cellEndX = connectorShape.get_CellsSRC(VISIO_SECTION_OJBECT_INDEX, (short)VisioApi.VisRowIndices.visRowXForm1D, (short)VisioApi.VisCellIndices.vis1DEndX);
            cellEndX.GlueTo(shapeTo.get_CellsSRC(VISIO_SECTION_OJBECT_INDEX, (short)VisioApi.VisRowIndices.visRowXFormOut, (short)VisioApi.VisCellIndices.visXFormPinX));
        }

        /// <summary>
        /// Retrieves an entity from the local copy of CRM Metadata
        /// </summary>
        /// <param name="entityName">The name of the entity to find</param>
        /// <returns>NULL if the entity was not found, otherwise the entity's metadata</returns>
        private EntityMetadata GetEntityMetadata(string entityName)
        {
            foreach (EntityMetadata md in _metadataResponse.EntityMetadata)
            {
                if (md.LogicalName == entityName)
                {
                    return md;
                }
            }

            return null;
        }

        /// <summary>
        /// Retrieves an attribute from an EntityMetadata object
        /// </summary>
        /// <param name="entity">The entity metadata that contains the attribute</param>
        /// <param name="attributeName">The name of the attribute to find</param>
        /// <returns>NULL if the attribute was not found, otherwise the attribute's metadata</returns>
        private AttributeMetadata GetAttributeMetadata(EntityMetadata entity, string attributeName)
        {
            foreach (AttributeMetadata attrib in entity.Attributes)
            {
                if (attrib.LogicalName == attributeName)
                {
                    return attrib;
                }
            }

            return null;
        }
    }

    class ProcessedEntity
    {
        public string entityName { get; set; }
        public List<string> attributes { get; set; }
    }
}