using System.Net.Http.Headers;
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
                properties = new
                {
                    auto_offset_reset = "earliest"
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            using var response = await _httpClient.PostAsync("/query-stream", content, stoppingToken);

            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync(stoppingToken);
            using var reader = new StreamReader(stream);

            while (!reader.EndOfStream && !stoppingToken.IsCancellationRequested)
            {
                var line = await reader.ReadLineAsync();

                if (!string.IsNullOrWhiteSpace(line))
                {
                    Console.WriteLine($"[KSQL EVENT] {line}");
                }
            }
        }
    }
}
