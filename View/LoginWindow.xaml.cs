using System.Windows;
using OficinaEletrodomesticos.Data;
using OficinaEletrodomesticos.Models;

namespace OficinaEletrodomesticos.View
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            Usuario usuario = UsuarioRepository.AutenticarUsuario(username, password);

            // Se o usuário for autenticado com sucesso
            if (usuario != null)
            {
                string tipoPessoa = usuario.PessoaAssociada.TipoPessoa;

                // Se for um funcionário, obtém o cargo do funcionário
                string cargo = tipoPessoa == "Funcionário"
                    ? ((Funcionario)usuario.PessoaAssociada).Cargo.ToString()
                    : tipoPessoa;

                new MenuWindow(usuario.PessoaAssociada, cargo).Show();
                // Fecha a janela de login
                Close();
            }
            else
            {
                lblError.Text = "Usuário ou senha incorretos!";
            }
        }

        private void btnCriar_Click(object sender, RoutedEventArgs e)
        {
            new CriarAcessoWindow().ShowDialog();
        }
    }
}
