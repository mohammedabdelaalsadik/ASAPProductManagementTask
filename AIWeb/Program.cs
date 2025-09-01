using AIModel;

namespace AIWeb
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddScoped<QdrantService>();
            builder.Services.AddHttpClient<OllamaService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}


/*
 docker run -p 6333:6333 -p 6334:6334 qdrant/qdrant
ollama run mistral

 ollama pull all-minilm

 
 */
