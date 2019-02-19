using GL.Sdk.TenantId;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GL.TenantID
{
    class Program
    {
        static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        static async Task MainAsync()
        {
            var helper = new AzureTenantHelper(new HttpClient(), null);
            var id = await helper.ResolveId("mspbipro.onmicrosoft.com");
        }
    }
}
