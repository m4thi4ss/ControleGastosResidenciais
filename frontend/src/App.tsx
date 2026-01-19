import { useState, useEffect } from 'react'
import axios from 'axios'
import './App.css'

/**
 * Configuração da URL base da API backend.
 * Todas as requisições HTTP serão feitas para este endereço.
 */
const API_BASE_URL = 'http://localhost:5000/api'

/**
 * Interfaces TypeScript para tipagem dos dados.
 * Garantem type-safety e autocomplete no código.
 */

/**
 * Interface que representa uma pessoa cadastrada no sistema.
 */
interface Pessoa {
  id: number              // Identificador único gerado automaticamente
  nome: string            // Nome completo da pessoa
  idade: number           // Idade da pessoa (número inteiro positivo)
}

/**
 * Interface que representa uma categoria de transação.
 */
interface Categoria {
  id: number                                    // Identificador único gerado automaticamente
  descricao: string                             // Descrição da categoria (ex: "Alimentação")
  finalidade: 'Despesa' | 'Receita' | 'Ambas'  // Finalidade: pode ser usada para despesas, receitas ou ambas
}

/**
 * Interface que representa uma transação financeira.
 */
interface Transacao {
  id: number                    // Identificador único gerado automaticamente
  descricao: string             // Descrição da transação
  valor: number                 // Valor da transação (número decimal)
  tipo: 'Despesa' | 'Receita'   // Tipo: despesa ou receita
  categoriaId: number           // ID da categoria associada
  categoriaDescricao: string    // Descrição da categoria (para exibição)
  pessoaId: number             // ID da pessoa associada
  pessoaNome: string           // Nome da pessoa (para exibição)
}

/**
 * Interface que representa os totais financeiros de uma pessoa.
 */
interface TotalPorPessoa {
  pessoaId: number        // ID da pessoa
  pessoaNome: string      // Nome da pessoa
  pessoaIdade: number     // Idade da pessoa
  totalReceitas: number   // Soma de todas as receitas da pessoa
  totalDespesas: number   // Soma de todas as despesas da pessoa
  saldo: number          // Saldo calculado (receitas - despesas)
}

/**
 * Interface que representa os totais gerais de todas as pessoas.
 */
interface TotalGeralPessoas {
  totalGeralReceitas: number  // Soma de todas as receitas de todas as pessoas
  totalGeralDespesas: number // Soma de todas as despesas de todas as pessoas
  saldoLiquido: number       // Saldo líquido geral (receitas - despesas)
}

/**
 * Componente principal da aplicação.
 * Gerencia todo o estado da aplicação e renderiza a interface do usuário.
 * Utiliza React Hooks (useState, useEffect) para gerenciar estado e efeitos colaterais.
 */
function App() {
  // Estados para gerenciar pessoas
  const [pessoas, setPessoas] = useState<Pessoa[]>([]) // Lista de todas as pessoas cadastradas
  const [novaPessoa, setNovaPessoa] = useState({ nome: '', idade: '' }) // Dados do formulário de nova pessoa
  const [erroPessoa, setErroPessoa] = useState('') // Mensagem de erro para operações com pessoas

  // Estados para gerenciar categorias
  const [categorias, setCategorias] = useState<Categoria[]>([]) // Lista de todas as categorias cadastradas
  const [novaCategoria, setNovaCategoria] = useState({ descricao: '', finalidade: 'Despesa' as 'Despesa' | 'Receita' | 'Ambas' }) // Dados do formulário de nova categoria
  const [erroCategoria, setErroCategoria] = useState('') // Mensagem de erro para operações com categorias

  // Estados para gerenciar transações
  const [transacoes, setTransacoes] = useState<Transacao[]>([]) // Lista de todas as transações cadastradas
  const [novaTransacao, setNovaTransacao] = useState({ 
    descricao: '', 
    valor: '', 
    tipo: 'Despesa' as 'Despesa' | 'Receita',
    categoriaId: '',
    pessoaId: ''
  }) // Dados do formulário de nova transação
  const [erroTransacao, setErroTransacao] = useState('') // Mensagem de erro para operações com transações

  // Estados para gerenciar consultas de totais
  const [totaisPorPessoa, setTotaisPorPessoa] = useState<TotalPorPessoa[]>([]) // Lista de totais por pessoa
  const [totalGeralPessoas, setTotalGeralPessoas] = useState<TotalGeralPessoas | null>(null) // Total geral de todas as pessoas
  const [mostrarTotais, setMostrarTotais] = useState(false) // Controla se a seção de totais está visível
  
  // Estado para indicar o status da conexão com o backend
  const [statusConexao, setStatusConexao] = useState<'conectado' | 'desconectado' | 'verificando'>('verificando')

  /**
   * Hook useEffect que executa quando o componente é montado (carregado pela primeira vez).
   * Carrega os dados iniciais da API: pessoas, categorias e transações.
   * O array vazio [] como segundo parâmetro garante que execute apenas uma vez.
   */
  useEffect(() => {
    // As funções de carregar já verificam a conexão e atualizam o status
    carregarPessoas()
    carregarCategorias()
    carregarTransacoes()
  }, [])

  /**
   * Função para carregar todas as pessoas cadastradas da API.
   * Faz uma requisição GET para o endpoint /api/Pessoa e atualiza o estado com os dados recebidos.
   * Em caso de sucesso, atualiza o status de conexão para 'conectado'.
   * Em caso de erro, exibe mensagem de erro e atualiza o status para 'desconectado'.
   */
  const carregarPessoas = async () => {
    try {
      console.log('Carregando pessoas...')
      const response = await axios.get(`${API_BASE_URL}/Pessoa`)
      console.log('Pessoas carregadas:', response.data)
      setPessoas(response.data)
      setErroPessoa('')
      setStatusConexao('conectado')
    } catch (error: any) {
      console.error('Erro ao carregar pessoas:', error)
      const mensagem = error.response?.data?.mensagem 
        || error.message 
        || 'Erro ao carregar pessoas. Verifique se o backend está rodando em http://localhost:5000'
      setErroPessoa(mensagem)
      setStatusConexao('desconectado')
      
      // Mostrar detalhes do erro no console para facilitar debug
      if (error.response) {
        console.error('Status:', error.response.status)
        console.error('Data:', error.response.data)
      } else if (error.request) {
        console.error('Sem resposta do servidor. Backend está rodando?')
      }
    }
  }

  /**
   * Função para carregar todas as categorias cadastradas da API.
   * Faz uma requisição GET para o endpoint /api/Categoria.
   * Converte os enums numéricos retornados pela API para strings legíveis ('Despesa', 'Receita', 'Ambas').
   * Atualiza o estado com as categorias formatadas.
   */
  const carregarCategorias = async () => {
    try {
      console.log('Carregando categorias...')
      const response = await axios.get(`${API_BASE_URL}/Categoria`)
      console.log('Categorias carregadas:', response.data)
      // Os enums já vêm como strings da API, mas garantimos o formato correto
      // Se vier como número, converte: 1=Despesa, 2=Receita, 3=Ambas
      const categoriasFormatadas = response.data.map((cat: any) => ({
        ...cat,
        finalidade: typeof cat.finalidade === 'number' 
          ? (cat.finalidade === 1 ? 'Despesa' : cat.finalidade === 2 ? 'Receita' : 'Ambas')
          : cat.finalidade
      }))
      setCategorias(categoriasFormatadas)
      setErroCategoria('')
    } catch (error: any) {
      console.error('Erro ao carregar categorias:', error)
      const mensagem = error.response?.data?.mensagem 
        || error.message 
        || 'Erro ao carregar categorias'
      setErroCategoria(mensagem)
    }
  }

  /**
   * Função para carregar todas as transações cadastradas da API.
   * Faz uma requisição GET para o endpoint /api/Transacao.
   * Converte os enums numéricos retornados pela API para strings legíveis ('Despesa', 'Receita').
   * Atualiza o estado com as transações formatadas.
   */
  const carregarTransacoes = async () => {
    try {
      console.log('Carregando transações...')
      const response = await axios.get(`${API_BASE_URL}/Transacao`)
      console.log('Transações carregadas:', response.data)
      // Os enums já vêm como strings da API, mas garantimos o formato correto
      // Se vier como número, converte: 1=Despesa, 2=Receita
      const transacoesFormatadas = response.data.map((trans: any) => ({
        ...trans,
        tipo: typeof trans.tipo === 'number' 
          ? (trans.tipo === 1 ? 'Despesa' : 'Receita')
          : trans.tipo
      }))
      setTransacoes(transacoesFormatadas)
      setErroTransacao('')
    } catch (error: any) {
      console.error('Erro ao carregar transações:', error)
      const mensagem = error.response?.data?.mensagem 
        || error.message 
        || 'Erro ao carregar transações'
      setErroTransacao(mensagem)
    }
  }

  /**
   * Função para criar uma nova pessoa no sistema.
   * Faz uma requisição POST para o endpoint /api/Pessoa com os dados do formulário.
   * Após criar com sucesso, limpa o formulário e recarrega a lista de pessoas.
   * Em caso de erro, exibe mensagem de erro para o usuário.
   * 
   * @param e - Evento do formulário (preventDefault para evitar reload da página)
   */
  const criarPessoa = async (e: React.FormEvent) => {
    e.preventDefault() // Previne o comportamento padrão do formulário (reload da página)
    setErroPessoa('')
    
    try {
      console.log('Criando pessoa...', { nome: novaPessoa.nome, idade: novaPessoa.idade })
      const response = await axios.post(`${API_BASE_URL}/Pessoa`, {
        nome: novaPessoa.nome,
        idade: parseInt(novaPessoa.idade) // Converte string para número
      })
      console.log('Pessoa criada:', response.data)
      setNovaPessoa({ nome: '', idade: '' }) // Limpa o formulário após sucesso
      await carregarPessoas() // Recarrega a lista para mostrar a nova pessoa
    } catch (error: any) {
      console.error('Erro ao criar pessoa:', error)
      const mensagem = error.response?.data?.mensagem 
        || error.message 
        || 'Erro ao criar pessoa. Verifique se o backend está rodando.'
      setErroPessoa(mensagem)
      
      if (error.response) {
        console.error('Status:', error.response.status)
        console.error('Data:', error.response.data)
      } else if (error.request) {
        console.error('Sem resposta do servidor. Backend está rodando em http://localhost:5000?')
      }
    }
  }

  /**
   * Função para criar uma nova categoria no sistema.
   * Faz uma requisição POST para o endpoint /api/Categoria.
   * Converte a finalidade de string para número (Despesa=1, Receita=2, Ambas=3) antes de enviar.
   * Após criar com sucesso, limpa o formulário e recarrega a lista de categorias.
   * 
   * @param e - Evento do formulário
   */
  const criarCategoria = async (e: React.FormEvent) => {
    e.preventDefault()
    setErroCategoria('')
    
    try {
      await axios.post(`${API_BASE_URL}/Categoria`, {
        descricao: novaCategoria.descricao,
        // Converte finalidade de string para número conforme esperado pela API
        finalidade: novaCategoria.finalidade === 'Despesa' ? 1 : novaCategoria.finalidade === 'Receita' ? 2 : 3
      })
      setNovaCategoria({ descricao: '', finalidade: 'Despesa' }) // Limpa o formulário
      carregarCategorias() // Recarrega a lista
    } catch (error: any) {
      setErroCategoria(error.response?.data?.mensagem || 'Erro ao criar categoria')
    }
  }

  /**
   * Função para criar uma nova transação no sistema.
   * Faz uma requisição POST para o endpoint /api/Transacao.
   * Converte os valores de string para os tipos corretos (número, enum) antes de enviar.
   * Após criar com sucesso, limpa o formulário, recarrega as transações e atualiza os totais se estiverem sendo exibidos.
   * 
   * @param e - Evento do formulário
   */
  const criarTransacao = async (e: React.FormEvent) => {
    e.preventDefault()
    setErroTransacao('')
    
    try {
      await axios.post(`${API_BASE_URL}/Transacao`, {
        descricao: novaTransacao.descricao,
        valor: parseFloat(novaTransacao.valor), // Converte string para número decimal
        tipo: novaTransacao.tipo === 'Despesa' ? 1 : 2, // Converte string para enum numérico
        categoriaId: parseInt(novaTransacao.categoriaId), // Converte string para número
        pessoaId: parseInt(novaTransacao.pessoaId) // Converte string para número
      })
      setNovaTransacao({ descricao: '', valor: '', tipo: 'Despesa', categoriaId: '', pessoaId: '' }) // Limpa o formulário
      carregarTransacoes() // Recarrega a lista de transações
      // Se os totais estiverem sendo exibidos, atualiza também
      if (mostrarTotais) {
        carregarTotaisPorPessoa()
      }
    } catch (error: any) {
      setErroTransacao(error.response?.data?.mensagem || 'Erro ao criar transação')
    }
  }

  /**
   * Função para deletar uma pessoa do sistema.
   * Solicita confirmação do usuário antes de deletar, pois a ação é irreversível.
   * Ao deletar uma pessoa, todas as suas transações são removidas automaticamente (cascade delete).
   * Após deletar, recarrega as listas de pessoas e transações, e atualiza os totais se estiverem sendo exibidos.
   * 
   * @param id - Identificador único da pessoa a ser deletada
   */
  const deletarPessoa = async (id: number) => {
    // Solicita confirmação do usuário antes de deletar
    if (!window.confirm('Tem certeza que deseja deletar esta pessoa? Todas as transações desta pessoa serão removidas.')) {
      return // Cancela a operação se o usuário não confirmar
    }
    
    try {
      await axios.delete(`${API_BASE_URL}/Pessoa/${id}`)
      carregarPessoas() // Recarrega a lista de pessoas
      carregarTransacoes() // Recarrega a lista de transações (as transações da pessoa deletada já foram removidas pelo backend)
      // Se os totais estiverem sendo exibidos, atualiza também
      if (mostrarTotais) {
        carregarTotaisPorPessoa()
      }
    } catch (error: any) {
      alert(error.response?.data?.mensagem || 'Erro ao deletar pessoa')
    }
  }

  /**
   * Função para carregar os totais financeiros por pessoa.
   * Faz uma requisição GET para o endpoint /api/Consulta/TotaisPorPessoa.
   * Retorna o total de receitas, despesas e saldo de cada pessoa, além do total geral.
   * Atualiza o estado para exibir os totais na interface.
   */
  const carregarTotaisPorPessoa = async () => {
    try {
      const response = await axios.get(`${API_BASE_URL}/Consulta/TotaisPorPessoa`)
      setTotaisPorPessoa(response.data.totaisPorPessoa) // Lista de totais por pessoa
      setTotalGeralPessoas(response.data.totalGeral) // Total geral de todas as pessoas
      setMostrarTotais(true) // Exibe a seção de totais na interface
    } catch (error) {
      console.error('Erro ao carregar totais:', error)
    }
  }

  /**
   * Filtra as categorias disponíveis baseado no tipo de transação selecionado.
   * Regra de negócio: Uma categoria só pode ser usada se sua finalidade for compatível com o tipo da transação.
   * - Se o tipo for "Despesa": mostra apenas categorias com finalidade "Despesa" ou "Ambas"
   * - Se o tipo for "Receita": mostra apenas categorias com finalidade "Receita" ou "Ambas"
   */
  const categoriasDisponiveis = categorias.filter(cat => {
    if (novaTransacao.tipo === 'Despesa') {
      return cat.finalidade === 'Despesa' || cat.finalidade === 'Ambas'
    } else {
      return cat.finalidade === 'Receita' || cat.finalidade === 'Ambas'
    }
  })

  /**
   * Filtra as pessoas disponíveis baseado no tipo de transação selecionado.
   * Regra de negócio: Menores de idade (menores de 18 anos) só podem ter transações do tipo "Despesa".
   * - Se o tipo for "Receita": remove pessoas menores de 18 anos da lista
   * - Se o tipo for "Despesa": mostra todas as pessoas (incluindo menores)
   */
  const pessoasDisponiveis = pessoas.filter(pessoa => {
    if (novaTransacao.tipo === 'Receita' && pessoa.idade < 18) {
      return false // Menores de idade não podem ter receitas
    }
    return true // Todas as outras pessoas podem ser selecionadas
  })

  return (
    <div className="container">
      <div className="header">
        <h1>Controle de Gastos Residenciais</h1>
        <p>Sistema de gerenciamento de gastos e receitas familiares</p>
        <div style={{ marginTop: '10px', padding: '10px', borderRadius: '4px', backgroundColor: statusConexao === 'conectado' ? '#27ae60' : statusConexao === 'desconectado' ? '#e74c3c' : '#f39c12', color: 'white', display: 'inline-block' }}>
          {statusConexao === 'conectado' && 'Conectado à API'}
          {statusConexao === 'desconectado' && 'Desconectado - Verifique se o backend está rodando em http://localhost:5000'}
          {statusConexao === 'verificando' && 'Verificando conexão...'}
        </div>
      </div>

      {/* Seção de Cadastro de Pessoas */}
      <div className="section">
        <h2>Cadastro de Pessoas</h2>
        <form onSubmit={criarPessoa}>
          <div className="form-group">
            <label>Nome:</label>
            <input
              type="text"
              value={novaPessoa.nome}
              onChange={(e) => setNovaPessoa({ ...novaPessoa, nome: e.target.value })}
              required
            />
          </div>
          <div className="form-group">
            <label>Idade:</label>
            <input
              type="number"
              min="1"
              value={novaPessoa.idade}
              onChange={(e) => setNovaPessoa({ ...novaPessoa, idade: e.target.value })}
              required
            />
          </div>
          <button type="submit" className="button button-success">Cadastrar Pessoa</button>
          {erroPessoa && <div className="error">{erroPessoa}</div>}
        </form>

        <h3 style={{ marginTop: '30px', marginBottom: '15px' }}>Pessoas Cadastradas</h3>
        <table className="table">
          <thead>
            <tr>
              <th>ID</th>
              <th>Nome</th>
              <th>Idade</th>
              <th>Ações</th>
            </tr>
          </thead>
          <tbody>
            {pessoas.map((pessoa) => (
              <tr key={pessoa.id}>
                <td>{pessoa.id}</td>
                <td>{pessoa.nome}</td>
                <td>{pessoa.idade}</td>
                <td>
                  <button
                    className="button button-danger"
                    onClick={() => deletarPessoa(pessoa.id)}
                  >
                    Deletar
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {/* Seção de Cadastro de Categorias */}
      <div className="section">
        <h2>Cadastro de Categorias</h2>
        <form onSubmit={criarCategoria}>
          <div className="form-group">
            <label>Descrição:</label>
            <input
              type="text"
              value={novaCategoria.descricao}
              onChange={(e) => setNovaCategoria({ ...novaCategoria, descricao: e.target.value })}
              required
            />
          </div>
          <div className="form-group">
            <label>Finalidade:</label>
            <select
              value={novaCategoria.finalidade}
              onChange={(e) => setNovaCategoria({ ...novaCategoria, finalidade: e.target.value as 'Despesa' | 'Receita' | 'Ambas' })}
              required
            >
              <option value="Despesa">Despesa</option>
              <option value="Receita">Receita</option>
              <option value="Ambas">Ambas</option>
            </select>
          </div>
          <button type="submit" className="button button-success">Cadastrar Categoria</button>
          {erroCategoria && <div className="error">{erroCategoria}</div>}
        </form>

        <h3 style={{ marginTop: '30px', marginBottom: '15px' }}>Categorias Cadastradas</h3>
        <table className="table">
          <thead>
            <tr>
              <th>ID</th>
              <th>Descrição</th>
              <th>Finalidade</th>
            </tr>
          </thead>
          <tbody>
            {categorias.map((categoria) => (
              <tr key={categoria.id}>
                <td>{categoria.id}</td>
                <td>{categoria.descricao}</td>
                <td>{categoria.finalidade}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {/* Seção de Cadastro de Transações */}
      <div className="section">
        <h2>Cadastro de Transações</h2>
        <form onSubmit={criarTransacao}>
          <div className="form-group">
            <label>Descrição:</label>
            <input
              type="text"
              value={novaTransacao.descricao}
              onChange={(e) => setNovaTransacao({ ...novaTransacao, descricao: e.target.value })}
              required
            />
          </div>
          <div className="form-group">
            <label>Valor:</label>
            <input
              type="number"
              step="0.01"
              min="0.01"
              value={novaTransacao.valor}
              onChange={(e) => setNovaTransacao({ ...novaTransacao, valor: e.target.value })}
              required
            />
          </div>
          <div className="form-group">
            <label>Tipo:</label>
            <select
              value={novaTransacao.tipo}
              onChange={(e) => setNovaTransacao({ ...novaTransacao, tipo: e.target.value as 'Despesa' | 'Receita', categoriaId: '', pessoaId: '' })}
              required
            >
              <option value="Despesa">Despesa</option>
              <option value="Receita">Receita</option>
            </select>
          </div>
          <div className="form-group">
            <label>Categoria:</label>
            <select
              value={novaTransacao.categoriaId}
              onChange={(e) => setNovaTransacao({ ...novaTransacao, categoriaId: e.target.value })}
              required
            >
              <option value="">Selecione uma categoria</option>
              {categoriasDisponiveis.map((categoria) => (
                <option key={categoria.id} value={categoria.id}>
                  {categoria.descricao} ({categoria.finalidade})
                </option>
              ))}
            </select>
          </div>
          <div className="form-group">
            <label>Pessoa:</label>
            <select
              value={novaTransacao.pessoaId}
              onChange={(e) => setNovaTransacao({ ...novaTransacao, pessoaId: e.target.value })}
              required
            >
              <option value="">Selecione uma pessoa</option>
              {pessoasDisponiveis.map((pessoa) => (
                <option key={pessoa.id} value={pessoa.id}>
                  {pessoa.nome} ({pessoa.idade} anos)
                </option>
              ))}
            </select>
            {novaTransacao.tipo === 'Receita' && pessoasDisponiveis.length === 0 && (
              <div className="error">Nenhuma pessoa maior de idade disponível para receitas</div>
            )}
          </div>
          <button type="submit" className="button button-success">Cadastrar Transação</button>
          {erroTransacao && <div className="error">{erroTransacao}</div>}
        </form>

        <h3 style={{ marginTop: '30px', marginBottom: '15px' }}>Transações Cadastradas</h3>
        <table className="table">
          <thead>
            <tr>
              <th>ID</th>
              <th>Descrição</th>
              <th>Valor</th>
              <th>Tipo</th>
              <th>Categoria</th>
              <th>Pessoa</th>
            </tr>
          </thead>
          <tbody>
            {transacoes.map((transacao) => (
              <tr key={transacao.id}>
                <td>{transacao.id}</td>
                <td>{transacao.descricao}</td>
                <td>R$ {transacao.valor.toFixed(2)}</td>
                <td>{transacao.tipo}</td>
                <td>{transacao.categoriaDescricao}</td>
                <td>{transacao.pessoaNome}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {/* Seção de Consulta de Totais */}
      <div className="section">
        <h2>Consulta de Totais por Pessoa</h2>
        <button className="button" onClick={carregarTotaisPorPessoa}>
          {mostrarTotais ? 'Atualizar Totais' : 'Consultar Totais'}
        </button>

        {mostrarTotais && (
          <>
            <h3 style={{ marginTop: '30px', marginBottom: '15px' }}>Totais por Pessoa</h3>
            <table className="table">
              <thead>
                <tr>
                  <th>Pessoa</th>
                  <th>Idade</th>
                  <th>Total Receitas</th>
                  <th>Total Despesas</th>
                  <th>Saldo</th>
                </tr>
              </thead>
              <tbody>
                {totaisPorPessoa.map((total) => (
                  <tr key={total.pessoaId}>
                    <td>{total.pessoaNome}</td>
                    <td>{total.pessoaIdade}</td>
                    <td>R$ {total.totalReceitas.toFixed(2)}</td>
                    <td>R$ {total.totalDespesas.toFixed(2)}</td>
                    <td className={total.saldo >= 0 ? 'total-positive' : 'total-negative'}>
                      R$ {total.saldo.toFixed(2)}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>

            {totalGeralPessoas && (
              <div className="totals-section">
                <h3>Total Geral</h3>
                <div className="total-item">
                  <span className="total-label">Total Geral de Receitas:</span>
                  <span className="total-value">R$ {totalGeralPessoas.totalGeralReceitas.toFixed(2)}</span>
                </div>
                <div className="total-item">
                  <span className="total-label">Total Geral de Despesas:</span>
                  <span className="total-value">R$ {totalGeralPessoas.totalGeralDespesas.toFixed(2)}</span>
                </div>
                <div className="total-item">
                  <span className="total-label">Saldo Líquido:</span>
                  <span className={totalGeralPessoas.saldoLiquido >= 0 ? 'total-positive' : 'total-negative'}>
                    R$ {totalGeralPessoas.saldoLiquido.toFixed(2)}
                  </span>
                </div>
              </div>
            )}
          </>
        )}
      </div>
    </div>
  )
}

export default App
