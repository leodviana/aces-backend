using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class NotaEntradaConfiguration : EntityTypeConfiguration<NotaEntrada>
    {
        public NotaEntradaConfiguration()
        {
            // Primary Key
            // Primary Key
            this.HasKey(t => t.Id_stqnoten);

            // Properties
            this.Property(t => t.serie_nf)
                .HasMaxLength(4);

            this.Property(t => t.historico_nf)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("stqnoten", "dbgtec_2");
            this.Property(t => t.Id_stqnoten).HasColumnName("Id_stqnoten");
            this.Property(t => t.Id_stqtpent).HasColumnName("Id_stqtpent");
            this.Property(t => t.dt_entrada).HasColumnName("dt_entrada");
            this.Property(t => t.Id_grlfornecedor).HasColumnName("Id_grlfornecedor");
            this.Property(t => t.Id_stqalmox).HasColumnName("Id_stqalmox");
            this.Property(t => t.Id_grlcccust).HasColumnName("Id_grlcccust");
            this.Property(t => t.num_nf).HasColumnName("num_nf");
            this.Property(t => t.serie_nf).HasColumnName("serie_nf");
            this.Property(t => t.dt_emissao_nf).HasColumnName("dt_emissao_nf");
            this.Property(t => t.Id_stqtpnot).HasColumnName("Id_stqtpnot");
            this.Property(t => t.historico_nf).HasColumnName("historico_nf");
            this.Property(t => t.Id_grlcdusu).HasColumnName("Id_grlcdusu");
            this.Property(t => t.status).HasColumnName("status");

            // Relationships
            this.HasOptional(t => t.CentrodeCustos)
                .WithMany(t => t.NotasEntradas)
                .HasForeignKey(d => d.Id_grlcccust);
            
            this.HasOptional(t => t.grlfornecedor)
                .WithMany(t => t.NotasEntradas)
                .HasForeignKey(d => d.Id_grlfornecedor);
            
            this.HasOptional(t => t.TipoNotas)
                .WithMany(t => t.NotasEntradas)
                .HasForeignKey(d => d.Id_stqtpent);

            this.HasOptional(t => t.TipoEntrada)
                .WithMany(t => t.NotasEntradas)
                .HasForeignKey(d => d.Id_stqtpnot);

        }
    }
}
