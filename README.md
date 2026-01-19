# Controle de Gastos Residenciais

Sistema desenvolvido para controle de gastos residenciais, permitindo gerenciar pessoas, categorias e transações financeiras de forma organizada.

## Sobre o Projeto

Este projeto foi desenvolvido como uma aplicação completa de controle financeiro residencial. O back-end foi implementado em C# usando ASP.NET Core, enquanto o front-end utiliza React com TypeScript para criar uma interface moderna e responsiva.

A aplicação permite que os usuários cadastrem pessoas, categorizem transações e acompanhem seus gastos e receitas de forma simples e eficiente. O sistema foi construído seguindo os princípios de arquitetura MVC, separando responsabilidades entre camadas de controle, acesso a dados e apresentação.

## Tecnologias Utilizadas

### Back-end
* **Linguagem:** C#
* **Framework:** ASP.NET Core 8.0
* **Arquitetura:** MVC (Model-View-Controller)
* **ORM:** Entity Framework Core
* **Banco de Dados:** SQLite
* **IDE:** Visual Studio / Visual Studio Code

### Front-end
* **Linguagem:** TypeScript
* **Framework:** React 18
* **Build Tool:** Vite
* **HTTP Client:** Axios

### Controle de Versão
* Git
* GitHub

## Estrutura do Projeto

A estrutura do projeto está organizada em camadas, seguindo os princípios de arquitetura limpa e separação de responsabilidades.

### Controllers - Camada de Controle

Responsáveis por receber as requisições HTTP e retornar as respostas adequadas. Fazem a ponte entre a API e os repositórios.

* `PessoaController.cs` - Gerencia todas as operações relacionadas a pessoas (criar, listar, deletar)
* `CategoriaController.cs` - Gerencia operações de categorias (criar, listar)
* `TransacaoController.cs` - Gerencia operações de transações financeiras (criar, listar) com validações de regras de negócio
* `ConsultaController.cs` - Gerencia as consultas de totais financeiros por pessoa e por categoria

### DTOs (Data Transfer Objects)

Objetos utilizados para transferir dados entre as camadas da aplicação de forma segura, evitando expor diretamente as entidades do domínio.

* `CadastroEditarPessoaDto.cs` - DTO utilizado no cadastro e edição de pessoas
* `CadastroEditarCategoriaDto.cs` - DTO utilizado no cadastro e edição de categorias
* `CadastroEditarTransacaoDto.cs` - DTO utilizado no cadastro e edição de transações

### Interfaces - Contratos dos Repositórios

Definem os contratos que os repositórios devem implementar, garantindo que todas as implementações sigam o mesmo padrão.

* `IPessoaRepository.cs` - Interface que define os métodos de acesso a dados para pessoas
* `ICategoriaRepository.cs` - Interface que define os métodos de acesso a dados para categorias
* `ITransacaoRepository.cs` - Interface que define os métodos de acesso a dados para transações

### Models - Entidades do Domínio

Representam as entidades principais do sistema e são mapeadas para as tabelas do banco de dados.

* `Pessoa.cs` - Representa uma pessoa cadastrada no sistema, contendo identificador, nome e idade
* `Categoria.cs` - Representa uma categoria de transação, com descrição e finalidade (Despesa, Receita ou Ambas)
* `Transacao.cs` - Representa uma transação financeira, contendo descrição, valor, tipo e relacionamentos com pessoa e categoria

### Repositories - Implementações de Acesso a Dados

Classes que implementam o acesso ao banco de dados utilizando Entity Framework Core, seguindo as interfaces definidas.

* `PessoaRepository.cs` - Implementação do repositório de pessoas, contendo métodos para listar, buscar, criar e deletar
* `CategoriaRepository.cs` - Implementação do repositório de categorias, contendo métodos para listar, buscar e criar
* `TransacaoRepository.cs` - Implementação do repositório de transações, contendo métodos para listar, buscar e criar, incluindo filtros por pessoa e categoria

### ViewModels - Estruturas de Visualização

Modelos utilizados para formatar os dados que serão retornados pela API, contendo apenas as informações necessárias para exibição.

* `ListarPessoaViewModel.cs` - ViewModel para exibir informações de pessoas na listagem
* `ListarCategoriaViewModel.cs` - ViewModel para exibir informações de categorias na listagem
* `ListarTransacaoViewModel.cs` - ViewModel para exibir informações completas de transações, incluindo dados relacionados
* `TotalPorPessoaViewModel.cs` - ViewModel para exibir os totais financeiros de cada pessoa
* `TotalPorCategoriaViewModel.cs` - ViewModel para exibir os totais financeiros de cada categoria

### Data - Contexto do Banco de Dados

* `ApplicationDbContext.cs` - Contexto do Entity Framework Core que gerencia a conexão com o banco de dados SQLite e define os relacionamentos entre as entidades

### Frontend - Aplicação React

* `src/App.tsx` - Componente principal da aplicação React, contendo toda a lógica de gerenciamento de estado e comunicação com a API
* `src/main.tsx` - Ponto de entrada da aplicação React
* `src/index.css` - Estilos globais da aplicação

### Configurações e Arquivos Globais

* `appsettings.json` - Arquivo de configurações gerais da aplicação, incluindo string de conexão com o banco de dados
* `appsettings.Development.json` - Configurações específicas para ambiente de desenvolvimento
* `Program.cs` - Ponto de entrada da aplicação, onde são configurados os serviços, middlewares e pipeline HTTP
* `ControleGastosResidenciais.sln` - Arquivo de solução do Visual Studio

## Funcionalidades

### Cadastro de Pessoas

Permite cadastrar pessoas no sistema com informações básicas. Ao deletar uma pessoa, todas as suas transações são removidas automaticamente para manter a integridade dos dados.

* **Criação:** Cadastro de novas pessoas informando nome e idade
* **Listagem:** Visualização de todas as pessoas cadastradas no sistema
* **Deleção:** Remoção de pessoas, com remoção automática de todas as transações associadas

**Campos obrigatórios:**
* Identificador (gerado automaticamente pelo sistema)
* Nome (texto, máximo 200 caracteres)
* Idade (número inteiro positivo entre 1 e 150)

### Cadastro de Categorias

Permite criar categorias para classificar as transações financeiras. Cada categoria possui uma finalidade que define para quais tipos de transação ela pode ser utilizada.

* **Criação:** Cadastro de novas categorias informando descrição e finalidade
* **Listagem:** Visualização de todas as categorias cadastradas

**Campos obrigatórios:**
* Identificador (gerado automaticamente pelo sistema)
* Descrição (texto, máximo 200 caracteres)
* Finalidade (Despesa, Receita ou Ambas)

### Cadastro de Transações

Permite registrar transações financeiras associadas a uma pessoa e uma categoria. O sistema valida as regras de negócio antes de permitir o cadastro.

* **Criação:** Cadastro de novas transações financeiras com validações de regras de negócio
* **Listagem:** Visualização de todas as transações cadastradas com informações completas

**Regras de negócio implementadas:**
* Menores de idade (menores de 18 anos) podem cadastrar apenas despesas. O sistema impede o cadastro de receitas para menores de idade
* A categoria selecionada deve ter uma finalidade compatível com o tipo da transação. Por exemplo, não é possível usar uma categoria com finalidade "Receita" para uma transação do tipo "Despesa"

**Campos obrigatórios:**
* Identificador (gerado automaticamente pelo sistema)
* Descrição (texto, máximo 500 caracteres)
* Valor (número decimal positivo)
* Tipo (Despesa ou Receita)
* Categoria (identificador da categoria, que deve ser compatível com o tipo)
* Pessoa (identificador da pessoa)

### Consulta de Totais por Pessoa

Fornece uma visão consolidada dos totais financeiros de cada pessoa cadastrada no sistema, facilitando o acompanhamento individual.

* Lista todas as pessoas cadastradas
* Para cada pessoa, exibe o total de receitas, total de despesas e o saldo calculado (receitas - despesas)
* Ao final da listagem, exibe o total geral considerando todas as pessoas, incluindo total geral de receitas, total geral de despesas e o saldo líquido

### Consulta de Totais por Categoria (Opcional)

Funcionalidade adicional que permite analisar os gastos e receitas por categoria, facilitando a identificação de onde o dinheiro está sendo gasto ou de onde está vindo.

* Lista todas as categorias cadastradas
* Para cada categoria, exibe o total de receitas, total de despesas e o saldo calculado
* Ao final da listagem, exibe o total geral considerando todas as categorias

## Como Executar o Projeto

### Pré-requisitos

Antes de executar o projeto, é necessário ter instalado:

* .NET 8.0 SDK
* Node.js versão 18 ou superior
* npm (geralmente vem junto com o Node.js)

### Back-end (API)

1. Abra um terminal e navegue até a pasta do projeto backend:
```bash
cd ControleGastosResidenciais
```

2. Restaure as dependências do projeto (se necessário):
```bash
dotnet restore
```

3. Execute a aplicação:
```bash
dotnet run
```

A API estará disponível em `http://localhost:5000`. A documentação Swagger estará disponível em `http://localhost:5000/swagger`.

### Front-end (React)

1. Abra um novo terminal e navegue até a pasta do frontend:
```bash
cd frontend
```

2. Instale as dependências do projeto (apenas na primeira vez ou quando houver alterações):
```bash
npm install
```

3. Execute a aplicação:
```bash
npm run dev
```

A aplicação estará disponível em `http://localhost:3000`.

### Banco de Dados

O banco de dados SQLite é criado automaticamente na primeira execução da aplicação. O arquivo `controle_gastos.db` será gerado na pasta raiz do projeto back-end. Os dados são persistidos neste arquivo e mantidos mesmo após reiniciar a aplicação.

## Estrutura da API

### Endpoints de Pessoas

* `GET /api/Pessoa` - Retorna uma lista com todas as pessoas cadastradas
* `GET /api/Pessoa/{id}` - Retorna os dados de uma pessoa específica pelo seu identificador
* `POST /api/Pessoa` - Cria uma nova pessoa no sistema
* `DELETE /api/Pessoa/{id}` - Remove uma pessoa do sistema (remove também todas as suas transações)

### Endpoints de Categorias

* `GET /api/Categoria` - Retorna uma lista com todas as categorias cadastradas
* `GET /api/Categoria/{id}` - Retorna os dados de uma categoria específica pelo seu identificador
* `POST /api/Categoria` - Cria uma nova categoria no sistema

### Endpoints de Transações

* `GET /api/Transacao` - Retorna uma lista com todas as transações cadastradas, incluindo dados relacionados de pessoa e categoria
* `GET /api/Transacao/{id}` - Retorna os dados de uma transação específica pelo seu identificador
* `POST /api/Transacao` - Cria uma nova transação no sistema, validando as regras de negócio antes de salvar

### Endpoints de Consultas

* `GET /api/Consulta/TotaisPorPessoa` - Retorna os totais financeiros de cada pessoa e o total geral
* `GET /api/Consulta/TotaisPorCategoria` - Retorna os totais financeiros de cada categoria e o total geral

## Observações Importantes

* Todos os identificadores são gerados automaticamente pelo sistema, não sendo necessário informá-los ao criar novos registros
* Ao deletar uma pessoa, todas as suas transações são removidas automaticamente devido ao relacionamento configurado no banco de dados (cascade delete)
* Menores de idade só podem cadastrar despesas. O sistema valida essa regra tanto no front-end quanto no back-end
* As categorias devem ter uma finalidade compatível com o tipo da transação. O sistema valida essa compatibilidade antes de permitir o cadastro
* Os dados são persistidos em banco de dados SQLite e mantidos permanentemente após reiniciar o sistema
* O banco de dados é criado automaticamente na primeira execução, não sendo necessária configuração adicional

