using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class SexoConfiguration : EntityTypeConfiguration<Sexo>
    {
        public SexoConfiguration()
        {
            // Primary Key
            this.HasKey(t => t.Id_gercdsexo);

            // Properties
            this.Property(t => t.descricao)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("gercdsexo", "dbgtec_2");
            this.Property(t => t.Id_gercdsexo).HasColumnName("Id_gercdsexo");
            this.Property(t => t.descricao).HasColumnName("desc_sexo");
            this.Property(t => t.Id_grlcdusu).HasColumnName("Id_grlcdusu");

            // Relationships
           /* this.HasOptional(t => t.grlcdusu)
                .WithMany(t => t.gercdsexo)
                .HasForeignKey(d => d.Id_grlcdusu);*/
        }
    }
}
