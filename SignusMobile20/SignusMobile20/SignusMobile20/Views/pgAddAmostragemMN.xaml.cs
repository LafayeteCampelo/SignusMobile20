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
    public partial class pgAddAmostragemMN : ContentPage
    {
        AmostragemMN detalheAmostragemMN;
        private SQLiteConnection conn;
        public AmostragemMN amostragemmn;
        public string pktalhoescr;
        public string idfazenda;
        public string idtalhao;

        public pgAddAmostragemMN(AmostragemMN details, string pktalh, string idfaz, string idtalh)
        {
            InitializeComponent();
            //Retornar estas duas linhas abaixo caso dê algo errado
            conn = DependencyService.Get<ISQLite>().GetConnection();
            conn.CreateTable<AmostragemMN>();

            pktalhoescr = pktalh;
            idfazenda = idfaz;
            idtalhao = idtalh;

            IdFazenda.Text = "Fazenda/Proj.: " + idfaz;
            IdTalhao.Text = "Talhão: " + idtalh;


            if (details != null)
            {
                detalheAmostragemMN = details;
                PopulateDetails(detalheAmostragemMN);
            }

        }

        private void PopulateDetails(AmostragemMN details)
        {
            //try { PKTalhoesCR.Text = details.PKTalhoesCR.ToString("#"); }
            //catch { DisplayAlert("Message", "Não foi possível ler o valor 'Código do talhão'", "Ok"); }
            try { PKAmostragemMN.Text = details.PKAmostragemMN.ToString("#"); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor 'Código da amostragem'", "Ok"); }
            try { IdAmostragem.Text = details.IdAmostragem; }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + IdAmostragem.Placeholder + "'", "Ok"); }
            if (details.DataAmost != null)
            {
                try { DataAmost.Date = DateTime.ParseExact(details.DataAmost, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture); }
                catch { DisplayAlert("Message", "Não foi possível ler o valor 'DataAval'", "Ok"); }
            }
            try { Objetivo.Text = details.Objetivo; }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Objetivo.Placeholder + "'", "Ok"); }
            if (details.NParcelas != 0)
            {
                try { NParcelas.Text = details.NParcelas.ToString(); }
                catch { DisplayAlert("Message", "Não foi possível ler o valor '" + NParcelas.Placeholder + "'", "Ok"); }
            }
            if (details.NArvParc != 0)
            {
                try { NArvParc.Text = details.NArvParc.ToString(); }
                catch { DisplayAlert("Message", "Não foi possível ler o valor '" + NArvParc.Placeholder + "'", "Ok"); }
            }
            if (details.NRepTecidosTalh != 0)
            {
                try { NRepTecidosTalh.Text = details.NRepTecidosTalh.ToString(); }
                catch { DisplayAlert("Message", "Não foi possível ler o valor '" + NRepTecidosTalh.Placeholder + "'", "Ok"); }
            }
            if (details.NRepTecidosParc != 0)
            {
                try { NRepTecidosParc.Text = details.NRepTecidosParc.ToString(); }
                catch { DisplayAlert("Message", "Não foi possível ler o valor '" + NRepTecidosParc.Placeholder + "'", "Ok"); }
            }

            if (details.Folhas == true) { Folhas.IsChecked = true; } else { Folhas.IsChecked = false; }
            if (details.Galhos == true) { Galhos.IsChecked = true; } else { Galhos.IsChecked = false; }
            if (details.Casca == true) { Casca.IsChecked = true; } else { Casca.IsChecked = false; }
            if (details.Lenho == true) { Lenho.IsChecked = true; } else { Lenho.IsChecked = false; }
            if (details.Raizes == true) { Raizes.IsChecked = true; } else { Raizes.IsChecked = false; }

            if (details.NRepSoloTalh != 0)
            {
                try { NRepSoloTalh.Text = details.NRepSoloTalh.ToString(); }
                catch { DisplayAlert("Message", "Não foi possível ler o valor '" + NRepSoloTalh.Placeholder + "'", "Ok"); }
            }
            if (details.NRepSoloParc != 0)
            {
                try { NRepSoloParc.Text = details.NRepSoloParc.ToString(); }
                catch { DisplayAlert("Message", "Não foi possível ler o valor '" + NRepSoloParc.Placeholder + "'", "Ok"); }
            }
            if (details.NCamadasSolo != 0)
            {
                try { NCamadasSolo.Text = details.NCamadasSolo.ToString(); }
                catch { DisplayAlert("Message", "Não foi possível ler o valor '" + NCamadasSolo.Placeholder + "'", "Ok"); }
            }
            try { Responsavel.Text = details.Responsavel; }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Responsavel.Placeholder + "'", "Ok"); }
            try { Observacoes.Text = details.Observacoes; }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Observacoes.Placeholder + "'", "Ok"); }
            //Device.BeginInvokeOnMainThread(async () => { await DisplayAlert("Alert", "No internet connection", "Ok"); });
            SaveBtn.Text = "Atualizar";
            this.Title = "Editar Amostragem";
        }
        private void SaveAmostragemMN(object sender, EventArgs e)
        {
            if (SaveBtn.Text == "Salvar")
            {
                conn.BeginTransaction();
                //Device.BeginInvokeOnMainThread(async () => { await DisplayAlert("Alert", "No internet connection", "Ok"); });
                //DisplayAlert("Message", "Não foi possível atualizar. Verifique o valor digitado.", "Ok");
                AmostragemMN amostragemmn = new AmostragemMN();
                //amostragemmn = new AmostragemMN();

                try { amostragemmn.PKTalhoesCR = Convert.ToInt32(pktalhoescr); }
                catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo 'PKTalhoesCR'", "Ok"); }

                if (IdAmostragem.Text != null)
                {
                    try { amostragemmn.IdAmostragem = IdAmostragem.Text; }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + IdAmostragem.Placeholder + "'", "Ok"); }
                }
                try { amostragemmn.DataAmost = DataAmost.Date.ToString("dd/MM/yyyy"); }
                catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo 'Data da amostragem'", "Ok"); }

                if (Objetivo.Text != null)
                {
                    try { amostragemmn.Objetivo = Objetivo.Text; }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + Objetivo.Placeholder + "'", "Ok"); }
                }
                if (NParcelas.Text != null)
                {
                    try { amostragemmn.NParcelas = Convert.ToInt32(NParcelas.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + NParcelas.Placeholder + "'", "Ok"); }
                }
                if (NArvParc.Text != null)
                {
                    try { amostragemmn.NArvParc = Convert.ToInt32(NArvParc.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + NArvParc.Placeholder + "'", "Ok"); }
                }
                if (NRepTecidosTalh.Text != null)
                {
                    try { amostragemmn.NRepTecidosTalh = Convert.ToInt32(NRepTecidosTalh.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + NRepTecidosTalh.Placeholder + "'", "Ok"); }
                }
                if (NRepTecidosParc.Text != null)
                {
                    try { amostragemmn.NRepTecidosParc = Convert.ToInt32(NRepTecidosParc.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + NRepTecidosParc.Placeholder + "'", "Ok"); }
                }
                try { if (Folhas.IsChecked) { amostragemmn.Folhas = true; } else { amostragemmn.Folhas = false; } }
                catch { DisplayAlert("Message", "Não foi possível ler o valor para o componente Folhas", "Ok"); }
                try { if (Galhos.IsChecked) { amostragemmn.Galhos = true; } else { amostragemmn.Galhos = false; } }
                catch { DisplayAlert("Message", "Não foi possível ler o valor para o componente Galhos", "Ok"); }
                try { if (Casca.IsChecked) { amostragemmn.Casca = true; } else { amostragemmn.Casca = false; } }
                catch { DisplayAlert("Message", "Não foi possível ler o valor para o componente Casca", "Ok"); }
                try { if (Lenho.IsChecked) { amostragemmn.Lenho = true; } else { amostragemmn.Lenho = false; } }
                catch { DisplayAlert("Message", "Não foi possível ler o valor para o componente Lenho", "Ok"); }
                try { if (Raizes.IsChecked) { amostragemmn.Raizes = true; } else { amostragemmn.Raizes = false; } }
                catch { DisplayAlert("Message", "Não foi possível ler o valor para o componente Raizes", "Ok"); }
                if (NRepSoloTalh.Text != null)
                {
                    try { amostragemmn.NRepSoloTalh = Convert.ToInt32(NRepSoloTalh.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + NRepSoloTalh.Placeholder + "'", "Ok"); }
                }
                if (NRepSoloParc.Text != null)
                {
                    try { amostragemmn.NRepSoloParc = Convert.ToInt32(NRepSoloParc.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + NRepSoloParc.Placeholder + "'", "Ok"); }
                }
                if (NCamadasSolo.Text != null)
                {
                    try { amostragemmn.NCamadasSolo = Convert.ToInt32(NCamadasSolo.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + NCamadasSolo.Placeholder + "'", "Ok"); }
                }
                try { amostragemmn.Responsavel = Responsavel.Text; }
                catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + Responsavel.Placeholder + "'", "Ok"); }
                try { amostragemmn.Observacoes = Observacoes.Text; }
                catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + Observacoes.Placeholder + "'", "Ok"); }

                conn.Insert(amostragemmn);
                conn.Commit();
                SaveBtn.IsEnabled = false;
                CancelBtn.Text = "Voltar";
            }
            else
            {
                //Atualiza o registro
                conn.BeginTransaction();
                string sql = $"UPDATE AmostragemMN SET ";
                sql += $"IdAmostragem = '{IdAmostragem.Text}'";
                sql += $", DataAmost = '{DataAmost.Date:dd/MM/yyyy}'" +
                    $", Objetivo = '{Objetivo.Text}'";
                if ((NParcelas.Text != "") && (NParcelas.Text != null)) { sql += $", NParcelas = {NParcelas.Text}"; }
                else { sql += $", NParcelas = NULL"; }
                if ((NArvParc.Text != "") && (NArvParc.Text != null)) { sql += $", NArvParc = {NArvParc.Text}"; }
                else { sql += $", NArvParc = NULL"; }
                if ((NRepTecidosTalh.Text != "") && (NRepTecidosTalh.Text != null)) { sql += $", NRepTecidosTalh = {NRepTecidosTalh.Text}"; }
                else { sql += $", NRepTecidosTalh = NULL"; }
                if ((NRepTecidosParc.Text != "") && (NRepTecidosParc.Text != null)) { sql += $", NRepTecidosParc = {NRepTecidosParc.Text}"; }
                else { sql += $", NRepTecidosParc = NULL"; }
                if (Folhas.IsChecked) { sql += $", Folhas = 1"; }
                else { sql += $", Folhas = 0"; }
                if (Galhos.IsChecked) { sql += $", Galhos = 1"; }
                else { sql += $", Galhos = 0"; }
                if (Casca.IsChecked) { sql += $", Casca = 1"; }
                else { sql += $", Casca = 0"; }
                if (Lenho.IsChecked) { sql += $", Lenho = 1"; }
                else { sql += $", Lenho = 0"; }
                if (Raizes.IsChecked) { sql += $", Raizes = 1"; }
                else { sql += $", Raizes = 0"; }
                if ((NRepSoloTalh.Text != "") && (NRepSoloTalh.Text != null)) { sql += $", NRepSoloTalh = {NRepSoloTalh.Text}"; }
                else { sql += $", NRepSoloTalh = NULL"; }
                if ((NRepSoloParc.Text != "") && (NRepSoloParc.Text != null)) { sql += $", NRepSoloParc = {NRepSoloParc.Text}"; }
                else { sql += $", NRepSoloParc = NULL"; }
                if ((NCamadasSolo.Text != "") && (NCamadasSolo.Text != null)) { sql += $", NCamadasSolo = {NCamadasSolo.Text}"; }
                else { sql += $", NCamadasSolo = NULL"; }
                sql += $", Responsavel = '{Responsavel.Text}'" +
                    $", Observacoes = '{Observacoes.Text}'" +
                    $" WHERE PKAmostragemMN = {PKAmostragemMN.Text}";
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

        private void NRepTecidosParc_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if ((Convert.ToInt32(NRepTecidosParc.Text) > 0) && (Convert.ToInt32(NParcelas.Text) > 0))
                {
                    int NAmTecTalh = Convert.ToInt32(NRepTecidosParc.Text) * Convert.ToInt32(NParcelas.Text);
                    NRepTecidosTalh.Text = NAmTecTalh.ToString();
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Erro: ", ex.Message, "Ok");
            }
        }

        private void NRepSoloParc_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if ((Convert.ToInt32(NRepSoloParc.Text) > 0) && (Convert.ToInt32(NParcelas.Text) > 0))
                {
                    int NAmSoloTalh = Convert.ToInt32(NRepSoloParc.Text) * Convert.ToInt32(NParcelas.Text);
                    NRepSoloTalh.Text = NAmSoloTalh.ToString();
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Erro: ", ex.Message, "Ok");
            }
        }

    }
}