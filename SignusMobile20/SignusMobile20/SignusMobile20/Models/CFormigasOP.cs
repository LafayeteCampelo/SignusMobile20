using SQLite;

namespace SignusMobile20.Models
{
    public class CFormigasOP
    {
        [PrimaryKey, AutoIncrement]
        public int PKCFormigasOP { get; set; }
        public int PKTalhoesCR { get; set; }
        public int CodCFormigasOP { get; set; }
        public int CodLocal { get; set; }
        public string IdentifOperacao { get; set; }
        public int ProcedOperac { get; set; }
        public string TipoContr { get; set; }
        public string Produto { get; set; }
        public float Dose { get; set; }
        public string UnidDose { get; set; }
        public string MetodoContr { get; set; }
        public string Equipamento { get; set; }
        public string DataContr { get; set; }
        public float NumHoras { get; set; }
        public float Rendimento { get; set; }
        public string EmpresaResp { get; set; }
        public string Observacoes { get; set; }
        public bool Visible { get; set; }
        public string Imagem { get; set; }
    }
}
