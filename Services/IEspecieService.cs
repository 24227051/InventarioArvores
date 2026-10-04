using InventarioArvores.Models;

namespace InventarioArvores.Services
{
    public interface IEspecieService
    {
        Task CreateAsync(Especie especie);
        Task<IEnumerable<Especie>> GetAllAsync();
        Task<Especie?> GetByIdAsync(string id);
        Task<bool> UpdateAsync(string id, Especie especieAtualizada);
        Task<bool> DeleteAsync(string id);
    }
}
