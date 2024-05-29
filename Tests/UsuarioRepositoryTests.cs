using Xunit;
using OficinaEletrodomesticos.Data;
using OficinaEletrodomesticos.Models;

namespace Testes.Repository
{
    public class UsuarioRepositoryTests
    {
        [Fact]
        public void AutenticarUsuario_DeveRetornarUsuarioQuandoAutenticadoComSucesso()
        {
            // Arrange
            string nomeUsuario = "testuser";
            string senha = "testpassword";

            // Act
            Usuario usuario = UsuarioRepository.AutenticarUsuario(nomeUsuario, senha);

            // Assert
            Assert.NotNull(usuario);
            Assert.Equal(nomeUsuario, usuario.NomeUsuario);
        }

        [Fact]
        public void AutenticarUsuario_DeveRetornarNullQuandoCredenciaisIncorretas()
        {
            // Arrange
            string nomeUsuario = "invaliduser";
            string senha = "invalidpassword";

            // Act
            Usuario usuario = UsuarioRepository.AutenticarUsuario(nomeUsuario, senha);

            // Assert
            Assert.Null(usuario);
        }
    }
}