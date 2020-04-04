using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class EstadoCivilConfiguration : EntityTypeConfiguration<EstadoCivil>
    {
        public EstadoCivilConfiguration()
        {
            // Primary Key
            this.HasKey(t => t.Id_grlcivil);

            // Properties
            this.Property(t => t.descricao)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("grlcivil", "dbgtec_2");
            this.Property(t => t.Id_grlcivil).HasColumnName("Id_grlcivil");
            this.Property(t => t.descricao).HasColumnName("desc_civil");
            this.Property(t => t.Id_grlcdusu).HasColumnName("Id_grlcdusu");

            // Relationships
           /* this.HasOptional(t => t.grlcdusu)
                .WithMany(t => t.gercdsexo)
                .HasForeignKey(d => d.Id_grlcdusu);*/
        }
    }
}
