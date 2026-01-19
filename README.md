# Controle de Gastos Residenciais

Projeto desenvolvido para sistema de controle de gastos residenciais, permitindo o gerenciamento de pessoas, categorias e transações financeiras.

## Sobre o Projeto

O **Controle de Gastos Residenciais** é uma aplicação desenvolvida em C# (.NET) para o back-end e React com TypeScript para o front-end, que permite aos usuários gerenciar gastos e receitas de forma organizada e eficiente.  
Este projeto visa aplicar na prática os conhecimentos de desenvolvimento web, arquitetura MVC, APIs REST e interfaces modernas, integrando conceitos de programação orientada a objetos, manipulação de dados e interfaces gráficas.

## Tecnologias Utilizadas

### Back-end
* Linguagem: C#
* Framework: ASP.NET Core 8.0
* Arquitetura: MVC (Model-View-Controller)
* ORM: Entity Framework Core
* Banco de Dados: SQLite
* IDE: Visual Studio / Visual Studio Code

### Front-end
* Linguagem: TypeScript
* Framework: React 18
* Build Tool: Vite
* HTTP Client: Axios

### Controle de Versão
* Git
* Plataforma: GitHub

## Estrutura do Projeto

A seguir, estão os principais diretórios e arquivos do projeto, organizados por função:

### Controllers — Camada de Controle da Aplicação

Responsáveis por receber requisições e retornar respostas, conectando as views aos serviços.

* `PessoaController.cs` - Gerencia operações de pessoas (criar, listar, deletar)
* `CategoriaController.cs` - Gerencia operações de categorias (criar, listar)
* `TransacaoController.cs` - Gerencia operações de transações (criar, listar)
* `ConsultaController.cs` - Gerencia consultas de totais financeiros

### DTOs (Data Transfer Objects)

Objetos usados para transferência de dados entre camadas de forma segura.

* `CadastroEditarPessoaDto.cs` - DTO para cadastro e edição de pessoas
* `CadastroEditarCategoriaDto.cs` - DTO para cadastro e edição de categorias
* `CadastroEditarTransacaoDto.cs` - DTO para cadastro e edição de transações

### Interfaces — Contratos dos Repositórios

Definem os métodos que as classes de repositório devem implementar.

* `IPessoaRepository.cs` - Interface do repositório de pessoas
* `ICategoriaRepository.cs` - Interface do repositório de categorias
* `ITransacaoRepository.cs` - Interface do repositório de transações

### Models — Entidades do Domínio

Representam as tabelas e entidades principais do sistema.

* `Pessoa.cs` - Entidade que representa uma pessoa cadastrada
* `Categoria.cs` - Entidade que representa uma categoria de transação
* `Transacao.cs` - Entidade que representa uma transação financeira

### Repositories — Implementações de Acesso a Dados

Classes que acessam o banco de dados com base nas interfaces.

* `PessoaRepository.cs` - Implementação do repositório de pessoas
* `CategoriaRepository.cs` - Implementação do repositório de categorias
* `TransacaoRepository.cs` - Implementação do repositório de transações

### ViewModels — Estruturas de Visualização

Modelos usados para exibir informações específicas na interface ou retornos.

* `ListarPessoaViewModel.cs` - ViewModel para listagem de pessoas
* `ListarCategoriaViewModel.cs` - ViewModel para listagem de categorias
* `ListarTransacaoViewModel.cs` - ViewModel para listagem de transações
* `TotalPorPessoaViewModel.cs` - ViewModel para totais por pessoa
* `TotalPorCategoriaViewModel.cs` - ViewModel para totais por categoria

### Data — Contexto do Banco de Dados

* `ApplicationDbContext.cs` - Contexto do Entity Framework Core para acesso ao banco de dados

### Frontend — Aplicação React

* `src/App.tsx` - Componente principal da aplicação React
* `src/main.tsx` - Ponto de entrada da aplicação React
* `src/index.css` - Estilos globais da aplicação

### Configurações e Arquivos Globais

* `appsettings.json` – Configurações gerais da aplicação
* `appsettings.Development.json` – Configurações específicas para ambiente de desenvolvimento
* `Program.cs` – Ponto de entrada da aplicação
* `ControleGastosResidenciais.sln` – Arquivo de solução do Visual Studio

## Funcionalidades

### Cadastro de Pessoas

* **Criação**: Cadastro de novas pessoas com nome e idade
* **Listagem**: Visualização de todas as pessoas cadastradas
* **Deleção**: Remoção de pessoas (ao deletar uma pessoa, todas as suas transações são removidas automaticamente)

**Campos obrigatórios:**
* Identificador (gerado automaticamente)
* Nome (texto)
* Idade (número inteiro positivo)

### Cadastro de Categorias

* **Criação**: Cadastro de novas categorias com descrição e finalidade
* **Listagem**: Visualização de todas as categorias cadastradas

**Campos obrigatórios:**
* Identificador (gerado automaticamente)
* Descrição (texto)
* Finalidade (Despesa/Receita/Ambas)

### Cadastro de Transações

* **Criação**: Cadastro de novas transações financeiras
* **Listagem**: Visualização de todas as transações cadastradas

**Regras de negócio:**
* Menores de idade (menores de 18 anos) podem cadastrar apenas despesas
* A categoria deve ter uma finalidade compatível com o tipo da transação (ex: não é possível usar uma categoria de "Receita" para uma transação do tipo "Despesa")

**Campos obrigatórios:**
* Identificador (gerado automaticamente)
* Descrição (texto)
* Valor (número decimal positivo)
* Tipo (Despesa/Receita)
* Categoria (identificador da categoria)
* Pessoa (identificador da pessoa)

### Consulta de Totais por Pessoa

* Lista todas as pessoas cadastradas
* Exibe o total de receitas, despesas e o saldo (receita - despesa) de cada pessoa
* Ao final, exibe o total geral de todas as pessoas incluindo o total de receitas, total de despesas e o saldo líquido

### Consulta de Totais por Categoria (Opcional)

* Lista todas as categorias cadastradas
* Exibe o total de receitas, despesas e o saldo (receita - despesa) de cada categoria
* Ao final, exibe o total geral de todas as categorias incluindo o total de receitas, total de despesas e o saldo líquido

## Como Executar o Projeto

### Pré-requisitos

* .NET 8.0 SDK instalado
* Node.js (versão 18 ou superior) e npm instalados

### Back-end (API)

1. Navegue até a pasta do projeto:
```bash
cd ControleGastosResidenciais
```

2. Restaure as dependências:
```bash
dotnet restore
```

3. Execute a aplicação:
```bash
dotnet run
```

A API estará disponível em `http://localhost:5000` (ou a porta configurada).

### Front-end (React)

1. Navegue até a pasta do frontend:
```bash
cd frontend
```

2. Instale as dependências:
```bash
npm install
```

3. Execute a aplicação:
```bash
npm run dev
```

A aplicação estará disponível em `http://localhost:3000`.

### Banco de Dados

O banco de dados SQLite será criado automaticamente na primeira execução da aplicação. O arquivo `controle_gastos.db` será gerado na pasta raiz do projeto back-end.

## Estrutura da API

### Endpoints de Pessoas

* `GET /api/Pessoa` - Lista todas as pessoas
* `GET /api/Pessoa/{id}` - Busca uma pessoa por ID
* `POST /api/Pessoa` - Cria uma nova pessoa
* `DELETE /api/Pessoa/{id}` - Remove uma pessoa

### Endpoints de Categorias

* `GET /api/Categoria` - Lista todas as categorias
* `GET /api/Categoria/{id}` - Busca uma categoria por ID
* `POST /api/Categoria` - Cria uma nova categoria

### Endpoints de Transações

* `GET /api/Transacao` - Lista todas as transações
* `GET /api/Transacao/{id}` - Busca uma transação por ID
* `POST /api/Transacao` - Cria uma nova transação

### Endpoints de Consultas

* `GET /api/Consulta/TotaisPorPessoa` - Consulta totais por pessoa
* `GET /api/Consulta/TotaisPorCategoria` - Consulta totais por categoria

## Observações Importantes

* Todos os identificadores são gerados automaticamente pelo sistema
* Ao deletar uma pessoa, todas as suas transações são removidas automaticamente (cascade delete)
* Menores de idade só podem cadastrar despesas
* As categorias devem ter finalidade compatível com o tipo da transação
* Os dados são persistidos em banco de dados SQLite e mantidos após reiniciar o sistema

## Contribuições

Este projeto foi desenvolvido como parte de um teste técnico, aplicando boas práticas de desenvolvimento em .NET e React.
