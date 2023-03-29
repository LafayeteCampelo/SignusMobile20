using SQLite;

namespace SignusMobile20.Models
{
    public class PlantioCQ
    {
        [PrimaryKey, AutoIncrement]
        public int PKPlantioCQ { get; set; }
        public int PKPlantioOP { get; set; }
        public int CodPlantioCQ { get; set; }
        public int CodPlantioOP { get; set; }
        public int Repeticao { get; set; }
        public int NumObs { get; set; }
        public float EspacELMed { get; set; }
        public float EspacELDesv { get; set; }
        public float EspacEPMed { get; set; }
        public float EspacEPDesv { get; set; }
        public int EspacELInfMin { get; set; }
        public int EspacELInfMed { get; set; }
        public int EspacELSupMed { get; set; }
        public int EspacELSupMax { get; set; }
        public int EspacEPInfMin { get; set; }
        public int EspacEPInfMed { get; set; }
        public int EspacEPSupMed { get; set; }
        public int EspacEPSupMax { get; set; }
        public int ColAfogado { get; set; }
        public int SubstExp { get; set; }
        public int MudaDanif { get; set; }
        public int MudaInclin { get; set; }
        public int MudaNaoFirme { get; set; }
        public int SemBacia { get; set; }
        public int CovasSemMuda { get; set; }
        public int MudasCob { get; set; }
        public int MudasForaSulco { get; set; }
        public string ResponsavelAval { get; set; }
        public string DataAval { get; set; }
        public string Observacoes { get; set; }
        public bool Visible { get; set; }
        public string Imagem { get; set; }
    }
}
