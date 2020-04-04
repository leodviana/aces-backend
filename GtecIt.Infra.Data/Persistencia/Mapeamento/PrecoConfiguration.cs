using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class PrecoConfiguration : EntityTypeConfiguration<Preco>
    {
        public PrecoConfiguration()
        {
            // Primary Key
            this.HasKey(t => t.id_grlproceprecos);

            // Properties
            // Table & Column Mappings
            this.ToTable("grlproceprecos", "dbgtec_2");
            this.Property(t => t.id_grlproceprecos).HasColumnName("id_grlproceprecos");
            this.Property(t => t.id_grlconvenio).HasColumnName("id_grlconvenio");
            this.Property(t => t.id_stqcdprd).HasColumnName("id_stqcdprd");
            this.Property(t => t.vigencia).HasColumnName("vigencia");
            this.Property(t => t.preco).HasColumnName("preco");
            this.Property(t => t.cd_usuario).HasColumnName("cd_usuario");

            // Relationships
            this.HasOptional(t => t.convenios)
                .WithMany(t => t.Precos)
                .HasForeignKey(d => d.id_grlconvenio);
            this.HasOptional(t => t.produtos)
                .WithMany(t => t.precos)
                .HasForeignKey(d => d.id_stqcdprd);

        }

    }
}

