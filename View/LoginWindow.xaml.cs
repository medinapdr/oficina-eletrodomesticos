using OficinaEletrodomesticos.Data;
using OficinaEletrodomesticos.Models;
using System.Windows;

namespace OficinaEletrodomesticos.View
{
    public partial class LoginWindow : Window
    {
        private readonly ConexaoBanco _conexaoBanco;
        private readonly UsuarioRepository _usuarioRepository;

        public LoginWindow()
        {
            InitializeComponent();
            _conexaoBanco = new ConexaoBanco();
            _usuarioRepository = new UsuarioRepository(_conexaoBanco);
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            Usuario usuario = _usuarioRepository.AutenticarUsuario(username, password);

            if (usuario != null)
            {
                string tipoPessoa = usuario.PessoaAssociada.TipoPessoa;
                string cargo = tipoPessoa == "Funcionário"
                    ? ((Funcionario)usuario.PessoaAssociada).Cargo.ToString()
                    : tipoPessoa;

                new MenuWindow(_conexaoBanco, cargo).Show(); // Use _conexaoBanco aqui
                Close();
            }
            else
            {
                lblError.Text = "Usuário ou senha incorretos!";
            }
        }

        private void btnCriar_Click(object sender, RoutedEventArgs e)
        {
            new CriarAcessoWindow(_conexaoBanco).ShowDialog();
        }
    }
}
