using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class ConvenioConfiguration : EntityTypeConfiguration<Convenio>
    {

        public ConvenioConfiguration()
        {
            // Primary Key
            this.HasKey(t => t.id_grlconvenio);

            // Properties
            this.Property(t => t.Guia)
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("grlconvenio", "dbgtec_2");
            this.Property(t => t.id_grlconvenio).HasColumnName("id_grlconvenio");
            this.Property(t => t.id_grlbasico).HasColumnName("id_grlbasico");
            this.Property(t => t.Guia).HasColumnName("Guia");
            this.Property(t => t.Ativo).HasColumnName("Ativo");
            this.Property(t => t.cd_usuario).HasColumnName("cd_usuario");

            // Relationships
            this.HasOptional(t => t.grlbasic)
                .WithMany(t => t.convenios)
                .HasForeignKey(d => d.id_grlbasico);

        }
    }
}

