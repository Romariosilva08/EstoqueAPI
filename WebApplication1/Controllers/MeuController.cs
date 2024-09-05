using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MinhaAPIEstoque.Data;
using MinhaAPIEstoque.Models;
using MinhaAPIEstoque.Utils;



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
                return BadRequest("Os dados do produto não foram fornecidos.");
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
                return BadRequest("O produto não foi enviado corretamente.");
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
                return NotFound("Produto não encontrado.");
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

        //    // Verificar se há estoque suficiente
        //    if (produto.Quantidade < detalhes.Quantidade)
        //    {
        //        return BadRequest("Estoque insuficiente.");
        //    }

        //    // Calcular o preço total com base na quantidade
        //    var precoTotal = detalhes.Quantidade * produto.Preco;

        //    // Atualizar o estoque do produto
        //    produto.Quantidade -= detalhes.Quantidade;

        //    // Atualizar o preço total do produto
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
                return BadRequest("Os dados do usuário não foram fornecidos.");
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
                return StatusCode(500, $"Ocorreu um erro ao criar o usuário: {ex.Message}");
            }
        }



        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginUsuario([FromBody] Login loginModel)
        {
            if (loginModel == null || string.IsNullOrEmpty(loginModel.Email) || string.IsNullOrEmpty(loginModel.Senha))
            {
                return BadRequest("E-mail e senha são obrigatórios.");
            }

            string lowerCaseEmail = loginModel.Email.Trim().ToLower();

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email.Trim().ToLower() == lowerCaseEmail /*== loginmodel.email*/);

            //var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == loginModel.Email);

            if (usuario == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            // Verificar se a senha fornecida corresponde à senha armazenada (sem criptografia)
            if (!BCrypt.Net.BCrypt.Verify(loginModel.Senha, usuario.Senha))
            {
                return Unauthorized("Credenciais inválidas.");
            }

            // Gerar o token JWT para o usuário autenticado
            var token = GerarToken(usuario);

            return Ok(new { Token = token, Nome = usuario.Nome });

            //return Ok(new LoginResponse { Token = token });
        }










        [HttpPost("api/usuarios/esqueci-senha")]
        public async Task<ActionResult> EsqueciSenha([FromBody] EsqueciSenhaModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Email))
            {
                return BadRequest("E-mail é obrigatório.");
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email.Trim().ToLower() == model.Email.Trim().ToLower());

            if (usuario == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            // Gerar um token de redefinição de senha
            string resetToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));

            // Armazenar o token e a data de expiração no banco de dados
            usuario.ResetToken = resetToken;
            usuario.ResetTokenExpiry = DateTime.UtcNow.AddHours(1);
            await _context.SaveChangesAsync();

            // EnviarEmailRedefinicaoSenha(usuario.Email, resetToken);

            return Ok("Token de redefinição de senha enviado para o seu e-mail.");
        }


        //private string GenerateSimpleToken(string userEmail)
        //{
        //    // Gerar um token usando o e-mail do usuário
        //    string token = Convert.ToBase64String(Encoding.UTF8.GetBytes(userEmail));

        //    // Retornar o token gerado
        //    return token;
        //}




















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










