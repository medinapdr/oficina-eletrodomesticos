using System.Data.SqlClient;

namespace OficinaEletrodomesticos.Data
{
    public class ConexaoBanco
    {
        public SqlConnection ConectaBanco()
        {
            string stringConexao = "Server=tcp:oficinaeletro.database.windows.net,1433;Initial Catalog=Oficina;Persist Security Info=False;User ID=oficinator;Password=DCC603@admin;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            SqlConnection conexao = new(stringConexao);
            return conexao;
        }
        public bool CriarUsuario(string nomeUsuario, string senha, int pessoaId)
        {
            var conexao = ConectaBanco();
            conexao.Open();

            string hashedSenha = BCrypt.Net.BCrypt.HashPassword(senha);

            string query = @"INSERT INTO Oficina.dbo.Usuario (NomeUsuario, Senha, PessoaId) 
                             VALUES (@NomeUsuario, @Senha, @PessoaId)";

            SqlCommand cmd = new SqlCommand(query, conexao);

            cmd.Parameters.AddWithValue("@NomeUsuario", nomeUsuario);
            cmd.Parameters.AddWithValue("@Senha", hashedSenha);
            cmd.Parameters.AddWithValue("@PessoaId", pessoaId);

            try
            {
                cmd.ExecuteNonQuery();
                conexao.Close();
                return true;
            }
            catch (Exception ex)
            {
                // Lida com exceções
                conexao.Close();
                return false;
            }
        }

        public Usuario AutenticarUsuario(string username, string password)
        {
            using (var conexao = ConectaBanco())
            {
                conexao.Open();

                string query = @"SELECT u.NomeUsuario, p.Id, p.Nome, p.CPF, p.Telefone, p.Endereco, p.TipoPessoa, u.Senha,
                        f.Cargo, f.Salario, f.Departamento
                        FROM OFICINA.dbo.Usuario u
                        INNER JOIN OFICINA.dbo.Pessoa p ON u.PessoaId = p.Id
                        LEFT JOIN OFICINA.dbo.Funcionario f ON p.Id = f.PessoaId
                        WHERE u.NomeUsuario = @Username";

                using (var cmd = new SqlCommand(query, conexao))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string storedHash = reader.GetString(7); // Senha
                            string tipoPessoa = reader.GetString(6); // TipoPessoa

                            if (BCrypt.Net.BCrypt.Verify(password, storedHash))
                            {
                                if (tipoPessoa == "Cliente")
                                {
                                    var pessoa = new Cliente
                                    {
                                        Nome = reader.GetString(2), // Nome
                                        CPF = reader.GetString(3), // CPF
                                        Telefone = reader.IsDBNull(4) ? null : reader.GetString(4), // Telefone
                                        Endereco = reader.IsDBNull(5) ? null : reader.GetString(5), // Endereco
                                        TipoPessoa = tipoPessoa
                                    };

                                    var usuario = new Usuario
                                    {
                                        NomeUsuario = reader.GetString(0), // NomeUsuario
                                        Senha = storedHash,
                                        PessoaAssociada = pessoa
                                    };

                                    return usuario;
                                }
                                else if (tipoPessoa == "Funcionário")
                                {
                                    var pessoa = new Funcionario
                                    {
                                        Nome = reader.GetString(2), // Nome
                                        CPF = reader.GetString(3), // CPF
                                        Telefone = reader.IsDBNull(4) ? null : reader.GetString(4), // Telefone
                                        Endereco = reader.IsDBNull(5) ? null : reader.GetString(5), // Endereco
                                        TipoPessoa = tipoPessoa,
                                        Cargo = (Cargo)Enum.Parse(typeof(Cargo), reader.GetString(8)), // Cargo
                                        Salario = reader.GetDecimal(9), // Salario
                                        Departamento = (Departamento)Enum.Parse(typeof(Departamento), reader.GetString(10)) // Departamento
                                    };

                                    var usuario = new Usuario
                                    {
                                        NomeUsuario = reader.GetString(0), // NomeUsuario
                                        Senha = storedHash,
                                        PessoaAssociada = pessoa
                                    };

                                    return usuario;
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }


        public bool AdicionarPeca(Peca peca)
        {
            using (var conexao = ConectaBanco())
            {
                conexao.Open();
                using (var transaction = conexao.BeginTransaction())
                {
                    string query = @"INSERT INTO Oficina.dbo.Peca (Nome, Preco, Largura, Altura, Comprimento, Peso, Fabricante, Quantidade)
                             OUTPUT INSERTED.Id
                             VALUES (@NomePeca, @Preco, @Largura, @Altura, @Comprimento, @Peso, @Fabricante, @Quantidade)";

                    SqlCommand cmd = new SqlCommand(query, conexao, transaction);

                    // Adiciona parâmetros
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
                        // Executa a consulta e obtém o ID gerado
                        int id = (int)cmd.ExecuteScalar();
                        peca.Id = id;

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        // Lida com exceções
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }


        public bool RemoverPeca(Peca peca)
        {
            var conexao = ConectaBanco();
            conexao.Open();
            string query = @"DELETE FROM Oficina.dbo.Peca WHERE Id = @Id";

            SqlCommand cmd = new SqlCommand(query, conexao);

            // Adiciona parâmetro
            cmd.Parameters.AddWithValue("@Id", peca.Id);

            try
            {
                cmd.ExecuteNonQuery();
                conexao.Close();
                return true;
            }
            catch (Exception ex)
            {
                // Lida com exceções
                conexao.Close();
                return false;
            }
        }

        public List<Peca> ConsultarEstoque()
        {
            var conexao = ConectaBanco();
            conexao.Open();
            string query = @"SELECT Id, Nome, Preco, Largura, Altura, Comprimento, Peso, Fabricante, Quantidade FROM Oficina.dbo.Peca";

            SqlCommand cmd = new SqlCommand(query, conexao);
            SqlDataReader reader = cmd.ExecuteReader();

            List<Peca> estoque = new List<Peca>();

            while (reader.Read())
            {
                Peca peca = new Peca();
                peca.Id = reader.GetInt32(0);
                peca.Nome = reader.GetString(1);
                peca.Preco = reader.GetDecimal(2);
                peca.Altura = reader.GetDecimal(3);
                peca.Largura = reader.GetDecimal(4);
                peca.Comprimento = reader.GetDecimal(5);
                peca.Peso = reader.GetDecimal(6);
                peca.Fabricante = reader.GetString(7);
                peca.Quantidade = reader.GetInt32(8);
                estoque.Add(peca);
            }

            reader.Close();
            conexao.Close();

            return estoque;
        }

        public bool CriarPessoa(string nome, string cpf, string telefone, string endereco, string tipoPessoa, string cargo = null, decimal salario = 0, string departamento = null)
        {
            var conexao = ConectaBanco();
            conexao.Open();

            string queryPessoa = @"INSERT INTO Oficina.dbo.Pessoa (Nome, CPF, Telefone, Endereco, TipoPessoa) 
                           VALUES (@Nome, @CPF, @Telefone, @Endereco, @TipoPessoa);
                           SELECT SCOPE_IDENTITY();"; // Retorna o ID da pessoa recém-inserida

            SqlCommand cmdPessoa = new SqlCommand(queryPessoa, conexao);

            // Adiciona parâmetros
            cmdPessoa.Parameters.AddWithValue("@Nome", nome);
            cmdPessoa.Parameters.AddWithValue("@CPF", cpf);
            cmdPessoa.Parameters.AddWithValue("@Telefone", string.IsNullOrEmpty(telefone) ? (object)DBNull.Value : telefone);
            cmdPessoa.Parameters.AddWithValue("@Endereco", string.IsNullOrEmpty(endereco) ? (object)DBNull.Value : endereco);
            cmdPessoa.Parameters.AddWithValue("@TipoPessoa", tipoPessoa);

            int pessoaId = Convert.ToInt32(cmdPessoa.ExecuteScalar()); // Obtém o ID da pessoa inserida

            if (pessoaId > 0)
            {
                try
                {
                    if (tipoPessoa == "Cliente")
                    {
                        string queryCliente = @"INSERT INTO Oficina.dbo.Cliente (PessoaId) VALUES (@Id)";
                        SqlCommand cmdCliente = new SqlCommand(queryCliente, conexao);
                        cmdCliente.Parameters.AddWithValue("@Id", pessoaId);
                        cmdCliente.ExecuteNonQuery();
                    }
                    else if (tipoPessoa == "Funcionário")
                    {
                        string queryFuncionario = @"INSERT INTO Oficina.dbo.Funcionario (PessoaId, Cargo, Salario, Departamento) 
                                            VALUES (@Id, @Cargo, @Salario, @Departamento)";
                        SqlCommand cmdFuncionario = new SqlCommand(queryFuncionario, conexao);
                        cmdFuncionario.Parameters.AddWithValue("@Id", pessoaId);
                        cmdFuncionario.Parameters.AddWithValue("@Cargo", cargo);
                        cmdFuncionario.Parameters.AddWithValue("@Salario", salario);
                        cmdFuncionario.Parameters.AddWithValue("@Departamento", departamento);
                        cmdFuncionario.ExecuteNonQuery();
                    }

                    conexao.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    // Lida com exceções
                    conexao.Close();
                    return false;
                }
            }
            else
            {
                // Se não conseguiu obter o ID da pessoa inserida, algo deu errado
                conexao.Close();
                return false;
            }
        }
        public bool AtualizarPeca(Peca peca)
        {
            using (var conexao = ConectaBanco())
            {
                conexao.Open();
                string query = @"UPDATE Oficina.dbo.Peca 
                         SET Nome = @Nome, Preco = @Preco, Largura = @Largura, Altura = @Altura, 
                             Comprimento = @Comprimento, Peso = @Peso, Fabricante = @Fabricante, 
                             Quantidade = @Quantidade
                         WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(query, conexao);

                // Adiciona parâmetros
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
                    int rowsAffected = cmd.ExecuteNonQuery();
                    conexao.Close();
                    return rowsAffected > 0; // Retorna true se pelo menos uma linha foi afetada pela atualização
                }
                catch (Exception ex)
                {
                    // Lida com exceções
                    conexao.Close();
                    return false;
                }
            }
        }
    }
}
