using SQLite;

namespace SignusMobile20.Models
{
    public class MedicaoArvore
    {
        [PrimaryKey, AutoIncrement]
        public int PKMedicaoArv { get; set; }
        public int PKAmostragemMN { get; set; }
        public int CodMedicaoArvSR { get; set; }
        public int CodMedicaoArvMN { get; set; }
        public string IdParcela { get; set; }
        public int ArvNum { get; set; }
        public int FusteNum { get; set; }
        public string TipoMedD { get; set; }
        public float HTotal { get; set; }
        public float MedD { get; set; }
        public float HDom { get; set; }
        public float HFuste { get; set; }
        public float DCopa { get; set; }
        public float HCopa { get; set; }
        public string Observacoes { get; set; }
        public bool Visible { get; set; }
        public string Imagem { get; set; }
    }
}
