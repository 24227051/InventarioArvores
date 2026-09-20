using MongoDB.Bson.Serialization.Attributes;

namespace InventarioArvores.Models
{
    public class LaudoTecnicoInfo
    {
        [BsonElement("id_laudo")]
        public int IdLaudo { get; set; }

        [BsonElement("data_inspecao")]
        public string DataInspecao { get; set; } = string.Empty;

        [BsonElement("responsavel_tecnico")]
        public string ResponsavelTecnico { get; set; } = string.Empty;

        [BsonElement("condicao_sanitaria")]
        public string CondicaoSanitaria { get; set; } = string.Empty;

        [BsonElement("risco_queda")]
        public string RiscoQueda { get; set; } = string.Empty;

        [BsonElement("recomendacao_manejo")]
        public string RecomendacaoManejo { get; set; } = string.Empty;
    }
}
