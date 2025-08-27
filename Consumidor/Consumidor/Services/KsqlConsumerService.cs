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
            _httpClient.BaseAddress = new Uri("http://localhost:8088/query-stream"); 
        }

        public async Task ExecutePushQueryAsync(string streamName, CancellationToken stoppingToken)
        {
            var payload = new
            {
                sql = $"SELECT * FROM {streamName} EMIT CHANGES;",
                streamsProperties = new
                {
                    auto_offset_reset = "earliest" 
                }
            };


            try
            {
                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                using var response = await _httpClient.PostAsync("/query-stream", content, stoppingToken);
                response.EnsureSuccessStatusCode();

                using var stream = await response.Content.ReadAsStreamAsync(stoppingToken);
                using var reader = new StreamReader(stream);


                var header = await reader.ReadLineAsync();
                Console.WriteLine($"Schema: {header}");

                while (!reader.EndOfStream && !stoppingToken.IsCancellationRequested)
                {
                    var line = await reader.ReadLineAsync();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    // Cada linha é um JSON
                    var jsonDoc = JsonDocument.Parse(line);
                    var record = jsonDoc.RootElement;

                    Console.WriteLine($"Filtrado: {line}");
                }
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("Push query cancelled.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing push query: {ex.Message}");
            }
        }
    }
}
