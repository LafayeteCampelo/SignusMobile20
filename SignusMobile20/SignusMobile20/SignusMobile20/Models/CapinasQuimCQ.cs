using SQLite;

namespace SignusMobile20.Models
{
    public class CapinasQuimCQ
    {
        [PrimaryKey, AutoIncrement]
        public int PKCapinasQuimCQ { get; set; }
        public int PKCapinasQuimOP { get; set; }
        public int CodCapQuimicasCQ { get; set; }
        public int CodTrCulturaisOP { get; set; }
        public int CodLocal { get; set; }
        public int Repeticao { get; set; }
        public float AreaAplic { get; set; }
        public float VolBico1 { get; set; }
        public float VolBico2 { get; set; }
        public float VolBico3 { get; set; }
        public float VolBico4 { get; set; }
        public float VolBico5 { get; set; }
        public float VolBico6 { get; set; }
        public float VolBico7 { get; set; }
        public float VolBico8 { get; set; }
        public float VolBico9 { get; set; }
        public float VolBico10 { get; set; }
        public float VolTotal { get; set; }
        public int NPtsAval { get; set; }
        public float VazaoMed { get; set; }
        public float VazaoDesv { get; set; }
        public int VazaoInfMin { get; set; }
        public int VazaoInfMed { get; set; }
        public int VazaoSupMed { get; set; }
        public int VazaoSupMax { get; set; }
        public float PressaoMed { get; set; }
        public float PressaoDesv { get; set; }
        public int PressaoInfMin { get; set; }
        public int PressaoInfMed { get; set; }
        public int PressaoSupMed { get; set; }
        public int PressaoSupMax { get; set; }
        public int InconfCobert { get; set; }
        public int InconfDeriva { get; set; }
        public string Avaliador { get; set; }
        public string DataAval { get; set; }
        public string Observacoes { get; set; }
        public bool Visible { get; set; }
        public string Imagem { get; set; }

    }
}
