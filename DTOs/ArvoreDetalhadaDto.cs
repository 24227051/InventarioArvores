using InventarioArvores.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InventarioArvores.DTOs
{
    public class ArvoreDetalhadaDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = default!;

        [BsonElement("especie_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string EspecieId { get; set; } = default!;

        [BsonElement("data_registro")]
        public DateTime DataRegistro { get; set; }

        [BsonElement("status_viva")]
        public bool StatusViva { get; set; }

        [BsonElement("localizacao")]
        public LocalizacaoGeoJson Localizacao { get; set; } = new();

        [BsonElement("dendrometria")]
        public DendrometriaInfo Dendrometria { get; set; } = new();

        [BsonElement("fotos")]
        public List<FotoInfo> Fotos { get; set; } = new();

        [BsonElement("laudos_tecnicos")]
        public List<LaudoTecnicoInfo> LaudosTecnicos { get; set; } = new();

        [BsonElement("especie")]
        public Especie? Especie { get; set; }
    }
}
