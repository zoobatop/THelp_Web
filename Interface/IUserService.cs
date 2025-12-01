using THelp_Web.Models.Domain;
using THelp_Web.Models.Input;

namespace THelp_Web.Interface
{
    public interface IUsuarioService
    {
        Task<List<Usuario>> GetAllAsync();
        Task<Usuario> GetByIdAsync(int id);
        Task<Usuario> CreateAsync(UsuarioInput usuarioInput);
        Task<Usuario> UpdateAsync(int id, UsuarioInput usuarioInput);
        Task<bool> DeleteAsync(int id);
    }
}