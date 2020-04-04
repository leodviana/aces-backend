using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class CentrodecustoConfiguration : EntityTypeConfiguration<CentrodeCusto>
    {

        public CentrodecustoConfiguration()
            {
            // Primary Key
            this.HasKey(t => t.Id_grlccust);

            // Properties
            this.Property(t => t.desc_ccusto)
                .HasMaxLength(30);

            this.Property(t => t.Ativo)
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("grlccust", "dbgtec_2");
            this.Property(t => t.Id_grlccust).HasColumnName("Id_grlccust");
            this.Property(t => t.desc_ccusto).HasColumnName("desc_ccusto");
            this.Property(t => t.Id_grlcdusu).HasColumnName("Id_grlcdusu");
            this.Property(t => t.Ativo).HasColumnName("Ativo");

            // Relationships
           
        }
      }
    }

