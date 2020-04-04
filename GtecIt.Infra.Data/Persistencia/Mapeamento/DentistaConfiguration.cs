using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class DentistaConfiguration : EntityTypeConfiguration<Dentista>
    {
        
            public DentistaConfiguration()
            {
                // Primary Key
                this.HasKey(t => t.id_grldentista);

                // Properties
               // this.Property(t => t.cd_usuario)
                 //   .HasMaxLength(1);

                // Table & Column Mappings
                this.ToTable("grldentista", "dbgtec_2");

                
                this.Property(t => t.id_grldentista).HasColumnName("id_grldentista");
                this.Property(t => t.Id_grlbasico).HasColumnName("Id_grlbasico");
                //this.Property(t => t.cd_usuario).HasColumnName("cd_usuario");
                this.Property(t => t.Ativo).HasColumnName("Ativo");
                // Relationships
                this.HasOptional(t => t.Idgrlbasic)
                    .WithMany(t => t.dentistas)
                    .HasForeignKey(d => d.Id_grlbasico);

            }
      }
    }

