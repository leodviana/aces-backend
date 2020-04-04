using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class EmpresaConfiguration : EntityTypeConfiguration<Empresa>
    {

        public EmpresaConfiguration()
        {
            // Primary Key
            this.HasKey(t => t.id_Grlempresa);

            // Properties
            this.Property(t => t.descricao)
                .HasMaxLength(45);

            this.Property(t => t.endereco)
                .HasMaxLength(100);

            this.Property(t => t.logo)
                .HasMaxLength(45);

            this.Property(t => t.Status)
                    .HasMaxLength(1);

            this.Property(t => t.altura_logo).HasMaxLength(1);

            this.Property(t => t.comprimentro_logo).HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("grlempresa", "dbgtec_2");
            this.Property(t => t.id_Grlempresa).HasColumnName("id_Grlempresa");
            this.Property(t => t.descricao).HasColumnName("descricao");
            this.Property(t => t.endereco).HasColumnName("endereco");
            this.Property(t => t.logo).HasColumnName("logo");
            this.Property(t => t.altura_logo).HasColumnName("altura_logo");
            this.Property(t => t.comprimentro_logo).HasColumnName("comprimentro_logo");
            this.Property(t => t.Status).HasColumnName("Status");
        }
    }
}

