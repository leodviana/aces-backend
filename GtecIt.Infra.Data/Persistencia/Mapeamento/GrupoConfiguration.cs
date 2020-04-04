using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class GrupoConfiguration : EntityTypeConfiguration<Grupo>
    {

        public GrupoConfiguration()
        {
            // Primary Key
            this.HasKey(t => t.Id_stqcdgrp);

            // Properties
            this.Property(t => t.desc_grupo)
                .HasMaxLength(60);

            this.Property(t => t.participa_rateio)
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("stqcdgrp", "dbgtec_2");
            this.Property(t => t.Id_stqcdgrp).HasColumnName("Id_stqcdgrp");
            this.Property(t => t.desc_grupo).HasColumnName("desc_grupo");
            this.Property(t => t.participa_rateio).HasColumnName("participa_rateio");
            this.Property(t => t.Id_grlcdusu).HasColumnName("Id_grlcdusu");

            
        }


    }
}

