using System;
using System.Data.Entity;
using System.Linq;
using GtecIt.Domain.Entities;
using GtecIt.Infra.Data.Core.Interfaces;
using GtecIt.Infra.Data.Persistencia.Dto;

namespace GtecIt.Infra.Data.Persistencia.Repositorios
{
    public class AulaLogRepositorio : Repositorio<AulasLog>, IAulaLogRepositorio
    {
        public AulaLogRepositorio(DbContext context) : base(context)
        {
        }

        public GtecContext GtecContext
        {
            get { return Context as GtecContext; }
        }

        public IQueryable<AulaslogDto> ObterAulas(int id, DateTime inicio, DateTime fim,int contrato)
        {
            string aux_data = fim.ToString().Substring(0, 10);
            aux_data = aux_data + " 23:59:59";
            fim = Convert.ToDateTime(aux_data);

            var query = (from p in GtecContext.AulaLog
                         join b in GtecContext.Dentistas on
                         p.id_grldentista_inicial equals b.id_grldentista
                         join c in GtecContext.Dentistas on
                         p.id_grldentista_final equals c.id_grldentista

                         select new AulaslogDto
                         {
                             idGercdAulasLog = p.idGercdAulasLog,
                             id_Stqcporcamento_inicio = p.id_Stqcporcamento_inicio,
                             inicio = p.inicio,
                             Fim = p.Fim,
                             status_inicial = p.status_inicial,
                             dia_semana_inicial = p.dia_semana_inicial,
                             Subject_inicial = p.Subject_inicial,
                             id_grldentista_inicial = p.id_grldentista_inicial,
                             idGercdaulas_inicial = p.idGercdaulas_inicial,
                             idGercdaulas_final = p.idGercdaulas_final,
                             id_Stqcporcamento_final = p.id_Stqcporcamento_final,
                             horario_inicio_final = p.horario_inicio_final,
                             hora_final_final = p.hora_final_final,
                             status_final = p.status_final,
                             dia_semana_final = p.dia_semana_final,
                             Subject_final = p.Subject_final,
                             id_grldentista_final = p.id_grldentista_final,
                             id_usuario = p.id_usuario,
                             nome_dentista_inicial =b.Idgrlbasic.nome,
                             nome_dentista_final = c.Idgrlbasic.nome

                         }).AsQueryable();
            if (id != 0)
            {
                return query = query.Where(x => x.idGercdAulasLog == id);
            }
            if (contrato != 0)
            {
                return query = query.Where(x => x.id_Stqcporcamento_inicio == contrato);
            }
            // string aux_data = inicio.Value.Year.ToString() + "-" + inicio.Value.Month.ToString() + "-" + inicio.Value.Day.ToString();
            // inicio = Convert.ToDateTime(aux_data);

            query = query.Where(x => x.inicio >= inicio && x.inicio <= fim);
            

            return query;

            //GtecContext.Orcamentos
        }

       
    }
}
