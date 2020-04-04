using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class CefalometriaConfiguration : EntityTypeConfiguration<Cefalometria>
    {

        public CefalometriaConfiguration()
            {
             // Primary Key
            this.HasKey(t => t.id_GrlCefalometrias);

            // Properties
            this.Property(t => t.desc_cefalometria)
                .HasMaxLength(45);

            // Table & Column Mappings
            this.ToTable("grlcefalometrias", "dbgtec_2");
            this.Property(t => t.id_GrlCefalometrias).HasColumnName("id_GrlCefalometrias");
            this.Property(t => t.desc_cefalometria).HasColumnName("desc_cefalometria");
          }
      }
    }

