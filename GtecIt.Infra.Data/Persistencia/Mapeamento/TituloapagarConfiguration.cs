using System.Data.Entity.ModelConfiguration;
using GtecIt.Domain.Entities;

namespace GtecIt.Infra.Data.Persistencia.Mapeamento
{
    public class TituloapagarConfiguration : EntityTypeConfiguration<Tituloapagar>
    {

        public TituloapagarConfiguration()
        {
            // Primary Key
            this.HasKey(t => t.id_fintitpg);

            // Properties
            this.Property(t => t.num_titulo)
                .HasMaxLength(10);

            this.Property(t => t.doc_pagamento)
                .HasMaxLength(10);

            this.Property(t => t.agencia)
                .HasMaxLength(10);

            this.Property(t => t.num_conta)
                .HasMaxLength(10);

            this.Property(t => t.Status)
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("fintitpg", "dbgtec_2");
            this.Property(t => t.id_fintitpg).HasColumnName("id_fintitpg");
            this.Property(t => t.dt_emissao).HasColumnName("dt_emissao");
            this.Property(t => t.Id_stqnoten).HasColumnName("Id_stqnoten");
            this.Property(t => t.num_titulo).HasColumnName("num_titulo");
            this.Property(t => t.dt_vencimento).HasColumnName("dt_vencimento");
            this.Property(t => t.valor).HasColumnName("valor");
            this.Property(t => t.Valor_pago).HasColumnName("Valor_pago");
            this.Property(t => t.dt_pagamento).HasColumnName("dt_pagamento");
            this.Property(t => t.id_fintipotitulo).HasColumnName("id_fintipotitulo");
            this.Property(t => t.id_fintipopagamento).HasColumnName("id_fintipopagamento");
            this.Property(t => t.doc_pagamento).HasColumnName("doc_pagamento");
            this.Property(t => t.agencia).HasColumnName("agencia");
            this.Property(t => t.cd_banco).HasColumnName("cd_banco");
            this.Property(t => t.cd_usuario).HasColumnName("cd_usuario");
            this.Property(t => t.num_conta).HasColumnName("num_conta");
            this.Property(t => t.Status).HasColumnName("Status");

            // Relationships
            this.HasOptional(t => t.fincdbanco)
                .WithMany(t => t.fintitpgs)
                .HasForeignKey(d => d.cd_banco);
            this.HasOptional(t => t.fintipopagamento)
                .WithMany(t => t.titulosapagar)
                .HasForeignKey(d => d.id_fintipopagamento);
            this.HasOptional(t => t.NotaEntradas)
                .WithMany(t => t.titulosapagar)
                .HasForeignKey(d => d.Id_stqnoten);

        }
    }
    }

