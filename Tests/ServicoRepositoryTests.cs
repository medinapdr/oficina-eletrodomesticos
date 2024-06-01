using Xunit;
using OficinaEletrodomesticos.Data;
using OficinaEletrodomesticos.Models;

namespace Testes.Repository
{
    public class ServicoRepositoryTests
    {
        [Fact]
        public void ObterTodosServicos_DeveRetornarListaDeServicos()
        {
            // Act
            List<Servico> servicos = ServicoRepository.ObterTodosServicos();

            // Assert
            Assert.NotNull(servicos);
            Assert.NotEmpty(servicos);
            foreach (var servico in servicos)
            {
                Assert.True(servico.Id > 0);
                Assert.False(string.IsNullOrEmpty(servico.Descricao));
            }
        }

        [Fact]
        public void ObterTecnicos_DeveRetornarListaDeTecnicos()
        {
            // Act
            List<Funcionario> tecnicos = ServicoRepository.ObterTecnicos();

            // Assert
            Assert.NotNull(tecnicos);
            Assert.NotEmpty(tecnicos);
            foreach (var tecnico in tecnicos)
            {
                Assert.True(tecnico.Id > 0);
                Assert.False(string.IsNullOrEmpty(tecnico.Nome));
            }
        }

        [Fact]
        public void ObterServicosPorTecnico_DeveRetornarListaDeServicosParaTecnico()
        {
            // Arrange
            int tecnicoId = 11;

            // Act
            List<Servico> servicos = ServicoRepository.ObterServicosPorTecnico(tecnicoId);

            // Assert
            Assert.NotNull(servicos);
            Assert.NotEmpty(servicos);
            foreach (var servico in servicos)
            {
                Assert.True(servico.Id > 0);
                Assert.False(string.IsNullOrEmpty(servico.Descricao));
                Assert.Equal(tecnicoId, servico.TecnicoResponsavel.Id);
            }
        }

        [Fact]
        public void AtualizarStatusServico_DeveRetornarTrueQuandoAtualizadoComSucesso()
        {
            // Arrange
            int servicoId = 1;
            StatusServico novoStatus = StatusServico.Realizado;

            // Act
            bool resultado = ServicoRepository.AtualizarStatusServico(servicoId, novoStatus);

            // Assert
            Assert.True(resultado);
        }

        [Fact]
        public void ConfirmarPagamentoServico_DeveRetornarTrueQuandoConfirmadoComSucesso()
        {
            // Arrange
            int servicoId = 1;
            double? valorPagamento = 500.00;

            // Act
            bool resultado = ServicoRepository.ConfirmarPagamentoServico(servicoId, valorPagamento);

            // Assert
            Assert.True(resultado);
        }

        [Fact]
        public void AdicionarServico_DeveRetornarTrueQuandoAdicionadoComSucesso()
        {
            // Arrange
            var tecnico = new Funcionario { Id = 1, Nome = "Tecnico Teste" };
            var orcamento = new Orcamento { Id = 1 };
            var servico = new Servico
            {
                TecnicoResponsavel = tecnico,
                Orcamento = orcamento,
                Descricao = "Reparar Geladeira",
                Status = StatusServico.EmAndamento
            };

            // Act
            bool resultado = ServicoRepository.AdicionarServico(servico);

            // Assert
            Assert.True(resultado);
        }
    }
}
