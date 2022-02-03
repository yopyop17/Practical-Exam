using Exam_Project.Api.Commands;
using Exam_Project.Api.Constants;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Exam_Project.Api.Helpers
{
    public class HeaderPair
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public static class HttpRequestHelper
    {
        public static async Task<T> GetAsync<T>(string uri, string token = null)
        {
            var client = new HttpClient();
            client.SetBearerToken(token);
            var response = await client.GetAsync(uri);
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseBody);
        }
        public static async Task<T> PostAsync<T>(string uri, string token, object body = null, HttpRequest request = null)
        {
            var client = new HttpClient();
            client.SetBearerToken(token);
            if (request != null && request.Headers["sub"].Count > 0)
            {
                var value = request.Headers["sub"][0] as string;
                client.SetHeaders(new HeaderPair() { Key = "sub", Value = value });
            }
            StringContent jsonBody = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(uri, jsonBody);
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        public static void SetBearerToken(this HttpClient client, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public static void SetHeaders(this HttpClient client, HeaderPair header)
        {
            client.DefaultRequestHeaders.Add(header.Key, header.Value);
        }

        public static async Task<T> Auth<T>(string uri, LoginMediaCommand command, string client_password)
        {
            var client = new HttpClient();
            var multiForm = new MultipartFormDataContent();


            client.SetHeaders(new HeaderPair() { Key = "client_id", Value = "m2m" });
            multiForm.Add(new StringContent("appClient"), "client_id");
            multiForm.Add(new StringContent(client_password), "client_secret");
            multiForm.Add(new StringContent("password"), "grant_type");
            multiForm.Add(new StringContent(command.Username), "username");
            multiForm.Add(new StringContent(command.Password), "password");



            if (!string.IsNullOrWhiteSpace(command?.MediaToken)) multiForm.Add(new StringContent(command?.MediaToken), LoginData.GoogleAccessToken);
            if (!string.IsNullOrWhiteSpace(command?.MediaID)) multiForm.Add(new StringContent(command?.MediaID), LoginData.Google_ID);
            if (!string.IsNullOrWhiteSpace(command?.MediaPicture)) multiForm.Add(new StringContent(command?.MediaPicture), LoginData.Google_DP);



            var response = await client.PostAsync(uri, multiForm);
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

    }
}
