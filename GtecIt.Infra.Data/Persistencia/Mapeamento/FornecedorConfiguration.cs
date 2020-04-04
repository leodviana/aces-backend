using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class FornecedorConfiguration : EntityTypeConfiguration<Fornecedor>
    {
        public FornecedorConfiguration()
        {
            // Primary Key
            this.HasKey(t => t.Id_grlfornecedor);

            // Properties
            

            this.Property(t => t.Ativo)
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("grlfornecedor", "dbgtec_2");
            this.Property(t => t.Id_grlfornecedor).HasColumnName("Id_grlfornecedor");
            this.Property(t => t.Id_grlbasic).HasColumnName("Id_grlbasic");
            this.Property(t => t.Id_grlcdusu).HasColumnName("Id_grlcdusu");
            this.Property(t => t.cd_usuario).HasColumnName("cd_usuario");
            this.Property(t => t.Ativo).HasColumnName("Ativo");

            // Relationships
            this.HasRequired(t => t.grlbasico)
                .WithMany(t => t.Fornecedores)
                .HasForeignKey(d => d.Id_grlbasic);
            
        }
    }
}
