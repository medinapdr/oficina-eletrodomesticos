namespace OficinaEletrodomesticos.Models
{
    public class SolicitacaoOrcamento
    {
        public int Id { get; set; }
        public Aparelho Aparelho { get; set; }
        public Cliente Cliente { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public string? Descricao { get; set; }
    }
}
