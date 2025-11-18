using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using THelp_Web.Models.API;

namespace THelp_Web.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiConfig _config;
        private readonly ILogger<ApiService> _logger;

        public ApiService(HttpClient httpClient, IOptions<ApiConfig> config, ILogger<ApiService> logger)
        {
            _httpClient = httpClient;
            _config = config.Value;
            _logger = logger;

            // Configura o HttpClient com as opções do JSON
            _httpClient.BaseAddress = new Uri(_config.BaseUrl);
            _httpClient.Timeout = TimeSpan.FromSeconds(_config.TimeoutSeconds);
        }

        /// <summary>
        /// Verifica se a API está respondendo
        /// </summary>
        public async Task<bool> PingAsync()
        {
            try
            {
                _logger.LogInformation($"Testando conexão com: {_config.BaseUrl}");

                var response = await _httpClient.GetAsync("/api/health");

                _logger.LogInformation($"Status da API: {response.StatusCode}");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Erro de conexão: {ex.Message}");
                return false;
            }
            catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
            {
                _logger.LogError($"Timeout ao conectar com a API: {_config.BaseUrl}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro inesperado: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Obtém informações da conexão atual
        /// </summary>
        public ApiStatus GetApiStatus()
        {
            return new ApiStatus
            {
                BaseUrl = _config.BaseUrl,
                Timeout = _config.TimeoutSeconds,
                LastChecked = DateTime.Now
            };
        }

        /// <summary>
        /// Testa conexão com um endpoint específico
        /// </summary>
        public async Task<ConnectionResult> TestConnectionAsync()
        {
            var result = new ConnectionResult
            {
                BaseUrl = _config.BaseUrl,
                TestTime = DateTime.Now
            };

            try {
                var startTime = DateTime.Now;
                var response = await _httpClient.GetAsync("/api/health");
                var endTime = DateTime.Now;

                result.ResponseTimeMs = (int)(endTime - startTime).TotalMilliseconds;
                result.StatusCode = (int)response.StatusCode;
                result.IsSuccess = response.IsSuccessStatusCode;
                result.Message = response.IsSuccessStatusCode ?
                    "Conexão estabelecida com sucesso!" :
                    $"Erro: {response.StatusCode}";
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = $"Falha na conexão: {ex.Message}";
                result.ResponseTimeMs = -1;
                result.StatusCode = -1;
            }

            return result;
        }
    }

}