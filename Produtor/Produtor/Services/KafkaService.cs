using Produtor.Model;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace Produtor.Services
{
    public class KafkaService
    {
        private readonly HttpClient _httpClient;


        public KafkaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:8088");
        }

        public async Task<string> SendMessage(Produto produto)
        {
            string sql = $"INSERT INTO PRODUTOS (id, nome, valor) VALUES ('{produto.id}','{produto.nome}', {produto.valor});";
            // Payload correto usando INSERT INTO
            var payload = new
            {
                ksql= sql,
                streamsProperties = new { }
            };

            // Cria o content para enviar no POST
            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/vnd.ksql.v1+json" // tipo de mídia correto para KSQLDB
            );

            try
            {
                var response = await _httpClient.PostAsync("/ksql", content);

                string responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Status: {response.StatusCode}");
                Console.WriteLine($"Resposta: {responseContent}");
                return responseContent;

            }
            catch (Exception ex)
            {
                return $"Erro ao enviar mensagem: {ex.Message}";
            }
        }
    }
}
