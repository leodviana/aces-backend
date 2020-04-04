using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class BancoConfiguration : EntityTypeConfiguration<Banco>
    {

        public BancoConfiguration()
            {
            // Primary Key
            this.HasKey(t => t.id_Fincdbanco);

            // Properties
            this.Property(t => t.cd_banco)
                .HasMaxLength(5);

            this.Property(t => t.desc_banco)
                .HasMaxLength(45);

            // Table & Column Mappings
            this.ToTable("fincdbanco", "dbgtec_2");
            this.Property(t => t.id_Fincdbanco).HasColumnName("id_Fincdbanco");
            this.Property(t => t.cd_banco).HasColumnName("cd_banco");
            this.Property(t => t.desc_banco).HasColumnName("desc_banco");
            this.Property(t => t.cd_usuario).HasColumnName("cd_usuario");
          }
      }
    }

