using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Network.Core.Interfaces;
using Network.Core.Models;

namespace Network.Core.Core
{
    public class HttpClientBuilder : IHttpClientBuilder
    {
        private HttpClient client;
        public HttpClientBuilder(ClientRequestInfo model)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(model.Address);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(model.MediaType));
            if(!string.IsNullOrEmpty(model.Bearer) && !string.IsNullOrEmpty(model.Token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(model.Bearer, model.Token);
            }                
        }
        public async Task<ClientResponseInfo> GetAsync(string Uri)
        {
            ClientResponseInfo result = new ClientResponseInfo();
            HttpResponseMessage response = await client.GetAsync(Uri);
            result.IsStatusCode = response.IsSuccessStatusCode;
            result.StatusCode = response.StatusCode.ToString();
            result.Content = await response.Content.ReadAsStringAsync();
            return result;
        }
        public async Task<ClientResponseInfo> PostAsync(string Uri, string JsonContent)
        {
            ClientResponseInfo result = new ClientResponseInfo();
            StringContent content = new StringContent(JsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(Uri, content);
            result.IsStatusCode = response.IsSuccessStatusCode;
            result.StatusCode = response.StatusCode.ToString();
            result.Content = await response.Content.ReadAsStringAsync();
            return result;
        }
        public async Task<ClientResponseInfo> PutAsync(string Uri, string JsonContent)
        {
            ClientResponseInfo result = new ClientResponseInfo();
            StringContent content = new StringContent(JsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(Uri, content);
            result.IsStatusCode = response.IsSuccessStatusCode;
            result.StatusCode = response.StatusCode.ToString();
            result.Content = await response.Content.ReadAsStringAsync();
            return result;
        }
        public async Task<ClientResponseInfo> DeleteAsync(string Uri)
        {
            ClientResponseInfo result = new ClientResponseInfo();            
            HttpResponseMessage response = await client.DeleteAsync(Uri);
            result.IsStatusCode = response.IsSuccessStatusCode;
            result.StatusCode = response.StatusCode.ToString();
            result.Content = await response.Content.ReadAsStringAsync();
            return result;
        }
        public async Task<ClientResponseInfo> DeleteAsJsonAsync(string Uri, string JsonContent)
        {
            ClientResponseInfo result = new ClientResponseInfo();
            HttpResponseMessage response = new HttpResponseMessage();
            StringContent content = new StringContent(JsonContent, Encoding.UTF8, "application/json");
            HttpRequestMessage request = new HttpRequestMessage
            {
                Content = content,
                Method = HttpMethod.Delete,
                RequestUri = new Uri(Uri, UriKind.Relative)
            };
            response = await client.SendAsync(request);
            result.IsStatusCode = response.IsSuccessStatusCode;
            result.StatusCode = response.StatusCode.ToString();
            result.Content = await response.Content.ReadAsStringAsync();
            return result;
        }
    }
}
