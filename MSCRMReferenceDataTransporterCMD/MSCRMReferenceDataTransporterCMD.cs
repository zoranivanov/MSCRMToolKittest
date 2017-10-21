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
using System.IO;

namespace MSCRMToolKit
{
    internal class MSCRMReferenceDataTransporterCMD
    {
        private static void Main(string[] args)
        {
            //Set the application directory as the current directory
            string appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            appPath = appPath.Replace("file:\\", "");
            Directory.SetCurrentDirectory(appPath);

            MSCRMTransportationProfilesManager man = new MSCRMTransportationProfilesManager();
            string selectedTransportationProfileName = "";
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
                foreach (TransportationProfile profile in man.Profiles)
                {
                    Console.WriteLine(tpCpt + ". " + profile.ProfileName);
                    tpCpt++;
                }

                String input = Console.ReadLine();
                if (input == String.Empty)
                {
                    input = "1";
                }
                int tpNumber;
                Int32.TryParse(input, out tpNumber);
                if (tpNumber > 0 && tpNumber <= man.Profiles.Count)
                {
                    selectedTransportationProfileName = man.Profiles[tpNumber - 1].ProfileName;
                }
                else
                {
                    Console.WriteLine("The specified does not exist.");
                    return;
                }
            }
            else
            {
                //Check that the Profile name is provided
                if (string.IsNullOrEmpty(args[0]))
                    return;
                selectedTransportationProfileName = args[0];
            }

            TransportationProfile p = man.GetProfile(selectedTransportationProfileName);
            if (p == null)
            {
                Console.WriteLine("The specified Profile does not exist.");
                return;
            }

            man.RunProfile(p);
        }
    }
}