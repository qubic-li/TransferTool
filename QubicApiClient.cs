using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace li.qubic.transfertool
{
    public class LoginRequest
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }

    public class LoginReponse
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public List<string> Privileges { get; set; }
    }

    public class QubicApiClient
    {
        private string _baseUrl;
        private string _token;

        public QubicApiClient(string baseUrl)
        {
            if (!baseUrl.EndsWith("/"))
            {
                baseUrl += "/";
            }
            _baseUrl= baseUrl;

            // login with public user
            _token = this.PostToApi<LoginReponse, LoginRequest>("auth/login", new LoginRequest()
            {
                UserName = "guest@qubic.li",
                Password = "guest13@Qubic.li",
            }).GetAwaiter().GetResult().Token;
        }

        public async Task<T?> GetFromApi<T>(string endpoint)
            where T : class, new()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _token);
                using (var response = await httpClient.GetAsync(_baseUrl + endpoint))
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        return JsonSerializer.Deserialize<T>(apiResponse, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }); ;
                    }
                    else
                    {
                        throw new Exception("Error Getting Data From API");
                    }
                }
            }
        }

        public async Task<Toutput> PostToApi<Toutput, TpostObject>(string endpoint, TpostObject postObject)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _token);
                var content = JsonContent.Create(postObject, typeof(TpostObject), new MediaTypeHeaderValue("application/json"));
                var response = await httpClient.PostAsync(_baseUrl + endpoint, content);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<Toutput>(apiResponse, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                }
                else
                {
                    throw new Exception("Error Posting Data To API");
                }
            }
        }

    }
}
