using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class OrcamentoItemConfiguration : EntityTypeConfiguration<OrcamentoItem>
    {

        public OrcamentoItemConfiguration()
        {

            // Primary Key
            this.HasKey(t => t.id_stqitorcamento);

            // Properties
            this.Property(t => t.status)
                .HasMaxLength(1);

            this.Property(t => t.cd_usuario)
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("stqitorcamento", "dbgtec_2");
            this.Property(t => t.id_stqitorcamento).HasColumnName("id_stqitorcamento");
            this.Property(t => t.id_stqporcamento).HasColumnName("id_stqporcamento");
            this.Property(t => t.id_stqcdprd).HasColumnName("id_stqcdprd");
            this.Property(t => t.qtd).HasColumnName("qtd");
            this.Property(t => t.Vl_unitario).HasColumnName("Vl_unitario");
            this.Property(t => t.desconto).HasColumnName("desconto");
            this.Property(t => t.descontoperc).HasColumnName("descontoperc");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.cd_usuario).HasColumnName("cd_usuario");

            // Relationships
            this.HasOptional(t => t.produtos)
                .WithMany(t => t.OrcamentosiItems)
                .HasForeignKey(d => d.id_stqcdprd);
            this.HasOptional(t => t.orcamentos)
                .WithMany(t => t.itemorcamentos)
                .HasForeignKey(d => d.id_stqporcamento);


        }
    }
}

