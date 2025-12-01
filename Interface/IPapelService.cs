using THelp_Web.Models.Domain;
using THelp_Web.Models.Input;

namespace THelp_Web.Interface
{
    public interface IPapelService
    {
        Task<List<Papel>> GetAllAsync();
        Task<Papel> GetByIdAsync(int id);
        Task<Papel> GetByNomeAsync(string nome);
        Task<List<Papel>> SearchAsync(string nome);
        Task<Papel> CreateAsync(PapelInput papelInput);
        Task<Papel> CreateIfNotExistsAsync(string nome, string? descricao);
        Task<Papel> UpdateAsync(int id, PapelInput papelInput);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsByNomeAsync(string nome);
    }
}