﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtecIt.Infra.Data.Persistencia.Dto
{
    public class AulaslogDto
    {
        public int idGercdAulasLog { get; set; }
        public int? id_Stqcporcamento_inicio { get; set; }

        public DateTime inicio { get; set; }
        public DateTime Fim { get; set; }
        public string status_inicial { get; set; }
        public string dia_semana_inicial { get; set; }
        public string Subject_inicial { get; set; }
        public int? id_grldentista_inicial { get; set; }
        public int idGercdaulas_inicial { get; set; }
        public int idGercdaulas_final { get; set; }
        public int? id_Stqcporcamento_final { get; set; }

        public DateTime horario_inicio_final { get; set; }
        public DateTime hora_final_final { get; set; }
        public string status_final { get; set; }
        public string dia_semana_final { get; set; }
        public string Subject_final { get; set; }
        public int? id_grldentista_final { get; set; }

        public int id_usuario { get; set; }
        public string  nome_dentista_inicial { get; set; }
        public string  nome_dentista_final { get; set; }
    }
}
