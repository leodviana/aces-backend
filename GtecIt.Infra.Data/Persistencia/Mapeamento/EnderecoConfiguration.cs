using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class EnderecoConfiguration : EntityTypeConfiguration<Endereco>
    {
        
            public EnderecoConfiguration()
            {
                // Primary Key
                this.HasKey(t => t.Id_Grendbasico);

                // Properties
                this.Property(t => t.Logradouro)
                    .HasMaxLength(50);

                this.Property(t => t.Complemento)
                    .HasMaxLength(45);

                this.Property(t => t.Bairro)
                    .HasMaxLength(45);

                this.Property(t => t.Cidade)
                    .HasMaxLength(45);

                this.Property(t => t.Estado)
                    .HasMaxLength(45);

                this.Property(t => t.Cep)
                    .HasMaxLength(10);

                // Table & Column Mappings
                this.ToTable("grendbasico", "dbgtec_2");
                this.Property(t => t.Id_Grendbasico).HasColumnName("Id_Grendbasico");
                this.Property(t => t.Id_grlbasic).HasColumnName("Id_grlbasic");
                this.Property(t => t.Logradouro).HasColumnName("Logradouro");
                this.Property(t => t.Complemento).HasColumnName("Complemento");
                this.Property(t => t.Bairro).HasColumnName("Bairro");
                this.Property(t => t.Cidade).HasColumnName("Cidade");
                this.Property(t => t.Estado).HasColumnName("Estado");
                this.Property(t => t.Cep).HasColumnName("Cep");
                this.Property(t => t.Id_grlidtel).HasColumnName("Id_grlidtel");
                this.Property(t => t.Id_grlcdusu).HasColumnName("Id_grlcdusu");

                this.HasOptional(t => t.grlbasic)
                .WithMany(t => t.enderecos)
                .HasForeignKey(d => d.Id_grlbasic);

                 this.HasOptional(t =>t.grlidtel)
                .WithMany(t => t.Enderecos)
                .HasForeignKey(d => d.Id_grlidtel);
            }
      }
    }

