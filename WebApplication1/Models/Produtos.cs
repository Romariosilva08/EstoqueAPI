namespace MinhaAPIEstoque.Models;

public class Produtos
{
    public int? Id { get; set; }
    public string? Nome { get; set; }
    public string? Categoria { get; set; }
    public DateTime? DataProducao { get; set; }
    public decimal? Preco { get; set; }
    public int? Quantidade { get; set; }
    //public decimal Total { get; set; } 

    public string Status
    {
        get
        {
            if (Quantidade > 100)
            {
                return "Estoque alto";
            }
            else if (Quantidade >= 50)
            {
                return "Estoque médio";
            }
            else
            {
                return "Estoque baixo";
            }
        }
    }
}


public class Usuarios
{
    public int Id { get; set; } 

    public string Nome { get; set; }

    public string Email { get; set; }

    public string Senha { get; set; }



}


public class Login
{
    public string Email { get; set; }
    public string Senha { get; set; }
}


public class LoginResponse
{
    public string Token { get; set; }
}

