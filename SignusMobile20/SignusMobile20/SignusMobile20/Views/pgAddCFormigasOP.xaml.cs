using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SignusMobile20.Models;
using SQLite;


namespace SignusMobile20
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pgAddCFormigasOP : ContentPage
    {
        CFormigasOP detalheCFormigasOP;
        private SQLiteConnection conn;
        public CFormigasOP cformigasop;

        public pgAddCFormigasOP(CFormigasOP details, string pktalhoescr)
        {
            InitializeComponent();
            //Retornar estas duas linhas abaixo caso dê algo errado
            conn = DependencyService.Get<ISQLite>().GetConnection();
            conn.CreateTable<CFormigasOP>();

            PKTalhoesCR.Text = pktalhoescr;

            if (details != null)
            {
                detalheCFormigasOP = details;
                PopulateDetails(detalheCFormigasOP);
            }

        }

        private void PopulateDetails(CFormigasOP details)
        {
            try { PKTalhoesCR.Text = details.PKTalhoesCR.ToString(); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor 'Código do talhão'", "Ok"); }
            try { PKCFormigasOP.Text = details.PKCFormigasOP.ToString(); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor 'Código do controle de formigas'", "Ok"); }
            try { IdentifOperacao.Text = details.IdentifOperacao; }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + IdentifOperacao.Placeholder + "'", "Ok"); }
            try { ProcedOperac.Text = details.ProcedOperac.ToString(); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + ProcedOperac.Placeholder + "'", "Ok"); }
            try { TipoContr.Text = details.TipoContr; }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + TipoContr.Placeholder + "'", "Ok"); }
            try { Produto.Text = details.Produto; }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Produto.Placeholder + "'", "Ok"); }
            try { Dose.Text = details.Dose.ToString("#.00"); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Dose.Placeholder + "'", "Ok"); }
            try { UnidDose.Text = details.UnidDose; }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + UnidDose.Placeholder + "'", "Ok"); }
            try { MetodoContr.Text = details.MetodoContr; }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + MetodoContr.Placeholder + "'", "Ok"); }
            try { Equipamento.Text = details.Equipamento; }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Equipamento.Placeholder + "'", "Ok"); }
            if (details.DataContr != null)
            {
                //DisplayAlert("Message", "details.DataInicio", "Ok");
                try { DataContr.Date = DateTime.ParseExact(details.DataContr, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture); }
                catch { DisplayAlert("Message", "Não foi possível ler o valor 'Data da operação'", "Ok"); }
            }
            try { NumHoras.Text = details.NumHoras.ToString("#.00"); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + NumHoras.Placeholder + "'", "Ok"); }
            try { Rendimento.Text = details.Rendimento.ToString("#.00"); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Rendimento.Placeholder + "'", "Ok"); }
            try { EmpresaResp.Text = details.EmpresaResp; }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + EmpresaResp.Placeholder + "'", "Ok"); }
            try { Observacoes.Text = details.Observacoes; }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Observacoes.Placeholder + "'", "Ok"); }
            //Device.BeginInvokeOnMainThread(async () => { await DisplayAlert("Alert", "Sem conexão.", "Ok"); });
            SaveBtn.Text = "Atualizar";
            this.Title = "Editar contr. de formigas";

        }
        private void SaveCFormigasOP(object sender, EventArgs e)
        {
            if (SaveBtn.Text == "Salvar")
            {
                conn.BeginTransaction();
                CFormigasOP cformigasop = new CFormigasOP();
                cformigasop = new CFormigasOP();
                if (PKTalhoesCR.Text != null)
                {
                    try { cformigasop.PKTalhoesCR = Convert.ToInt32(PKTalhoesCR.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo 'PKTalhoesCR'", "Ok"); }
                }
                if (IdentifOperacao.Text != null)
                {
                    try { cformigasop.IdentifOperacao = IdentifOperacao.Text; }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + IdentifOperacao.Placeholder + "'", "Ok"); }
                }
                if (ProcedOperac.Text != null)
                {
                    try { cformigasop.ProcedOperac = Convert.ToInt32(ProcedOperac.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo 'Código do P.O.'", "Ok"); }
                }
                try { cformigasop.TipoContr = TipoContr.Text; }
                catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo 'Tipo de controle'", "Ok"); }
                try { cformigasop.Produto = Produto.Text; }
                catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo 'Produto utilizado'", "Ok"); }
                if (Dose.Text != null)
                {
                    try { cformigasop.Dose = float.Parse(Dose.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo 'Dose'", "Ok"); }
                }
                try { cformigasop.UnidDose = UnidDose.Text; }
                catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo 'Unidade da dose'", "Ok"); }
                try { cformigasop.MetodoContr = MetodoContr.Text; }
                catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo 'Método de controle'", "Ok"); }
                try { cformigasop.Equipamento = Equipamento.Text; }
                catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo 'Equipamento'", "Ok"); }
                try { cformigasop.DataContr = DataContr.Date.ToString("dd/MM/yyyy"); }
                catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo 'Data da operação'", "Ok"); }
                if (NumHoras.Text != null)
                {
                    try { cformigasop.NumHoras = float.Parse(NumHoras.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo 'Núm. de horas'", "Ok"); }
                }
                if (Rendimento.Text != null)
                {
                    try { cformigasop.Rendimento = float.Parse(Rendimento.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo 'Rendimento'", "Ok"); }
                }
                try { cformigasop.EmpresaResp = EmpresaResp.Text; }
                catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo 'Empresa resp.'", "Ok"); }
                try { cformigasop.Observacoes = Observacoes.Text; }
                catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo 'Observações'", "Ok"); }
                conn.Insert(cformigasop);
                conn.Commit();
                SaveBtn.IsEnabled = false;
                CancelBtn.Text = "Voltar";
            }
            else
            {
                conn.BeginTransaction();
                string sql = $"UPDATE CFormigasOP SET" +
                    $" IdentifOperacao = '{IdentifOperacao.Text}'";
                if ((ProcedOperac.Text != "") && (ProcedOperac.Text != null)) { sql += $", ProcedOperac = {ProcedOperac.Text}"; }
                else { sql += $", ProcedOperac = NULL"; }
                sql += $", TipoContr = '{TipoContr.Text}'" +
                    $", Produto = '{Produto.Text}'";
                if ((Dose.Text != "") && (Dose.Text != null)) { sql += $", Dose = {Math.Round(float.Parse(Dose.Text) * 100)}*1.0/100"; }
                else { sql += $", Dose = NULL"; }
                sql += $", UnidDose = '{UnidDose.Text}'" +
                    $", MetodoContr = '{MetodoContr.Text}'" +
                    $", Equipamento = '{Equipamento.Text}'" +
                    $", DataInicio = '{DataContr.Date:dd/MM/yyyy}'";
                if ((NumHoras.Text != "") && (NumHoras.Text != null)) { sql += $", NumHoras = {Math.Round(float.Parse(NumHoras.Text) * 100)}*1.0/100"; }
                else { sql += $", NumHoras = NULL"; }
                if ((Rendimento.Text != "") && (Rendimento.Text != null)) { sql += $", Rendimento = {Math.Round(float.Parse(Rendimento.Text) * 100)}*1.0/100"; }
                else { sql += $", Rendimento = NULL"; }
                sql += $", EmpresaResp = '{EmpresaResp.Text}'" +
                    $", Observacoes = '{Observacoes.Text}'" +
                    $" WHERE PKCFormigasOP = {PKCFormigasOP.Text}";
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