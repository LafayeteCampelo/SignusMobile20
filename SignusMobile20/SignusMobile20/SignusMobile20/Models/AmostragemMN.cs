using SQLite;

namespace SignusMobile20.Models
{
    public class AmostragemMN
    {
        [PrimaryKey, AutoIncrement]
        public int PKAmostragemMN { get; set; }
        public int PKTalhoesCR { get; set; }
        public int CodAmostragemMNSR { get; set; }
        public int CodAmostragemMNMN { get; set; }
        public string IdAmostragem { get; set; }
        public string DataAmost { get; set; }
        public string Objetivo { get; set; }
        public int NParcelas { get; set; }
        public int NArvParc { get; set; }
        public int NRepTecidosTalh { get; set; }
        public int NRepTecidosParc { get; set; }
        public bool Folhas { get; set; }
        public bool Galhos { get; set; }
        public bool Casca { get; set; }
        public bool Lenho { get; set; }
        public bool Raizes { get; set; }
        public int NRepSoloTalh { get; set; }
        public int NRepSoloParc { get; set; }
        public int NCamadasSolo { get; set; }
        public string Responsavel { get; set; }
        public string Observacoes { get; set; }
        public bool Visible { get; set; }
        public string Imagem { get; set; }

    }
}
