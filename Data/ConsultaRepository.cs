using System.Data.SqlClient;
using OficinaEletrodomesticos.Data;
using OficinaEletrodomesticos.Models;

namespace OficinaEletrodomesticos.Data
{
    public class ConsultaRepository
    {
        public static List<SolicitacaoOrcamento> ObterSolicitacoes(int clienteId)
        {
            var solicitacoes = new List<SolicitacaoOrcamento>();
            const string query = @"SELECT so.Id, so.Descricao, a.Tipo, a.Marca, p.Nome, so.DataSolicitacao
                                    FROM SolicitacaoOrcamento so
                                    INNER JOIN Aparelho a ON so.AparelhoId = a.Id
                                    INNER JOIN Cliente c ON so.ClienteId = c.PessoaId
                                    INNER JOIN Pessoa p ON c.PessoaId = p.Id
                                    WHERE c.PessoaId = @ClienteId;";

            using (var conexao = ConexaoBanco.ConectaBanco())
            {
                conexao.Open();
                using var cmd = new SqlCommand(query, conexao);
                cmd.Parameters.AddWithValue("@ClienteId", clienteId);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    solicitacoes.Add(new SolicitacaoOrcamento
                    {
                        Id = reader.GetInt32(0),
                        Descricao = reader.GetString(1),
                        Aparelho = new Aparelho
                        {
                            Tipo = reader.GetString(2),
                            Marca = reader.GetString(3)
                        },
                        Cliente = new Cliente
                        {
                            Nome = reader.GetString(4)
                        },
                        DataSolicitacao = reader.GetDateTime(5)
                    });
                }
            }
            return solicitacoes;
        }

        public static List<Orcamento> ObterOrcamentos(int clienteId)
        {
            var orcamentos = new List<Orcamento>();
            const string query = @"SELECT o.Id, o.DataOrcamento, o.ValorTotal, o.PrazoEntrega, o.Autorizado,
                                   s.Descricao AS SolicitacaoDescricao 
                                   FROM Orcamento o
                                   JOIN SolicitacaoOrcamento s ON o.SolicitacaoId = s.Id
                                   WHERE s.ClienteId = @ClienteId;";

            using var conexao = ConexaoBanco.ConectaBanco();
            conexao.Open();

            using var cmd = new SqlCommand(query, conexao);
            cmd.Parameters.AddWithValue("@ClienteId", clienteId);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                orcamentos.Add(new Orcamento
                {
                    Id = reader.GetInt32(0),
                    DataOrcamento = reader.GetDateTime(1),
                    ValorTotal = reader.GetDecimal(2),
                    PrazoEntrega = reader.GetDateTime(3),
                    Autorizado = reader.GetBoolean(4),
                    SolicitacaoDescricao = reader.GetString(5)
                });
            }
            return orcamentos;
        }

        public static List<Servico> ObterServicos(int clienteId)
        {
            var servicos = new List<Servico>();
            const string query = @"SELECT s.Id, s.Descricao, s.ValorPagamento, s.DataPagamento, sts.Nome AS Status, p.Nome AS NomeTecnico 
                                   FROM Servico s
                                   LEFT JOIN Funcionario f ON s.TecnicoResponsavelId = f.PessoaId
                                   JOIN StatusServico sts ON s.StatusId = sts.Id
                                   JOIN Pessoa p ON s.TecnicoResponsavelId = p.Id
                                   JOIN Orcamento o ON s.OrcamentoId = o.Id
                                   JOIN SolicitacaoOrcamento so ON o.SolicitacaoId = so.Id
                                   WHERE so.ClienteId = @ClienteId;";

            using var conexao = ConexaoBanco.ConectaBanco();
            conexao.Open();

            using var cmd = new SqlCommand(query, conexao);
            cmd.Parameters.AddWithValue("@ClienteId", clienteId);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                servicos.Add(new Servico
                {
                    Id = reader.GetInt32(0),
                    Descricao = reader.GetString(1),
                    ValorPagamento = reader.IsDBNull(2) ? null : (double?)reader.GetDouble(2),
                    DataPagamento = reader.IsDBNull(3) ? null : (DateTime?)reader.GetDateTime(3),
                    Status = (StatusServico)Enum.Parse(typeof(StatusServico), reader.GetString(4)),
                    NomeTecnico = reader.IsDBNull(5) ? "" : reader.GetString(5)
                });
            }
            return servicos;
        }

        public static List<Cliente> ObterClientes()
        {
            var clientes = new List<Cliente>();
            const string query = @"SELECT p.Id, p.Nome, p.CPF, p.Telefone, p.Endereco, p.TipoPessoa
                                   FROM Cliente c
                                   JOIN Pessoa p ON c.PessoaId = p.Id;";

            using var conexao = ConexaoBanco.ConectaBanco();
            conexao.Open();

            using var cmd = new SqlCommand(query, conexao);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                clientes.Add(new Cliente
                {
                    Id = reader.GetInt32(0),
                    Nome = reader.GetString(1),
                    CPF = reader.GetString(2),
                    Telefone = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Endereco = reader.IsDBNull(4) ? null : reader.GetString(4),
                    TipoPessoa = reader.GetString(5)
                });
            }
            return clientes;
        }

        public List<Peca> ObterPecasOrcamento(int orcamentoId)
        {
            var pecas = new List<Peca>();
            const string query = @"SELECT op.OrcamentoId, op.PecaId, p.Nome, op.Quantidade
                           FROM OrcamentoPeca op
                           JOIN Peca p ON op.PecaId = p.Id
                           WHERE op.OrcamentoId = @OrcamentoId;";

            using var conexao = ConexaoBanco.ConectaBanco();
            conexao.Open();

            using var cmd = new SqlCommand(query, conexao);
            cmd.Parameters.AddWithValue("@OrcamentoId", orcamentoId);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                pecas.Add(new Peca
                {
                    Id = reader.GetInt32(1),
                    Nome = reader.GetString(2),
                    Quantidade = reader.GetInt32(3)
                });
            }
            return pecas;
        }

    }
}
