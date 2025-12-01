using THelp_Web.Config;
using THelp_Web.Interface;
using THelp_Web.Models.Domain;
using THelp_Web.Models.Input;

namespace THelp_Web.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiEndpoint;

        public UsuarioService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiEndpoint = $"{Env.ApiBaseUrl}/user";
        }

        public async Task<List<Usuario>> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_apiEndpoint);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<Usuario>>() ?? new List<Usuario>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar usuários: {ex.Message}");
                return new List<Usuario>();
            }
        }

        public async Task<Usuario> GetByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiEndpoint}/{id}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Usuario>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar usuário {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<Usuario> CreateAsync(UsuarioInput usuarioInput)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_apiEndpoint, usuarioInput);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Usuario>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao criar usuário: {ex.Message}");
                return null;
            }
        }

        public async Task<Usuario> UpdateAsync(int id, UsuarioInput usuarioInput)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{_apiEndpoint}/{id}", usuarioInput);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Usuario>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar usuário {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_apiEndpoint}/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao deletar usuário {id}: {ex.Message}");
                return false;
            }
        }
    }
}