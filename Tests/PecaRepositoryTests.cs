using Xunit;
using OficinaEletrodomesticos.Data;
using OficinaEletrodomesticos.Models;

namespace Testes.Repository
{
    public class EstoqueRepositoryTests
    {
        [Fact]
        public void AdicionarPeca_DeveRetornarTrueQuandoAdicionadoComSucesso()
        {
            // Arrange
            var peca = new Peca
            {
                Nome = "Motor",
                Preco = 150.00m,
                Largura = 10.5m,
                Altura = 5.5m,
                Comprimento = 20.0m,
                Peso = 2.5m,
                Fabricante = "ABC Motors",
                Quantidade = 10
            };

            // Act
            bool resultado = EstoqueRepository.AdicionarPeca(peca);

            // Assert
            Assert.True(resultado);
            Assert.True(peca.Id > 0); // Verificar se o ID foi definido corretamente.
        }

        [Fact]
        public void RemoverPeca_DeveRetornarTrueQuandoRemovidoComSucesso()
        {
            // Arrange
            var peca = new Peca { Id = 12 }; // Exemplo de um ID que exisa no banco

            // Act
            bool resultado = EstoqueRepository.RemoverPeca(peca);

            // Assert
            Assert.True(resultado);
        }

        [Fact]
        public void ConsultarEstoque_DeveRetornarListaDePecas()
        {
            // Act
            List<Peca> estoque = EstoqueRepository.ConsultarEstoque();

            // Assert
            Assert.NotNull(estoque);
            Assert.NotEmpty(estoque);
            foreach (var peca in estoque)
            {
                Assert.NotNull(peca);
                Assert.True(peca.Id > 0);
                Assert.NotNull(peca.Nome);
                Assert.True(peca.Preco > 0);
                Assert.True(peca.Quantidade >= 0);
            }
        }

        [Fact]
        public void AtualizarPeca_DeveRetornarTrueQuandoAtualizadoComSucesso()
        {
            // Arrange
            var peca = new Peca
            {
                Id = 1, // Suponha que essa peça existe no banco.
                Nome = "Motor Atualizado",
                Preco = 160.00m,
                Largura = 11.0m,
                Altura = 6.0m,
                Comprimento = 21.0m,
                Peso = 3.0m,
                Fabricante = "ABC Motors",
                Quantidade = 15
            };

            // Act
            bool resultado = EstoqueRepository.AtualizarPeca(peca);

            // Assert
            Assert.True(resultado);
        }
    }
}
