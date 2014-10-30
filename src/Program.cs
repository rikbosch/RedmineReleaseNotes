using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;

using CommandLine;
using CommandLine.Text;

using Newtonsoft.Json;

using Nito.AsyncEx;



using ReleaseNotesGenerator.Redmine;

namespace ReleaseNotesGenerator
{

    class Options
    {
        [Option('u', "url", Required = true, HelpText = "The Url to your Redmine instance")]
        public string RedmineUrl { get; set; }

        [Option('p', "projectname", Required = true, HelpText = "The Redmine Project Identifier")]
        public string ProjectName { get; set; }

        [Option('k', "apikey", Required = true, HelpText = "Your Redmine API Key")]
        public string ApiKey { get; set; }

        [Option('v', "version", Required = true, HelpText = "The Version")]
        public string Version { get; set; }

        [Option("releasenotesfield",DefaultValue ="Releasenotes", HelpText = "The custom field to fetch release notes from.")]
        public string ReleasenoteField { get; set; }

        [Option("verbose",DefaultValue = null, HelpText = "Print details during execution.")]
        public bool Verbose { get; set; }

        [Option('i', "templatefile", HelpText = "Your custom templatefile")]
        public string TemplateFile { get; set; }

        [Option('o', "outputfile",DefaultValue = "ReleaseNotes.txt", HelpText = "The output file")]
        public string OutputFile { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var help = HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
         
            help.AddOptions(this);

            return help;
        }

        [ParserState]
        public IParserState LastParserState { get; set; }
    }

    internal class Program
    {
        private static int Main(string[] args)
        {
            int returnCode = 0;
            var options = new Options() { ReleasenoteField = "Releasenote", Verbose = false };

            CommandLine.Parser.Default.ParseArgumentsStrict(args,
                options,
                () =>
                {
                    Environment.Exit(-2);
                });

            Logger.VerboseEnabled = options.Verbose;

            using (var service = RedmineApiClientFactory.CreateApiClient(options))
            {
                var generator = new Generator(service, options);
                try
                {
                    AsyncContext.Run(() => generator.GenerateReleaseNotes());
                }
                catch (Exception ex)
                {

                    Logger.LogException(ex);
                    returnCode = -1;
                }
                if (Debugger.IsAttached)
                {
                    Logger.LogInfo("Press any key to exit");
                    Console.ReadKey();
                }
            }
            return returnCode;
        }
    }
}

