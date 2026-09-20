using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InventarioArvores.Models
{
    public class Especie
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = default!;

        [BsonElement("nome_cientifico")]
        public string NomeCientifico { get; set; } = string.Empty;

        [BsonElement("nome_popular")]
        public string NomePopular { get; set; } = string.Empty;

        [BsonElement("familia")]
        public string Familia { get; set; } = string.Empty;

        [BsonElement("origem")]
        public string Origem { get; set; } = string.Empty;
    }
}
