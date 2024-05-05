namespace MinhaAPIEstoque.Models
{
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






        // Propriedades para armazenar o token de redefinição de senha IMPLEMENTANDO AINDA
        //public string ResetToken { get; set; }
        //public DateTime? ResetTokenExpiry { get; set; }
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







    //IMPLEMENTANDO AINDA
    //public class EsqueciSenhaModel
    //{
    //    public string Email { get; set; }
    //}

    //public class ResetarSenhaModel
    //{
    //    public string Token { get; set; }
    //    public string NovaSenha { get; set; }
    //}

}
