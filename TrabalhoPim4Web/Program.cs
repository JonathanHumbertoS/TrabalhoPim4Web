// No arquivo Program.cs
using TrabalhoPim4Web.DataAccess; // Adicionar este using
// ...

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. Obter a String de Conexão
// No arquivo Program.cs
var connectionString = builder.Configuration.GetConnectionString("PostgreConnection")
    ?? throw new InvalidOperationException("A string de conexão 'PostgreConnection' não foi encontrada.");
// 2. Registrar os DAOs (Services)
builder.Services.AddScoped<UsuarioDAO>(s => new UsuarioDAO(connectionString));
builder.Services.AddScoped<ChamadoDAO>(s => new ChamadoDAO(connectionString));


var app = builder.Build();
// ... (O restante do código app.UseExceptionHandler, app.UseStaticFiles, etc.) ...

app.UseRouting();

app.UseAuthorization(); // Mantenha isso

// 3. Configurar a Rota Padrão para o Login (Account/Login)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();