using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GL.Sdk.TenantId
{
    public class AzureTenantHelper
    {
        private readonly HttpClient m_client;
        private readonly ILogger m_logger;

        public AzureTenantHelper(HttpClient client, ILogger logger)
        {
            m_client = client ?? throw new ArgumentNullException(nameof(client));
            m_logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public AzureTenantHelper(HttpClient client)
        {
            m_client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<Guid> ResolveId(string tenantName)
        {
            var tenantId = Guid.Empty;

            try
            {
                var url = $"https://login.windows.net/{tenantName}/.well-known/openid-configuration";
                var jsonString = await m_client.GetStringAsync(url);
                var jObject = JObject.Parse(jsonString);

                if (jObject.TryGetValue("issuer", out JToken issuer))
                {
                    var issuerTokenValue = issuer.ToString();
                    var split = issuerTokenValue.Split('/');

                    var textId = split[3];
                    tenantId = Guid.Parse(textId);
                }
            }
            catch (Exception ex)
            {
                m_logger?.LogError(ex.Message, ex);
            }

            return tenantId;
        }
    }
}
