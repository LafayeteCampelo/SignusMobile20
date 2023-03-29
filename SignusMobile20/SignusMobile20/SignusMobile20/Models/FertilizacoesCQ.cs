using SQLite;

namespace SignusMobile20.Models
{
    public class FertilizacoesCQ
    {
        [PrimaryKey, AutoIncrement]
        public int PKFertilizacoesCQ { get; set; }
        public int PKFertilizacoesOP { get; set; }
        public int CodFertilizacoesCQ { get; set; }
        public int CodFertilizacoesOP { get; set; }
        public int CodLocal { get; set; }
        public int Repeticao { get; set; }
        public int NumPtasAvaliadas { get; set; }
        public int PtasNaoAdubadas { get; set; }
        public float DistPlantaMed { get; set; }
        public float DistPlantaDesv { get; set; }
        public float ProfundMed { get; set; }
        public float ProfundDesv { get; set; }
        public int DistPlantaInfMin { get; set; }
        public int DistPlantaInfMed { get; set; }
        public int DistPlantaSupMed { get; set; }
        public int DistPlantaSupMax { get; set; }
        public int ProfundInfMin { get; set; }
        public int ProfundInfMed { get; set; }
        public int ProfundSupMed { get; set; }
        public int ProfundSupMax { get; set; }
        public float DistPercEquip { get; set; }
        public float Saida1 { get; set; }
        public float Saida2 { get; set; }
        public float Saida3 { get; set; }
        public float Saida4 { get; set; }
        public float Saida5 { get; set; }
        public float TotalSaidas { get; set; }
        public int ForaDaCova { get; set; }
        public string DataAval { get; set; }
        public string ResponsavelAval { get; set; }
        public string Observacoes { get; set; }
        public bool Visible { get; set; }
        public string Imagem { get; set; }

    }
}
