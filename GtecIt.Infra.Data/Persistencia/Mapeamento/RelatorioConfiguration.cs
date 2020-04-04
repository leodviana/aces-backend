using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class RelatorioConfiguration : EntityTypeConfiguration<Relatorio>
    {

        public RelatorioConfiguration()
            {
              // Primary Key
            this.HasKey(t => t.id_Grlrelatorios);

            // Properties
            this.Property(t => t.relatorio)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("grlrelatorios", "dbgtec_2");
            this.Property(t => t.id_Grlrelatorios).HasColumnName("id_Grlrelatorios");
            this.Property(t => t.Empresa).HasColumnName("Empresa");
            this.Property(t => t.relatorio).HasColumnName("relatorio");

            // Relationships
            this.HasOptional(t => t.grlempresa)
                .WithMany(t => t.grlrelatorios)
                .HasForeignKey(d => d.Empresa);

          }
      }
    }

