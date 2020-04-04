using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class UsuarioConfiguration : EntityTypeConfiguration<Usuario>
    {
        public UsuarioConfiguration()
        {
            // Primary Key
            this.HasKey(t => t.UsuarioId);

            // Properties
            this.Property(t => t.Login)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.Senha)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Administrador)
                .HasMaxLength(1);

            this.Property(t => t.Ativo)
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("usuario", "dbgtec_2");
            this.Property(t => t.UsuarioId).HasColumnName("UsuarioId");
            this.Property(t => t.Id_grlbasico).HasColumnName("Id_grlbasico");
            this.Property(t => t.Login).HasColumnName("Login");
            this.Property(t => t.Senha).HasColumnName("Senha");
            this.Property(t => t.Administrador).HasColumnName("Administrador");
            this.Property(t => t.Ativo).HasColumnName("Ativo");

            // Relationships
            this.HasOptional(t => t.pessoas)
                .WithMany(t => t.usuarios)
                .HasForeignKey(d => d.Id_grlbasico);

        }
    }
    }

