using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class TipoEntradaConfiguration : EntityTypeConfiguration<TipoEntrada>
    {
        public TipoEntradaConfiguration()
        {
            // Primary Key
            this.HasKey(t => t.Id_stqtpent);

            // Properties
            this.Property(t => t.desc_tp_entrada)
                .HasMaxLength(30);

            // Table & Column Mappings
            this.ToTable("stqtpent", "dbgtec_2");
            this.Property(t => t.Id_stqtpent).HasColumnName("Id_stqtpent");
            this.Property(t => t.desc_tp_entrada).HasColumnName("desc_tp_entrada");
            this.Property(t => t.Id_grlcdusu).HasColumnName("Id_grlcdusu");

            // Relationships
            this.HasOptional(t => t.grlcdusu)
                .WithMany(t => t.TipoEntradas)
                .HasForeignKey(d => d.Id_grlcdusu);
        }
    }
}
