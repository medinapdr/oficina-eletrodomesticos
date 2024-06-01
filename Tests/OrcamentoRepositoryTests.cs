using Xunit;
using OficinaEletrodomesticos.Data;
using OficinaEletrodomesticos.Models;

namespace Testes.Repository
{
    public class OrcamentoRepositoryTests
    {
        [Fact]
        public void ObterClientes_DeveRetornarListaDeClientes()
        {
            // Act
            List<Cliente> clientes = OrcamentoRepository.ObterClientes();

            // Assert
            Assert.NotNull(clientes);
            Assert.NotEmpty(clientes);
            foreach (var cliente in clientes)
            {
                Assert.False(string.IsNullOrEmpty(cliente.Nome));
                Assert.False(string.IsNullOrEmpty(cliente.CPF));
                Assert.True(cliente.Id > 0);
            }
        }

        [Fact]
        public void CriarSolicitacao_DeveRetornarTrueQuandoCriadoComSucesso()
        {
            // Arrange
            string tipo = "Geladeira";
            string marca = "Electrolux";
            string descricao = "Não está refrigerando adequadamente.";
            int clienteId = 10;

            // Act
            bool resultado = OrcamentoRepository.CriarSolicitacao(tipo, marca, descricao, clienteId);

            // Assert
            Assert.True(resultado);
        }

        [Fact]
        public void AutorizarOrcamento_DeveRetornarTrueQuandoAutorizadoComSucesso()
        {
            // Arrange
            int orcamentoId = 2;

            // Act
            bool resultado = OrcamentoRepository.AutorizarOrcamento(orcamentoId);

            // Assert
            Assert.True(resultado);
        }

        [Fact]
        public void CriarOrcamento_DeveRetornarTrueQuandoCriadoComSucesso()
        {
            // Arrange
            int solicitacaoId = 6;
            decimal valorTotal = 1200.00m;
            DateTime prazoEntrega = DateTime.Now.AddDays(15);
            bool autorizado = false;
            List<(Peca, int)> pecasQuantidade = new List<(Peca, int)>
            {
                (new Peca { Id = 1, Nome = "Compressor" }, 1),
                (new Peca { Id = 2, Nome = "Termostato" }, 2)
            };

            // Act
            bool resultado = OrcamentoRepository.CriarOrcamento(solicitacaoId, valorTotal, prazoEntrega, autorizado, pecasQuantidade);

            // Assert
            Assert.True(resultado);
        }
    }
}
