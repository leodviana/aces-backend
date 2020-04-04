using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class TipoPagamentoConfiguration : EntityTypeConfiguration<TipoPagamento>
    {
        public TipoPagamentoConfiguration()
        {
            // Primary Key
            this.HasKey(t => t.Codigo);

            // Properties
            this.Property(t => t.Descricao)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("fintipopagamento");
            this.Property(t => t.Codigo).HasColumnName("codigo");
            this.Property(t => t.Descricao).HasColumnName("descricao");
        }
    }
}
