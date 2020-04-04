using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class HorarioProfessorConfiguration : EntityTypeConfiguration<HorarioProfessor>
    {

        public HorarioProfessorConfiguration()
        {
            // Primary Key
            this.HasKey(t => t.idgercdhorarioProf);
                       
            // Table & Column Mappings
            this.ToTable("gercdhorarioprof", "dbgtec_aces");
            this.Property(t => t.idgercdhorarioProf).HasColumnName("idgercdhorarioProf");
            this.Property(t => t.id_grldentista).HasColumnName("id_grldentista");
                       
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.Dia).HasColumnName("dia");
            this.Property(t => t.id_Stqcporcamento).HasColumnName("id_Stqcporcamento");
            this.Property(t => t.id_Stqcporcamento_dupla).HasColumnName("id_Stqcporcamento_dupla");

            this.HasOptional(t => t.dentistas)
               .WithMany(t => t.HorarioProfessor)
               .HasForeignKey(d => d.id_grldentista);

           
        }
    }
}

