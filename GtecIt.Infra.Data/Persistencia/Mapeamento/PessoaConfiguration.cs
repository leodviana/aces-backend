using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class PessoaConfiguration : EntityTypeConfiguration<Pessoa>
    {
        
            public PessoaConfiguration()
            {
                // Primary Key
                this.HasKey(t => t.Id_grlbasico);

                // Properties
                this.Property(t => t.nome)
                    .HasMaxLength(50);

               
                this.Property(t => t.cpf)
                    .HasMaxLength(14);
                this.Property(t => t.rg)
                    .HasMaxLength(20);
                
                this.Property(t => t.contato)
                    .HasMaxLength(20);

                this.Property(t => t.ddd_telefone)
                    .HasMaxLength(20);

                this.Property(t => t.ddd_telefone2)
                    .HasMaxLength(20);

                this.Property(t => t.email)
                    .HasMaxLength(50);

                this.Property(t => t.Email2)
                    .HasMaxLength(50);

                this.Property(t => t.status)
                   .HasMaxLength(1);

                // Table & Column Mappings
                this.ToTable("grlbasico", "dbgtec_2");
                this.Property(t => t.Id_grlbasico).HasColumnName("Id_grlbasico");
                this.Property(t => t.Id_grltopessoa).HasColumnName("Id_grltopessoa");
                this.Property(t => t.nome).HasColumnName("nome");
              this.Property(t => t.sexo).HasColumnName("Id_gercdsexo");
                this.Property(t => t.dt_nascimento).HasColumnName("dt_nascimento");
               this.Property(t => t.cpf).HasColumnName("cpf");
                this.Property(t => t.rg).HasColumnName("rg");
                this.Property(t => t.contato).HasColumnName("contato");
                this.Property(t => t.ddd_telefone).HasColumnName("ddd_telefone");
                this.Property(t => t.Id_grlidtel).HasColumnName("Id_grlidtel");
                this.Property(t => t.ddd_telefone2).HasColumnName("ddd_telefone2");
                this.Property(t => t.Id_grlidtel02).HasColumnName("Id_grlidtel02");
                this.Property(t => t.email).HasColumnName("email");
                this.Property(t => t.Email2).HasColumnName("Email2");
                this.Property(t => t.Id_grlcdusu).HasColumnName("Id_grlcdusu");
                this.Property(t => t.status).HasColumnName("status");

                // Relationships
                // Relationships
                this.HasRequired(t => t.gercdsexo)
                    .WithMany(t => t.Pessoas)
                    .HasForeignKey(d => d.sexo);
              
                this.HasOptional(t => t.grlidtel)
                    .WithMany(t => t.Pessoas)
                    .HasForeignKey(d => d.Id_grlidtel);
                this.HasOptional(t => t.grlidtel1)
                    .WithMany(t => t.Pessoas1)
                    .HasForeignKey(d => d.Id_grlidtel02);

            }
      }
    }

