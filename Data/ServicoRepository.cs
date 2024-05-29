using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using OficinaEletrodomesticos.Models;

namespace OficinaEletrodomesticos.Data
{
    public static class ServicoRepository
    {
        public static List<Servico> ObterTodosServicos()
        {
            const string query = @"SELECT s.Id, s.Descricao, s.ValorPagamento, s.DataPagamento, sts.Nome AS Status, p.Nome AS NomeTecnico 
                            FROM Servico s
                            LEFT JOIN Funcionario f ON s.TecnicoResponsavelId = f.PessoaId
                            JOIN StatusServico sts ON s.StatusId = sts.Id
                            JOIN Pessoa p ON s.TecnicoResponsavelId = p.Id";
            var servicos = new List<Servico>();

            using var conexao = ConexaoBanco.ConectaBanco();
            conexao.Open();

            using var cmd = new SqlCommand(query, conexao);

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

        public static List<Funcionario> ObterTecnicos()
        {
            const string query = @"SELECT p.Id, p.Nome 
                           FROM Funcionario f
                           JOIN Pessoa p ON f.PessoaId = p.Id
                           WHERE f.Cargo = @CargoTecnico";
            var tecnicos = new List<Funcionario>();

            using var conexao = ConexaoBanco.ConectaBanco();
            conexao.Open();

            using var cmd = new SqlCommand(query, conexao);
            cmd.Parameters.AddWithValue("@CargoTecnico", Cargo.Técnico.ToString());

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                tecnicos.Add(new Funcionario
                {
                    Id = reader.GetInt32(0),
                    Nome = reader.GetString(1)
                });
            }
            return tecnicos;
        }

        public static List<Servico> ObterServicosPorTecnico(int tecnicoId)
        {
            const string query = @"SELECT s.Id, s.Descricao, s.ValorPagamento, s.DataPagamento, sts.Nome AS Status, p.Nome AS NomeTecnico 
                                    FROM Servico s
                                    LEFT JOIN Funcionario f ON s.TecnicoResponsavelId = f.PessoaId
                                    JOIN StatusServico sts ON s.StatusId = sts.Id
                                    JOIN Pessoa p ON s.TecnicoResponsavelId = p.Id
                                    WHERE TecnicoResponsavelId = @TecnicoId";
            var servicos = new List<Servico>();

            using var conexao = ConexaoBanco.ConectaBanco();
            conexao.Open();

            using var cmd = new SqlCommand(query, conexao);
            cmd.Parameters.AddWithValue("@TecnicoId", tecnicoId);

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

        public static bool AtualizarStatusServico(int servicoId, StatusServico status)
        {
            const string query = @"UPDATE Servico SET StatusId = @Status WHERE Id = @Id";

            using var conexao = ConexaoBanco.ConectaBanco();
            conexao.Open();

            using var cmd = new SqlCommand(query, conexao);
            cmd.Parameters.AddWithValue("@Id", servicoId);
            cmd.Parameters.AddWithValue("@Status", (int)status);

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

        public static bool ConfirmarPagamentoServico(int servicoId, double? valorPagamento)
        {
            const string query = @"UPDATE Servico SET ValorPagamento = @ValorPagamento, DataPagamento = @DataPagamento WHERE Id = @Id";

            using var conexao = ConexaoBanco.ConectaBanco();
            conexao.Open();

            using var cmd = new SqlCommand(query, conexao);
            cmd.Parameters.AddWithValue("@Id", servicoId);
            cmd.Parameters.AddWithValue("@ValorPagamento", valorPagamento ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@DataPagamento", DateTime.Now);

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

        public static bool AdicionarServico(Servico servico)
        {
            const string query = @"INSERT INTO Servico (TecnicoResponsavelId, OrcamentoId, Descricao, StatusId)
                                   VALUES (@TecnicoResponsavelId, @OrcamentoId, @Descricao, @StatusId)";

            using var conexao = ConexaoBanco.ConectaBanco();
            conexao.Open();

            using var cmd = new SqlCommand(query, conexao);
            cmd.Parameters.AddWithValue("@TecnicoResponsavelId", servico.TecnicoResponsavel.Id);
            cmd.Parameters.AddWithValue("@OrcamentoId", servico.Orcamento.Id);
            cmd.Parameters.AddWithValue("@Descricao", servico.Descricao);
            cmd.Parameters.AddWithValue("@StatusId", (int)servico.Status);

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
    }
}
