using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Produtor.Model
{
    public class Produto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public int Id { get; set; }

        [BsonElement("Nome")]
        public string nome { get; set; }

        [BsonElement("preco")]
        public double preco { get; set; }

        [BsonElement("estaEmPromocao")]
        public bool estaEmPromocao { get; set; }
    }
}

