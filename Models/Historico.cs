using OficinaEletrodomesticos.Data;

namespace OficinaEletrodomesticos.Models
{
    public class Historico(ConexaoBanco conexaoBanco)
    {
        public List<SolicitacaoOrcamento> Solicitacoes { get; } = [];
        public List<Orcamento> Orcamentos { get; } = [];
        public List<Servico> Servicos { get; } = [];

        public void AdicionarSolicitacao(SolicitacaoOrcamento solicitacao)
        {
            Solicitacoes.Add(solicitacao);
        }

        public void AdicionarOrcamento(Orcamento orcamento)
        {
            Orcamentos.Add(orcamento);
        }

        public void AdicionarServico(Servico servico)
        {
            Servicos.Add(servico);
        }
    }
}
