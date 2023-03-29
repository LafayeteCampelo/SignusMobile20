using SQLite;

namespace SignusMobile20.Models
{
    public class AmostSolos
    {
        [PrimaryKey, AutoIncrement]
        public int PKAmostSolos { get; set; }
        public int PKAmostragemMN { get; set; }
        public int CodAmostSolosSR { get; set; }
        public int CodAmostSolosMN { get; set; }
        public string IdAmostra { get; set; }
        public string DataAmostra { get; set; }
        public int Repeticao { get; set; }
        public double ProfIni { get; set; }
        public double ProfFin { get; set; }
        public string Objetivo { get; set; }
        public int NAmSimples { get; set; }
        public double Latit { get; set; }
        public double Longit { get; set; }
        public double Altit { get; set; }
        public double PrecisH { get; set; }
        public double PrecisV { get; set; }
        public string Responsavel { get; set; }
        public string Observacoes { get; set; }
        public bool Visible { get; set; }
        public string Imagem { get; set; }
    }
}
