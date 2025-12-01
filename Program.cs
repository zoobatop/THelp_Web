using THelp_Web.Config;
using THelp_Web.Interface;
using THelp_Web.Services;

namespace THelp_Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Env.Initialize(builder.Configuration);

            // Add services to the container.
            builder.Services.AddRazorPages();

            // Configuração do HttpClient (agora simplificada)
            builder.Services.AddHttpClient("ApiService", client =>
            {
                client.BaseAddress = new Uri(Env.ApiBaseUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            builder.Services.AddScoped<IOrganizacaoService, OrganizacaoService>();

            builder.Services.AddScoped<IUsuarioService, UsuarioService>();

            builder.Services.AddScoped<IPapelService, PapelService>();

            builder.Services.AddHttpContextAccessor();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Add services to the container.
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
