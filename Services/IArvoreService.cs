using InventarioArvores.DTOs;
using InventarioArvores.Models;

namespace InventarioArvores.Services
{
    public interface IArvoreService
    {
        Task CreateAsync(Arvore arvore);
        Task<IEnumerable<ArvoreDetalhadaDto>> GetAllAsync();
        Task<ArvoreDetalhadaDto?> GetByIdAsync(string id);
        Task<bool> UpdateAsync(string id, Arvore arvoreAtualizada);
        Task<bool> DeleteAsync(string id);
    }
}
