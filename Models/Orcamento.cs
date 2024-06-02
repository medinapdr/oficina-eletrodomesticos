namespace OficinaEletrodomesticos.Models
{
    public class Orcamento
    {
        public int Id { get; set; }
        public SolicitacaoOrcamento Solicitacao { get; set; }
        public DateTime DataOrcamento { get; set; }
        public List<(Peca, int)>? PecasNecessarias { get; set; } = [];
        public decimal ValorTotal { get; set; }
        public DateTime PrazoEntrega { get; set; }
        public bool Autorizado { get; set; }
    }
}
