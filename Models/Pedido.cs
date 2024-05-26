namespace OficinaEletrodomesticos.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public Peca Peca { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal ValorTotal { get; set; }
        public int Quantidade { get; set; }
        public string Fornecedor { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataRecebimento { get; set; }

    }
}
