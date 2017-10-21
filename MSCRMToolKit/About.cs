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
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace MSCRMToolKit
{
    /// <summary>
    /// About class
    /// </summary>
    public partial class About : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="About"/> class.
        /// </summary>
        public About()
        {
            InitializeComponent();

            Assembly ThisAssembly = Assembly.GetExecutingAssembly();
            AssemblyName ThisAssemblyName = ThisAssembly.GetName();

            Array Attributes = ThisAssembly.GetCustomAttributes(false);

            string Title = "MSCRM ToolKit";
            string Copyright = "Zoran IVANOV";
            string Description = "";
            string Company = "";
            foreach (object o in Attributes)
            {
                if (o is AssemblyTitleAttribute)
                    Title = ((AssemblyTitleAttribute)o).Title;
                else if (o is AssemblyCopyrightAttribute)
                    Copyright = ((AssemblyCopyrightAttribute)o).Copyright;
                else if (o is AssemblyCompanyAttribute)
                    Company = ((AssemblyCompanyAttribute)o).Company;
                else if (o is AssemblyDescriptionAttribute)
                    Description = ((AssemblyDescriptionAttribute)o).Description;
            }

            this.Text = "About " + Title;
            labelTitle.Text = Title;
            labelAuthor.Text = "by " + Company;
            labelVersion.Text = "Version: " + ThisAssemblyName.Version;
            textBoxDescription.Text = Description;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "IEXPLORE.EXE";
            startInfo.Arguments = "https://github.com/zoranivanov/MSCRMToolKit";
            Process.Start(startInfo);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "IEXPLORE.EXE";
            startInfo.Arguments = "https://github.com/zoranivanov/MSCRMToolKit/wiki";
            Process.Start(startInfo);
        }
    }
}