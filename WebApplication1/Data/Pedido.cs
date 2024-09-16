namespace MinhaAPIEstoque.Data
{
    public class Pedido
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime Data { get; set; }
        public List<DetalhesPedido> Detalhes { get; set; }
        public decimal Total { get; set; }
    }

    public class DetalhesPedido
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public int ProdutoId { get; set; }
        public string NomeProduto { get; set; }
        public decimal PrecoUnitario { get; set; }
        public int Quantidade { get; set; }
        public decimal Total { get; set; }
    }

    public class DetalhesCompra
    {
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
    }
}