using SQLite;

namespace SignusMobile20.Models
{
    public class ResultadosFinaisDRIS
    {
        [PrimaryKey, AutoIncrement]
        public int PKResFinaisDRIS { get; set; }
        public int CodInform_Local { get; set; }
        public string CodEPTCR { get; set; }
        public string Dt_Col { get; set; }
        public float ID_a { get; set; }
        public string TipoInv { get; set; }
        public string DataInventario { get; set; }
        public int AnoMed { get; set; }
        public float DAP { get; set; }
        public float DAPq { get; set; }
        public float HTotal { get; set; }
        public float Volume { get; set; }
        public float IMA { get; set; }
        public float VariavelY { get; set; }
        public int CodDadosArvore_AQ { get; set; }
        public string Parcela { get; set; }
        public string Arvore { get; set; }
        public string ParteArvore { get; set; }
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
        public string NutrGC1 { get; set; }
        public string NutrGC2 { get; set; }
        public string NutrGC3 { get; set; }
        public string NutrGC4 { get; set; }
        public string NutrGC5 { get; set; }
        public string NutrGC6 { get; set; }
        public string NutrGC7 { get; set; }
        public string NutrGC8 { get; set; }
        public string NutrGC9 { get; set; }
        public string NutrGC10 { get; set; }
        public string NutrGC11 { get; set; }
        public string NutrGC12 { get; set; }
        public string NutrGC13 { get; set; }
        public string NutrGC14 { get; set; }
        public float ValorGC1 { get; set; }
        public float ValorGC2 { get; set; }
        public float ValorGC3 { get; set; }
        public float ValorGC4 { get; set; }
        public float ValorGC5 { get; set; }
        public float ValorGC6 { get; set; }
        public float ValorGC7 { get; set; }
        public float ValorGC8 { get; set; }
        public float ValorGC9 { get; set; }
        public float ValorGC10 { get; set; }
        public float ValorGC11 { get; set; }
        public float ValorGC12 { get; set; }
        public float ValorGC13 { get; set; }
        public float ValorGC14 { get; set; }
        public float IdadeMed { get; set; }
        public float IMA1 { get; set; }
        public float IMA18 { get; set; }
        public float IMA2 { get; set; }
        public int AnoMN { get; set; }
        public float DAP_Ref { get; set; }
        public float HTotal_Ref { get; set; }
        public float Volume_Ref { get; set; }
        public float IMA_Ref { get; set; }
        public float DAP_Ref1 { get; set; }
        public float HTotal_Ref1 { get; set; }
        public float Volume_Ref1 { get; set; }
        public float IMA_Ref1 { get; set; }
        public float IDN { get; set; }
        public bool Visible { get; set; }
        public string Imagem { get; set; }

    }
}
