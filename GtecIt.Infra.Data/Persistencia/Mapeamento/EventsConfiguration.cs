using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class EventsConfiguration : EntityTypeConfiguration<Events>
    {

        public EventsConfiguration()
            {
            // Primary Key
            this.HasKey(t => t.EventID);

                                

            // Table & Column Mappings
            this.ToTable("events", "dbgtec_aces");
            this.Property(t => t.EventID).HasColumnName("EventID");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Subject).HasColumnName("Subject");
            this.Property(t => t.Start).HasColumnName("Start");
            this.Property(t => t.End).HasColumnName("End");
            this.Property(t => t.IsFullDay).HasColumnName("IsFullDay");
            this.Property(t => t.ThemeColor).HasColumnName("ThemeColor");

           
        }
      }
    }

