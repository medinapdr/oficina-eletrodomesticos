using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using OficinaEletrodomesticos.Models;

namespace OficinaEletrodomesticos.Data
{
    public class PecaRepository(ConexaoBanco conexaoBanco)
    {
        public bool AdicionarPeca(Peca peca)
        {
            const string query = @"INSERT INTO Oficina.dbo.Peca (Nome, Preco, Largura, Altura, Comprimento, Peso, Fabricante, Quantidade)
                                   OUTPUT INSERTED.Id
                                   VALUES (@NomePeca, @Preco, @Largura, @Altura, @Comprimento, @Peso, @Fabricante, @Quantidade)";

            using (var conexao = conexaoBanco.ConectaBanco())
            {
                conexao.Open();
                using (var transaction = conexao.BeginTransaction())
                using (var cmd = new SqlCommand(query, conexao, transaction))
                {
                    cmd.Parameters.AddWithValue("@NomePeca", peca.Nome);
                    cmd.Parameters.AddWithValue("@Preco", peca.Preco);
                    cmd.Parameters.AddWithValue("@Largura", peca.Largura);
                    cmd.Parameters.AddWithValue("@Altura", peca.Altura);
                    cmd.Parameters.AddWithValue("@Comprimento", peca.Comprimento);
                    cmd.Parameters.AddWithValue("@Peso", peca.Peso);
                    cmd.Parameters.AddWithValue("@Fabricante", peca.Fabricante);
                    cmd.Parameters.AddWithValue("@Quantidade", peca.Quantidade);

                    try
                    {
                        peca.Id = (int)cmd.ExecuteScalar();
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

        public bool RemoverPeca(Peca peca)
        {
            const string query = @"DELETE FROM Oficina.dbo.Peca WHERE Id = @Id";

            using (var conexao = conexaoBanco.ConectaBanco())
            using (var cmd = new SqlCommand(query, conexao))
            {
                cmd.Parameters.AddWithValue("@Id", peca.Id);
                try
                {
                    conexao.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }

        public List<Peca> ConsultarEstoque()
        {
            const string query = @"SELECT Id, Nome, Preco, Largura, Altura, Comprimento, Peso, Fabricante, Quantidade FROM Oficina.dbo.Peca";
            var estoque = new List<Peca>();

            using (var conexao = conexaoBanco.ConectaBanco())
            using (var cmd = new SqlCommand(query, conexao))
            {
                conexao.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        estoque.Add(new Peca
                        {
                            Id = reader.GetInt32(0),
                            Nome = reader.GetString(1),
                            Preco = reader.GetDecimal(2),
                            Largura = reader.GetDecimal(3),
                            Altura = reader.GetDecimal(4),
                            Comprimento = reader.GetDecimal(5),
                            Peso = reader.GetDecimal(6),
                            Fabricante = reader.GetString(7),
                            Quantidade = reader.GetInt32(8)
                        });
                    }
                }
            }
            return estoque;
        }

        public bool AtualizarPeca(Peca peca)
        {
            const string query = @"UPDATE Oficina.dbo.Peca 
                                   SET Nome = @Nome, Preco = @Preco, Largura = @Largura, Altura = @Altura, 
                                       Comprimento = @Comprimento, Peso = @Peso, Fabricante = @Fabricante, 
                                       Quantidade = @Quantidade
                                   WHERE Id = @Id";

            using (var conexao = conexaoBanco.ConectaBanco())
            using (var cmd = new SqlCommand(query, conexao))
            {
                cmd.Parameters.AddWithValue("@Id", peca.Id);
                cmd.Parameters.AddWithValue("@Nome", peca.Nome);
                cmd.Parameters.AddWithValue("@Preco", peca.Preco);
                cmd.Parameters.AddWithValue("@Largura", peca.Largura);
                cmd.Parameters.AddWithValue("@Altura", peca.Altura);
                cmd.Parameters.AddWithValue("@Comprimento", peca.Comprimento);
                cmd.Parameters.AddWithValue("@Peso", peca.Peso);
                cmd.Parameters.AddWithValue("@Fabricante", peca.Fabricante);
                cmd.Parameters.AddWithValue("@Quantidade", peca.Quantidade);

                try
                {
                    conexao.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }
    }
}
