using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class PedidoItemConfiguration : EntityTypeConfiguration<PedidoItem>
    {
        public PedidoItemConfiguration()
        {
            this.HasKey(t => t.Id_saida);

            // Properties
            this.Property(t => t.und_opc)
                .HasMaxLength(4);

            this.Property(t => t.status_saida)
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("stqsaida", "dbgtec");
            this.Property(t => t.Id_saida).HasColumnName("Id_saida");
            this.Property(t => t.num_saida).HasColumnName("num_saida");
            this.Property(t => t.num_item).HasColumnName("num_item");
            this.Property(t => t.tp_saida).HasColumnName("tp_saida");
            this.Property(t => t.cd_almox).HasColumnName("cd_almox");
            this.Property(t => t.dt_saida).HasColumnName("dt_saida");
            this.Property(t => t.cd_produto).HasColumnName("cd_produto");
            this.Property(t => t.qtd_saida).HasColumnName("qtd_saida");
            this.Property(t => t.valor_unitario).HasColumnName("valor_unitario");
            this.Property(t => t.valor_total).HasColumnName("valor_total");
            this.Property(t => t.valor_desconto).HasColumnName("valor_desconto");
            this.Property(t => t.valor_ipi).HasColumnName("valor_ipi");
            this.Property(t => t.qtd_saida_opc).HasColumnName("qtd_saida_opc");
            this.Property(t => t.und_opc).HasColumnName("und_opc");
            this.Property(t => t.valor_total_opc).HasColumnName("valor_total_opc");
            this.Property(t => t.status_saida).HasColumnName("status_saida");
            this.Property(t => t.cd_usuario).HasColumnName("cd_usuario");
            this.Property(t => t.vl_desc_perc).HasColumnName("vl_desc_perc");

            this.HasRequired(t => t.Pedido)
               .WithMany(t => t.PedidoItems)
               .HasForeignKey(d => d.num_saida);
        }
    }
}
