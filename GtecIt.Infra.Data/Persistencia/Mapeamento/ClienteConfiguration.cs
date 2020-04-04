using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class ClienteConfiguration : EntityTypeConfiguration<Cliente>
    {
        
            public ClienteConfiguration()
            {
                // Primary Key
                this.HasKey(t => t.id_Grlcliente);

                // Properties
                this.Property(t => t.cd_usuario)
                    .HasMaxLength(1);

                // Table & Column Mappings
                this.ToTable("grlcliente", "dbgtec_2");
                this.Property(t => t.id_Grlcliente).HasColumnName("id_Grlcliente");
                this.Property(t => t.Id_grlbasico).HasColumnName("Id_grlbasico");
                this.Property(t => t.cd_usuario).HasColumnName("cd_usuario");
                this.Property(t => t.Ativo).HasColumnName("Ativo");
                // Relationships
                this.HasOptional(t => t.grlbasic)
                    .WithMany(t => t.clientes)
                    .HasForeignKey(d => d.Id_grlbasico);

            }
      }
    }

