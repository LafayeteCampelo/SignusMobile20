using SQLite;

namespace SignusMobile20.Models
{
    public class CapinasQuimOP
    {
        [PrimaryKey, AutoIncrement]
        public int PKCapinasQuimOP { get; set; }
        public int PKTalhoesCR { get; set; }
        public int CodTrCulturaisOP { get; set; }
        public int CodLocal { get; set; }
        public string IdentifOperacao { get; set; }
        public int ProcedOperac { get; set; }
        public string TipoTC { get; set; }
        public string ProdutoUtilizado { get; set; }
        public float DoseProd { get; set; }
        public float DoseSoluc { get; set; }
        public string UnidDoseProd { get; set; }
        public string UnidDoseSoluc { get; set; }
        public string MetodoAplic { get; set; }
        public string Equipamento { get; set; }
        public string MarcaEquip { get; set; }
        public string ModeloEquip { get; set; }
        public string DataOper { get; set; }
        public float NumHoras { get; set; }
        public float Rendimento { get; set; }
        public string EmpresaResp { get; set; }
        public string ResponsavelOper { get; set; }
        public string Observacoes { get; set; }
        public bool Visible { get; set; }
        public string Imagem { get; set; }

    }
}
