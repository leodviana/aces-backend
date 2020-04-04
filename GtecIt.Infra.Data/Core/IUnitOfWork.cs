using System;
using GtecIt.Infra.Data.Core.Interfaces;


namespace GtecIt.Infra.Data.Core
     
{
    public interface IUnitOfWork : IDisposable
    {
        // IProfessorRepository Professors { get; }
        ITipoPagamentoRepositorio TipoPagamentos { get; }

       
        IBancoRepositorio Bancos { get; }
        ICefalometriaRepositorio Cefalometrias { get; }
        ITipoTituloRepositorio TipoTitulos { get; }
        ITipoTelefoneRepositorio TipoTelefones { get; }
        ISexoRepositorio Sexos { get; }
        IEstadoCivilRepositorio EstadoCivils { get; }
        IProfissaoRepositorio Profissoes { get; }
        IPessoaRepositorio Pessoas { get; }
        IClienteRepositorio Clientes { get; }
        IDentistaRepositorio Dentistas { get; }
        IConvenioRepositorio Convenios { get; }
        IEnderecoRepositorio Enderecos { get; }

        IFornecedorRepositorio Fornecedores { get; }
        ICentrodecustoRepositorio Centrodecusto { get; }
        INotaEntradaRepositorio NotaEntradas { get; }
        INotaEntradaItemRepositorio NotaEntradaItems { get; }
        ITipoEntradaRepositorio TipoEntradas { get; }

        ITipoAtendimentoRepositorio TipoAtendimentos { get; }

        ITituloapagarRepositorio Titulosapagar { get; }
        ITipoNotaRepositorio TipoNotas { get; }

        IOrcamentoRepositorio Orcamentos { get; }
        IOrcamentoItemRepositorio OrcamentoItens { get; }
        ITituloRepositorio Titulos { get; }
        
        IPedidoRepositorio Pedidos { get; }
        IPedidoItemRepositorio PedidoItems { get; }
        IGrupoRepositorio Grupos { get; }
        ISubGrupoRepositorio SubGrupos { get; }
        IProdutoRepositorio Produtos { get; }
        ICefalometriaItemRepositorio CefalometriaItems { get; }
        IPrecoRepositorio Precos { get; }
        IPrecoPlanoRepositorio PrecosPlano { get; }
        IEmpresaRepositorio Empresas { get; }
        IUsuarioRepositorio Usuarios { get; }
        IRelatorioRepositorio Relatorios { get; }
        IPlanoRepositorio Planos { get; set; }
        IAulaRepositorio Aulas { get; set; }
        IEventsRepositorio Events { get; set; }
        IHorarioProfessorRepositorio horarioprofessor { get; set; }
        IEntregaRepositorio entrega { get; set; }
        int Complete();
    }
}