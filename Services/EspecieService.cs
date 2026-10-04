using InventarioArvores.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace InventarioArvores.Services
{
    public class EspecieService : IEspecieService
    {
        private readonly IMongoCollection<Especie> _especiesCollection;

        public EspecieService(IMongoDatabase database)
        {
            _especiesCollection = database.GetCollection<Especie>("especies");
        }

        public async Task CreateAsync(Especie especie)
        {
            await _especiesCollection.InsertOneAsync(especie);
        }

        public async Task<IEnumerable<Especie>> GetAllAsync()
        {
            return await _especiesCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Especie?> GetByIdAsync(string id)
        {
            if (!ObjectId.TryParse(id, out _)) return null;

            return await _especiesCollection.Find(e => e.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(string id, Especie especieAtualizada)
        {
            if (!ObjectId.TryParse(id, out _)) return false;

            especieAtualizada.Id = id;
            var resultado = await _especiesCollection.ReplaceOneAsync(e => e.Id == id, especieAtualizada);
            return resultado.MatchedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            if (!ObjectId.TryParse(id, out _)) return false;

            var resultado = await _especiesCollection.DeleteOneAsync(e => e.Id == id);
            return resultado.DeletedCount > 0;
        }
    }
}
