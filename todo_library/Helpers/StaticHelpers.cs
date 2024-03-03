using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace todo_library.Helpers
{
    public static class StaticHelpers
    {
        public static HttpClient Create()
        {
            var client = new HttpClient();

            client.BaseAddress = new Uri("https://localhost:7007/api/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        public static void SetAuthorizationHeader(HttpClient client,string token)
        {

            if (!string.IsNullOrWhiteSpace(token))
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }
        }
    }
}
