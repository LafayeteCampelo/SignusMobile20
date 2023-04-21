using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SignusMobile20.Models;
using SQLite;
using System.Globalization;
using Math = System.Math;


namespace SignusMobile20
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pgAddCapinasQuimCQ : ContentPage
    {
        CapinasQuimCQ detalheCapinasQuimCQ;
        private SQLiteConnection conn;
        public CapinasQuimCQ capinasquimcq;
        private string pkcapquimop = "";

        public pgAddCapinasQuimCQ(CapinasQuimCQ details, string pkcapinasquimop)
        {
            InitializeComponent();
            //Retornar estas duas linhas abaixo caso dê algo errado
            conn = DependencyService.Get<ISQLite>().GetConnection();
            conn.CreateTable<CapinasQuimCQ>();
            pkcapquimop = pkcapinasquimop;

            PKCapinasQuimOP.Text = pkcapinasquimop;

            if (details != null)
            {
                detalheCapinasQuimCQ = details;
                PopulateDetails(detalheCapinasQuimCQ);
            }
        }

        private void PopulateDetails(CapinasQuimCQ details)
        {
            try { PKCapinasQuimOP.Text = details.PKCapinasQuimOP.ToString("#"); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor 'Código da capina química'", "Ok"); }
            try { PKCapinasQuimCQ.Text = details.PKCapinasQuimCQ.ToString("#"); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor 'Código do c.q. capina química'", "Ok"); }
            try { Repeticao.Text = details.Repeticao.ToString("#"); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Repeticao.Placeholder + "'", "Ok"); }
            try { AreaAplic.Text = details.AreaAplic.ToString("#.00"); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + AreaAplic.Placeholder + "'", "Ok"); }
            try { VolBico1.Text = details.VolBico1.ToString("#.00"); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + VolBico1.Placeholder + "'", "Ok"); }
            try { VolBico2.Text = details.VolBico2.ToString("#.00"); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + VolBico2.Placeholder + "'", "Ok"); }
            try { VolBico3.Text = details.VolBico3.ToString("#.00"); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + VolBico3.Placeholder + "'", "Ok"); }
            try { VolBico4.Text = details.VolBico4.ToString("#.00"); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + VolBico4.Placeholder + "'", "Ok"); }
            try { VolBico5.Text = details.VolBico5.ToString("#.00"); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + VolBico5.Placeholder + "'", "Ok"); }
            try { VolBico6.Text = details.VolBico6.ToString("#.00"); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + VolBico6.Placeholder + "'", "Ok"); }
            try { VolBico7.Text = details.VolBico7.ToString("#.00"); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + VolBico7.Placeholder + "'", "Ok"); }
            try { VolBico8.Text = details.VolBico8.ToString("#.00"); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + VolBico8.Placeholder + "'", "Ok"); }
            try { VolBico9.Text = details.VolBico9.ToString("#.00"); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + VolBico9.Placeholder + "'", "Ok"); }
            try { VolBico10.Text = details.VolBico10.ToString("#.00"); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + VolBico10.Placeholder + "'", "Ok"); }
            try { VolTotal.Text = details.VolTotal.ToString("#.00"); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + VolTotal.Placeholder + "'", "Ok"); }
            try { NPtsAval.Text = details.NPtsAval.ToString("#"); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + NPtsAval.Placeholder + "'", "Ok"); }
            try { VazaoMed.Text = details.VazaoMed.ToString("#.00"); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + VazaoMed.Placeholder + "'", "Ok"); }
            try { VazaoDesv.Text = details.VazaoDesv.ToString("#.00"); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + VazaoDesv.Placeholder + "'", "Ok"); }
            try { VazaoInfMin.Text = details.VazaoInfMin.ToString("#"); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + VazaoInfMin.Placeholder + "'", "Ok"); }
            try { VazaoInfMed.Text = details.VazaoInfMed.ToString("#"); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + VazaoInfMed.Placeholder + "'", "Ok"); }
            try { VazaoSupMed.Text = details.VazaoSupMed.ToString("#"); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + VazaoSupMed.Placeholder + "'", "Ok"); }
            try { VazaoSupMax.Text = details.VazaoSupMax.ToString("#"); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + VazaoSupMax.Placeholder + "'", "Ok"); }
            try { PressaoMed.Text = details.PressaoMed.ToString("#.00"); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + PressaoMed.Placeholder + "'", "Ok"); }
            try { PressaoDesv.Text = details.PressaoDesv.ToString("#.00"); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + PressaoDesv.Placeholder + "'", "Ok"); }
            try { PressaoInfMin.Text = details.PressaoInfMin.ToString("#"); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + PressaoInfMin.Placeholder + "'", "Ok"); }
            try { PressaoInfMed.Text = details.PressaoInfMed.ToString("#"); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + PressaoInfMed.Placeholder + "'", "Ok"); }
            try { PressaoSupMed.Text = details.PressaoSupMed.ToString("#"); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + PressaoSupMed.Placeholder + "'", "Ok"); }
            try { PressaoSupMax.Text = details.PressaoSupMax.ToString("#"); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + PressaoSupMax.Placeholder + "'", "Ok"); }
            try { InconfCobert.Text = details.InconfCobert.ToString("#"); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + InconfCobert.Placeholder + "'", "Ok"); }
            try { InconfDeriva.Text = details.InconfDeriva.ToString("#"); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + InconfDeriva.Placeholder + "'", "Ok"); }
            try { Avaliador.Text = details.Avaliador; }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Avaliador.Placeholder + "'", "Ok"); }
            if (details.DataAval != null)
            {
                try { DataAval.Date = DateTime.ParseExact(details.DataAval, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture); }
                catch { DisplayAlert("Message", "Não foi possível ler o valor 'Data da avaliação'", "Ok"); }
            }
            try { Observacoes.Text = details.Observacoes; }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Observacoes.Placeholder + "'", "Ok"); }
            //Device.BeginInvokeOnMainThread(async () => { await DisplayAlert("Alert", "No internet connection", "Ok"); });
            SaveBtn.Text = "Atualizar";
            this.Title = "Editar Operação";
        }
        private void SaveCapinasQuimCQ(object sender, EventArgs e)
        {
            if (SaveBtn.Text == "Salvar")
            {
                conn.BeginTransaction();
                //Device.BeginInvokeOnMainThread(async () => { await DisplayAlert("Alert", "No internet connection", "Ok"); });
                //DisplayAlert("Message", "Não foi possível atualizar. Verifique o valor digitado.", "Ok");
                CapinasQuimCQ capinasquimcq = new CapinasQuimCQ();
                capinasquimcq = new CapinasQuimCQ();
                if (PKCapinasQuimOP.Text != null)
                {
                    try { capinasquimcq.PKCapinasQuimOP = Convert.ToInt32(PKCapinasQuimOP.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo 'PKCapinasQuimOP'", "Ok"); }
                }
                if (Repeticao.Text != null)
                {
                    try { capinasquimcq.Repeticao = Convert.ToInt32(Repeticao.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + Repeticao.Placeholder + "'", "Ok"); }
                }
                if (AreaAplic.Text != null)
                {
                    try { capinasquimcq.AreaAplic = float.Parse(AreaAplic.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + AreaAplic.Placeholder + "'", "Ok"); }
                }
                if (VolBico1.Text != null)
                {
                    try { capinasquimcq.VolBico1 = float.Parse(VolBico1.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + VolBico1.Placeholder + "'", "Ok"); }
                }
                if (VolBico2.Text != null)
                {
                    try { capinasquimcq.VolBico2 = float.Parse(VolBico2.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + VolBico2.Placeholder + "'", "Ok"); }
                }
                if (VolBico3.Text != null)
                {
                    try { capinasquimcq.VolBico3 = float.Parse(VolBico3.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + VolBico3.Placeholder + "'", "Ok"); }
                }
                if (VolBico4.Text != null)
                {
                    try { capinasquimcq.VolBico4 = float.Parse(VolBico4.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + VolBico4.Placeholder + "'", "Ok"); }
                }
                if (VolBico5.Text != null)
                {
                    try { capinasquimcq.VolBico5 = float.Parse(VolBico5.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + VolBico5.Placeholder + "'", "Ok"); }
                }
                if (VolBico6.Text != null)
                {
                    try { capinasquimcq.VolBico6 = float.Parse(VolBico6.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + VolBico6.Placeholder + "'", "Ok"); }
                }
                if (VolBico7.Text != null)
                {
                    try { capinasquimcq.VolBico7 = float.Parse(VolBico7.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + VolBico7.Placeholder + "'", "Ok"); }
                }
                if (VolBico8.Text != null)
                {
                    try { capinasquimcq.VolBico8 = float.Parse(VolBico8.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + VolBico8.Placeholder + "'", "Ok"); }
                }
                if (VolBico9.Text != null)
                {
                    try { capinasquimcq.VolBico9 = float.Parse(VolBico9.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + VolBico9.Placeholder + "'", "Ok"); }
                }
                if (VolBico10.Text != null)
                {
                    try { capinasquimcq.VolBico10 = float.Parse(VolBico10.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + VolBico10.Placeholder + "'", "Ok"); }
                }
                if (VolTotal.Text != null)
                {
                    try { capinasquimcq.VolTotal = float.Parse(VolTotal.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + VolTotal.Placeholder + "'", "Ok"); }
                }
                if (NPtsAval.Text != null)
                {
                    try { capinasquimcq.NPtsAval = Convert.ToInt32(NPtsAval.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + NPtsAval.Placeholder + "'", "Ok"); }
                }
                if (VazaoMed.Text != null)
                {
                    try { capinasquimcq.VazaoMed = float.Parse(VazaoMed.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + VazaoMed.Placeholder + "'", "Ok"); }
                }
                if (VazaoDesv.Text != null)
                {
                    try { capinasquimcq.VazaoDesv = float.Parse(VazaoDesv.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + VazaoDesv.Placeholder + "'", "Ok"); }
                }
                if (VazaoInfMin.Text != null)
                {
                    try { capinasquimcq.VazaoInfMin = Convert.ToInt32(VazaoInfMin.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + VazaoInfMin.Placeholder + "'", "Ok"); }
                }
                if (VazaoInfMed.Text != null)
                {
                    try { capinasquimcq.VazaoInfMed = Convert.ToInt32(VazaoInfMed.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + VazaoInfMed.Placeholder + "'", "Ok"); }
                }
                if (VazaoSupMed.Text != null)
                {
                    try { capinasquimcq.VazaoSupMed = Convert.ToInt32(VazaoSupMed.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + VazaoSupMed.Placeholder + "'", "Ok"); }
                }
                if (VazaoSupMax.Text != null)
                {
                    try { capinasquimcq.VazaoSupMax = Convert.ToInt32(VazaoSupMax.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + VazaoSupMax.Placeholder + "'", "Ok"); }
                }
                if (PressaoMed.Text != null)
                {
                    try { capinasquimcq.PressaoMed = float.Parse(PressaoMed.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + PressaoMed.Placeholder + "'", "Ok"); }
                }
                if (PressaoDesv.Text != null)
                {
                    try { capinasquimcq.PressaoDesv = float.Parse(PressaoDesv.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + PressaoDesv.Placeholder + "'", "Ok"); }
                }
                if (PressaoInfMin.Text != null)
                {
                    try { capinasquimcq.PressaoInfMin = Convert.ToInt32(PressaoInfMin.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + PressaoInfMin.Placeholder + "'", "Ok"); }
                }
                if (PressaoInfMed.Text != null)
                {
                    try { capinasquimcq.PressaoInfMed = Convert.ToInt32(PressaoInfMed.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + PressaoInfMed.Placeholder + "'", "Ok"); }
                }
                if (PressaoSupMed.Text != null)
                {
                    try { capinasquimcq.PressaoSupMed = Convert.ToInt32(PressaoSupMed.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + PressaoSupMed.Placeholder + "'", "Ok"); }
                }
                if (PressaoSupMax.Text != null)
                {
                    try { capinasquimcq.PressaoSupMax = Convert.ToInt32(PressaoSupMax.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + PressaoSupMax.Placeholder + "'", "Ok"); }
                }
                if (InconfCobert.Text != null)
                {
                    try { capinasquimcq.InconfCobert = Convert.ToInt32(InconfCobert.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + InconfCobert.Placeholder + "'", "Ok"); }
                }
                if (InconfDeriva.Text != null)
                {
                    try { capinasquimcq.InconfDeriva = Convert.ToInt32(InconfDeriva.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + InconfDeriva.Placeholder + "'", "Ok"); }
                }
                try { capinasquimcq.Avaliador = Avaliador.Text; }
                catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + Avaliador.Placeholder + "'", "Ok"); }
                try { capinasquimcq.DataAval = DataAval.Date.ToString("dd/MM/yyyy"); }
                catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo 'Data da avaliação'", "Ok"); }
                try { capinasquimcq.Observacoes = Observacoes.Text; }
                catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + Observacoes.Placeholder + "'", "Ok"); }
                conn.Insert(capinasquimcq);
                conn.Commit();
                SaveBtn.IsEnabled = false;
                CancelBtn.Text = "Voltar";
            }
            else
            {
                //Atualiza o registro
                conn.BeginTransaction();
                string sql = $"UPDATE CapinasQuimCQ SET";
                if ((Repeticao.Text != "") && (Repeticao.Text != null)) { sql += $" Repeticao = {Repeticao.Text}"; }
                else { sql += $" Repeticao = NULL"; }
                if ((AreaAplic.Text != "") && (AreaAplic.Text != null)) { sql += $", AreaAplic = {float.Parse(AreaAplic.Text).ToString("F2", CultureInfo.InvariantCulture)}"; }
                //if ((AreaAplic.Text != "") && (AreaAplic.Text != null)) { sql += $", AreaAplic = {Math.Round(float.Parse(AreaAplic.Text) * 100)}*1.0/100"; }
                else { sql += $", AreaAplic = NULL"; }
                if ((VolBico1.Text != "") && (VolBico1.Text != null)) { sql += $", VolBico1 = {Math.Round(float.Parse(VolBico1.Text) * 100)}*1.0/100"; }
                else { sql += $", VolBico1 = NULL"; }
                if ((VolBico2.Text != "") && (VolBico2.Text != null)) { sql += $", VolBico2 = {Math.Round(float.Parse(VolBico2.Text) * 100)}*1.0/100"; }
                else { sql += $", VolBico2 = NULL"; }
                if ((VolBico3.Text != "") && (VolBico3.Text != null)) { sql += $", VolBico3 = {Math.Round(float.Parse(VolBico3.Text) * 100)}*1.0/100"; }
                else { sql += $", VolBico3 = NULL"; }
                if ((VolBico4.Text != "") && (VolBico4.Text != null)) { sql += $", VolBico4 = {Math.Round(float.Parse(VolBico4.Text) * 100)}*1.0/100"; }
                else { sql += $", VolBico4 = NULL"; }
                if ((VolBico5.Text != "") && (VolBico5.Text != null)) { sql += $", VolBico5 = {Math.Round(float.Parse(VolBico5.Text) * 100)}*1.0/100"; }
                else { sql += $", VolBico5 = NULL"; }
                if ((VolBico6.Text != "") && (VolBico6.Text != null)) { sql += $", VolBico6 = {Math.Round(float.Parse(VolBico6.Text) * 100)}*1.0/100"; }
                else { sql += $", VolBico6 = NULL"; }
                if ((VolBico7.Text != "") && (VolBico7.Text != null)) { sql += $", VolBico7 = {Math.Round(float.Parse(VolBico7.Text) * 100)}*1.0/100"; }
                else { sql += $", VolBico7 = NULL"; }
                if ((VolBico8.Text != "") && (VolBico8.Text != null)) { sql += $", VolBico8 = {Math.Round(float.Parse(VolBico8.Text) * 100)}*1.0/100"; }
                else { sql += $", VolBico8 = NULL"; }
                if ((VolBico9.Text != "") && (VolBico9.Text != null)) { sql += $", VolBico9 = {Math.Round(float.Parse(VolBico9.Text) * 100)}*1.0/100"; }
                else { sql += $", VolBico9 = NULL"; }
                if ((VolBico10.Text != "") && (VolBico10.Text != null)) { sql += $", VolBico10 = {Math.Round(float.Parse(VolBico10.Text) * 100)}*1.0/100"; }
                else { sql += $", VolBico10 = NULL"; }
                if ((VolTotal.Text != "") && (VolTotal.Text != null)) { sql += $", VolTotal = {Math.Round(float.Parse(VolTotal.Text) * 100)}*1.0/100"; }
                else { sql += $", VolTotal = NULL"; }
                if ((NPtsAval.Text != "") && (NPtsAval.Text != null)) { sql += $", NPtsAval = {NPtsAval.Text}"; }
                else { sql += $", NPtsAval = NULL"; }
                if ((VazaoMed.Text != "") && (VazaoMed.Text != null)) { sql += $", VazaoMed = {Math.Round(float.Parse(VazaoMed.Text) * 100)}*1.0/100"; }
                else { sql += $", VazaoMed = NULL"; }
                if ((VazaoDesv.Text != "") && (VazaoDesv.Text != null)) { sql += $", VazaoDesv = {Math.Round(float.Parse(VazaoDesv.Text) * 100)}*1.0/100"; }
                else { sql += $", VazaoDesv = NULL"; }
                if ((VazaoInfMin.Text != "") && (VazaoInfMin.Text != null)) { sql += $", VazaoInfMin = {VazaoInfMin.Text}"; }
                else { sql += $", VazaoInfMin = NULL"; }
                if ((VazaoInfMed.Text != "") && (VazaoInfMed.Text != null)) { sql += $", VazaoInfMed = {VazaoInfMed.Text}"; }
                else { sql += $", VazaoInfMed = NULL"; }
                if ((VazaoSupMed.Text != "") && (VazaoSupMed.Text != null)) { sql += $", VazaoSupMed = {VazaoSupMed.Text}"; }
                else { sql += $", VazaoSupMed = NULL"; }
                if ((VazaoSupMax.Text != "") && (VazaoSupMax.Text != null)) { sql += $", VazaoSupMax = {VazaoSupMax.Text}"; }
                else { sql += $", VazaoSupMax = NULL"; }
                if ((PressaoMed.Text != "") && (PressaoMed.Text != null)) { sql += $", PressaoMed = {Math.Round(float.Parse(PressaoMed.Text) * 100)}*1.0/100"; }
                else { sql += $", PressaoMed = NULL"; }
                if ((PressaoDesv.Text != "") && (PressaoDesv.Text != null)) { sql += $", PressaoDesv = {Math.Round(float.Parse(PressaoDesv.Text) * 100)}*1.0/100"; }
                else { sql += $", PressaoDesv = NULL"; }
                if ((PressaoInfMin.Text != "") && (PressaoInfMin.Text != null)) { sql += $", PressaoInfMin = {PressaoInfMin.Text}"; }
                else { sql += $", PressaoInfMin = NULL"; }
                if ((PressaoInfMed.Text != "") && (PressaoInfMed.Text != null)) { sql += $", PressaoInfMed = {PressaoInfMed.Text}"; }
                else { sql += $", PressaoInfMed = NULL"; }
                if ((PressaoSupMed.Text != "") && (PressaoSupMed.Text != null)) { sql += $", PressaoSupMed = {PressaoSupMed.Text}"; }
                else { sql += $", PressaoSupMed = NULL"; }
                if ((PressaoSupMax.Text != "") && (PressaoSupMax.Text != null)) { sql += $", PressaoSupMax = {PressaoSupMax.Text}"; }
                else { sql += $", PressaoSupMax = NULL"; }
                if ((InconfCobert.Text != "") && (InconfCobert.Text != null)) { sql += $", InconfCobert = {InconfCobert.Text}"; }
                else { sql += $", InconfCobert = NULL"; }
                if ((InconfDeriva.Text != "") && (InconfDeriva.Text != null)) { sql += $", InconfDeriva = {InconfDeriva.Text}"; }
                else { sql += $", InconfDeriva = NULL"; }
                sql += $", Avaliador = '{Avaliador.Text}'" +
                    $", DataAval = '{DataAval.Date:dd/MM/yyyy}'" +
                    $", Observacoes = '{Observacoes.Text}'" +
                    $" WHERE PKCapinasQuimCQ = {PKCapinasQuimCQ.Text}";
                try
                {
                    conn.Execute(sql);
                    conn.Commit();
                    CancelBtn.Text = "Voltar";
                }
                catch
                {
                    conn.Rollback();
                    DisplayAlert("Message", "Não foi possível atualizar. Verifique o valor digitado.", "Ok");
                }
            }
        }

        async void CancelarEdicao(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        pgAddTalhoesCR TextCh = new pgAddTalhoesCR(null);
        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            TextCh.TextChanged(sender, e);
        }

    }
}