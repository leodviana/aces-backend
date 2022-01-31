using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class RankingConfiguration : EntityTypeConfiguration<Ranking>
    {

        public RankingConfiguration()
            {
            // Primary Key
            this.HasKey(t => t.id_ranking);

                                

            // Table & Column Mappings
            this.ToTable("ranking", "dbgtec_aces_backup");
            this.Property(t => t.id_ranking).HasColumnName("id_ranking");
            this.Property(t => t.id_grlbasico).HasColumnName("id_grlbasico");
            this.Property(t => t.id_grlbasico_dupla).HasColumnName("id_grlbasico_dupla");
            this.Property(t => t.categoria).HasColumnName("categoria");
            this.Property(t => t.pontos).HasColumnName("pontos");
            this.Property(t => t.posicao).HasColumnName("posicao");



        }
      }
    }

