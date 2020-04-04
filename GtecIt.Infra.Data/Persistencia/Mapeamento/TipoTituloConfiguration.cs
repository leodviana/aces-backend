using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class TipoTituloConfiguration : EntityTypeConfiguration<TipoTitulo>
    {
        public TipoTituloConfiguration()
        {
            // Primary Key
            this.HasKey(t => t.Codigo);

            // Properties
            this.Property(t => t.Descricao)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("fintipotitulo");
            this.Property(t => t.Codigo).HasColumnName("codigo");
            this.Property(t => t.Descricao).HasColumnName("descricao");
        }
    }
}
