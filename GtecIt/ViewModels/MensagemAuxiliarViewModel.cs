using System.Collections.Generic;

namespace GtecIt.ViewModels
{
    public class MensagemAuxiliarViewModel
    {
        public MensagemAuxiliarViewModel()
        {
            Mensagens = new List<string>();
        }
        public string Tipo { get; set; }
        public List<string> Mensagens { get; set; }

        public string Icone
        {
            get
            {
                switch (Tipo)
                {
                    case "danger":
                        return "exclamation-sign";
                    case "info":
                        return "info-sign";
                    case "warning":
                        return "warning-sign";
                    case "success":
                        return "ok-sign";
                    default:
                        return "";
                }
            }
            
        }
    }
}