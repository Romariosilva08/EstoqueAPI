using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MinhaAPIEstoque.Data;
using MinhaAPIEstoque.Models;
using MinhaAPIEstoque.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;



namespace MinhaAPIEstoque.Controllers
{
    public class ProdutosController : ControllerBase
    {
        private readonly MeuDbContext _context;
        private readonly IConfiguration _configuration;

        public ProdutosController(MeuDbContext context, IConfiguration configuration)

        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet("api/produtos")]
        public async Task<ActionResult<IEnumerable<Produtos>>> GetProdutos()
        {
            var produtos = await _context.Produtos.ToListAsync();
            return Ok(produtos);
        }

        [HttpGet("api/produtos/{id}")]
        public async Task<ActionResult<Produtos>> GetProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
            {
                return NotFound();
            }

            return produto;
        }

        [HttpPost("api/produtos")]
        public async Task<ActionResult<Produtos>> CriarProduto([FromBody] Produtos produto)
        {
            if (produto == null)
            {
                return BadRequest("Os dados do produto n�o foram fornecidos.");
            }

            try
            {
                produto.DataProducao = ConversorData.ConverterData(produto.DataProducao.Value.ToString("dd/MM/yyyy"));

                _context.Produtos.Add(produto);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetProduto), new { id = produto.Id }, produto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro ao criar o produto: {ex.Message}");
            }
        }

        [HttpDelete("api/produtos/{id}")]
        public async Task<ActionResult> DeletarProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
            {
                return NotFound();
            }

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpPut("api/produtos/{id}")]
        public async Task<ActionResult> PutProduto(int id, [FromBody] Produtos produto)
        {
            if (produto == null)
            {
                return BadRequest("O produto n�o foi enviado corretamente.");
            }

            if (id <= 0)
            {
                return BadRequest("O ID do produto deve ser maior que zero.");
            }

            if (_context == null)
            {
                return StatusCode(500, "Ocorreu um erro ao conectar ao banco de dados.");
            }

            var produtoExistente = await _context.Produtos.FindAsync(id);

            if (produtoExistente == null)
            {
                return NotFound("Produto n�o encontrado.");
            }

            try
            {
                produtoExistente.Nome = produto.Nome;
                produtoExistente.Categoria = produto.Categoria;
                produtoExistente.DataProducao = produto.DataProducao;
                produtoExistente.Preco = produto.Preco;
                produtoExistente.Quantidade = produto.Quantidade;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro ao atualizar o produto: {ex.Message}");
            }
        }


        //ainda implementando
        //[HttpPost("api/produtos/comprar/{id}")]
        //public async Task<ActionResult> ComprarProduto(int id, [FromBody] DetalhesCompra detalhes)
        //{
        //    var produto = await _context.Produtos.FindAsync(id);

        //    if (produto == null)
        //    {
        //        return NotFound();
        //    }

        //    // Verificar se h� estoque suficiente
        //    if (produto.Quantidade < detalhes.Quantidade)
        //    {
        //        return BadRequest("Estoque insuficiente.");
        //    }

        //    // Calcular o pre�o total com base na quantidade
        //    var precoTotal = detalhes.Quantidade * produto.Preco;

        //    // Atualizar o estoque do produto
        //    produto.Quantidade -= detalhes.Quantidade;

        //    // Atualizar o pre�o total do produto
        //    produto.Total = precoTotal;

        //    await _context.SaveChangesAsync();

        //    return Ok("Compra realizada com sucesso!");
        //}


    }


    public class UsuariosController : ControllerBase
    {
        private readonly MeuDbContext _context;
        private readonly IConfiguration _configuration;


        public UsuariosController(MeuDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }

        [HttpGet("api/usuarios")]
        public async Task<ActionResult<IEnumerable<Usuarios>>> GetUsuarios()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return Ok(usuarios);
        }

        [HttpGet("api/usuarios/{id}")]
        public async Task<ActionResult<Usuarios>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        [HttpPost("api/usuarios")]
        public async Task<ActionResult<Usuarios>> CriarUsuario([FromBody] Usuarios usuario)
        {
            if (usuario == null)
            {
                return BadRequest("Os dados do usu�rio n�o foram fornecidos.");
            }

            try
            {
                // Hash the password before saving the user
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(usuario.Senha, BCrypt.Net.BCrypt.GenerateSalt());
                usuario.Senha = hashedPassword;

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro ao criar o usu�rio: {ex.Message}");
            }
        }



        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginUsuario([FromBody] Login loginModel)
        {
            if (loginModel == null || string.IsNullOrEmpty(loginModel.Email) || string.IsNullOrEmpty(loginModel.Senha))
            {
                return BadRequest("E-mail e senha s�o obrigat�rios.");
            }

            string lowerCaseEmail = loginModel.Email.Trim().ToLower();

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email.Trim().ToLower() == lowerCaseEmail /*== loginmodel.email*/);

            //var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == loginModel.Email);

            if (usuario == null)
            {
                return NotFound("Usu�rio n�o encontrado.");
            }

            // Verificar se a senha fornecida corresponde � senha armazenada (sem criptografia)
            if (!BCrypt.Net.BCrypt.Verify(loginModel.Senha, usuario.Senha))
            {
                return Unauthorized("Credenciais inv�lidas.");
            }

            // Gerar o token JWT para o usu�rio autenticado
            var token = GerarToken(usuario);

            return Ok(new { Token = token, Nome = usuario.Nome });

        }

        [HttpPost("api/produtos/comprar/{id}")]
        public async Task<ActionResult> ComprarProduto(int id, [FromBody] DetalhesCompra detalhes)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var produto = await _context.Produtos.FindAsync(id);

                    if (produto == null)
                    {
                        return NotFound("Produto n�o encontrado.");
                    }

                    if (produto.Quantidade < detalhes.Quantidade)
                    {
                        return Ok(new { Mensagem = "Estoque insuficiente. Gostaria de fazer uma encomenda?" });
                    }

                    //if (produto.Quantidade < detalhes.Quantidade)
                    //{
                    //    return BadRequest("Estoque insuficiente.");
                    //}

                    var precoTotal = detalhes.Quantidade * produto.Preco.Value;

                    produto.Quantidade -= detalhes.Quantidade;

                    var totalProduto = precoTotal;

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return Ok(new { Mensagem = "Compra realizada com sucesso!", Total = totalProduto });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(500, $"Ocorreu um erro ao processar a compra: {ex.Message}");
                }
            }
        }



        [HttpPost("api/produtos/verificar-estoque")]
        public async Task<ActionResult> VerificarEstoque()
        {
            var produtosBaixoEstoque = await _context.Produtos.Where(p => p.Quantidade < 50).ToListAsync();

            if (produtosBaixoEstoque.Count > 0)
            {
                await EnviarNotificacaoEstoque(produtosBaixoEstoque);

                return Ok("Notifica��es enviadas para administradores sobre o estoque baixo.");
            }

            return Ok("Estoque dentro dos n�veis aceit�veis.");
        }

        private async Task EnviarNotificacaoEstoque(List<Produtos> produtosBaixoEstoque)
        {
            var emailService = new EmailService(_configuration);

            foreach (var produto in produtosBaixoEstoque)
            {
                string mensagem = $"Produto {produto.Nome} est� com estoque baixo: {produto.Quantidade} unidades restantes.";
                string destinatario = "unideliciasdotrigo@outlook.com";

                await emailService.EnviarEmailAsync(destinatario, "Alerta: Produto com estoque baixo", mensagem);
            }
        }





        private string GerarToken(Usuarios usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }

}










