namespace OficinaEletrodomesticos.Models
{
    public class Pedido(List<Peca> pecas, decimal valorTotal, string fornecedor)
    {
        public List<Peca> Pecas { get; } = pecas;
        public decimal ValorTotal { get; } = valorTotal;
        public string Fornecedor { get; } = fornecedor;
    }
}
