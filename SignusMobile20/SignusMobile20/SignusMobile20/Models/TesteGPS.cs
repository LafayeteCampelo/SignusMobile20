using SQLite;

namespace SignusMobile20.Models
{
    public class TesteGPS
    {
        [PrimaryKey, AutoIncrement]
        public int PKTesteGPS { get; set; }
        public int PKTalhoesCR { get; set; }
        public float LatitudeDec { get; set; }
        public float LongitudeDec { get; set; }
    }
}
