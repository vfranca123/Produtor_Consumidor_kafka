namespace Consumidor.Model
{
    public class StreamRequest
    {
        public string NomeStream { get; set; } =string.Empty;
        public string Campos { get; set; } = string.Empty;
        public string KafkaTopic { get; set; } =string.Empty;
        public string Filtros { get; set; } =string.Empty;
    }
}
