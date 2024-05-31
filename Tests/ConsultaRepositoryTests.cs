using Xunit;
using OficinaEletrodomesticos.Data;
using OficinaEletrodomesticos.Models;

namespace Testes.Repository
{
    public class ConsultaRepositoryTests
    {
        [Fact]
        public void ObterSolicitacoes_DeveRetornarListaDeSolicitacoesQuandoClienteExiste()
        {
            // Arrange
            int clienteIdExistente = 1;

            // Act
            List<SolicitacaoOrcamento> solicitacoes = ConsultaRepository.ObterSolicitacoes(clienteIdExistente);

            // Assert
            Assert.NotNull(solicitacoes);
            Assert.NotEmpty(solicitacoes);
            Assert.All(solicitacoes, s => Assert.NotNull(s.Cliente));
        }

        [Fact]
        public void ObterSolicitacoes_DeveRetornarListaVaziaQuandoClienteNaoExiste()
        {
            // Arrange
            int clienteIdInexistente = -1;

            // Act
            List<SolicitacaoOrcamento> solicitacoes = ConsultaRepository.ObterSolicitacoes(clienteIdInexistente);

            // Assert
            Assert.NotNull(solicitacoes);
            Assert.Empty(solicitacoes);
        }

        [Fact]
        public void ObterOrcamentos_DeveRetornarListaDeOrcamentosQuandoClienteExiste()
        {
            // Arrange
            int clienteIdExistente = 1;

            // Act
            List<Orcamento> orcamentos = ConsultaRepository.ObterOrcamentos(clienteIdExistente);

            // Assert
            Assert.NotNull(orcamentos);
            Assert.NotEmpty(orcamentos);
            Assert.All(orcamentos, o => Assert.True(o.SolicitacaoDescricao != null || o.SolicitacaoDescricao != ""));
        }

        [Fact]
        public void ObterOrcamentos_DeveRetornarListaVaziaQuandoClienteNaoExiste()
        {
            // Arrange
            int clienteIdInexistente = -1;

            // Act
            List<Orcamento> orcamentos = ConsultaRepository.ObterOrcamentos(clienteIdInexistente);

            // Assert
            Assert.NotNull(orcamentos);
            Assert.Empty(orcamentos);
        }

        [Fact]
        public void ObterServicos_DeveRetornarListaDeServicosQuandoClienteExiste()
        {
            // Arrange
            int clienteIdExistente = 1;

            // Act
            List<Servico> servicos = ConsultaRepository.ObterServicos(clienteIdExistente);

            // Assert
            Assert.NotNull(servicos);
            Assert.NotEmpty(servicos);
            Assert.All(servicos, s => Assert.NotNull(s.Orcamento.Solicitacao.Cliente));
        }

        [Fact]
        public void ObterServicos_DeveRetornarListaVaziaQuandoClienteNaoExiste()
        {
            // Arrange
            int clienteIdInexistente = -1;

            // Act
            List<Servico> servicos = ConsultaRepository.ObterServicos(clienteIdInexistente);

            // Assert
            Assert.NotNull(servicos);
            Assert.Empty(servicos);
        }

        [Fact]
        public void ObterClientes_DeveRetornarListaDeClientes()
        {
            // Act
            List<Cliente> clientes = ConsultaRepository.ObterClientes();

            // Assert
            Assert.NotNull(clientes);
            Assert.NotEmpty(clientes);
        }

        [Fact]
        public void ObterPecasOrcamento_DeveRetornarListaDePecasQuandoOrcamentoExiste()
        {
            // Arrange
            int orcamentoIdExistente = 1;

            // Act
            List<Peca> pecas = new ConsultaRepository().ObterPecasOrcamento(orcamentoIdExistente);

            // Assert
            Assert.NotNull(pecas);
            Assert.NotEmpty(pecas);
        }

        [Fact]
        public void ObterPecasOrcamento_DeveRetornarListaVaziaQuandoOrcamentoNaoExiste()
        {
            // Arrange
            int orcamentoIdInexistente = -1;

            // Act
            List<Peca> pecas = new ConsultaRepository().ObterPecasOrcamento(orcamentoIdInexistente);

            // Assert
            Assert.NotNull(pecas);
            Assert.Empty(pecas);
        }
    }
}
