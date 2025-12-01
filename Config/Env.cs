namespace THelp_Web.Config
{
    public static class Env
    {
        private static IConfiguration _configuration;

        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static string ApiBaseUrl => _configuration?["ApiBaseUrl"] ??
                                         "http://localhost:8080/api";

        public static string ConnectionString => _configuration?.GetConnectionString("DefaultConnection") ??
                                               string.Empty;

        // Você pode adicionar outras configurações aqui
        public static string Environment => _configuration?["ASPNETCORE_ENVIRONMENT"] ??
                                          "Production";

        public static bool IsDevelopment => Environment == "Development";

        // Método para obter qualquer configuração
        public static string GetValue(string key, string defaultValue = "")
        {
            return _configuration?[key] ?? defaultValue;
        }
    }
}
