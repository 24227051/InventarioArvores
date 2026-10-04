using InventarioArvores.DTOs;
using InventarioArvores.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace InventarioArvores.Services
{
    public class ArvoreService : IArvoreService
    {
        private readonly IMongoCollection<Arvore> _arvoresCollection;
        private readonly IMongoCollection<Especie> _especiesCollection;

        public ArvoreService(IMongoDatabase database)
        {
            _arvoresCollection = database.GetCollection<Arvore>("arvores");
            _especiesCollection = database.GetCollection<Especie>("especies");
        }

        public async Task CreateAsync(Arvore arvore)
        {
            await _arvoresCollection.InsertOneAsync(arvore);
        }

        public async Task<IEnumerable<ArvoreDetalhadaDto>> GetAllAsync()
        {
            var documentos = await _arvoresCollection
                .Aggregate()
                .Lookup("especies", "especie_id", "_id", "especie")
                .Project(new BsonDocument
                {
                    { "_id", 1 },
                    { "especie_id", 1 },
                    { "data_registro", 1 },
                    { "status_viva", 1 },
                    { "localizacao", 1 },
                    { "dendrometria", 1 },
                    { "fotos", 1 },
                    { "laudos_tecnicos", 1 },
                    { "especie", new BsonDocument("$arrayElemAt", new BsonArray { "$especie", 0 }) }
                })
                .ToListAsync();

            var arvores = documentos
                .Select(doc => BsonSerializer.Deserialize<ArvoreDetalhadaDto>(doc))
                .ToList();

            return arvores;
        }

        public async Task<ArvoreDetalhadaDto?> GetByIdAsync(string id)
        {
            if (!ObjectId.TryParse(id, out _)) return null;

            var documento = await _arvoresCollection
                .Aggregate()
                .Match(a => a.Id == id)
                .Lookup("especies", "especie_id", "_id", "especie")
                .Project(new BsonDocument
                {
                    { "_id", 1 },
                    { "especie_id", 1 },
                    { "data_registro", 1 },
                    { "status_viva", 1 },
                    { "localizacao", 1 },
                    { "dendrometria", 1 },
                    { "fotos", 1 },
                    { "laudos_tecnicos", 1 },
                    { "especie", new BsonDocument("$arrayElemAt", new BsonArray { "$especie", 0 }) }
                })
                .FirstOrDefaultAsync();

            if (documento == null) return null;

            return BsonSerializer.Deserialize<ArvoreDetalhadaDto>(documento);
        }

        public async Task<bool> UpdateAsync(string id, Arvore arvoreAtualizada)
        {
            if (!ObjectId.TryParse(id, out _)) return false;

            arvoreAtualizada.Id = id;
            var resultado = await _arvoresCollection.ReplaceOneAsync(e => e.Id == id, arvoreAtualizada);
            return resultado.MatchedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            if (!ObjectId.TryParse(id, out _)) return false;

            var resultado = await _arvoresCollection.DeleteOneAsync(e => e.Id == id);
            return resultado.DeletedCount > 0;
        }
    }
}
