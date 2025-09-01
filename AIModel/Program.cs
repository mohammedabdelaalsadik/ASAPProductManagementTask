using Qdrant.Client.Grpc;
using System.Net.Http.Json;

namespace AIModel
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            var docText = "The login API requires a POST request with username and password. " +
              "Responses include a JWT token for authenticated sessions.";


            var chunks = docText.Split(new[] { ". " }, StringSplitOptions.RemoveEmptyEntries);


            //var docs = File.ReadAllText("docs.txt");
            //var chunks = SplitIntoChunks(docs, 500); // your own helper


            var queryEmbedding = await _ollama.GetEmbeddingAsync(question);

            // 2. Search Qdrant for relevant docs
            var docs = await _qdrant.SearchAsync(queryEmbedding);

            // 3. Build prompt
            var context = string.Join("\n", docs);
            var prompt = $"Answer the question based on context:\n{context}\n\nQuestion: {question}";

            // 4. Generate answer
            var answer = await _ollama.GenerateAnswerAsync(prompt);




            Console.WriteLine("Hello, World!");
        }
    }
}
