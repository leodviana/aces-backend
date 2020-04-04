using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class SubGrupoConfiguration : EntityTypeConfiguration<SubGrupo>
    {

        public SubGrupoConfiguration()
        {
            this.HasKey(t => t.Id_stqsbgrp);

            // Properties
            this.Property(t => t.desc_subgrupo)
                .HasMaxLength(60);

            // Table & Column Mappings
            this.ToTable("stqsbgrp", "dbgtec_2");
            this.Property(t => t.Id_stqsbgrp).HasColumnName("Id_stqsbgrp");
            this.Property(t => t.desc_subgrupo).HasColumnName("desc_subgrupo");
            this.Property(t => t.Id_grlcdusu).HasColumnName("Id_grlcdusu");

            
        }


    }
}

