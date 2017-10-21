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
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Xml;

namespace MSCRMToolKit
{
	/// <summary>
	/// MSCRM Solutions Transport Manager class extending MSCRMToolKitProfileManager
	/// </summary>
	public class MSCRMSolutionsTransportManager : MSCRMToolKitProfileManager
	{
		/// <summary>
		/// The profiles
		/// </summary>
		public List<MSCRMSolutionsTransportProfile> Profiles = new List<MSCRMSolutionsTransportProfile>();
		/// <summary>
		/// The Workspace folder
		/// </summary>
		public string Folder = "SolutionsTransporter";
		/// <summary>
		/// The configuration file name
		/// </summary>
		private string ConfigurationFileName = "SolutionsTransporter\\SolutionTransportProfiles.xml";
		/// <summary>
		/// Initializes a new instance of the <see cref="MSCRMSolutionsTransportManager"/> class.
		/// </summary>
		public MSCRMSolutionsTransportManager()
		{
			if (!Directory.Exists(Folder))
				Directory.CreateDirectory(Folder);
			ReadProfiles();
		}

		/// <summary>
		/// Creates the profile.
		/// </summary>
		/// <param name="profile">The profile.</param>
		public void CreateProfile(MSCRMSolutionsTransportProfile profile)
		{
			if (!Directory.Exists(Folder + "\\" + profile.ProfileName))
				Directory.CreateDirectory(Folder + "\\" + profile.ProfileName);

			//Creating new Profile
			Profiles.Add(profile);
			WriteProfiles();
			LogManager.WriteLog("Solutions Transport Profile " + profile.ProfileName + " created");
		}

		/// <summary>
		/// Updates the profile.
		/// </summary>
		/// <param name="profile">The profile.</param>
		/// <exception cref="System.Exception">Solutions Transport Profile Update failed. The Solution Transport Profile  + profile.ProfileName +  was not found in the configuration file.</exception>
		public void UpdateProfile(MSCRMSolutionsTransportProfile profile)
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
				LogManager.WriteLog("Solutions Transport Profile Update failed. The Solution Transport Profile " + profile.ProfileName + " was not found in the configuration file.");
				throw new Exception("Solutions Transport Profile Update failed. The Solution Transport Profile " + profile.ProfileName + " was not found in the configuration file.");
			}

			WriteProfiles();
			LogManager.WriteLog("Solutions Transport Profile " + profile.ProfileName + " updated.");
		}

		/// <summary>
		/// Deletes the profile.
		/// </summary>
		/// <param name="profile">The profile.</param>
		/// <exception cref="System.Exception">Solutions Transport Profile deletion failed. The Solution Transport Profile  + profile.ProfileName +  was not found in the configuration file.</exception>
		public void DeleteProfile(MSCRMSolutionsTransportProfile profile)
		{
			int index = Profiles.FindIndex(d => d.ProfileName == profile.ProfileName);
			if (index > -1)
			{
				Profiles.RemoveAt(index);
			}
			else
			{
				LogManager.WriteLog("Solutions Transport Profile deletion failed. The Solution Transport Profile " + profile.ProfileName + " was not found in the configuration file.");
				throw new Exception("Solutions Transport Profile deletion failed. The Solution Transport Profile " + profile.ProfileName + " was not found in the configuration file.");
			}

			//Delete Profile folder
			try
			{
				if (Directory.Exists(profile.SolutionExportFolder))
					Directory.Delete(profile.SolutionExportFolder, true);
			}
			catch (Exception)
			{
				throw;
			}

			//Save profiles
			WriteProfiles();
			LogManager.WriteLog("Solutions Transport Profile " + profile.ProfileName + " updated");
		}

		/// <summary>
		/// Gets the profile.
		/// </summary>
		/// <param name="profileName">Name of the profile.</param>
		/// <returns></returns>
		public MSCRMSolutionsTransportProfile GetProfile(string profileName)
		{
			return Profiles.Find(p => p.ProfileName == profileName);
		}

		/// <summary>
		/// Runs the profile.
		/// </summary>
		/// <param name="profile">The profile.</param>
		public void RunProfile(MSCRMSolutionsTransportProfile profile)
		{
			LogManager.WriteLog("Running Solutions Transport Profile: " + profile.ProfileName);

			try
			{
				if (profile.Operation == 0)
					Export(profile);
				else if (profile.Operation == 1)
					Import(profile);
				else if (profile.Operation == 2)
				{
					Export(profile);
					Import(profile);
				}
			}
			catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
			{
				LogManager.WriteLog("Error:" + ex.Detail.Message + "\n" + ex.Detail.TraceText);
				return;
			}
			catch (Exception ex)
			{
				if (ex.InnerException != null)
				{
					LogManager.WriteLog("Error:" + ex.Message + "\n" + ex.InnerException.Message);
					return;
				}
				else
				{
					LogManager.WriteLog("Error:" + ex.Message);
					return;
				}
			}			
		}

		/// <summary>
		/// Reads the profiles.
		/// </summary>
		private void ReadProfiles()
		{
			DataContractSerializer ser = new DataContractSerializer(typeof(List<MSCRMSolutionsTransportProfile>));
			if (!File.Exists(ConfigurationFileName))
			{
				FileStream writer = new FileStream(ConfigurationFileName, FileMode.Create);
				ser.WriteObject(writer, Profiles);
				writer.Close();
			}
			
			XmlDictionaryReaderQuotas XRQ = new XmlDictionaryReaderQuotas();
			XRQ.MaxStringContentLength = int.MaxValue;

			using(FileStream fs = new FileStream(ConfigurationFileName, FileMode.Open))
			using (XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, XRQ))
			{
				Profiles = (List<MSCRMSolutionsTransportProfile>)ser.ReadObject(reader, true);

				foreach (MSCRMSolutionsTransportProfile profile in Profiles)
				{
					profile.setSourceConneciton();
				}
			}
		}

		/// <summary>
		/// Writes the profiles.
		/// </summary>
		private void WriteProfiles()
		{
			FileStream writer = new FileStream(ConfigurationFileName, FileMode.Create);
			DataContractSerializer ser = new DataContractSerializer(typeof(List<MSCRMSolutionsTransportProfile>));
			ser.WriteObject(writer, Profiles);
			writer.Close();
		}

		/// <summary>
		/// Reads the solutions.
		/// </summary>
		/// <param name="connectionName">Name of the connection.</param>
		/// <returns>List of solutions.</returns>
		public List<MSCRMSolution> ReadSolutions(string connectionName)
		{
			DataContractSerializer ser = new DataContractSerializer(typeof(List<MSCRMSolution>));
			if (!File.Exists(Folder + "\\" + connectionName + ".xml"))
			{
				return new List<MSCRMSolution>();
			}

			XmlDictionaryReaderQuotas XRQ = new XmlDictionaryReaderQuotas();
			XRQ.MaxStringContentLength = int.MaxValue;

			using(FileStream fs = new FileStream(Folder + "\\" + connectionName + ".xml", FileMode.Open))
			using (XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, XRQ))
			{
				List<MSCRMSolution> Solutions = (List<MSCRMSolution>)ser.ReadObject(reader, true);
				return Solutions;
			}
		}

		/// <summary>
		/// Writes the solutions.
		/// </summary>
		/// <param name="sourceName">Name of the source.</param>
		/// <param name="Solutions">The solutions.</param>
		public void WriteSolutions(string sourceName, List<MSCRMSolution> Solutions)
		{
			FileStream writer = new FileStream(Folder + "\\" + sourceName + ".xml", FileMode.Create);
			DataContractSerializer ser = new DataContractSerializer(typeof(List<MSCRMSolution>));
			ser.WriteObject(writer, Solutions);
			writer.Close();
		}

		/// <summary>
		/// Downloads the solutions.
		/// </summary>
		/// <param name="connection">The connection.</param>
		/// <returns>List of solutions.</returns>
		public List<MSCRMSolution> DownloadSolutions(MSCRMConnection connection)
		{
			List<MSCRMSolution> solutionsLst = new List<MSCRMSolution>();
			_serviceProxy = cm.connect(connection);

			QueryExpression querySampleSolution = new QueryExpression
			{
				EntityName = "solution",
				ColumnSet = new ColumnSet(true),
				Criteria = new FilterExpression(),
			};
			querySampleSolution.Criteria.AddCondition("ismanaged", ConditionOperator.Equal, false);
			querySampleSolution.Criteria.AddCondition("isvisible", ConditionOperator.Equal, true);
			EntityCollection solutions = _serviceProxy.RetrieveMultiple(querySampleSolution);

			foreach (Entity solution in solutions.Entities)
			{
				EntityReference publisher = (EntityReference)solution["publisherid"];
				string description = solution.Attributes.Contains("description") ? (string)solution["description"] : "";
				MSCRMSolution MESCRMSolution = new MSCRMSolution
				{
					UniqueName = (string)solution["uniquename"],
					DisplayName = (string)solution["friendlyname"],
					Version = (string)solution["version"],
					Publisher = publisher.Name,
					Description = description
				};
				solutionsLst.Add(MESCRMSolution);
			}

			WriteSolutions(connection.ConnectionName, solutionsLst);

			return solutionsLst;
		}

		/// <summary>
		/// Exports the specified profile.
		/// </summary>
		/// <param name="profile">The profile.</param>
		private void Export(MSCRMSolutionsTransportProfile profile)
		{
			try
			{
				//Set Data export folder
				if (!Directory.Exists(profile.SolutionExportFolder))
					Directory.CreateDirectory(profile.SolutionExportFolder);

				MSCRMConnection connection = profile.getSourceConneciton();
				_serviceProxy = cm.connect(connection);

				//Download fresh list of solutions for versions update
				List<MSCRMSolution> solutions = DownloadSolutions(connection);			

				DateTime now = DateTime.Now;
				string folderName = String.Format("{0:yyyyMMddHHmmss}", now);

				if (!Directory.Exists(profile.SolutionExportFolder + "\\" + folderName))
					Directory.CreateDirectory(profile.SolutionExportFolder + "\\" + folderName);

				foreach (string SolutionName in profile.SelectedSolutionsNames)
				{
					//Check if customizations are to be published
					if (profile.PublishAllCustomizationsSource)
					{
						LogManager.WriteLog("Publishing all Customizations on " + connection.ConnectionName + " ...");
						PublishAllXmlRequest publishRequest = new PublishAllXmlRequest();
						_serviceProxy.Execute(publishRequest);
					}
					LogManager.WriteLog("Exporting Solution " + SolutionName + " from " + connection.ConnectionName);

					ExportSolutionRequest exportSolutionRequest = new ExportSolutionRequest();
					exportSolutionRequest.Managed = profile.ExportAsManaged;
					exportSolutionRequest.SolutionName = SolutionName;
					exportSolutionRequest.ExportAutoNumberingSettings = profile.ExportAutoNumberingSettings;
					exportSolutionRequest.ExportCalendarSettings = profile.ExportCalendarSettings;
					exportSolutionRequest.ExportCustomizationSettings = profile.ExportCustomizationSettings;
					exportSolutionRequest.ExportEmailTrackingSettings = profile.ExportEmailTrackingSettings;
					exportSolutionRequest.ExportGeneralSettings = profile.ExportGeneralSettings;
					exportSolutionRequest.ExportIsvConfig = profile.ExportIsvConfig;
					exportSolutionRequest.ExportMarketingSettings = profile.ExportMarketingSettings;
					exportSolutionRequest.ExportOutlookSynchronizationSettings = profile.ExportOutlookSynchronizationSettings;
					exportSolutionRequest.ExportRelationshipRoles = profile.ExportRelationshipRoles;

					string managed = "";
					if (profile.ExportAsManaged)
						managed = "managed";
					MSCRMSolution selectedSolution = solutions.Find(s => s.UniqueName == SolutionName);
					string selectedSolutionVersion = selectedSolution.Version.Replace(".","_");
					ExportSolutionResponse exportSolutionResponse = (ExportSolutionResponse)_serviceProxy.Execute(exportSolutionRequest);
					byte[] exportXml = exportSolutionResponse.ExportSolutionFile;
					string filename = SolutionName + "_" + selectedSolutionVersion + "_" + managed + ".zip";
					File.WriteAllBytes(profile.SolutionExportFolder + "\\" + folderName + "\\" + filename, exportXml);
					LogManager.WriteLog("Export finished for Solution: " + SolutionName + ". Exported file: " + filename);
				}
				LogManager.WriteLog("Export finished for Profile: " + profile.ProfileName);
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
		}

		/// <summary>
		/// Imports the specified profile.
		/// </summary>
		/// <param name="profile">The profile.</param>
		private void Import(MSCRMSolutionsTransportProfile profile)
		{
			//Check if there is a solutions to import
			if (Directory.Exists(profile.SolutionExportFolder))
			{
				IOrderedEnumerable<string> subDirectories = Directory.GetDirectories(profile.SolutionExportFolder).OrderByDescending(x => x);
				if (subDirectories.Count<string>() == 0)
				{
					LogManager.WriteLog("There are no solutions for import.");
					return;
				}

				//Check which solutions to import: Newest, Oldest, specific exprot date solutions
				string solutionsToImportFolder = "";
				if (profile.SolutionsToImport == "Newest")
					solutionsToImportFolder = subDirectories.ElementAt(0);
				else if (profile.SolutionsToImport == "Oldest")
					solutionsToImportFolder = subDirectories.ElementAt(subDirectories.Count<string>() - 1);
				else
					solutionsToImportFolder = subDirectories.First(s => s.EndsWith(profile.SolutionsToImport));

				if (solutionsToImportFolder == "")
				{
					LogManager.WriteLog("The specified solutions to import were not found.");
					return;
				}

				//get all solutions archives
				string[] solutionsArchivesNames = Directory.GetFiles(solutionsToImportFolder, "*.zip");
				if (solutionsArchivesNames.Count<string>() == 0)
				{
					LogManager.WriteLog("There are no solutions for import.");
					return;
				}

				string[] pathList = solutionsToImportFolder.Split(new Char [] {'\\'});
				string DirectoryName = pathList[pathList.Count<string>() -1];
				LogManager.WriteLog("Importing solutions from " + DirectoryName);

				MSCRMConnection connection = profile.getTargetConneciton();
				_serviceProxy = cm.connect(connection);
				LogManager.WriteLog("Start importing solutions in " + connection.ConnectionName);

				foreach (string solutionArchiveName in solutionsArchivesNames)
				{
					bool selectedsolutionfound = false;
					foreach (string solutionname in profile.SelectedSolutionsNames)
					{
						if (Path.GetFileName(solutionArchiveName).StartsWith(solutionname))
							selectedsolutionfound = true;
					}

					if (!selectedsolutionfound)
						continue;

					//Import Solution
					LogManager.WriteLog("Importing solution archive " + Path.GetFileName(solutionArchiveName) + " into " + connection.ConnectionName); 
				   
					byte[] fileBytes = File.ReadAllBytes(solutionArchiveName);

					Guid ImportJobId = Guid.NewGuid();

					ImportSolutionRequest impSolReqWithMonitoring = new ImportSolutionRequest()
					{
						CustomizationFile = fileBytes,
						OverwriteUnmanagedCustomizations = profile.OverwriteUnmanagedCustomizations,
						PublishWorkflows = profile.PublishWorkflows,
						ImportJobId = ImportJobId
					};

					_serviceProxy.Execute(impSolReqWithMonitoring);

					
					Entity ImportJob = _serviceProxy.Retrieve("importjob", impSolReqWithMonitoring.ImportJobId, new ColumnSet(true));
					//File.WriteAllText(solutionsToImportFolder + "\\importlog_ORIGINAL_" + Path.GetFileNameWithoutExtension(solutionArchiveName) + ".xml", (string)ImportJob["data"]);

					RetrieveFormattedImportJobResultsRequest importLogRequest = new RetrieveFormattedImportJobResultsRequest()
					{
						ImportJobId = ImportJobId
					};
					RetrieveFormattedImportJobResultsResponse importLogResponse = (RetrieveFormattedImportJobResultsResponse)_serviceProxy.Execute(importLogRequest);

					DateTime now = DateTime.Now;
					string timeNow = String.Format("{0:yyyyMMddHHmmss}", now);

					string exportedSolutionFileName = solutionsToImportFolder + "\\importlog_" + Path.GetFileNameWithoutExtension(solutionArchiveName) + "_" + timeNow + ".xml";
					
					//Fix bad Status and Message export
					System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
					doc.LoadXml((string)ImportJob["data"]);
					String SolutionImportResult = doc.SelectSingleNode("//solutionManifest/result/@result").Value;

					File.WriteAllText(exportedSolutionFileName, importLogResponse.FormattedResults);
					
					LogManager.WriteLog("Solution " + Path.GetFileName(solutionArchiveName) + " was imported with success in " + connection.ConnectionName);
				}

				//Check if customizations are to be published
				if (profile.PublishAllCustomizationsTarget)
				{
					LogManager.WriteLog("Publishing all Customizations on " + connection.ConnectionName + " ...");
					PublishAllXmlRequest publishRequest = new PublishAllXmlRequest();
					_serviceProxy.Execute(publishRequest);
				}

				LogManager.WriteLog("Solutions Import finished for Profile: " + profile.ProfileName);				
			}
			else
			{
				LogManager.WriteLog("There are no solutions for import.");
				return;
			}

		}
	}
}