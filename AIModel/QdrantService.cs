using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qdrant.Client;
using Qdrant.Client.Grpc;

namespace AIModel
{
 
    public class QdrantService
    {
        private readonly QdrantClient _client;

        public QdrantService()
        {
            _client = new QdrantClient("localhost", 6334);
        }

        public async Task CreateCollectionAsync()
        {
            await _client.CreateCollectionAsync("docs", 384); // 384 = dimension size of embeddings
        }

        public async Task StoreEmbeddingAsync(string id, float[] embedding, string text)
        {
            await _client.UpsertAsync("docs", new[]
            {
            new PointStruct
            {
                Id = id,
                Vectors = embedding,
                Payload = new Dictionary<string, object>
                {
                    ["text"] = text
                }
            }
        });
        }

        public async Task<List<string>> SearchAsync(float[] queryEmbedding, int topK = 3)
        {
            var results = await _client.SearchAsync("docs", queryEmbedding, limit: topK);
            return results.Select(r => r.Payload["text"].ToString()).ToList();
        }
    }

}
