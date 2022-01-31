using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class AulaLogConfiguration : EntityTypeConfiguration<AulasLog>
    {

        public AulaLogConfiguration()
            {
            // Primary Key
            this.HasKey(t => t.idGercdAulasLog);

                                

            // Table & Column Mappings
            this.ToTable("gercdaulaslog", "dbgtec_aces");
            this.Property(t => t.idGercdAulasLog).HasColumnName("idGercdAulasLog");
            this.Property(t => t.idGercdaulas_inicial).HasColumnName("idGercdaulas_inicial");
            this.Property(t => t.id_Stqcporcamento_inicio).HasColumnName("id_Stqcporcamento_inicio");
            this.Property(t => t.inicio).HasColumnName("inicio");
            this.Property(t => t.idGercdaulas_final).HasColumnName("idGercdaulas_final");
            this.Property(t => t.dia_semana_inicial).HasColumnName("dia_semana_inicial");
            this.Property(t => t.dia_semana_final).HasColumnName("dia_semana_final");
            this.Property(t => t.Fim).HasColumnName("Fim");
            this.Property(t => t.hora_final_final).HasColumnName("hora_final_final");

            this.Property(t => t.id_grldentista_inicial).HasColumnName("id_grldentista_inicial");


            this.Property(t => t.id_grldentista_final).HasColumnName("id_grldentista_final");
               

            this.Property(e => e.horario_inicio_final).HasColumnName("horario_inicio_final");



            this.Property(t => t.status_inicial).HasColumnName("status_inicial");
                               
            this.Property(t => t.status_final).HasColumnName("status_final");

            this.Property(t => t.Subject_inicial).HasColumnName("Subject_inicial");
            this.Property(t => t.Subject_final).HasColumnName("Subject_final");
                   
        }
    }
    }

