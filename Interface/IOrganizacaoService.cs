using THelp_Web.Models.Domain;
using THelp_Web.Models.Input;

namespace THelp_Web.Services
{
    public interface IOrganizacaoService
    {
        Task<List<Organizacao>> GetAllAsync();
        Task<List<Organizacao>> GetAtivasAsync();
        Task<Organizacao> GetByIdAsync(int id);
        Task<Organizacao> GetByCnpjAsync(string cnpj);
        Task<Organizacao> CreateAsync(OrganizacaoInput organizacaoInput);
        Task<Organizacao> UpdateAsync(int id, OrganizacaoInput organizacaoInput);
        Task<bool> DeleteAsync(int id);
        Task<Organizacao> DesativarAsync(int id);
    }
}