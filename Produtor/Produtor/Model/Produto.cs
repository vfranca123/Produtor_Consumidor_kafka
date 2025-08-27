using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Produtor.Model
{
    public class Produto
    {

        public string Id { get; set; }

        public string nome { get; set; }


        public double valor { get; set; }

        public bool estaEmPromocao { get; set; }
    }
}

