using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class NotaEntradaItemConfiguration : EntityTypeConfiguration<NotaEntradaItem>
    {
        public NotaEntradaItemConfiguration()
        {

            // Primary Key
            this.HasKey(t => t.Id_stqentra);

            // Properties
            this.Property(t => t.status_entrada)
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("stqentra", "dbgtec_2");
            this.Property(t => t.Id_stqentra).HasColumnName("Id_stqentra");
            this.Property(t => t.id_stqnoten).HasColumnName("id_stqnoten");
            this.Property(t => t.num_item).HasColumnName("num_item");
            this.Property(t => t.cd_almox).HasColumnName("cd_almox");
            this.Property(t => t.tp_entrada).HasColumnName("tp_entrada");
            this.Property(t => t.dt_entrada).HasColumnName("dt_entrada");
            this.Property(t => t.cd_produto).HasColumnName("cd_produto");
            this.Property(t => t.qtd_entrada).HasColumnName("qtd_entrada");
            this.Property(t => t.valor_total).HasColumnName("valor_total");
            this.Property(t => t.status_entrada).HasColumnName("status_entrada");
            this.Property(t => t.Id_grlcdusu).HasColumnName("Id_grlcdusu");

            // Relationships
          
          
            this.HasOptional(t => t.stqcdprd)
                .WithMany(t => t.ItensEntrada)
                .HasForeignKey(d => d.cd_produto);
            this.HasRequired(t => t.NotasEntradas)
                .WithMany(t => t.itens_entrada)
                .HasForeignKey(d => d.id_stqnoten);
        }
    }
}
