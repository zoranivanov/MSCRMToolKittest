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

using Microsoft.Xrm.Sdk.Deployment;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using System.Windows.Forms;

namespace MSCRMToolKit
{
    /// <summary>
    /// DeploymentProperties class
    /// </summary>
    public partial class DeploymentProperties : Form
    {
        private MSCRMConnectionsManager cm = new MSCRMConnectionsManager();
        private MSCRMConnection connection = null;
        private DeploymentServiceClient serviceClient = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeploymentProperties"/> class.
        /// </summary>
        public DeploymentProperties()
        {
            InitializeComponent();
            foreach (MSCRMConnection connection in cm.MSCRMConnections)
            {
                this.comboBoxSource.Items.AddRange(new object[] { connection.ConnectionName });
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            connection = cm.MSCRMConnections[comboBoxSource.SelectedIndex];
            LogManager.WriteLog("Loading Server Settings.");
            toolStripStatusLabel1.Text = "";
            try
            {
                string deploymentURI = connection.ServerAddress.Replace(connection.OrganizationName + "/", "") + "XRMDeployment/2011/Deployment.svc";
                serviceClient = Microsoft.Xrm.Sdk.Deployment.Proxy.ProxyClientHelper.CreateClient(new Uri(deploymentURI));
                serviceClient.ClientCredentials.Windows.ClientCredential.UserName = connection.UserName;
                serviceClient.ClientCredentials.Windows.ClientCredential.Password = connection.Password;

                // Retrieve all deployed instances of Microsoft Dynamics CRM.
                var organizations = serviceClient.RetrieveAll(DeploymentEntityType.Organization);

                Microsoft.Xrm.Sdk.Deployment.EntityInstanceId currentOrganization = null;
                foreach (var organization in organizations)
                {
                    if (organization.Name.ToLower() == connection.OrganizationName.ToLower())
                        currentOrganization = organization;
                }

                RetrieveAdvancedSettingsRequest request = new RetrieveAdvancedSettingsRequest()
                {
                    ConfigurationEntityName = "Deployment",
                    ColumnSet = new ColumnSet()
                };

                ConfigurationEntity ce = ((RetrieveAdvancedSettingsResponse)serviceClient.Execute(request)).Entity;

                foreach (var setting in ce.Attributes)
                {
                    if (setting.Key == "AggregateQueryRecordLimit")
                        numericUpDownAggregateQueryRecordLimit.Text = setting.Value.ToString();
                    else if (setting.Key == "AutomaticallyInstallDatabaseUpdates")
                        checkBoxAutomaticallyInstallDatabaseUpdates.Checked = (bool)setting.Value;
                    else if (setting.Key == "AutomaticallyReprovisionLanguagePacks")
                        checkBoxAutomaticallyReprovisionLanguagePacks.Checked = (bool)setting.Value;
                }

                // Retrieve details of first organization from previous call.
                Microsoft.Xrm.Sdk.Deployment.Organization deployment =
                    (Microsoft.Xrm.Sdk.Deployment.Organization)serviceClient.Retrieve(
                        DeploymentEntityType.Organization,
                        currentOrganization);

                // Print out retrieved details about your organization.
                string organizationProperties = "";
                organizationProperties += "Friendly Name: " + deployment.FriendlyName + "\r\n";
                organizationProperties += "Unique Name: " + deployment.UniqueName + "\r\n";
                organizationProperties += "Organization Version: " + deployment.Version + "\r\n";
                organizationProperties += "SQL Server Name: " + deployment.SqlServerName + "\r\n";
                organizationProperties += "SRS URL: " + deployment.SrsUrl + "\r\n";
                organizationProperties += "Base Currency Code: " + deployment.BaseCurrencyCode + "\r\n";
                organizationProperties += "Base Currency Name: " + deployment.BaseCurrencyName + "\r\n";
                organizationProperties += "Base Currency Precision: " + deployment.BaseCurrencyPrecision + "\r\n";
                organizationProperties += "Base Currency Symbol: " + deployment.BaseCurrencySymbol + "\r\n";
                organizationProperties += "Base Language Code: " + deployment.BaseLanguageCode + "\r\n";
                organizationProperties += "Database Name: " + deployment.DatabaseName + "\r\n";
                organizationProperties += "Sql Collation: " + deployment.SqlCollation + "\r\n";
                organizationProperties += "Sqm Is Enabled: " + deployment.SqmIsEnabled + "\r\n";
                organizationProperties += "State: " + deployment.State + "\r\n";

                textBoxOrganizationProperties.Text = organizationProperties;

                // Retrieve license and user information for your organization.
                TrackLicenseRequest licenseRequest = new TrackLicenseRequest();
                TrackLicenseResponse licenseResponse = (TrackLicenseResponse)serviceClient.Execute(licenseRequest);

                // Print out the number of servers and the user list.
                string licenseanduserinformation = "Number of servers: ";
                //licenseanduserinformation += licenseResponse.NumberOfServers.HasValue ? licenseResponse.NumberOfServers.Value.ToString() : "null";
                //licenseanduserinformation += "\r\n";
                //licenseanduserinformation += "Users:\r\n";
                //foreach (String user in licenseResponse.UsersList)
                //{
                //    licenseanduserinformation += user + "\r\n";
                //}
                textBoxLicenceanduserinformation.Text = licenseanduserinformation;

                // Retrieve server settings for your organization.
                RetrieveAdvancedSettingsRequest requestServerSettings =
                    new RetrieveAdvancedSettingsRequest
                    {
                        ConfigurationEntityName = "ServerSettings",
                        ColumnSet = new ColumnSet(false)
                    };
                ConfigurationEntity configuration = ((RetrieveAdvancedSettingsResponse)serviceClient.Execute(requestServerSettings)).Entity;

                // Print out all advanced settings where IsWritable==true.
                foreach (var setting in configuration.Attributes)
                {
                    if (setting.Key != "Id")
                    {
                        if (setting.Key == "DisableUserInfoClaim")
                            checkBoxDisableUserInfoClaim.Checked = (bool)setting.Value;
                        else if (setting.Key == "MaxExpandCount")
                            numericUpDownMaxExpandCount.Text = setting.Value.ToString();
                        else if (setting.Key == "MaxResultsPerCollection")
                            numericUpDownMaxResultsPerCollection.Text = setting.Value.ToString();
                        else if (setting.Key == "NlbEnabled")
                            checkBoxNlbEnabled.Checked = (bool)setting.Value;
                        else if (setting.Key == "PostponeAppFabricRequestsInMinutes")
                            numericUpDownPostponeAppFabricRequestsInMinutes.Text = setting.Value.ToString();
                        else if (setting.Key == "PostViaExternalRouter")
                            checkBoxPostViaExternalRouter.Checked = (bool)setting.Value;
                        else if (setting.Key == "SslHeader")
                            textBoxSslHeader.Text = setting.Value.ToString();
                    }
                }

                toolStripStatusLabel1.Text = connection.OrganizationName + " deployment properties were successfully loaded.";
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                //string url = connection.ServerAddress + "main.aspx?pagetype=entityrecord&etn=eaf_contrat_client&id=" + entity.Id.ToString();
                LogManager.WriteLog("Error: " + ex.Detail.Message + "\n" + ex.Detail.TraceText);
                MessageBox.Show("Error: " + ex.Detail.Message + "\n" + ex.Detail.TraceText);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    LogManager.WriteLog("Error:" + ex.Message);
                    MessageBox.Show("Error:" + ex.Message + "\n" + ex.InnerException.Message);
                }
                else
                {
                    LogManager.WriteLog("Error:" + ex.Message);
                    MessageBox.Show("Error:" + ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigurationEntity entity = new ConfigurationEntity();
                entity.LogicalName = "Deployment";
                entity.Attributes = new Microsoft.Xrm.Sdk.Deployment.AttributeCollection();
                entity.Attributes.Add(new KeyValuePair<string, object>("AggregateQueryRecordLimit", numericUpDownAggregateQueryRecordLimit.Text));
                entity.Attributes.Add(new KeyValuePair<string, object>("AutomaticallyInstallDatabaseUpdates", checkBoxAutomaticallyInstallDatabaseUpdates.Checked));
                entity.Attributes.Add(new KeyValuePair<string, object>("AutomaticallyReprovisionLanguagePacks", checkBoxAutomaticallyReprovisionLanguagePacks.Checked));

                UpdateAdvancedSettingsRequest request2 = new UpdateAdvancedSettingsRequest();
                request2.Entity = entity;
                serviceClient.Execute(request2);
                toolStripStatusLabel1.Text = "Deployment Properties were successfully saved.";
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                //string url = connection.ServerAddress + "main.aspx?pagetype=entityrecord&etn=eaf_contrat_client&id=" + entity.Id.ToString();
                LogManager.WriteLog("Error: " + ex.Detail.Message + "\n" + ex.Detail.TraceText);
                MessageBox.Show("Error: " + ex.Detail.Message + "\n" + ex.Detail.TraceText);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    LogManager.WriteLog("Error:" + ex.Message);
                    MessageBox.Show("Error:" + ex.Message + "\n" + ex.InnerException.Message);
                }
                else
                {
                    LogManager.WriteLog("Error:" + ex.Message);
                    MessageBox.Show("Error:" + ex.Message);
                }
            }
        }

        private void buttonSaveServerProperties_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigurationEntity entity = new ConfigurationEntity();
                entity.LogicalName = "ServerSettings";
                entity.Attributes = new Microsoft.Xrm.Sdk.Deployment.AttributeCollection();
                entity.Attributes.Add(new KeyValuePair<string, object>("DisableUserInfoClaim", checkBoxDisableUserInfoClaim.Checked));
                entity.Attributes.Add(new KeyValuePair<string, object>("MaxExpandCount", numericUpDownMaxExpandCount.Text));
                entity.Attributes.Add(new KeyValuePair<string, object>("MaxResultsPerCollection", numericUpDownMaxResultsPerCollection.Text));
                entity.Attributes.Add(new KeyValuePair<string, object>("NlbEnabled", checkBoxNlbEnabled.Checked));
                entity.Attributes.Add(new KeyValuePair<string, object>("PostponeAppFabricRequestsInMinutes", numericUpDownPostponeAppFabricRequestsInMinutes.Text));
                entity.Attributes.Add(new KeyValuePair<string, object>("PostViaExternalRouter", checkBoxPostViaExternalRouter.Checked));
                entity.Attributes.Add(new KeyValuePair<string, object>("SslHeader", textBoxSslHeader.Text));

                UpdateAdvancedSettingsRequest request2 = new UpdateAdvancedSettingsRequest();
                request2.Entity = entity;
                serviceClient.Execute(request2);
                toolStripStatusLabel1.Text = "Server Settings were successfully saved.";
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                //string url = connection.ServerAddress + "main.aspx?pagetype=entityrecord&etn=eaf_contrat_client&id=" + entity.Id.ToString();
                LogManager.WriteLog("Error: " + ex.Detail.Message + "\n" + ex.Detail.TraceText);
                MessageBox.Show("Error: " + ex.Detail.Message + "\n" + ex.Detail.TraceText);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    LogManager.WriteLog("Error:" + ex.Message);
                    MessageBox.Show("Error:" + ex.Message + "\n" + ex.InnerException.Message);
                }
                else
                {
                    LogManager.WriteLog("Error:" + ex.Message);
                    MessageBox.Show("Error:" + ex.Message);
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "IEXPLORE.EXE";
            startInfo.Arguments = "https://github.com/zoranivanov/MSCRMToolKit/wiki#deploymentproperties";
            Process.Start(startInfo);
        }
    }
}