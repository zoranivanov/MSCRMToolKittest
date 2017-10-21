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

using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Xml;

namespace MSCRMToolKit
{
    /// <summary>
    /// Workflows Execution Manager class extending MSCRMToolKitProfileManager
    /// </summary>
    public class MSCRMWorkflowExecutionManager : MSCRMToolKitProfileManager
    {
        /// <summary>
        /// The Workflow Execution profiles
        /// </summary>
        public List<MSCRMWorkflowExecutionProfile> Profiles = new List<MSCRMWorkflowExecutionProfile>();
        /// <summary>
        /// The workspace folder
        /// </summary>
        public string Folder = "WorkflowExecutionManager";
        /// <summary>
        /// The configuration file name
        /// </summary>
        private string ConfigurationFileName = "WorkflowExecutionManager\\Profiles.xml";
        /// <summary>
        /// Initializes a new instance of the <see cref="MSCRMWorkflowExecutionManager" /> class.
        /// </summary>
        public MSCRMWorkflowExecutionManager()
        {
            if (!Directory.Exists(Folder))
                Directory.CreateDirectory(Folder);
            ReadProfiles();
        }

        /// <summary>
        /// Creates the Workflow Execution profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        public void CreateProfile(MSCRMWorkflowExecutionProfile profile)
        {
            if (!Directory.Exists(Folder + "\\" + profile.ProfileName))
                Directory.CreateDirectory(Folder + "\\" + profile.ProfileName);

            //Creating new Profile
            Profiles.Add(profile);
            WriteProfiles();
        }

        /// <summary>
        /// Updates the Workflow Execution profile.
        /// </summary>
        /// <param name="profile">The Workflow Execution profile.</param>
        /// <exception cref="System.Exception">Data Export Profile Update failed. The Data Export Profile  + profile.ProfileName +  was not found in the configuration file.</exception>
        public void UpdateProfile(MSCRMWorkflowExecutionProfile profile)
        {
            if (!Directory.Exists(Folder + "\\" + profile.ProfileName))
                Directory.CreateDirectory(Folder + "\\" + profile.ProfileName);

            int index = Profiles.FindIndex(d => d.ProfileName == profile.ProfileName);
            if (index > -1)
            {
                Profiles[index] = profile;
            }
            else
            {
                LogManager.WriteLog("Data Export Profile Update failed. The Data Export Profile " + profile.ProfileName + " was not found in the configuration file.");
                throw new Exception("Data Export Profile Update failed. The Data Export Profile " + profile.ProfileName + " was not found in the configuration file.");
            }

            WriteProfiles();
        }

        /// <summary>
        /// Deletes the Workflow Execution profile.
        /// </summary>
        /// <param name="profile">The Workflow Execution profile.</param>
        /// <exception cref="System.Exception">Workflow Execution Profile deletion failed. The Workflow Execution Profile  + profile.ProfileName +  was not found in the configuration file.</exception>
        public void DeleteProfile(MSCRMWorkflowExecutionProfile profile)
        {
            int index = Profiles.FindIndex(d => d.ProfileName == profile.ProfileName);
            if (index > -1)
            {
                Profiles.RemoveAt(index);
            }
            else
            {
                LogManager.WriteLog("Workflow Execution Profile deletion failed. The Workflow Execution Profile " + profile.ProfileName + " was not found in the configuration file.");
                throw new Exception("Workflow Execution Profile deletion failed. The Workflow Execution Profile " + profile.ProfileName + " was not found in the configuration file.");
            }

            //Delete Profile folder
            try
            {
                if (Directory.Exists(Folder + "\\" + profile.ProfileName))
                    Directory.Delete(Folder + "\\" + profile.ProfileName, true);
            }
            catch (Exception)
            {
                throw;
            }

            //Save Profiles
            WriteProfiles();
        }

        /// <summary>
        /// Gets the Workflow Execution profile.
        /// </summary>
        /// <param name="profileName">Name of the profile.</param>
        /// <returns></returns>
        public MSCRMWorkflowExecutionProfile GetProfile(string profileName)
        {
            return Profiles.Find(d => d.ProfileName == profileName);
        }

        /// <summary>
        /// Runs the Workflow Execution profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        public void RunProfile(MSCRMWorkflowExecutionProfile profile)
        {
            LogManager.WriteLog("Running Workflow Execution Profile: " + profile.ProfileName);

            try
            {
                MSCRMConnection connection = profile.getSourceConneciton();
                _serviceProxy = cm.connect(connection);

                EntityCollection result = _serviceProxy.RetrieveMultiple(new FetchExpression(profile.FetchXMLQuery));

                foreach (Entity record in result.Entities)
                {
                    ExecuteWorkflowRequest request = new ExecuteWorkflowRequest()
                    {
                        WorkflowId = profile.WorkflowId,
                        EntityId = record.Id
                    };

                    // Execute the workflow.
                    ExecuteWorkflowResponse response = (ExecuteWorkflowResponse)_serviceProxy.Execute(request);
                }
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                LogManager.WriteLog("Error:" + ex.Detail.Message + "\n" + ex.Detail.TraceText);
                throw;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    LogManager.WriteLog("Error:" + ex.Message + "\n" + ex.InnerException.Message);
                else
                    LogManager.WriteLog("Error:" + ex.Message);
                throw;
            }
            LogManager.WriteLog("All workflows were launched.");
        }

        /// <summary>
        /// Reads the Workflow Execution profiles.
        /// </summary>
        private void ReadProfiles()
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(List<MSCRMWorkflowExecutionProfile>));
            if (!File.Exists(ConfigurationFileName))
            {
                FileStream writer = new FileStream(ConfigurationFileName, FileMode.Create);
                ser.WriteObject(writer, Profiles);
                writer.Close();
            }

            XmlDictionaryReaderQuotas XRQ = new XmlDictionaryReaderQuotas();
            XRQ.MaxStringContentLength = int.MaxValue;

            using (FileStream fs = new FileStream(ConfigurationFileName, FileMode.Open))
            using (XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, XRQ))
            {
                Profiles = (List<MSCRMWorkflowExecutionProfile>)ser.ReadObject(reader, true);
            }

            foreach (MSCRMWorkflowExecutionProfile profile in Profiles)
            {
                profile.setSourceConneciton();
            }
        }

        /// <summary>
        /// Writes the profiles.
        /// </summary>
        private void WriteProfiles()
        {
            using (FileStream writer = new FileStream(ConfigurationFileName, FileMode.Create))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(List<MSCRMWorkflowExecutionProfile>));
                ser.WriteObject(writer, Profiles);
            }
        }

        /// <summary>
        /// Reads the workflows.
        /// </summary>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns></returns>
        public List<MSCRMWorkflow> ReadWorkflows(string connectionName)
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(List<MSCRMWorkflow>));
            if (!File.Exists(Folder + "\\" + connectionName + ".xml"))
            {
                FileStream writer = new FileStream(Folder + "\\" + connectionName + ".xml", FileMode.Create);
                ser.WriteObject(writer, new List<MSCRMWorkflow>());
                writer.Close();
            }

            XmlDictionaryReaderQuotas XRQ = new XmlDictionaryReaderQuotas();
            XRQ.MaxStringContentLength = int.MaxValue;

            using (FileStream fs = new FileStream(Folder + "\\" + connectionName + ".xml", FileMode.Open))
            using (XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, XRQ))
            {   
                return (List<MSCRMWorkflow>)ser.ReadObject(reader, true);
            }            
        }

        /// <summary>
        /// Writes the workflows.
        /// </summary>
        /// <param name="sourceName">Name of the source.</param>
        /// <param name="Workflows">The workflows.</param>
        public void WriteWorkflows(string sourceName, List<MSCRMWorkflow> Workflows)
        {
            FileStream writer = new FileStream(Folder + "\\" + sourceName + ".xml", FileMode.Create);
            DataContractSerializer ser = new DataContractSerializer(typeof(List<MSCRMWorkflow>));
            ser.WriteObject(writer, Workflows);
            writer.Close();
        }
    }
}