using SQLite;

namespace SignusMobile20.Models
{
    public class TalhoesCR
    {
        [PrimaryKey, AutoIncrement]
        public int PKTalhoesCR { get; set; }
        public int CodTalhoesCRSR { get; set; }
        public int CodInform_Local { get; set; }
        public int CodCadastro { get; set; }
        public string CodEPTCR { get; set; }
        public string Empresa { get; set; }
        public string Unidade { get; set; }
        public string Fazenda { get; set; }
        public string Talhao { get; set; }
        public string Subtalhao { get; set; }
        public int Ciclo { get; set; }
        public int Rotacao { get; set; }
        public float EspEL { get; set; }
        public float EspEP { get; set; }
        public string DataIniRot { get; set; }
        public float AreaTalh { get; set; }
        public float AreaSubtalh { get; set; }
        public string Especie { get; set; }
        public string MatGen { get; set; }
        public string Propag { get; set; }
        public string SoloClasse { get; set; }
        public string UnidMan { get; set; }
        public int LatitudeLocG { get; set; }
        public int LatitudeLocM { get; set; }
        public float LatitudeLocS { get; set; }
        public int LongitudeLocG { get; set; }
        public int LongitudeLocM { get; set; }
        public float LongitudeLocS { get; set; }
        public float UTMN { get; set; }
        public float UTME { get; set; }
        public string Datum { get; set; }
        public string Zona { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }
        public string Observacoes { get; set; }
        public bool Visible { get; set; }
        public string Imagem { get; set; }
    }
}
