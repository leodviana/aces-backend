using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class PlanoConfiguration : EntityTypeConfiguration<Plano>
    {

        public PlanoConfiguration()
            {
            // Primary Key
            this.HasKey(t => t.idGrlplanos);

            // Properties
           

            this.Property(t => t.desc_plano)
                .HasMaxLength(45);

            // Table & Column Mappings
            this.ToTable("grlplanos", "dbgtec_2");
            this.Property(t => t.idGrlplanos).HasColumnName("idGrlplanos");
           this.Property(t => t.desc_plano).HasColumnName("desc_plano");
            this.Property(t => t.cd_usuario).HasColumnName("cd_usuario");
          }
      }
    }

