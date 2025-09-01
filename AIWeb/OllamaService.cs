using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static QdrantService;

namespace AIModel
{
    public class OllamaService
    {
        private readonly HttpClient _http;

        public OllamaService(HttpClient http)
        {
            _http = http;
            _http.BaseAddress = new Uri("http://localhost:11434/");
        }


        public class OllamaEmbeddingItem
        {
            [JsonPropertyName("embedding")]
            public float[] Embedding { get; set; }
        }

        //public async Task<float[]> GetEmbeddingAsync(string text)
        //{
        //    var payload = new
        //    {
        //        model = "all-minilm",
        //        prompt = text
        //    };

        //    var response = await _http.PostAsJsonAsync("api/embeddings", payload);
        //    response.EnsureSuccessStatusCode();

        //    var raw = await response.Content.ReadAsStringAsync();
        //    Console.WriteLine(raw); // debug

        //    // Deserialize manually
        //    var result = JsonSerializer.Deserialize<OllamaEmbeddingResponse>(raw, new JsonSerializerOptions
        //    {
        //        PropertyNameCaseInsensitive = true
        //    });

        //    // Case 1: embedding at root
        //    if (result?.Embedding != null && result.Embedding.Length > 0)
        //        return result.Embedding;

        //    // Case 2: embedding inside data array
        //    if (result?.Data != null && result.Data.Count > 0 && result.Data[0].Embedding != null)
        //        return result.Data[0].Embedding;

        //    // fallback empty array
        //    return Array.Empty<float>();
        //}

        public async Task<string> GenerateAnswerAsync(string prompt)
        {
            var payload = new
            {
                model = "mistral",   // must be a generative model
                prompt = prompt,
                max_tokens = 500,
                stream = false
            };

            var response = await _http.PostAsJsonAsync("http://localhost:11434/api/generate", payload);
            response.EnsureSuccessStatusCode();

            var raw = await response.Content.ReadAsStringAsync();
            Console.WriteLine(raw); // debug

            var result = JsonSerializer.Deserialize<OllamaGenerateResponse>(raw, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result?.Response ?? "";
        }
        public class OllamaGenerateResponse2
        {
            [JsonPropertyName("text")]
            public string Text { get; set; }
        }


        public async Task<List<QdrantSearchResult>> SearchAsync(float[] queryEmbedding, int topK = 5)
        {
            var payload = new
            {
                vector = queryEmbedding,
                top = topK,
                with_payload = true
            };

            var response = await _http.PostAsJsonAsync(
                "http://localhost:6333/collections/myapp1/points/search",
                payload
            );
            response.EnsureSuccessStatusCode();

            var raw = await response.Content.ReadAsStringAsync();
            var wrapper = JsonSerializer.Deserialize<QdrantSearchResponse>(raw, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return wrapper?.Result ?? new List<QdrantSearchResult>();
        }

        public class QdrantSearchResponse
        {
            [JsonPropertyName("result")]
            public List<QdrantSearchResult> Result { get; set; }
        }

        public class OllamaEmbeddingResponse
        {
            public float[] Embedding { get; set; }
        }

        public class OllamaGenerateResponse
        {
            public string Response { get; set; }
        }
    }
}
