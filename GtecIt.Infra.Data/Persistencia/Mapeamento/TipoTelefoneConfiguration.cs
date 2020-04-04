using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class TipoTelefoneConfiguration : EntityTypeConfiguration<TipoTelefone>
    {
        public TipoTelefoneConfiguration()
        {
            // Primary Key
            this.HasKey(t => t.id_grlidtel);

            // Properties
            this.Property(t => t.descricao)
                .HasMaxLength(30);

            // Table & Column Mappings
            this.ToTable("grlidtel", "dbgtec_2");
            this.Property(t => t.id_grlidtel).HasColumnName("id_grlidtel");
            this.Property(t => t.descricao).HasColumnName("desc_idtel");
            this.Property(t => t.id_grlcdusu).HasColumnName("id_grlcdusu");

            // Relationships
            /*this.HasOptional(t => t.grlcdusu)
                .WithMany(t => t.grlidtels)
                .HasForeignKey(d => d.id_grlcdusu);
             */ 

        }
    }
}
