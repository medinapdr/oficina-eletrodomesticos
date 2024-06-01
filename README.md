# Gerenciamento de Oficina de Manutenção de Eletrodomésticos

## Descrição

Este é um programa desenvolvido em .NET 8 WPF, utilizando SQLite, para gerenciar uma oficina de manutenção/reparo de eletrodomésticos. O sistema oferece diversas funcionalidades, como controle de estoque, pedidos para fornecedores, solicitação e criação de orçamentos, atribuição e atualização de serviços, além do cadastro de pessoas (clientes e funcionários) e usuários

Além disso, o sistema se adapta ao usuário que está utilizando, permitindo ou não os acessos conforme as permissões atribuídas. Os clientes também podem utilizar o sistema para solicitar um orçamento ou acompanhar os serviços relacionados ao seu aparelho entregue.
## Funcionalidades

- **Controle de Estoque**: Gerencia o inventário de peças e componentes.
- **Pedidos para Fornecedores**: Permite realizar pedidos de reposição de estoque.
- **Solicitação e Criação de Orçamentos**: Facilita a geração de orçamentos para os clientes.
- **Atribuição e Atualização de Serviços**: Gerencia os serviços de manutenção e reparo.
- **Cadastro de Pessoas**: Registra clientes, funcionários e usuários do sistema.

## Estrutura do Projeto

O projeto está organizado da seguinte forma:

- **Data**: Contém a conexão com o banco de dados e os repositórios de métodos com as consultas SQL.

- **Models**: Inclui as classes de objetos que representam entidades do sistema.

- **View**: Contém as telas da aplicação em formato XAML e seus respectivos códigos-behind (.xaml.cs).

- **Tests**: Engloba os testes unitários e de integração realizados com XUnit.

## Tecnologias Utilizadas

- **.NET 8 WPF**: Framework para a construção da interface gráfica.
- **SQLite**: Banco de dados leve e autossuficiente utilizado localmente.
- **Bcrypt**: Biblioteca utilizada para hashing de senhas.
- **Azure SQL Server**: Servidor de banco de dados utilizado na nuvem.
- **XUnit**: Framework para testes unitários e de integração.

## Conexão com Banco de Dados

Foi utilizado um servidor na Azure com o banco de dados em SQL Server. As tabelas e os relacionamentos entre elas foram criados e gerenciados a partir desse servidor. A conexão com o banco e as operações são gerenciadas no diretório `/Data`.

## Padrões de Nomeação e Metodologia

Os padrões de nomeação de métodos, variáveis e classes seguem as convenções do C# sugeridas pelo Visual Studio (Microsoft).

## Requisitos

- .NET 8 SDK
- Visual Studio
- SQLite
- XUnit (para testes)

## Executando o Projeto

### Via Visual Studio:

1. Abra o projeto no Visual Studio.
2. Configure o projeto como startup.
3. Pressione F5 para iniciar a execução.

### Via Terminal (dotnet):

1. Navegue até o diretório do projeto no terminal.
2. Restaure as dependências: `dotnet restore`.
3. Compile o projeto: `dotnet build`.
4. Execute o projeto: `dotnet run`.

### Via .exe (Self-contained):

1. Faça o download do executável na seção de [Releases]([link_para_releases](https://github.com/medinapdr/oficina-eletrodomesticos/releases/download/beta/oficina-eletrodomesticos-x64.exe)) do GitHub.
2. Execute o arquivo .exe.

### Via .exe (Dependente do .NET 8):

1. Faça o download do executável na seção de [Releases]([link_para_releases](https://github.com/medinapdr/oficina-eletrodomesticos/releases/download/beta/oficina-eletrodomesticos-x64-.net8.exe)) do GitHub.
2. Certifique-se de ter o .NET 8 instalado na máquina.
3. Execute o arquivo .exe.

## Executando os Testes

### Via Terminal (dotnet):

1. Navegue até o diretório `Tests`.
2. Execute os testes: `dotnet test`.

### Via Visual Studio:

1. Abra o projeto no Visual Studio.
2. Selecione "Test" no menu superior de ferramentas.
3. Selecione "Run All Tests" para iniciar a execução dos testes.
