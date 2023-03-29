using SQLite;

namespace SignusMobile20.Models
{
    class ResultadosAnSolos
    {
        [PrimaryKey, AutoIncrement]
        public int PKResAnSolos { get; set; }
        public int CodInform_Local { get; set; }
        public string CodEPTCR { get; set; }
        public string DtColetaSolo { get; set; }
        public int AnoColetaSolo { get; set; }
        public float ProfInicial { get; set; }
        public float ProfFinal { get; set; }
        public float TeorARG { get; set; }
        public float PH_H2O { get; set; }
        public float PH_KCL { get; set; }
        public float PH_CACL2 { get; set; }
        public float pHSMP { get; set; }
        public float Nitrogenio { get; set; }
        public int MetodoN { get; set; }
        public float Pdisponivel { get; set; }
        public int MetodoPdisp { get; set; }
        public float P_Reman60 { get; set; }
        public int MetodoPrem { get; set; }
        public float Kdisponivel { get; set; }
        public int MetodoKdisp { get; set; }
        public float Cadisponivel { get; set; }
        public int MetodoCadisp { get; set; }
        public float Mgdisponivel { get; set; }
        public int MetodoMgdisp { get; set; }
        public float Sdisponivel { get; set; }
        public int MetodoSdisp { get; set; }
        public float AlTrocavel { get; set; }
        public int MetodoAltroc { get; set; }
        public float H_Al { get; set; }
        public int MetodoH_Al { get; set; }
        public float VrM { get; set; }
        public float CarbOrg { get; set; }
        public int MetodoCO { get; set; }
        public float MatOrg { get; set; }
        public int MetodoMOrg { get; set; }
        public float CTCEf { get; set; }
        public float CTCTot { get; set; }
        public float VrV { get; set; }
        public float mgdm3S { get; set; }
        public float mgdm3Cu { get; set; }
        public float mgdm3Fe { get; set; }
        public float mgdm3Mn { get; set; }
        public float mgdm3Zn { get; set; }
        public float mgdm3B { get; set; }
        public int MetodoCu { get; set; }
        public int MetodoFe { get; set; }
        public int MetodoMn { get; set; }
        public int MetodoZn { get; set; }
        public int MetodoB { get; set; }
        public string Objetivo { get; set; }
        public string Laboratorio { get; set; }
        public string Observacao { get; set; }
        public string CampoExtra { get; set; }
        public string IdentifAm { get; set; }
        public float TeorAreiaF { get; set; }
        public float TeorAreiaG { get; set; }
        public float TeorAreia { get; set; }
        public float TeorSilte { get; set; }
        public string ClasseText { get; set; }
        public float Umid01 { get; set; }
        public float Umid15 { get; set; }
        public float DensAp { get; set; }
        public float ArgDisp { get; set; }
        public float AguaDisp { get; set; }
        public float SB { get; set; }
        public string Obs { get; set; }
    }
}
