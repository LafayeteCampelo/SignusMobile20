using SQLite;

namespace SignusMobile20.Models
{
    public class CFormigasCQ
    {
        [PrimaryKey, AutoIncrement]
        public int PKCFormigasCQ { get; set; }
        public int PKCFormigasOP { get; set; }
        public int CodCFormigasCQ { get; set; }
        public int CodCFormigasOP { get; set; }
        public int Repeticao { get; set; }
        public int NumFormAval { get; set; }
        public int NumFormNaoAp { get; set; }
        public int NumFormLocInad { get; set; }
        public int NumAplFormNCort { get; set; }
        public string ResponsAval { get; set; }
        public string DataAval { get; set; }
        public string Observacoes { get; set; }
        public bool Visible { get; set; }
        public string Imagem { get; set; }
    }
}
