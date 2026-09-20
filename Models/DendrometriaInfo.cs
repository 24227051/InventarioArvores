using MongoDB.Bson.Serialization.Attributes;

namespace InventarioArvores.Models
{
    public class DendrometriaInfo
    {
        [BsonElement("cap")]
        public double Cap { get; set; }

        [BsonElement("altura_total")]
        public double AlturaTotal { get; set; }

        [BsonElement("altura_comercial")]
        public double AlturaComercial { get; set; }

        [BsonElement("diametro_copa_ns")]
        public double DiametroCopaNs { get; set; }

        [BsonElement("diametro_copa_lo")]
        public double DiametroCopaLo { get; set; }
    }
}
