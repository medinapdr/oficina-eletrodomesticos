namespace OficinaEletrodomesticos.Models
{
    public class Servico
    {
        public Tecnico TecnicoResponsavel { get; set; }
        public Aparelho Aparelho { get; set; }
        public Orcamento OrcamentoAssociado { get; set; }
        public string Descricao { get; set; }
        public double? ValorPagamento { get; set; }
        public DateTime? DataPagamento { get; set; }
        public StatusServico Status { get; set; }

        public Servico(Orcamento orcamento, Tecnico tecnico)
        {
            OrcamentoAssociado = orcamento;
            Aparelho = orcamento.AparelhoRelacionado;
            TecnicoResponsavel = tecnico;

            orcamento.Autorizado = true;
        }
    }
}
