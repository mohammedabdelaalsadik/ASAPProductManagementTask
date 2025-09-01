using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<float[]> GetEmbeddingAsync(string text)
        {
            var response = await _http.PostAsJsonAsync("api/embeddings", new
            {
                model = "mistral", // or "llama2"
                input = text
            });

            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<OllamaEmbeddingResponse>();
            return result?.Embedding ?? Array.Empty<float>();
        }

        public async Task<string> GenerateAnswerAsync(string prompt)
        {
            var response = await _http.PostAsJsonAsync("api/generate", new
            {
                model = "mistral",
                prompt = prompt,
                stream = false
            });

            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<OllamaGenerateResponse>();
            return result?.Response ?? "";
        }
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
