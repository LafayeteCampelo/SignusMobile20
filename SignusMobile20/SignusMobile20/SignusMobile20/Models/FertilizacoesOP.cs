using SQLite;

namespace SignusMobile20.Models
{
    public class FertilizacoesOP
    {
        [PrimaryKey, AutoIncrement]
        public int PKFertilizacoesOP { get; set; }
        public int PKTalhoesCR { get; set; }
        public int CodFertilizacoesOP { get; set; }
        public int CodLocal { get; set; }
        public string IdentifOperacao { get; set; }
        public string TipoAdubacao { get; set; }
        public int ProcedOperac { get; set; }
        public int CodFertilizante { get; set; }
        public float DoseFertilizante { get; set; }
        public string UnidDose { get; set; }
        public float DistanciaPlt { get; set; }
        public float Profundidade { get; set; }
        public float EpocaAplicFert { get; set; }
        public string DataAplicacao { get; set; }
        public string MetodoAplic { get; set; }
        public string Equipamento { get; set; }
        public float NumHoras { get; set; }
        public float Rendimento { get; set; }
        public string EmpresaResp { get; set; }
        public string ResponsavelOper { get; set; }
        public string Observacoes { get; set; }
        public bool Visible { get; set; }
        public string Imagem { get; set; }
    }
}
