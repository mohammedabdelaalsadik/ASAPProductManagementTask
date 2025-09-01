using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

public class QdrantService
{
    private readonly HttpClient _httpClient;
    private readonly HttpClient _ollamaClient;
    private readonly string _qdrantBaseUrl = "http://localhost:6333"; // REST port
    private readonly string _ollamaBaseUrl = "http://localhost:11434/api/embeddings"; // Ollama embeddings API
    private readonly string _collectionName = "myapp1";
    private readonly int _embeddingSize = 384;

    public QdrantService()
    {
        _httpClient = new HttpClient();
        _ollamaClient = new HttpClient();
    }

    // ======== Collection ========
    public async Task CreateCollectionAsync()
    {
        var payload = new
        {
            vectors = new
            {
                size = _embeddingSize,
                distance = "Cosine"
            }
        };

        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"{_qdrantBaseUrl}/collections/{_collectionName}", content);
        response.EnsureSuccessStatusCode();
    }

    // ======== Embedding ========
    public async Task<float[]> GetEmbeddingAsync(string text)
    {
        var payload = new { model = "all-minilm", prompt = text };
        var response = await _ollamaClient.PostAsJsonAsync(_ollamaBaseUrl, payload);
        response.EnsureSuccessStatusCode();

        var raw = await response.Content.ReadAsStringAsync();
        Console.WriteLine(raw); // Debug

        var result = JsonSerializer.Deserialize<OllamaEmbeddingResponse>(raw, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (result?.Embedding != null && result.Embedding.Length > 0)
            return result.Embedding;

        if (result?.Data != null && result.Data.Count > 0 && result.Data[0].Embedding != null)
            return result.Data[0].Embedding;

        return Array.Empty<float>();
    }

    // ======== Upsert points ========
    public async Task UpsertPointsAsync(List<QdrantPoint> points)
    {
        var payload = new QdrantUpsertRequest { Points = points };
        var response = await _httpClient.PutAsJsonAsync($"{_qdrantBaseUrl}/collections/{_collectionName}/points", payload);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpsertTextAsync(string longText, int chunkSize = 500)
    {
        var textChunks = SplitIntoChunks(longText, chunkSize);
        var points = new List<QdrantPoint>();

        foreach (var chunk in textChunks)
        {
            var embedding = await GetEmbeddingAsync(chunk);
            points.Add(new QdrantPoint
            {
                Id = Guid.NewGuid().ToString(),
                Payload = new Dictionary<string, object> { { "text", chunk } },
                Vector = embedding
            });
        }

        await UpsertPointsAsync(points);
    }

    // ======== Search ========
    public async Task<List<QdrantSearchResult>> SearchAsync(string userQuery, int topK = 5)
    {
        var embedding = await GetEmbeddingAsync(userQuery);
        var payload = new
        {
            vector = embedding,
            top = topK,
            with_payload = true
        };

        var response = await _httpClient.PostAsJsonAsync(
            $"{_qdrantBaseUrl}/collections/{_collectionName}/points/search",
            payload
        );

        response.EnsureSuccessStatusCode();
        var raw = await response.Content.ReadAsStringAsync();

        var searchResponse = JsonSerializer.Deserialize<QdrantSearchResponse>(raw, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return searchResponse?.Result ?? new List<QdrantSearchResult>();
    }

    // ======== Helpers ========
    public List<string> SplitIntoChunks(string text, int chunkSize = 500)
    {
        var words = text.Split(' ');
        var chunks = new List<string>();
        for (int i = 0; i < words.Length; i += chunkSize)
            chunks.Add(string.Join(" ", words.Skip(i).Take(chunkSize)));
        return chunks;
    }

    // ======== Data Models ========
    public class OllamaEmbeddingResponse
    {
        [JsonPropertyName("embedding")]
        public float[] Embedding { get; set; }

        [JsonPropertyName("data")]
        public List<OllamaEmbeddingItem> Data { get; set; }
    }

    public class OllamaEmbeddingItem
    {
        [JsonPropertyName("embedding")]
        public float[] Embedding { get; set; }
    }

    public class QdrantPoint
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("vector")]
        public float[] Vector { get; set; }

        [JsonPropertyName("payload")]
        public Dictionary<string, object> Payload { get; set; }
    }

    public class QdrantUpsertRequest
    {
        [JsonPropertyName("points")]
        public List<QdrantPoint> Points { get; set; }
    }

    public class QdrantSearchResult
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("score")]
        public float Score { get; set; }

        [JsonPropertyName("payload")]
        public Dictionary<string, object> Payload { get; set; }
    }

    public class QdrantSearchResponse
    {
        [JsonPropertyName("result")]
        public List<QdrantSearchResult> Result { get; set; }
    }
}
