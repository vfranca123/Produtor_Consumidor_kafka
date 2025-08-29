using System.Text;
using System.Text.Json;

namespace Consumidor.Services
{
    public class KsqlConsumerService
    {
        private readonly HttpClient _httpClient;

        public KsqlConsumerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:8088");
        }

        public async Task ExecutePushQueryAsync(string streamName, CancellationToken stoppingToken)
        {
            var payload = new
            {
                sql = $"SELECT * FROM {streamName} EMIT CHANGES;",
                streamsProperties = new Dictionary<string, string>
                {
                    ["auto.offset.reset"] = "latest" 
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            using var request = new HttpRequestMessage(HttpMethod.Post, "/query-stream") { Content = content };
            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, stoppingToken);
            response.EnsureSuccessStatusCode();

            await using var stream = await response.Content.ReadAsStreamAsync(stoppingToken);
            using var reader = new StreamReader(stream, Encoding.UTF8);

            // 1a linha é o schema/header
            var header = await reader.ReadLineAsync();
            Console.WriteLine($"Schema: {header}");

            // Lê continuamente até cancelar
            while (!stoppingToken.IsCancellationRequested)
            {
                var line = await reader.ReadLineAsync();
                if (line is null) break;        // servidor fechou
                if (line.Length == 0) continue; // keep-alive
                Console.WriteLine($"Filtrado: {line}");
            }
        }
    }
}
