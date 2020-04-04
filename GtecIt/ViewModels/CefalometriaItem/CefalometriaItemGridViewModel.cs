namespace GtecIt.ViewModels
{
    public class CefalometriaItemGridViewModel 
    {
        public int id_Grlcefdent { get; set; }
        public int? id_grldentista { get; set; }
        public int? id_GrlCefalometrias { get; set; }
        public int? cd_usuario { get; set; }
        public virtual CefalometriaEditViewModel grlcefalometria { get; set; }
        public virtual DentistaEditViewModel grldentista { get; set; }
    }
}