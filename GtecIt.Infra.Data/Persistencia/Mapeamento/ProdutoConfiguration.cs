using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class ProdutoConfiguration : EntityTypeConfiguration<Produto>
    {
        
            public ProdutoConfiguration()
            {
                // Primary Key
                this.HasKey(t => t.Id_stqcdprd);

                // Properties
                this.Property(t => t.desc_produto)
                    .HasMaxLength(65);

                this.Property(t => t.fracionado)
                    .HasMaxLength(1);

                this.Property(t => t.indicador_abc)
                    .HasMaxLength(1);

                this.Property(t => t.indicador_xyz)
                    .HasMaxLength(1);

                this.Property(t => t.status_produto)
                    .HasMaxLength(1);

                // Table & Column Mappings
                this.ToTable("stqcdprd", "dbgtec_2");
                this.Property(t => t.Id_stqcdprd).HasColumnName("Id_stqcdprd");
                this.Property(t => t.cd_produto).HasColumnName("cd_produto");
                this.Property(t => t.desc_produto).HasColumnName("desc_produto");
                this.Property(t => t.Id_stqcdgrp).HasColumnName("Id_stqcdgrp");
                this.Property(t => t.Id_stqsbgrp).HasColumnName("Id_stqsbgrp");
                this.Property(t => t.Id_stqvlunmed_con).HasColumnName("Id_stqvlunmed_con");
                this.Property(t => t.Id_stqvlunmed_est).HasColumnName("Id_stqvlunmed_est");
                this.Property(t => t.fracionado).HasColumnName("fracionado");
                this.Property(t => t.fator_fracao).HasColumnName("fator_fracao");
                this.Property(t => t.num_dias_estmin).HasColumnName("num_dias_estmin");
                this.Property(t => t.num_dias_estmax).HasColumnName("num_dias_estmax");
                this.Property(t => t.num_dias_reposi).HasColumnName("num_dias_reposi");
                this.Property(t => t.dt_calc_consumo).HasColumnName("dt_calc_consumo");
                this.Property(t => t.num_dias_calc).HasColumnName("num_dias_calc");
                this.Property(t => t.consumo_medio).HasColumnName("consumo_medio");
                this.Property(t => t.indicador_abc).HasColumnName("indicador_abc");
                this.Property(t => t.indicador_xyz).HasColumnName("indicador_xyz");
                this.Property(t => t.status_produto).HasColumnName("status_produto");
                this.Property(t => t.Id_grlcdusu).HasColumnName("Id_grlcdusu");

                // Relationships
              
                this.HasOptional(t => t.grupos)
                    .WithMany(t => t.produtos)
                    .HasForeignKey(d => d.Id_stqcdgrp);
                this.HasOptional(t => t.subgrupos)
                    .WithMany(t => t.produtos)
                    .HasForeignKey(d => d.Id_stqsbgrp);
                

            }
      }
    }

