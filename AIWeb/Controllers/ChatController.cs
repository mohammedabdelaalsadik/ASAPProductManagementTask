using AIModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace AIWeb.Controllers
{

    public class ChatRequest
    {
        public string Question { get; set; } = string.Empty;
    }
    [ApiController]
    [Route("api/[controller]")]
    public class ChaController : ControllerBase
    {
        private readonly OllamaService _ollama;
        private readonly QdrantService service;

        public ChaController(OllamaService ollama, QdrantService qdrant)
        {
            _ollama = ollama;
            service = qdrant;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] ChatRequest input)
        {
            //// 1. Get embedding of query
            //var queryEmbedding = await _ollama.GetEmbeddingAsync(input.Question);

            //// 2. Search Qdrant for relevant docs
            //var docs = await _qdrant.SearchAsync(queryEmbedding);

            //// 3. Build prompt
            //var context = string.Join("\n", docs);
            //var prompt = $"Answer the question based on context:\n{context}\n\nQuestion: {input.Question}";

            //// 4. Generate answer
            //var answer = await _ollama.GenerateAnswerAsync(prompt);
            //string filePath = "eCAPSe.docx";
            //string text = "";
            //using (WordprocessingDocument doc = WordprocessingDocument.Open(filePath, false))
            //{
            //    var body = doc.MainDocumentPart.Document.Body;
            //     text = body.InnerText;
            //    Console.WriteLine(text);
            //}

            //#region Upsert
            //Console.WriteLine("Creating collection...");
            //await service.CreateCollectionAsync();
            //Console.WriteLine("Collection created.");

            //// 2️⃣ Upsert long text
            //string longText = text;// "A Learning Management System (LMS) is software used to create, deliver, manage, and track online courses and training programs, facilitating learning and development for individuals and organizations in sectors like corporate training and higher education. Key functions include hosting educational content, monitoring learner progress, evaluating performance, enabling communication, and generating reports. LMS platforms help organizations deliver essential training, upskill employees, ensure compliance, and support career development, ultimately improving employee engagement, productivity, and retention.";
            //Console.WriteLine("Upserting text...");
            //await service.UpsertTextAsync(longText, chunkSize: 50); // smaller chunk for demo
            //Console.WriteLine("Text upserted.");


          //  #endregion Upsert 


            //Console.WriteLine("Enter your question:");
            string question = input.Question;// Console.ReadLine();

            // 1️⃣ Get embedding of user question
            var queryEmbedding = await service.GetEmbeddingAsync(question);

            // 2️⃣ Search Qdrant for top 5 relevant chunks
            var searchResults = await _ollama.SearchAsync(queryEmbedding, topK: 5);

            // 3️⃣ Combine the chunks as context
            string context = string.Join("\n", searchResults.ConvertAll(r =>
                r.Payload.ContainsKey("text") ? r.Payload["text"].ToString() : ""));

            // 4️⃣ Build prompt for LLM
            string prompt = $"Answer the question based on the following COE AI :\n{context}\n\nQuestion: {question}\nAnswer:";

            // 5️⃣ Call local Ollama LLM
            var answer = await _ollama.GenerateAnswerAsync(prompt);

            Console.WriteLine("\n=== AI Answer ===");
            Console.WriteLine(answer);
            var AICOEAnswer = FormatECapsText(answer);
            //return Ok(new { input.Question, answer, docs });
            return Ok(new {
                AICOEAnswer
            });
        }



        public  string FormatECapsText(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var lines = input.Split(new[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            var sb = new StringBuilder();

            foreach (var line in lines)
            {
                string trimmed = line.Trim();

                if (trimmed.StartsWith("1.") || trimmed.StartsWith("2.") || trimmed.StartsWith("3.") ||
                    trimmed.StartsWith("4.") || trimmed.StartsWith("5.") || trimmed.StartsWith("6."))
                {
                    sb.AppendLine(); // extra spacing before each step
                    sb.AppendLine(trimmed);
                }
                else
                {
                    sb.AppendLine(trimmed);
                }
            }

            return sb.ToString();
        }


       public List<string> SplitText(string text, int maxLength = 500)
        {
            var chunks = new List<string>();
            var sentences = text.Split(new[] { '.', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            var sb = new StringBuilder();
            foreach (var s in sentences)
            {
                if ((sb.Length + s.Length) > maxLength)
                {
                    chunks.Add(sb.ToString());
                    sb.Clear();
                }
                sb.Append(s).Append(". ");
            }
            if (sb.Length > 0) chunks.Add(sb.ToString());

            return chunks;
        }
      

    }
}


   
