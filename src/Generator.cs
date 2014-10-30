using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



using Nustache.Core;

using ReleaseNotesGenerator.Redmine;

namespace ReleaseNotesGenerator
{
    internal class Generator
    {
        private readonly IRedmineApi _api;

        private readonly Options _options;

        public Generator(IRedmineApi api, Options options)
        {
            this._api = api;
            this._options = options;
        }


        public async Task GenerateReleaseNotes()
        {
            Logger.LogInfo("Fetching versions for project {0}", this._options.ProjectName);
            var versions = await this._api.GetVersions(this._options.ProjectName);
            Logger.LogInfo("Found {0} versions, searching for {1}", versions.total_count, this._options.Version);
            var targetVersion =
                versions.versions.SingleOrDefault(
                    x => String.Equals(x.name, this._options.Version, StringComparison.InvariantCultureIgnoreCase));

            if (targetVersion == null)
            {
                Logger.LogError("Could not find target version {0} for project {1}",
                    this._options.Version,
                    this._options.ProjectName);
                return;
            }

            Logger.LogInfo("Found version {0} : {1} (id: {2}, project: {3}, date: {4})", targetVersion.name, targetVersion.description, targetVersion.id, targetVersion.project.name, targetVersion.due_date);


            //we use raw httpclient for this..
            //refit does something weird with encoding..
            var releaseNotes = new List<ReleaseNote>();

            int offset = 0;
            int pageSize = 100;

            var currentPage = 0;
            var pages = 1;
            do
            {
                offset = currentPage * pageSize;
                GetIssuesResponse issues;
                if (_options.QueryId.HasValue)
                {
                    Logger.LogInfo("Using queryId {0} on project {1}",_options.QueryId,_options.ProjectName);
                    issues = await this._api.GetIssuesByQueryId(this._options.ProjectName, _options.QueryId.Value, offset, pageSize);
                }
                else
                {
                    issues = await this._api.GetClosedIssuesForVersion(this._options.ProjectName, targetVersion.id, offset, pageSize);
                }
        
                var total = issues.total_count;

                Logger.LogInfo("Fetched {0} issues, total {1}, current page {2}", issues.issues.Length, total, currentPage + 1);

                pages = (int)Math.Ceiling((double)total / (double)pageSize);


                releaseNotes.AddRange(issues.issues.Select(x => this.CreateReleaseNote(x, this._options)));

                currentPage++;

            }
            while (currentPage < pages);

            var templateData = ReleaseNotes.Create(releaseNotes, targetVersion);
            //and now process the template for release notes
            var template = this.GetTemplateAsString(this._options);

            var fi = new FileInfo(this._options.OutputFile);

            if (fi.Exists)
            {
                Logger.LogWarning("outputfile {0} already exists, it will be overwritten.", fi.Name);
            }

       
            var result = Render.StringToString(template,templateData);

            Logger.LogVerbose("Write to {0}\r\n{1}", fi.FullName, result);

            File.WriteAllText(fi.FullName, result);

            Logger.LogInfo("Releasenotes written to {0}", fi.FullName);

        }

        private string GetTemplateAsString(Options options)
        {
            if (!string.IsNullOrEmpty(options.TemplateFile))
            {
                Logger.LogInfo("Using template from path {0}", options.TemplateFile);

                var fi = new FileInfo(options.TemplateFile);
                // Try to return a flat file from the same directory, if it doesn't
                // exist, use the built-in resource version
                if (fi.Exists)
                {
                    using (var s = fi.OpenText())
                    {
                        return s.ReadToEnd();
                    }
                }
                Logger.LogWarning("Could not find template at {0}, loading default template.", options.TemplateFile);
            }


            //template does not exist
            using (
                var src =
                    typeof(Program).Assembly.GetManifestResourceStream("ReleaseNotesGenerator.DefaultTemplate.mustache")
                )
            {
                var ms = new MemoryStream();
                src.CopyTo(ms);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        private ReleaseNote CreateReleaseNote(Issue issue, Options options)
        {
            var note = new ReleaseNote();
            if (issue.custom_fields !=null)
            {
                note.Description =
                    issue.custom_fields.Where(
                        x =>
                            string.Equals(x.name, options.ReleasenoteField, StringComparison.InvariantCultureIgnoreCase))
                        .Select(x => x.value)
                        .SingleOrDefault();

                note.CustomFields = issue.custom_fields.ToDictionary(x => x.name, x => x.value);
            }

            note.Subject = issue.subject;
            note.IssueNumber = issue.id;
            if (issue.category != null)
            {
                note.Category = issue.category.name;
            }
            else
            {
                note.Category = string.Empty;
            }

            note.Source = issue;
         
       

            return note;
        }

        public class ReleaseNotes
        {
            public Redmine.Version Version { get; set; }

            public CategorieGroup[] ByCategory { get; set; }

            public TrackerGroup[] ByTracker { get; set; }

            public IList<ReleaseNote> All { get; set; }

            public static ReleaseNotes Create(IList<ReleaseNote> notes, Redmine.Version version)
            {
                var r = new ReleaseNotes();

                var categorien = notes.GroupBy(x => new { Categorie = x.Category, x.Source.category });

                r.ByCategory = categorien.Select(
                    x =>
                        new CategorieGroup()
                        {
                            Name = x.Key.Categorie,
                            Source = x.Key.category,
                            TrackerGroup =
                                x.GroupBy(map => new { map.Source.tracker.name, map.Source.tracker })
                                    .Select(
                                        map =>
                                            new TrackerGroup()
                                            {
                                                Name = map.Key.name,
                                                Source = map.Key.tracker,
                                                ReleaseNotes = map.OrderBy(m=>m.IssueNumber).ToList()
                                            }).ToArray()
                        }).OrderBy(p=>p.Name).ToArray();

                r.ByTracker =
                    notes.GroupBy(x => new { x.Source.tracker.name, x.Source.tracker })
                        .Select(
                            x =>
                                new TrackerGroup()
                                {
                                    Name = x.Key.name,
                                    Source = x.Key.tracker,
                                    ReleaseNotes = x.ToList()
                                })
                        .ToArray();

                r.All = notes;
                r.Version = version;
                return r;

            }
        }

        public class CategorieGroup
        {
            public string Name { get; set; }
            public Category Source { get; set; }

            public IList<TrackerGroup> TrackerGroup { get; set; }
        }

        public class TrackerGroup
        {
            public string Name { get; set; }
            public Tracker Source { get; set; }
            public IList<ReleaseNote> ReleaseNotes { get; set; }
        }

        public class ReleaseNote
        {
            public string Description { get; set; }

            public string Subject { get; set; }

            public string Category { get; set; }

            public int IssueNumber { get; set; }

            public Issue Source { get; set; }

            public IDictionary<string, string> CustomFields { get; set; }
        }
    }
}

