using Xunit;
using OficinaEletrodomesticos.Data;
using OficinaEletrodomesticos.Models;

namespace Testes.Repository
{
    public class PedidoRepositoryTests
    {
        [Fact]
        public void ConsultarPedidos_DeveRetornarListaDePedidos()
        {
            // Act
            var pedidos = PedidoRepository.ConsultarPedidos();

            // Assert
            Assert.NotNull(pedidos);
            Assert.IsType<List<Pedido>>(pedidos);
            Assert.All(pedidos, p =>
            {
                Assert.NotNull(p);
                Assert.NotNull(p.Peca);
                Assert.NotNull(p.Fornecedor);
                Assert.True(p.Quantidade >= 0);
                Assert.True(p.ValorTotal >= 0);
                Assert.True(p.ValorUnitario >= 0);
            });
        }

        [Fact]
        public void AdicionarPedido_DeveAdicionarNovoPedido()
        {
            // Arrange
            var pedido = new Pedido
            {
                Peca = new Peca { Id = 1, Nome = "Peca1" },
                Quantidade = 10,
                ValorTotal = 100.00m,
                Fornecedor = "Fornecedor1",
                DataCriacao = DateTime.Now,
                ValorUnitario = 10.00m
            };

            // Act
            bool resultado = PedidoRepository.AdicionarPedido(pedido);

            // Assert
            Assert.True(resultado);
            Assert.True(pedido.Id > 0);
        }

        [Fact]
        public void ConfirmarRecebimentoPedido_DeveConfirmarRecebimentoPedido()
        {
            // Arrange
            int pedidoId = 1;
            DateTime dataRecebimento = DateTime.Now;

            // Act
            bool resultado = PedidoRepository.ConfirmarRecebimentoPedido(pedidoId, dataRecebimento);

            // Assert
            Assert.True(resultado);
        }
    }
}
