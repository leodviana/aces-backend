using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class PedidoConfiguration : EntityTypeConfiguration<Pedido>
    {
        public PedidoConfiguration()
        {
            this.HasKey(t => t.num_saida);

            // Properties
            this.Property(t => t.obs)
                .HasMaxLength(100);

            this.Property(t => t.serie_nf)
                .HasMaxLength(4);

            this.Property(t => t.status_pedido)
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("stqpedid", "dbgtec");
            this.Property(t => t.num_saida).HasColumnName("num_saida");
            this.Property(t => t.tp_saida).HasColumnName("tp_saida");
            this.Property(t => t.dt_saida).HasColumnName("dt_saida");
            this.Property(t => t.cd_cliente).HasColumnName("cd_cliente");
            this.Property(t => t.cd_almox).HasColumnName("cd_almox");
            this.Property(t => t.cd_vendedor).HasColumnName("cd_vendedor");
            this.Property(t => t.cd_trans).HasColumnName("cd_trans");
            this.Property(t => t.obs).HasColumnName("obs");
            this.Property(t => t.num_nf).HasColumnName("num_nf");
            this.Property(t => t.serie_nf).HasColumnName("serie_nf");
            this.Property(t => t.dt_emissao_nf).HasColumnName("dt_emissao_nf");
            this.Property(t => t.cd_prazo).HasColumnName("cd_prazo");
            this.Property(t => t.status_pedido).HasColumnName("status_pedido");
            this.Property(t => t.cd_usuario).HasColumnName("cd_usuario");
        }
    }
}
