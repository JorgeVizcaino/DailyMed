using DailyMed.Core.Interfaces;
using DailyMed.Core.Models.CopayCard;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DailyMed.Infrastructure.AI
{
    public class OpenAIApiService : IGenerativeAIService
    {
        private readonly string _apiKey;
        private readonly IHttpClientFactory _httpClientFactory;

        public OpenAIApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _apiKey = configuration["OpenAI:ApiKey"];
        }

        public async Task<List<string>> InferCoverageEligibilityAsync(string freeText, IPromptStrategy promptStrategy)
        {
            var prompt = promptStrategy.BuildPrompt(freeText);
            var responseContent = await SendPromptToOpenAIAsync(prompt);
            return JsonSerializer.Deserialize<List<string>>(responseContent
                , new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<List<RequirementDto>> InferRequirementsFromEligibilityAsync(string freeText, IPromptStrategy promptStrategy)
        {
            var prompt = promptStrategy.BuildPrompt(freeText);
            var responseContent = await SendPromptToOpenAIAsync(prompt);
            return JsonSerializer.Deserialize<List<RequirementDto>>(responseContent
                    , new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        private async Task<string> SendPromptToOpenAIAsync(string prompt)
        {
            var client = _httpClientFactory.CreateClient("APIOpenAI");

            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "user", content = prompt }
                },
                temperature = 0.7
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "chat/completions")
            {
                Headers = { { "Authorization", $"Bearer {_apiKey}" } },
                Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
            };

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return ParseOpenAIResponse(jsonResponse);
        }

        private string ParseOpenAIResponse(string jsonResponse)
        {
            using var doc = JsonDocument.Parse(jsonResponse);
            var response = doc.RootElement
                      .GetProperty("choices")[0]
                      .GetProperty("message")
                      .GetProperty("content")
                      .GetString();
            return response;
        }

        //private readonly string _apiKey;
        //private readonly IHttpClientFactory _httpClient;


        //public OpenAIApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        //{
        //    _httpClient = httpClientFactory;
        //    _apiKey = configuration["OpenAI:ApiKey"];

        //}

        //public async Task<List<string>> InferCoverageEligibilityAsync(string freeText, IPromptStrategy promptStrategy)
        //{
        //    var prompt = promptStrategy.BuildPrompt(freeText);

        //    var httpClientOpenAI = _httpClient.CreateClient("APIOpenAI");


        //    var request = new HttpRequestMessage(HttpMethod.Post, "chat/completions");
        //    request.Headers.Add("Authorization", $"Bearer {_apiKey}");

        //    var requestBody = new
        //    {
        //        model = "gpt-3.5-turbo",
        //        messages = new[]
        //        {
        //            new { role = "user", content = prompt }
        //        },
        //        temperature = 0.7
        //    };

        //    string json = JsonSerializer.Serialize(requestBody);
        //    request.Content = new StringContent(json, Encoding.UTF8, "application/json");

        //    var response = await httpClientOpenAI.SendAsync(request);
        //    response.EnsureSuccessStatusCode();

        //    var jsonResponse = await response.Content.ReadAsStringAsync();
        //    using var doc = JsonDocument.Parse(jsonResponse);
        //    var openAiResponse = doc.RootElement
        //              .GetProperty("choices")[0]
        //              .GetProperty("message")
        //              .GetProperty("content")
        //              .GetString();

        //    return JsonSerializer.Deserialize<List<string>>(openAiResponse);
        //}
        //public async Task<List<RequirementDto>> InferRequirementsFromEligibilityAsync(string freeText, IPromptStrategy promptStrategy)
        //{

        //    var prompt = promptStrategy.BuildPrompt(freeText);

        //    var httpClientOpenAI = _httpClient.CreateClient("APIOpenAI");


        //    var request = new HttpRequestMessage(HttpMethod.Post, "chat/completions");
        //    request.Headers.Add("Authorization", $"Bearer {_apiKey}");

        //    var requestBody = new
        //    {
        //        model = "gpt-3.5-turbo",
        //        messages = new[]
        //        {
        //            new { role = "user", content = prompt }
        //        },
        //        temperature = 0.7
        //    };

        //    string json = JsonSerializer.Serialize(requestBody);
        //    request.Content = new StringContent(json, Encoding.UTF8, "application/json");

        //    var response = await httpClientOpenAI.SendAsync(request);
        //    response.EnsureSuccessStatusCode();

        //    var jsonResponse = await response.Content.ReadAsStringAsync();
        //    using var doc = JsonDocument.Parse(jsonResponse);
        //    var openAiResponse = doc.RootElement
        //              .GetProperty("choices")[0]
        //              .GetProperty("message")
        //              .GetProperty("content")
        //              .GetString();

        //    return JsonSerializer.Deserialize<List<RequirementDto>>(openAiResponse);


        //}
    }
}
