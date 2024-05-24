namespace OficinaEletrodomesticos.Models
{
    public class Usuario
    {
        public string NomeUsuario { get; set; }
        public string Senha { get; set; }
        public Pessoa PessoaAssociada { get; set; }
    }
}
