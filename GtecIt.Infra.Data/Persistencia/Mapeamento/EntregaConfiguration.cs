using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class EntregaConfiguration : EntityTypeConfiguration<Entrega>
    {

        public EntregaConfiguration()
            {
            // Primary Key
            this.HasKey(t => t.idEntrega);

                                

            // Table & Column Mappings
            this.ToTable("entrega", "dbgtec_aces");
            this.Property(t => t.idEntrega).HasColumnName("idEntrega");
            this.Property(t => t.Desc_entrega).HasColumnName("Desc_entrega");
          
           
        }
      }
    }

