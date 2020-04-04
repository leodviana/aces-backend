using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class CefalometriaItemConfiguration : EntityTypeConfiguration<CefalometriaItem>
    {

        public CefalometriaItemConfiguration()
            {
                // Primary Key
                this.HasKey(t => t.id_Grlcefdent);

                // Properties
                // Table & Column Mappings
                this.ToTable("grlcefdent", "dbgtec_2");
                this.Property(t => t.id_Grlcefdent).HasColumnName("id_Grlcefdent");
                this.Property(t => t.id_grldentista).HasColumnName("id_grldentista");
                this.Property(t => t.id_GrlCefalometrias).HasColumnName("id_GrlCefalometrias");
                this.Property(t => t.cd_usuario).HasColumnName("cd_usuario");

                // Relationships
                this.HasOptional(t => t.grlcefalometria)
                    .WithMany(t => t.cefalometriaItem)
                    .HasForeignKey(d => d.id_GrlCefalometrias);
                this.HasOptional(t => t.grldentista)
                    .WithMany(t => t.cefalometriaItem)
                    .HasForeignKey(d => d.id_grldentista);
          }
      }
    }

