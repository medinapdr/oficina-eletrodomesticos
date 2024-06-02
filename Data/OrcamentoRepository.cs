using System.Data;
using System.Data.SqlClient;
using OficinaEletrodomesticos.Models;

namespace OficinaEletrodomesticos.Data
{
    public static class OrcamentoRepository
    {
        public static List<Cliente> ObterClientes()
        {
            var clientes = new List<Cliente>();
            const string query = @"SELECT PessoaId, CPF, Nome FROM Cliente JOIN Pessoa ON Cliente.PessoaId = Pessoa.Id";

            using (var conexao = ConexaoBanco.ConectaBanco())
            {
                conexao.Open();
                using var cmd = new SqlCommand(query, conexao);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    clientes.Add(new Cliente
                    {
                        Id = reader.GetInt32(0),
                        CPF = reader.GetString(1),
                        Nome = reader.GetString(2)
                    });
                }
            }
            return clientes;
        }

        // Método para obter as solicitações de orçamento, opcionalmente filtradas por pelo Id do Cliente
        public static List<SolicitacaoOrcamento> ObterSolicitacoes(int? clienteId = null)
        {
            var solicitacoes = new List<SolicitacaoOrcamento>();
            string query = @"
                SELECT so.Id, so.Descricao, a.Tipo, a.Marca, p.Nome, p.CPF, so.DataSolicitacao
                FROM SolicitacaoOrcamento so
                INNER JOIN Aparelho a ON so.AparelhoId = a.Id
                INNER JOIN Cliente c ON so.ClienteId = c.PessoaId
                INNER JOIN Pessoa p ON c.PessoaId = p.Id";

            if (clienteId.HasValue)
            {
                query += " WHERE c.PessoaId = @ClienteId";
            }

            using var conexao = ConexaoBanco.ConectaBanco();
            conexao.Open();
            using var cmd = new SqlCommand(query, conexao);

            if (clienteId.HasValue)
            {
                cmd.Parameters.AddWithValue("@ClienteId", clienteId.Value);
            }

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
                    Nome = reader.GetString(4),
                    CPF = reader.GetString(5)
                },
                    DataSolicitacao = reader.GetDateTime(6)
                });
            }
            return solicitacoes;
        }

        public static List<Peca> ObterPecas()
        {
            var pecas = new List<Peca>();
            const string query = @"SELECT Id, Nome, Preco, Largura, Altura, Comprimento, Peso, Fabricante, Quantidade FROM Peca";

            using var conexao = ConexaoBanco.ConectaBanco();
            conexao.Open();

            using var cmd = new SqlCommand(query, conexao);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                pecas.Add(new Peca
                {
                    Id = reader.GetInt32(0),
                    Nome = reader.GetString(1),
                    Preco = reader.GetDecimal(2),
                    // Verificação de nulidade dos campos opcionais
                    Largura = reader.IsDBNull(3) ? (decimal?)null : reader.GetDecimal(3),
                    Altura = reader.IsDBNull(4) ? (decimal?)null : reader.GetDecimal(4),
                    Comprimento = reader.IsDBNull(5) ? (decimal?)null : reader.GetDecimal(5),
                    Peso = reader.IsDBNull(6) ? (decimal?)null : reader.GetDecimal(6),
                    Fabricante = reader.GetString(7),
                    Quantidade = reader.GetInt32(8)
                });
            }
            return pecas;
        }

        public static List<Orcamento> ObterOrcamentos()
        {
            var orcamentos = new List<Orcamento>();
            const string query = @"
                SELECT o.Id, o.DataOrcamento, o.ValorTotal, o.PrazoEntrega, o.Autorizado, a.Tipo AS TipoAparelho,
                a.Marca AS MarcaAparelho,s.Descricao AS DescricaoSolicitacao, p.Nome AS NomeCliente, p.CPF AS CPFCliente
                FROM Orcamento o
                JOIN SolicitacaoOrcamento s ON o.SolicitacaoId = s.Id
                JOIN Aparelho a ON s.AparelhoId = a.Id
                JOIN Cliente c ON a.ClienteId = c.PessoaId
                JOIN Pessoa p ON c.PessoaId = p.Id";

            using var conexao = ConexaoBanco.ConectaBanco();
            conexao.Open();

            using var cmd = new SqlCommand(query, conexao);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Cliente cliente = new Cliente
                {
                    Nome = reader.GetString("NomeCliente"),
                };

                Aparelho aparelho = new Aparelho
                {
                    Tipo = reader.GetString("TipoAparelho"),
                    Marca = reader.GetString("MarcaAparelho"),
                    ClienteAssociado = cliente
                };

                SolicitacaoOrcamento solicitacao = new SolicitacaoOrcamento
                {
                    Descricao = reader.GetString("DescricaoSolicitacao"),
                    Cliente = cliente,
                    Aparelho = aparelho
                };

                orcamentos.Add(new Orcamento
                {
                    Id = reader.GetInt32("Id"),
                    DataOrcamento = reader.GetDateTime("DataOrcamento"),
                    ValorTotal = reader.GetDecimal("ValorTotal"),
                    PrazoEntrega = reader.GetDateTime("PrazoEntrega"),
                    Autorizado = reader.GetBoolean("Autorizado"),
                    Solicitacao = solicitacao
                });
            }
            return orcamentos;
        }

        public static Orcamento ObterUltimoOrcamento()
        {
            const string query = @"
                SELECT TOP 1 o.Id, o.DataOrcamento, o.ValorTotal, o.PrazoEntrega, o.Autorizado,
                s.Id AS SolicitacaoId, s.Descricao AS DescricaoSolicitacao, a.Tipo AS TipoAparelho,
                a.Marca AS MarcaAparelho, p.Nome AS NomeCliente, p.CPF AS CPFCliente
                FROM Orcamento o
                JOIN SolicitacaoOrcamento s ON o.SolicitacaoId = s.Id
                JOIN Aparelho a ON s.AparelhoId = a.Id
                JOIN Cliente c ON a.ClienteId = c.PessoaId
                JOIN Pessoa p ON c.PessoaId = p.Id
                ORDER BY o.Id DESC";

            using var conexao = ConexaoBanco.ConectaBanco();
            conexao.Open();

            using var cmd = new SqlCommand(query, conexao);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                Cliente cliente = new Cliente
                {
                    Nome = reader.GetString("NomeCliente"),
                    CPF = reader.GetString("CPFCliente")
                };

                Aparelho aparelho = new Aparelho
                {
                    Tipo = reader.GetString("TipoAparelho"),
                    Marca = reader.GetString("MarcaAparelho"),
                    ClienteAssociado = cliente
                };

                SolicitacaoOrcamento solicitacao = new SolicitacaoOrcamento
                {
                    Id = reader.GetInt32("SolicitacaoId"),
                    Descricao = reader.GetString("DescricaoSolicitacao"),
                    Cliente = cliente,
                    Aparelho = aparelho
                };

                return new Orcamento
                {
                    Id = reader.GetInt32("Id"),
                    DataOrcamento = reader.GetDateTime("DataOrcamento"),
                    ValorTotal = reader.GetDecimal("ValorTotal"),
                    PrazoEntrega = reader.GetDateTime("PrazoEntrega"),
                    Autorizado = reader.GetBoolean("Autorizado"),
                    Solicitacao = solicitacao
                };
            }

            return null;
        }


        public static bool CriarSolicitacao(string tipo, string marca, string descricao, int clienteId)
        {
            const string aparelhoQuery = @"
                INSERT INTO Aparelho (ClienteId, Tipo, Marca) VALUES (@ClienteId, @Tipo, @Marca);
                SELECT SCOPE_IDENTITY();";

            const string solicitacaoQuery = @"
                INSERT INTO SolicitacaoOrcamento (AparelhoId, ClienteId, Descricao, DataSolicitacao)
                VALUES (@AparelhoId, @ClienteId, @Descricao, GETDATE())";

            using var conexao = ConexaoBanco.ConectaBanco();
            conexao.Open();

            using var transaction = conexao.BeginTransaction();
            try
            {
                int aparelhoId;
                using (var cmdAparelho = new SqlCommand(aparelhoQuery, conexao, transaction))
                {
                    cmdAparelho.Parameters.AddWithValue("@ClienteId", clienteId);
                    cmdAparelho.Parameters.AddWithValue("@Tipo", tipo);
                    cmdAparelho.Parameters.AddWithValue("@Marca", marca);
                    aparelhoId = Convert.ToInt32(cmdAparelho.ExecuteScalar());
                }

                // Cria a solicitação associada ao aparelho
                using (var cmdSolicitacao = new SqlCommand(solicitacaoQuery, conexao, transaction))
                {
                    cmdSolicitacao.Parameters.AddWithValue("@AparelhoId", aparelhoId);
                    cmdSolicitacao.Parameters.AddWithValue("@ClienteId", clienteId);
                    cmdSolicitacao.Parameters.AddWithValue("@Descricao", descricao);
                    cmdSolicitacao.ExecuteNonQuery();
                }

                transaction.Commit();

                return true;
            }
            catch (SqlException)
            {
                transaction.Rollback();
                return false;
            }
        }

        public static bool AutorizarOrcamento(int orcamentoId)
        {
            const string query = @"UPDATE Orcamento SET Autorizado = 1 WHERE Id = @Id";

            using var conexao = ConexaoBanco.ConectaBanco();
            conexao.Open();
            using var cmd = new SqlCommand(query, conexao);

            cmd.Parameters.AddWithValue("@Id", orcamentoId);

            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }
        public static bool CriarOrcamento(int solicitacaoId, decimal valorTotal, DateTime prazoEntrega, bool autorizado, List<(Peca, int)> pecasQuantidade)
        {
            const string orcamentoQuery = @"
                INSERT INTO Orcamento (SolicitacaoId, DataOrcamento, ValorTotal, PrazoEntrega, Autorizado) 
                VALUES (@SolicitacaoId, GETDATE(), @ValorTotal, @PrazoEntrega, @Autorizado);
                SELECT SCOPE_IDENTITY();";

            const string orcamentoPecaQuery = @"
                INSERT INTO OrcamentoPeca (OrcamentoId, PecaId, NomePeca, Quantidade) 
                VALUES (@OrcamentoId, @PecaId, @NomePeca, @Quantidade)";

            using var conexao = ConexaoBanco.ConectaBanco();
            conexao.Open();

            using var transaction = conexao.BeginTransaction();
            try
            {
                int orcamentoId;
                // Cria o orçamento e obtém o ID gerado
                using (var cmdOrcamento = new SqlCommand(orcamentoQuery, conexao, transaction))
                {
                    cmdOrcamento.Parameters.AddWithValue("@SolicitacaoId", solicitacaoId);
                    cmdOrcamento.Parameters.AddWithValue("@ValorTotal", valorTotal);
                    cmdOrcamento.Parameters.AddWithValue("@PrazoEntrega", prazoEntrega);
                    cmdOrcamento.Parameters.AddWithValue("@Autorizado", autorizado);
                    orcamentoId = Convert.ToInt32(cmdOrcamento.ExecuteScalar());
                }

                // Insere as peças associadas ao orçamento na tabela OrcamentoPeca
                foreach (var (peca, quantidade) in pecasQuantidade)
                {
                    using (var cmdOrcamentoPeca = new SqlCommand(orcamentoPecaQuery, conexao, transaction))
                    {
                        cmdOrcamentoPeca.Parameters.AddWithValue("@OrcamentoId", orcamentoId);
                        cmdOrcamentoPeca.Parameters.AddWithValue("@PecaId", peca.Id);
                        cmdOrcamentoPeca.Parameters.AddWithValue("@NomePeca", peca.Nome);
                        cmdOrcamentoPeca.Parameters.AddWithValue("@Quantidade", quantidade);
                        cmdOrcamentoPeca.ExecuteNonQuery();
                    }
                }

                transaction.Commit();
                return true;
            }
            catch (SqlException)
            {
                transaction.Rollback();
                return false;
            }
        }
    }
}
