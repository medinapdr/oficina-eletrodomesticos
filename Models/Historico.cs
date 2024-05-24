using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OficinaEletrodomesticos.Models
{
    public class Historico
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
