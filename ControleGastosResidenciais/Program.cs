using ControleGastosResidenciais.Data;
using ControleGastosResidenciais.Interfaces;
using ControleGastosResidenciais.Repositories;
using Microsoft.EntityFrameworkCore;

//Criação do builder da aplicação.
var builder = WebApplication.CreateBuilder(args);

//Configuração dos serviços da aplicação.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        //Configuração do JSON para o retorno das respostas da API.
        //Converte os enum para string.
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        //Configura o nome das propriedades para camelCase.
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });
//Configuração dos endpoints da API.
builder.Services.AddEndpointsApiExplorer();

//Configuração do Swagger para a documentação da API.
builder.Services.AddSwaggerGen();

//Configuração do CORS para permitir requisições externas.
builder.Services.AddCors(options =>
{
    //Configuração do CORS para permitir requisições externas.
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

//Configuração do banco de dados SQLite.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

//Configuração dos repositórios.
builder.Services.AddScoped<IPessoaRepository, PessoaRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<ITransacaoRepository, TransacaoRepository>();

//Criação da aplicação.
var app = builder.Build();

// Configuração do pipeline HTTP
// IMPORTANTE: A ordem do middleware é crucial
// CORS deve vir antes de UseAuthorization e UseHttpsRedirection
app.UseCors("AllowReactApp");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Remover UseHttpsRedirection para evitar problemas com HTTP local
// app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

//Criação do banco de dados.
try
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.EnsureCreated();
        Console.WriteLine("Banco de dados inicializado com sucesso!");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Erro ao inicializar banco de dados: {ex.Message}");
}

app.Run();
