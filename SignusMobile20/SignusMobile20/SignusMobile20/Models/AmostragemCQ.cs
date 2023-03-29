using SQLite;

namespace SignusMobile20.Models
{
    public class AmostragemCQ
    {
        [PrimaryKey, AutoIncrement]
        public int PKAmostragemCQ { get; set; }
        public int PKTalhoesCR { get; set; }
        public int PKCQ { get; set; }
        public string TipoCQ { get; set; }
        public string Id { get; set; }
        public float Valor { get; set; }

    }
}
