using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class TipoNotaConfiguration : EntityTypeConfiguration<TipoNota>
    {
        public TipoNotaConfiguration()
        {
            // Primary Key
            this.HasKey(t => t.Id_stqtpnot);

            // Properties
            this.Property(t => t.desc_tp_nota)
                .HasMaxLength(30);

            // Table & Column Mappings
            this.ToTable("stqtpnot", "dbgtec_2");
            this.Property(t => t.Id_stqtpnot).HasColumnName("Id_stqtpnot");
            this.Property(t => t.desc_tp_nota).HasColumnName("desc_tp_nota");
            this.Property(t => t.Id_grlcdusu).HasColumnName("Id_grlcdusu");

            // Relationships
            this.HasOptional(t => t.usuario)
                .WithMany(t => t.TipoNotas)
                .HasForeignKey(d => d.Id_grlcdusu);
        }
    }
}
