using Microsoft.EntityFrameworkCore;
using MinhaAPIEstoque.Data;

var builder = WebApplication.CreateBuilder(args);

// Adiciona servi�os ao cont�iner.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adiciona o contexto do banco de dados como um servi�o, usando SQLite
builder.Services.AddDbContext<MeuDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configura o middleware CORS para permitir solicita��es de qualquer origem
app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Configure o pipeline de solicita��o HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

//app.UseEncoding(); // Add UTF-8 encoding to the HTTP response


app.MapControllers();

app.Run();
