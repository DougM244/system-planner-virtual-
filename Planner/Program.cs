using Microsoft.EntityFrameworkCore;
using Planner.IRepository;
using Planner.Models;
using Planner.Repository;
using Planner.Service;
using System.Text.Json.Serialization;

// usado para configurar os servi�os e o pipeline de middleware da aplica��o.
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Adiciona suporte para controladores e views no padr�o MVC � aplica��o
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        // Adiciona suporte para enums como strings
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Add the configuration
var configuration = builder.Configuration;
// Define o caminho, "appsettings.json" como fonte de configura��o(obrigatorio)
configuration.SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
             .AddEnvironmentVariables();

// Add DbContext
builder.Services.AddDbContext<Contexto>(options =>
    options.UseSqlite(configuration.GetConnectionString("ConexaoSQLite")));

// Adicionar UsuarioService ao cont�iner
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<UsuarioService>();

// Adicionar TarefaService ao cont�iner
builder.Services.AddScoped<ITarefaRepository, TarefaRepository>();
builder.Services.AddScoped<TarefaService>();

// Adicionar MetaService ao cont�iner
builder.Services.AddScoped<IMetaRepository, MetaRepository>();
builder.Services.AddScoped<MetaService>();

// Adicionar LembreteService ao cont�iner
builder.Services.AddScoped<ILembreteRepository, LembreteRepository>();
builder.Services.AddScoped<LembreteService>();

// Adicionar RelatorioService ao cont�iner
builder.Services.AddScoped<IRelatorioRepository, RelatorioRepository>();
builder.Services.AddScoped<RelatorioService>();

// Constr�i a aplica��o com base nas configura��es feitas no builder
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//HTTP -> HTTPS
app.UseHttpsRedirection();

//Habilita o servi�o de arquivos est�ticos, servindo arquivos como imagens, CSS e JavaScript da pasta wwwroot
app.UseStaticFiles();


// Habilita o roteamento, que define como as solicita��es HTTP s�o mapeadas para os controladores e a��es correspondentes.
app.UseRouting();


// Habilita a autentica��o e autoriza��o
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();