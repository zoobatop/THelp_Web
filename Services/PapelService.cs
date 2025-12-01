using THelp_Web.Config;
using THelp_Web.Interface;
using THelp_Web.Models.Domain;
using THelp_Web.Models.Input;

namespace THelp_Web.Services
{
    public class PapelService : IPapelService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiEndpoint;

        public PapelService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiEndpoint = $"{Env.ApiBaseUrl}/papeis";
        }

        public async Task<List<Papel>> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_apiEndpoint);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<Papel>>>();
                return result?.Data ?? new List<Papel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar papéis: {ex.Message}");
                return new List<Papel>();
            }
        }

        public async Task<Papel> GetByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiEndpoint}/{id}");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<Papel>>();
                return result?.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar papel {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<Papel> GetByNomeAsync(string nome)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiEndpoint}/nome/{nome}");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<Papel>>();
                return result?.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar papel por nome {nome}: {ex.Message}");
                return null;
            }
        }

        public async Task<List<Papel>> SearchAsync(string nome)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiEndpoint}/buscar?nome={Uri.EscapeDataString(nome)}");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<Papel>>>();
                return result?.Data ?? new List<Papel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar papéis com termo {nome}: {ex.Message}");
                return new List<Papel>();
            }
        }

        public async Task<Papel> CreateAsync(PapelInput papelInput)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_apiEndpoint, papelInput);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<Papel>>();
                return result?.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao criar papel: {ex.Message}");
                return null;
            }
        }

        public async Task<Papel> CreateIfNotExistsAsync(string nome, string? descricao)
        {
            try
            {
                var queryParams = $"?nome={Uri.EscapeDataString(nome)}";
                if (!string.IsNullOrEmpty(descricao))
                {
                    queryParams += $"&descricao={Uri.EscapeDataString(descricao)}";
                }

                var response = await _httpClient.PostAsync($"{_apiEndpoint}/criar-se-nao-existir{queryParams}", null);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<Papel>>();
                return result?.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao criar/recuperar papel {nome}: {ex.Message}");
                return null;
            }
        }

        public async Task<Papel> UpdateAsync(int id, PapelInput papelInput)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{_apiEndpoint}/{id}", papelInput);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<Papel>>();
                return result?.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar papel {id}: {ex.Message}");
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
                Console.WriteLine($"Erro ao deletar papel {id}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ExistsByNomeAsync(string nome)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiEndpoint}/verificar/{nome}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse<Dictionary<string, bool>>>();
                    return result?.Data?.ContainsKey("existe") == true && result.Data["existe"];
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao verificar existência do papel {nome}: {ex.Message}");
                return false;
            }
        }
    }
}