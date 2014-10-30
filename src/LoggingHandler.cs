using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReleaseNotesGenerator
{
    internal class LoggingHandler : DelegatingHandler
    {
        

        public LoggingHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {}

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (Logger.VerboseEnabled)
            {
                var sb = new StringBuilder();
                sb.AppendLine("Request:");
                sb.AppendLine(request.ToString());
                if (request.Content != null)
                {
                    sb.AppendLine(await request.Content.ReadAsStringAsync());
                }
                sb.AppendLine(request.Headers.ToString());
                sb.AppendLine();

                Logger.LogVerbose(sb.ToString());

                sb.Clear();
            }

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            if (Logger.VerboseEnabled)
            {
                var sb = new StringBuilder();
                sb.AppendLine("Response:");
                sb.AppendLine(response.ToString());
                if (response.Content != null)
                {
                    sb.AppendLine(await response.Content.ReadAsStringAsync());
                }
                sb.AppendLine();

                Logger.LogVerbose(sb.ToString());
            }
            return response;
        }
    }
}