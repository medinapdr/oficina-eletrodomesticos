using OficinaEletrodomesticos.Data;
using OficinaEletrodomesticos.Models;
using System.Windows;

namespace OficinaEletrodomesticos.View
{
    public partial class LoginWindow : Window
    {
        private readonly ConexaoBanco conexaoBanco;

        public LoginWindow()
        {
            InitializeComponent();
            conexaoBanco = new ConexaoBanco();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            Usuario usuario = conexaoBanco.AutenticarUsuario(username, password);

            if (usuario != null)
            {
                string tipoPessoa = usuario.PessoaAssociada.TipoPessoa;
                string cargo = tipoPessoa == "Funcionário"
                    ? ((Funcionario)usuario.PessoaAssociada).Cargo.ToString()
                    : tipoPessoa;

                new MenuWindow(cargo).Show();
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
