using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using GtecIt.Domain.Entities;
using GtecIt.Infra.Data.Persistencia.Mapeamento;


namespace GtecIt.Infra.Data.Persistencia
{
    public partial class GtecContext : DbContext
    {
        static GtecContext()
        {
            Database.SetInitializer<GtecContext>(null);
        }

        public GtecContext()
            : base("Name=GtecConn")
        {
        }

        public DbSet<TipoPagamento> TipoPagamentos { get; set; }
        public DbSet<Banco> Bancos { get; set; }
        public DbSet<Cefalometria> Cefalometrias { get; set; }
        public DbSet<TipoTitulo> TipoTitulos { get; set; }
        public DbSet<TipoTelefone> TipoTelefones { get; set; }

       
        public DbSet<Sexo> Sexos { get; set; }
        public DbSet<EstadoCivil> EstadoCivils { get; set; }
        public DbSet<Profissao> Profissoes { get; set; }
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Dentista> Dentistas { get; set; }
        public DbSet<Convenio> Convenios { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }

        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<CentrodeCusto> CentrodeCustos { get; set; }
        public DbSet<NotaEntrada> NotaEntradas { get; set; }
        public DbSet<NotaEntradaItem> NotaEntradaItems { get; set; }
        public DbSet<TipoEntrada> TipoEntrada { get; set; }

        public DbSet<TipoAtendimento> TipoAtendimentos { get; set; }
        public DbSet<TipoNota> TipoNotas { get; set; }
        public DbSet<Tituloapagar> Titulosapagar { get; set; }

        public DbSet<Orcamento> Orcamentos { get; set; }
        public DbSet<OrcamentoItem> OrcamentoItens { get; set; }
        public DbSet<Titulo> Titulos { get; set; }

        

        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoItem> PedidoItems { get; set; }
        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<SubGrupo> SubGrupos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<CefalometriaItem> CefalometriaItems { get; set; }
        public DbSet<Preco> Precos { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Relatorio> Relatorios { get; set; }
        public DbSet<Plano> Planos { get; set; }
        public DbSet<PrecoPlano> PlanoPreco { get; set; }
        public DbSet<Aulas> Aula { get; set; }

        public DbSet<Events> Events { get; set; }
        public DbSet<HorarioProfessor> horarioprofessor { get; set; }
        public DbSet<Entrega> entrega { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new TipoPagamentoConfiguration());

            
            modelBuilder.Configurations.Add(new TipoTituloConfiguration());
            
            modelBuilder.Configurations.Add(new TipoTelefoneConfiguration());
            modelBuilder.Configurations.Add(new SexoConfiguration());
            modelBuilder.Configurations.Add(new EstadoCivilConfiguration());
            modelBuilder.Configurations.Add(new GrupoConfiguration());
            modelBuilder.Configurations.Add(new SubGrupoConfiguration());
            modelBuilder.Configurations.Add(new ProdutoConfiguration());
            

            modelBuilder.Configurations.Add(new ProfissaoConfiguration());
            modelBuilder.Configurations.Add(new NotaEntradaConfiguration());
            modelBuilder.Configurations.Add(new NotaEntradaItemConfiguration());
            modelBuilder.Configurations.Add(new TipoNotaConfiguration());
            modelBuilder.Configurations.Add(new TipoEntradaConfiguration());
            modelBuilder.Configurations.Add(new TipoAtendimentoConfiguration());

            modelBuilder.Configurations.Add(new PessoaConfiguration());
            modelBuilder.Configurations.Add(new EnderecoConfiguration());

            modelBuilder.Configurations.Add(new ClienteConfiguration());
            modelBuilder.Configurations.Add(new DentistaConfiguration());
            modelBuilder.Configurations.Add(new FornecedorConfiguration());

            modelBuilder.Configurations.Add(new PedidoConfiguration());
            modelBuilder.Configurations.Add(new PedidoItemConfiguration());


            modelBuilder.Configurations.Add(new OrcamentoConfiguration());
            modelBuilder.Configurations.Add(new OrcamentoItemConfiguration());
            modelBuilder.Configurations.Add(new TituloConfiguration());
            modelBuilder.Configurations.Add(new BancoConfiguration());
            modelBuilder.Configurations.Add(new ConvenioConfiguration());
            modelBuilder.Configurations.Add(new CefalometriaConfiguration());
            modelBuilder.Configurations.Add(new CentrodecustoConfiguration());
            modelBuilder.Configurations.Add(new TituloapagarConfiguration());


            modelBuilder.Configurations.Add(new CefalometriaItemConfiguration());

            modelBuilder.Configurations.Add(new PrecoConfiguration());
            modelBuilder.Configurations.Add(new UsuarioConfiguration());
            modelBuilder.Configurations.Add(new EmpresaConfiguration());
            modelBuilder.Configurations.Add(new RelatorioConfiguration());
            modelBuilder.Configurations.Add(new PlanoConfiguration());
            modelBuilder.Configurations.Add(new PrecoPlanoConfiguration());
            modelBuilder.Configurations.Add(new AulaConfiguration());
            modelBuilder.Configurations.Add(new EventsConfiguration());
            modelBuilder.Configurations.Add(new HorarioProfessorConfiguration());
            modelBuilder.Configurations.Add(new EntregaConfiguration());
            //modelBuilder.Configurations.Add(new PedidoItemConfiguration());

        }
    }
}
