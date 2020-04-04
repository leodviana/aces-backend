using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class TipoAtendimentoConfiguration : EntityTypeConfiguration<TipoAtendimento>
    {
        public TipoAtendimentoConfiguration()
        {
            // Primary Key
            this.HasKey(t => t.id_Grltpatendimento);

            // Properties
            this.Property(t => t.desc_Grltpatendimento)
                .HasMaxLength(45);

            this.Property(t => t.cd_usuario)
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("grltpatendimento", "dbgtec_2");
            this.Property(t => t.id_Grltpatendimento).HasColumnName("id_Grltpatendimento");
            this.Property(t => t.desc_Grltpatendimento).HasColumnName("desc_Grltpatendimento");
            this.Property(t => t.cd_usuario).HasColumnName("cd_usuario");
        }
    }
}
