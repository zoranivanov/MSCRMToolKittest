// ========================================================================================
//  This file is part of the MSCRM ToolKit project.
//  https://github.com/zoranivanov/MSCRMToolKit
//  Author:         Zoran IVANOV
//  Created:        31/12/2013
//
//  Disclaimer:
//  This software is provided "as is" with no technical support.
//  Use it at your own risk.
//  The author does not take any responsibility for any damage in whatever form or context.
// ========================================================================================

using System;
using System.IO;

namespace MSCRMToolKit
{
    internal class MSCRMNtoNAssociationsTransportManagerCMD
    {
        private static void Main(string[] args)
        {
            //Set the application directory as the current directory
            string appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            appPath = appPath.Replace("file:\\", "");
            Directory.SetCurrentDirectory(appPath);

            MSCRMNtoNAssociationsTransportManager man = new MSCRMNtoNAssociationsTransportManager();
            string selectedProfileName = "";
            if (args.Length == 0)
            {
                if (man.Profiles.Count == 0)
                {
                    Console.WriteLine("\nNo profiles found.");
                    return;
                }

                //Display all profiles for selection
                Console.WriteLine("\nSpecify the Profile to run (1-{0}) [1] : ", man.Profiles.Count);
                int tpCpt = 1;
                foreach (NtoNAssociationsTransportProfile profile in man.Profiles)
                {
                    Console.WriteLine(tpCpt + ". " + profile.ProfileName);
                    tpCpt++;
                }

                String input = Console.ReadLine();
                if (input == String.Empty)
                {
                    input = "1";
                }
                int depNumber;
                Int32.TryParse(input, out depNumber);
                if (depNumber > 0 && depNumber <= man.Profiles.Count)
                {
                    selectedProfileName = man.Profiles[depNumber - 1].ProfileName;
                }
                else
                {
                    Console.WriteLine("The specified Profile does not exist.");
                    return;
                }
            }
            else
            {
                //Check that the Profile name is provided
                if (string.IsNullOrEmpty(args[0]))
                    return;
                selectedProfileName = args[0];
            }

            NtoNAssociationsTransportProfile p = man.GetProfile(selectedProfileName);
            if (p == null)
            {
                Console.WriteLine("The specified Profile does not exist.");
                return;
            }

            man.RunProfile(p);
        }
    }
}