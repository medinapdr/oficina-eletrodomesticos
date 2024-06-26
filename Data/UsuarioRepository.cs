﻿using System.Data.SqlClient;
using OficinaEletrodomesticos.Models;

namespace OficinaEletrodomesticos.Data
{
    public class UsuarioRepository
    {
        public static bool CriarPessoa(string nome, string cpf, string telefone, string endereco, string tipoPessoa, string? cargo = null, decimal salario = 0, string? departamento = null)
        {
            const string queryPessoa = @"
                INSERT INTO Oficina.dbo.Pessoa (Nome, CPF, Telefone, Endereco, TipoPessoa) 
                VALUES (@Nome, @CPF, @Telefone, @Endereco, @TipoPessoa);
                SELECT SCOPE_IDENTITY();"; // Insere os dados da pessoa e obtém o novo ID.

            using var conexao = ConexaoBanco.ConectaBanco();
            conexao.Open();

            using var transaction = conexao.BeginTransaction();
            using var cmdPessoa = new SqlCommand(queryPessoa, conexao, transaction);
            // Adiciona parâmetros à consulta para prevenir SQL injection.
            cmdPessoa.Parameters.AddWithValue("@Nome", nome);
            cmdPessoa.Parameters.AddWithValue("@CPF", cpf);
            cmdPessoa.Parameters.AddWithValue("@Telefone", string.IsNullOrEmpty(telefone) ? (object)DBNull.Value : telefone);
            cmdPessoa.Parameters.AddWithValue("@Endereco", string.IsNullOrEmpty(endereco) ? (object)DBNull.Value : endereco);
            cmdPessoa.Parameters.AddWithValue("@TipoPessoa", tipoPessoa);

            int pessoaId = Convert.ToInt32(cmdPessoa.ExecuteScalar());

            if (pessoaId > 0)
            {
                // Determina a consulta correta com base no tipo de pessoa.
                string query = tipoPessoa == "Cliente"
                    ? @"INSERT INTO Oficina.dbo.Cliente (PessoaId) VALUES (@Id)" // Insere como Cliente
                    : @"INSERT INTO Oficina.dbo.Funcionario (PessoaId, Cargo, Salario, Departamento) 
                        VALUES (@Id, @Cargo, @Salario, @Departamento)"; // Insere como Funcionário

                using var cmd = new SqlCommand(query, conexao, transaction);
                cmd.Parameters.AddWithValue("@Id", pessoaId);
                if (tipoPessoa == "Funcionário")
                {
                    // Adiciona parâmetros para os campos específicos do funcionário.
                    cmd.Parameters.AddWithValue("@Cargo", cargo);
                    cmd.Parameters.AddWithValue("@Salario", salario);
                    cmd.Parameters.AddWithValue("@Departamento", departamento);
                }

                try
                {
                    cmd.ExecuteNonQuery();
                    transaction.Commit();
                    return true;
                }
                catch (SqlException)
                {
                    transaction.Rollback();
                    return false;
                }
            }
            else
            {
                transaction.Rollback();
                return false;
            }
        }

        public static List<Pessoa> ObterPessoas()
        {
            var pessoas = new List<Pessoa>();
            const string query = @"SELECT Id, CPF, Nome FROM Oficina.dbo.Pessoa";

            using var conexao = ConexaoBanco.ConectaBanco();
            conexao.Open();

            using var cmd = new SqlCommand(query, conexao);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var pessoa = new Pessoa
                {
                    Id = reader.GetInt32(0),
                    CPF = reader.GetString(1),
                    Nome = reader.GetString(2)
                };
                pessoas.Add(pessoa);
            }
            return pessoas;
        }

        public static bool CriarUsuario(string nomeUsuario, string senha, int pessoaId)
        {
            const string query = @"
                INSERT INTO Oficina.dbo.Usuario (NomeUsuario, Senha, PessoaId) 
                VALUES (@NomeUsuario, @Senha, @PessoaId)";

            string hashedSenha = BCrypt.Net.BCrypt.HashPassword(senha); // Hasheia a senha para segurança.

            using var conexao = ConexaoBanco.ConectaBanco();

            using var cmd = new SqlCommand(query, conexao);
            cmd.Parameters.AddWithValue("@NomeUsuario", nomeUsuario);
            cmd.Parameters.AddWithValue("@Senha", hashedSenha);
            cmd.Parameters.AddWithValue("@PessoaId", pessoaId);

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

        public static Usuario? AutenticarUsuario(string username, string password)
        {
            const string query = @"
                SELECT u.NomeUsuario, p.Id, p.Nome, p.CPF, p.Telefone, p.Endereco, p.TipoPessoa, u.Senha,
                f.Cargo, f.Salario, f.Departamento
                FROM OFICINA.dbo.Usuario u
                INNER JOIN OFICINA.dbo.Pessoa p ON u.PessoaId = p.Id
                LEFT JOIN OFICINA.dbo.Funcionario f ON p.Id = f.PessoaId
                WHERE u.NomeUsuario = @Username";

            using var conexao = ConexaoBanco.ConectaBanco();
            conexao.Open();

            using var cmd = new SqlCommand(query, conexao);
            cmd.Parameters.AddWithValue("@Username", username);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string storedHash = reader.GetString(7);
                string tipoPessoa = reader.GetString(6);

                if (BCrypt.Net.BCrypt.Verify(password, storedHash))
                {
                    Pessoa pessoa = tipoPessoa == "Cliente"
                        ? new Cliente
                        {
                            Id = reader.GetInt32(1),
                            Nome = reader.GetString(2),
                            CPF = reader.GetString(3),
                            Telefone = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Endereco = reader.IsDBNull(5) ? null : reader.GetString(5),
                            TipoPessoa = tipoPessoa
                        }
                        : (Pessoa)new Funcionario
                        {
                            Id = reader.GetInt32(1),
                            Nome = reader.GetString(2),
                            CPF = reader.GetString(3),
                            Telefone = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Endereco = reader.IsDBNull(5) ? null : reader.GetString(5),
                            TipoPessoa = tipoPessoa,
                            Cargo = Enum.Parse<Cargo>(reader.GetString(8)),
                            Salario = reader.GetDecimal(9),
                            Departamento = Enum.Parse<Departamento>(reader.GetString(10))
                        };

                    // Retorna o usuário autenticado com os dados da pessoa associada.
                    return new Usuario
                    {
                        NomeUsuario = reader.GetString(0),
                        Senha = storedHash,
                        PessoaAssociada = pessoa
                    };
                }
            }
            return null;
        }
    }
}
