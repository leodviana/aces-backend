using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class ProfissaoConfiguration : EntityTypeConfiguration<Profissao>
    {
        public ProfissaoConfiguration()
        {
            // Primary Key
            this.HasKey(t => t.Id_grlprofi);

            // Properties
            this.Property(t => t.descricao)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("grlprofi", "dbgtec_2");
            this.Property(t => t.Id_grlprofi).HasColumnName("Id_grlprofi");
            this.Property(t => t.descricao).HasColumnName("desc_profissao");
            this.Property(t => t.Id_grlcdusu).HasColumnName("Id_grlcdusu");

            // Relationships
           /* this.HasOptional(t => t.grlcdusu)
                .WithMany(t => t.gercdsexo)
                .HasForeignKey(d => d.Id_grlcdusu);*/
        }
    }
}
