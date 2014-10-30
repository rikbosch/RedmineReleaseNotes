using System;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;



namespace ReleaseNotesGenerator.Redmine
{
    public interface IRedmineApi : IDisposable
    {
        Task<GetIssuesResponse> GetClosedIssuesForVersion(string project, int version, int offset = 0, int limit = 100);
        Task<GetVersionResponse> GetVersions(string project);
    }


    public class RedmineApi : IRedmineApi
    {
        private readonly HttpClient _client;

        public RedmineApi(HttpClient client)
        {
            _client = client;
        }

        public async Task<GetIssuesResponse> GetClosedIssuesForVersion(string project, int version, int offset = 0, int limit = 100)
        {
            var url =
                   string.Format(
                       "/projects/{0}/issues.json?f%5B%5D=status_id&f%5B%5D=tracker_id&f%5B%5D=fixed_version_id&op%5Bfixed_version_id%5D=%3D&op%5Bstatus_id%5D=%3D&op%5Btracker_id%5D=%3D&set_filter=1&utf8=%E2%9C%93&v%5Bfixed_version_id%5D%5B%5D={1}&v%5Bstatus_id%5D%5B%5D=10&v%5Bstatus_id%5D%5B%5D=20&v%5Bstatus_id%5D%5B%5D=12&v%5Bstatus_id%5D%5B%5D=5&v%5Btracker_id%5D%5B%5D=1&v%5Btracker_id%5D%5B%5D=2&offset={2}&limit={3}",
                     project,
                      version,
                       offset,
                       limit);

            return JsonConvert.DeserializeObject<GetIssuesResponse>(await _client.GetStringAsync(url));
        }

        public async Task<GetVersionResponse> GetVersions(string project)
        {
            var url = string.Format("/projects/{0}/versions.json", project);
            return JsonConvert.DeserializeObject<GetVersionResponse>(await _client.GetStringAsync(url));
        }

        public void Dispose()
        {
            if (_client != null)
            {
                _client.Dispose();
            }
        }
    }

}