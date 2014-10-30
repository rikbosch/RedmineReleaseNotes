using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
using CommandLine;

[assembly: AssemblyTitle("Redmine Releasenotes Generator")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("RGB+ Automatisering B.V.")]
[assembly: AssemblyProduct("Redmine Releasenotes Generator")]
[assembly: AssemblyCopyright("Copyright © RGB+ Automatisering B.V. 2014")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("29cf6959-9278-491e-a49c-ec1c269d0a95")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.1.*")]
[assembly: AssemblyFileVersion("1.0.1")]

[assembly: AssemblyLicense(
    "This is free software. You may redistribute copies of it under the terms of",
    "the MIT License <http://www.opensource.org/licenses/mit-license.php>.")]
[assembly: AssemblyUsage(
    "Usage: RedmineReleaseNotesGenerator.exe -u <redmine url> -p <projectid> -k <apikey> -v <versionname>",
    "       RedmineReleaseNotesGenerator.exe -u <redmine url> -p <projectid> -k <apikey> -v <versionname> -i <path to template file> --verbose")]
