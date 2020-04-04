using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class AulaConfiguration : EntityTypeConfiguration<Aulas>
    {

        public AulaConfiguration()
            {
            // Primary Key
            this.HasKey(t => t.idGercdaulas);

                                

            // Table & Column Mappings
            this.ToTable("gercdaulas", "dbgtec_aces");
            this.Property(t => t.idGercdaulas).HasColumnName("idGercdaulas");
            this.Property(t => t.id_Stqcporcamento).HasColumnName("id_Stqcporcamento");
            this.Property(t => t.inicio).HasColumnName("inicio");
            this.Property(t => t.final).HasColumnName("final");
            this.Property(t => t.dia_semana).HasColumnName("dia_semana");
            this.Property(t => t.status).HasColumnName("status");

           
        }
      }
    }

