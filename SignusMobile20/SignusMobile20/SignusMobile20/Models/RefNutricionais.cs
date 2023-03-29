using SQLite;

namespace SignusMobile20.Models
{
    public class RefNutricionais
    {
        [PrimaryKey, AutoIncrement]
        public int PKRefNutricionais { get; set; }
        public string Caracteristica { get; set; }
        public string Especie { get; set; }
        public string MatGen { get; set; }
        public string Regiao { get; set; }
        public float Ref1 { get; set; }
        public float Ref2 { get; set; }
        public float Ref3 { get; set; }
        public float Ref4 { get; set; }
        public float Ref5 { get; set; }
        public string Classe1 { get; set; }
        public string Classe2 { get; set; }
        public string Classe3 { get; set; }
        public string Classe4 { get; set; }
        public string Classe5 { get; set; }
        public string Classe6 { get; set; }
        public string Observacoes { get; set; }
    }
}
