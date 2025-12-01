using THelp_Web.Config;
using THelp_Web.Models.Domain;
using THelp_Web.Models.Input;

namespace THelp_Web.Services
{
    public class OrganizacaoService : IOrganizacaoService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiEndpoint;

        public OrganizacaoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiEndpoint = $"{Env.ApiBaseUrl}/organizacao";
        }

        public async Task<List<Organizacao>> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_apiEndpoint);
                response.EnsureSuccessStatusCode();

                // A API retorna um objeto com data, então precisamos extrair
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<Organizacao>>>();
                return result?.Data ?? new List<Organizacao>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar organizações: {ex.Message}");
                return new List<Organizacao>();
            }
        }

        public async Task<List<Organizacao>> GetAtivasAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiEndpoint}/ativas");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<Organizacao>>>();
                return result?.Data ?? new List<Organizacao>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar organizações ativas: {ex.Message}");
                return new List<Organizacao>();
            }
        }

        public async Task<Organizacao> GetByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiEndpoint}/{id}");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<Organizacao>>();
                return result?.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar organização {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<Organizacao> GetByCnpjAsync(string cnpj)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiEndpoint}/cnpj/{cnpj}");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<Organizacao>>();
                return result?.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar organização por CNPJ {cnpj}: {ex.Message}");
                return null;
            }
        }

        public async Task<Organizacao> CreateAsync(OrganizacaoInput organizacaoInput)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_apiEndpoint, organizacaoInput);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<Organizacao>>();
                return result?.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao criar organização: {ex.Message}");
                return null;
            }
        }

        public async Task<Organizacao> UpdateAsync(int id, OrganizacaoInput organizacaoInput)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{_apiEndpoint}/{id}", organizacaoInput);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<Organizacao>>();
                return result?.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar organização {id}: {ex.Message}");
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
                Console.WriteLine($"Erro ao deletar organização {id}: {ex.Message}");
                return false;
            }
        }

        public async Task<Organizacao> DesativarAsync(int id)
        {
            try
            {
                var response = await _httpClient.PutAsync($"{_apiEndpoint}/{id}/desativar", null);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<Organizacao>>();
                return result?.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao desativar organização {id}: {ex.Message}");
                return null;
            }
        }
    }

    // Classe para deserializar a resposta da API
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T Data { get; set; }
    }
}