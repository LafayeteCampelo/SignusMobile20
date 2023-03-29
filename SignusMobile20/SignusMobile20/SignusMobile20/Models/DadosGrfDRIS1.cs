using SQLite;

namespace SignusMobile20.Models
{
    public class DadosGrfDRIS1
    {
        [PrimaryKey, AutoIncrement]
        public int CodInform_Local { get; set; }
        public string Dt_Col { get; set; }
        public float N { get; set; }
        public float P { get; set; }
        public float K { get; set; }
        public float Ca { get; set; }
        public float Mg { get; set; }
        public float S { get; set; }
        public float B { get; set; }
        public float Zn { get; set; }
        public float Cu { get; set; }
        public float Mo { get; set; }
        public float Fe { get; set; }
        public float Mn { get; set; }
        public float MS { get; set; }
        public float IDN { get; set; }

    }
}
