using SQLite;

namespace SignusMobile20.Models
{
    public class PrepSoloCQ
    {
        [PrimaryKey, AutoIncrement]
        public int PKPrepSoloCQ { get; set; }
        public int PKPrepSoloOP { get; set; }
        public int CodPrepSoloCQ { get; set; }
        public int CodPrepSolo { get; set; }
        public int CodLocal { get; set; }
        public int Repeticao { get; set; }
        public int NumObs { get; set; }
        public float ProfundMed { get; set; }
        public float ProfundDesv { get; set; }
        public float EstrondLatMed { get; set; }
        public float EstrondLatDesv { get; set; }
        public float DistELMed { get; set; }
        public float DistELDesv { get; set; }
        public int ProfundInfMin { get; set; }
        public int ProfundInfMed { get; set; }
        public int EstrondLatInfMin { get; set; }
        public int EstrondLatInfMed { get; set; }
        public int DistELInfMin { get; set; }
        public int DistELInfMed { get; set; }
        public int DistELSupMed { get; set; }
        public int DistELSupMax { get; set; }
        public string ResponsavelAval { get; set; }
        public string DataAval { get; set; }
        public string Observacoes { get; set; }
        public bool Visible { get; set; }
        public string Imagem { get; set; }

    }
}
