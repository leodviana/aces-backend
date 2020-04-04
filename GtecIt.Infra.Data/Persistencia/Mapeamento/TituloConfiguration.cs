using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class TituloConfiguration : EntityTypeConfiguration<Titulo>
    {
        
            public TituloConfiguration()
            {
                this.HasKey(t => t.id_fintitrc);

                // Properties
                this.Property(t => t.num_titulo)
                    .HasMaxLength(10);

                this.Property(t => t.doc_pagamento)
                    .HasMaxLength(10);

                this.Property(t => t.agencia)
                    .HasMaxLength(10);

                this.Property(t => t.cd_usuario)
                    .HasMaxLength(1);

                this.Property(t => t.num_conta)
                    .HasMaxLength(20);

                this.Property(t => t.num_conta)
                    .HasMaxLength(1);
                // Table & Column Mappings
                this.ToTable("fintitrc", "dbgtec_2");
                this.Property(t => t.id_fintitrc).HasColumnName("id_fintitrc");
                this.Property(t => t.dt_emissao).HasColumnName("dt_emissao");
                this.Property(t => t.id_stqcporcamento).HasColumnName("id_stqcporcamento");
                this.Property(t => t.num_titulo).HasColumnName("num_titulo");
                this.Property(t => t.dt_vencimento).HasColumnName("dt_vencimento");
                this.Property(t => t.Valor).HasColumnName("Valor");
                this.Property(t => t.Valor_pago).HasColumnName("Valor_pago");
                this.Property(t => t.dt_pagamento).HasColumnName("dt_pagamento");
                this.Property(t => t.id_fintipotitulo).HasColumnName("id_fintipotitulo");
                this.Property(t => t.id_fintipopagamento).HasColumnName("id_fintipopagamento");
                this.Property(t => t.doc_pagamento).HasColumnName("doc_pagamento");
                this.Property(t => t.agencia).HasColumnName("agencia");
                this.Property(t => t.cd_banco).HasColumnName("cd_banco");
                this.Property(t => t.cd_usuario).HasColumnName("cd_usuario");
                this.Property(t => t.num_conta).HasColumnName("num_conta");
                this.Property(t => t.status).HasColumnName("status");

                // Relationships
                this.HasOptional(t => t.fintipopagamento)
                    .WithMany(t => t.Titulos)
                    .HasForeignKey(d => d.id_fintipopagamento);
                this.HasOptional(t => t.stqcporcamento)
                    .WithMany(t => t.Titulos)
                    .HasForeignKey(d => d.id_stqcporcamento);

                this.HasOptional(t => t.fincdbanco)
               .WithMany(t => t.fintitrcs)
               .HasForeignKey(d => d.cd_banco);

            }
      }
    }

