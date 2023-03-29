using SQLite;

namespace SignusMobile20.Models
{
    public class PrepSoloOP
    {
        [PrimaryKey, AutoIncrement]
        public int PKPrepSoloOP { get; set; }
        public int PKTalhoesCR { get; set; }
        public int CodPrepSolo { get; set; }
        public int CodLocal { get; set; }
        public string IdentifOperacao { get; set; }
        public int ProcedOperac { get; set; }
        public string Equipamento { get; set; }
        public string MarcaEquip { get; set; }
        public string ModeloEquip { get; set; }
        public string AplicFert { get; set; }
        public string DataInicio { get; set; }
        public string DataFinal { get; set; }
        public float Profundidade { get; set; }
        public float DistanciaEL { get; set; }
        public float NumHoras { get; set; }
        public float Rendimento { get; set; }
        public string EmpresaResp { get; set; }
        public string Operador { get; set; }
        public string Observacoes { get; set; }
        public bool Visible { get; set; }
        public string Imagem { get; set; }

    }
}
