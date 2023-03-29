using SQLite;

namespace SignusMobile20.Models
{
    public class ParcelasMedArv
    {
        [PrimaryKey, AutoIncrement]
        public int PKParcelasMedArv { get; set; }
        public int PKAmostragemMN { get; set; }
        public int CodParcelasMedArvSR { get; set; }
        public int CodParcelasMedArvMN { get; set; }
        public string IdParcela { get; set; }
        public float Largura { get; set; }
        public float Comprimento { get; set; }
        public float Area { get; set; }
        public int NumCovas { get; set; }
        public double Latit { get; set; }
        public double Longit { get; set; }
        public double Altit { get; set; }
        public double PrecisH { get; set; }
        public double PrecisV { get; set; }
        public string Observacoes { get; set; }
        public bool Visible { get; set; }
        public string Imagem { get; set; }
    }
}
