using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class PrecoPlanoConfiguration : EntityTypeConfiguration<PrecoPlano>
    {
        public PrecoPlanoConfiguration()
        {
            // Primary Key
            this.HasKey(t => t.idgrlplanosprecos);

            // Properties
            // Table & Column Mappings
            this.ToTable("grlplanosprecos", "dbgtec_aces");
            this.Property(t => t.idgrlplanosprecos).HasColumnName("idgrlplanosprecos");
            this.Property(t => t.idGrlplanos).HasColumnName("idGrlplanos");
            this.Property(t => t.id_stqcdprd).HasColumnName("id_stqcdprd");
            this.Property(t => t.vigencia).HasColumnName("vigencia");
            this.Property(t => t.preco).HasColumnName("preco");
            this.Property(t => t.qtd_aulas).HasColumnName("qtd_aulas");
            this.Property(t => t.qtd_dias_plano).HasColumnName("qtd_dias_plano");
            this.Property(t => t.cd_usuario).HasColumnName("cd_usuario");

            // Relationships
            this.HasOptional(t => t.Planos)
                .WithMany(t => t.Precosplano)
                .HasForeignKey(d => d.idGrlplanos);
            this.HasOptional(t => t.produtos)
                .WithMany(t => t.precosplano)
                .HasForeignKey(d => d.id_stqcdprd);

        }

    }
}

