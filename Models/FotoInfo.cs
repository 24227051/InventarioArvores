using MongoDB.Bson.Serialization.Attributes;

namespace InventarioArvores.Models
{
    public class FotoInfo
    {
        [BsonElement("id_foto")]
        public int IdFoto { get; set; }

        [BsonElement("url_foto")]
        public string UrlFoto { get; set; } = string.Empty;

        [BsonElement("data_registro")]
        public DateTime DataRegistro { get; set; }

        [BsonElement("tipo_foto")]
        public string TipoFoto { get; set; } = string.Empty;
    }
}
