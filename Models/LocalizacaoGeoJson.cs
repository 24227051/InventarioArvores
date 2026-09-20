using MongoDB.Bson.Serialization.Attributes;

namespace InventarioArvores.Models
{
    public class LocalizacaoGeoJson
    {
        [BsonElement("type")]
        public string Type { get; set; } = "Point";

        [BsonElement("coordinates")]
        public double[] Coordinates { get; set; } = new double[2]; // [longitude, latitude]

        [BsonElement("endereco_aproximado")]
        public string EnderecoAproximado { get; set; } = string.Empty;
    }
}
