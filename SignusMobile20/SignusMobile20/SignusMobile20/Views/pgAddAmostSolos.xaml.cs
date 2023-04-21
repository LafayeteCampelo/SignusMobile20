using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.ExternalMaps;
using Plugin.Geolocator;
using SignusMobile20.Models;
using SQLite;


namespace SignusMobile20
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pgAddAmostSolos : ContentPage
    {
        AmostSolos detalheAmostSolos;
        private SQLiteConnection conn;
        public AmostSolos AmostSolos;
        public string pkamostragem;
        public string idfazenda;
        public string idtalhao;
        public string idamostragem;
        public string idparcela;
        private double latitudeAmS = 0;
        private double longitudeAmS = 0;
        private double altitudeAmS = 0;
        private double precisaohAmS = 0;
        private double precisaovAmS = 0;


        public pgAddAmostSolos(AmostSolos details, string pkamost, string idfaz, string idtalh, string idamost, string idparc)
        {
            InitializeComponent();
            //Retornar estas duas linhas abaixo caso dê algo errado
            conn = DependencyService.Get<ISQLite>().GetConnection();
            //conn.DropTable<AmostSolos>();
            conn.CreateTable<AmostSolos>();

            pkamostragem = pkamost;
            idfazenda = idfaz;
            idtalhao = idtalh;
            idamostragem = idamost;
            idparcela = idparc;

            IdFazenda.Text = "Fazenda/Proj.: " + idfaz;
            IdTalhao.Text = "Talhão: " + idtalh;
            IdAmostragemMN.Text = "Amostragem: " + idamost;
            if (idparcela != "") { IdParcela.Text = "Parcela: " + idparcela; }
            else
            { IdParcela.Text = "Amostra(s) composta(s)"; }

            //PKAmostragemMN.Text = pkamostragemmn;

            if (details != null)
            {
                detalheAmostSolos = details;
                PopulateDetails(detalheAmostSolos);
            }
            else
            {
                preencheIdAmostra();
            }
        }

        //pgAddParcelasMedArv Coord = new pgAddParcelasMedArv(null, null, null, null, null);
        private void PopulateDetails(AmostSolos details)
        {
            //            try { PKAmostragemMN.Text = details.PKAmostragemMN.ToString("#"); }
            //            catch { DisplayAlert("Message", "Não foi possível ler o valor 'Código da amostragem'", "Ok"); }
            try { PKAmostSolos.Text = details.PKAmostSolos.ToString("#"); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor 'Código da amostra de solo'", "Ok"); }
            try { IdAmostra.Text = details.IdAmostra; }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + IdAmostra.Placeholder + "'", "Ok"); }
            try { Repeticao.Text = details.Repeticao.ToString("#"); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Repeticao.Placeholder + "'", "Ok"); }
            if (details.DataAmostra != null)
            {
                try { DataAmostra.Date = DateTime.ParseExact(details.DataAmostra, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture); }
                catch { DisplayAlert("Message", "Não foi possível ler o valor 'Data da amostragem'", "Ok"); }
            }
            try { ProfIni.Text = details.ProfIni.ToString("#.00"); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + ProfIni.Placeholder + "'", "Ok"); }
            try { ProfFin.Text = details.ProfFin.ToString("#"); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + ProfFin.Placeholder + "'", "Ok"); }
            try { Objetivo.Text = details.Objetivo; }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Objetivo.Placeholder + "'", "Ok"); }
            try { NAmSimples.Text = details.NAmSimples.ToString("#"); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + NAmSimples.Placeholder + "'", "Ok"); }
            if (details.Latit != 0)
            {
                latitudeAmS = details.Latit;
                try { Latit.Text = details.Latit.ToString("#.0000000"); }
                catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Latit.Placeholder + "'", "Ok"); }
            }
            if (details.Longit != 0)
            {
                longitudeAmS = details.Longit;
                try { Longit.Text = details.Longit.ToString("#.0000000"); }
                catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Longit.Placeholder + "'", "Ok"); }
            }
            if (details.Altit != 0)
            {
                altitudeAmS = details.Altit;
                try { Altit.Text = details.Altit.ToString("#.00"); }
                catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Altit.Placeholder + "'", "Ok"); }
            }
            if (details.PrecisH != 0)
            {
                try { PrecisH.Text = details.PrecisH.ToString("#.00"); }
                catch { DisplayAlert("Message", "Não foi possível ler o valor '" + PrecisH.Placeholder + "'", "Ok"); }
            }
            if (details.PrecisV != 0)
            {
                try { PrecisV.Text = details.PrecisV.ToString("#.00"); }
                catch { DisplayAlert("Message", "Não foi possível ler o valor '" + PrecisV.Placeholder + "'", "Ok"); }
            }
            try { Responsavel.Text = details.Responsavel; }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Responsavel.Placeholder + "'", "Ok"); }
            try { Observacoes.Text = details.Observacoes; }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Observacoes.Placeholder + "'", "Ok"); }
            //Device.BeginInvokeOnMainThread(async () => { await DisplayAlert("Alert", "No internet connection", "Ok"); });
            SaveBtn.Text = "Atualizar";
            this.Title = "Editar am. solos";
        }

        private void preencheIdAmostra()
        {
            try
            {
                IdAmostra.Text = $"{idfazenda}/ {idtalhao}/ {idamostragem}";
                if (idparcela != "") { IdAmostra.Text += $"/ {idparcela}"; };
            }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + IdAmostra.Placeholder + "'", "Ok"); }
            //Device.BeginInvokeOnMainThread(async () => { await DisplayAlert("Alert", "No internet connection", "Ok"); });
        }

        private void SaveAmostSolos(object sender, EventArgs e)
        {
            if (SaveBtn.Text == "Salvar")
            {
                conn.BeginTransaction();
                //Device.BeginInvokeOnMainThread(async () => { await DisplayAlert("Alert", "No internet connection", "Ok"); });
                //DisplayAlert("Message", "Não foi possível atualizar. Verifique o valor digitado.", "Ok");
                AmostSolos amostsolos = new AmostSolos();
                //AmostSolos = new AmostSolos();

                try { amostsolos.PKAmostragemMN = Convert.ToInt32(pkamostragem); }
                catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo 'PKAmostragemMN'", "Ok"); }
                /*
                                if (PKAmostSolos.Text != null)
                                {
                                    try { AmostSolos.PKAmostSolos = Convert.ToInt32(PKAmostSolos.Text); }
                                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo 'PKAmostSolos'", "Ok"); }
                                }
                */
                if (IdAmostra.Text != null)
                {
                    try { amostsolos.IdAmostra = IdAmostra.Text; }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + IdAmostra.Placeholder + "'", "Ok"); }
                }
                if (Repeticao.Text != null)
                {
                    try { amostsolos.Repeticao = Convert.ToInt32(Repeticao.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + Repeticao.Placeholder + "'", "Ok"); }
                }
                try { amostsolos.DataAmostra = DataAmostra.Date.ToString("dd/MM/yyyy"); }
                catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo 'Data da amostragem'", "Ok"); }
                if (ProfIni.Text != null)
                {
                    try { amostsolos.ProfIni = float.Parse(ProfIni.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + ProfIni.Placeholder + "'", "Ok"); }
                }
                if (ProfFin.Text != null)
                {
                    try { amostsolos.ProfFin = float.Parse(ProfFin.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + ProfFin.Placeholder + "'", "Ok"); }
                }
                if (Objetivo.Text != null)
                {
                    try { amostsolos.Objetivo = Objetivo.Text; }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + Objetivo.Placeholder + "'", "Ok"); }
                }
                if (NAmSimples.Text != null)
                {
                    try { amostsolos.NAmSimples = Convert.ToInt32(NAmSimples.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + NAmSimples.Placeholder + "'", "Ok"); }
                }
                if (Latit.Text != null)
                {
                    try { amostsolos.Latit = double.Parse(Latit.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + Latit.Placeholder + "'", "Ok"); }
                }
                if (Longit.Text != null)
                {
                    try { amostsolos.Longit = double.Parse(Longit.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + Longit.Placeholder + "'", "Ok"); }
                }
                if (Altit.Text != null)
                {
                    try { amostsolos.Altit = double.Parse(Altit.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + Altit.Placeholder + "'", "Ok"); }
                }
                if (PrecisH.Text != null)
                {
                    try { amostsolos.PrecisH = double.Parse(PrecisH.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + PrecisH.Placeholder + "'", "Ok"); }
                }
                if (PrecisV.Text != null)
                {
                    try { amostsolos.PrecisV = double.Parse(PrecisV.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + PrecisV.Placeholder + "'", "Ok"); }
                }
                if (Responsavel.Text != null)
                {
                    try { amostsolos.Responsavel = Responsavel.Text; }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + Responsavel.Placeholder + "'", "Ok"); }
                }
                try { amostsolos.Observacoes = Observacoes.Text; }
                catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + Observacoes.Placeholder + "'", "Ok"); }
                conn.Insert(amostsolos);
                conn.Commit();
                SaveBtn.IsEnabled = false;
                CancelBtn.Text = "Voltar";
            }
            else
            {
                //Atualiza o registro
                conn.BeginTransaction();
                string sql = $"UPDATE AmostSolos SET";
                if ((IdAmostra.Text != "") && (IdAmostra.Text != null)) { sql += $" IdAmostra = '{IdAmostra.Text}'"; }
                else { sql += $" IdAmostra = NULL"; }
                if ((Repeticao.Text != "") && (Repeticao.Text != null)) { sql += $", Repeticao = {Convert.ToInt32(Repeticao.Text)}"; }
                else { sql += $", Largura = NULL"; }
                sql += $", DataAmostra = '{DataAmostra.Date:dd/MM/yyyy}'";
                if ((ProfIni.Text != "") && (ProfIni.Text != null)) { sql += $", ProfIni = {Math.Round(float.Parse(ProfIni.Text) * 100)}*1.0/100"; }
                else { sql += $", ProfIni = NULL"; }
                if ((ProfFin.Text != "") && (ProfFin.Text != null)) { sql += $", ProfFin = {Math.Round(float.Parse(ProfFin.Text) * 100)}*1.0/100"; }
                else { sql += $", ProfFin = NULL"; }
                if ((Objetivo.Text != "") && (Objetivo.Text != null)) { sql += $", Objetivo = '{Objetivo.Text}'"; }
                else { sql += $", Objetivo = NULL"; }
                if ((NAmSimples.Text != "") && (NAmSimples.Text != null)) { sql += $", NAmSimples = {Convert.ToInt32(NAmSimples.Text)}"; }
                else { sql += $", NAmSimples = NULL"; }
                if ((Latit.Text != "") && (Latit.Text != null)) { sql += $", Latit = {Math.Round(double.Parse(Latit.Text) * 10000000)}*1.0/10000000"; }
                else { sql += $", Latit = NULL"; }
                if ((Longit.Text != "") && (Longit.Text != null)) { sql += $", Longit = {Math.Round(double.Parse(Longit.Text) * 10000000)}*1.0/10000000"; }
                else { sql += $", Longit = NULL"; }
                if ((Altit.Text != "") && (Altit.Text != null)) { sql += $", Altit = {Math.Round(double.Parse(Altit.Text) * 100)}*1.0/100"; }
                else { sql += $", Altit = NULL"; }
                if ((PrecisH.Text != "") && (PrecisH.Text != null)) { sql += $", PrecisH = {Math.Round(double.Parse(PrecisH.Text) * 100)}*1.0/100"; }
                else { sql += $", PrecisH = NULL"; }
                if ((PrecisV.Text != "") && (PrecisV.Text != null)) { sql += $", PrecisV = {Math.Round(double.Parse(PrecisV.Text) * 100)}*1.0/100"; }
                else { sql += $", PrecisV = NULL"; }
                if ((Responsavel.Text != "") && (Responsavel.Text != null)) { sql += $", Responsavel = '{Responsavel.Text}'"; }
                else { sql += $", Responsavel = NULL"; }
                sql += $", Observacoes = '{Observacoes.Text}'" +
                    $" WHERE PKAmostSolos = {PKAmostSolos.Text}";
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

        //pgAddParcelasMedArv BtnGeoloc = new pgAddParcelasMedArv(null, null, null, null, null);
        private async void btnGeolocalizacaoClick(object sender, EventArgs e)
        {
            //BtnGeoloc.btnGeolocalizacaoClick(sender, e);
            //lblGeolocalizacao.Text = "Obtendo a geolocalização...\n";
            try
            {
                //Este código utiliza o plugin Xam.Plugin.Geolocator
                Plugin.Geolocator.Abstractions.IGeolocator locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 30;

                //var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(30));

                //lblGeolocalizacao.Text += "Status: " + position.Timestamp + "\n";
                //lblGeolocalizacao.Text += "Latitude: " + position.Latitude + "\n";
                //lblGeolocalizacao.Text += "Longitude: " + position.Longitude;

                latitudeAmS = position.Latitude;
                longitudeAmS = position.Longitude;
                altitudeAmS = position.Altitude;
                precisaohAmS = position.Accuracy;
                precisaovAmS = position.AltitudeAccuracy;

                bool result = await DisplayAlert("Localização: ", $"\nLatitude (GD) = {latitudeAmS:0.0000000}; " +
                    $"\nLongitude (GD) = {longitudeAmS:0.0000000}; \nAltitude (m) = {altitudeAmS:0.00}; " +
                    $"\nPrecisão Hor. (m) = {precisaohAmS:0.00}; \nPrecisão Vert. (m) = {precisaovAmS:0.00}", "Ok", "Cancelar");

                if (result)
                {
                    if (latitudeAmS != 0)
                    {
                        //try { UTMN.Text = $"{latitude}"; }
                        try { Latit.Text = latitudeAmS.ToString("#.0000000"); }
                        catch { await DisplayAlert("Message", "Não foi possível ler o valor '" + Latit.Placeholder + "'", "Ok"); }
                    }

                    if (longitudeAmS != 0)
                    {
                        //try { UTME.Text = $"{longitude}"; }
                        try { Longit.Text = longitudeAmS.ToString("#.0000000"); }
                        catch { await DisplayAlert("Message", "Não foi possível ler o valor '" + Longit.Placeholder + "'", "Ok"); }
                    }
                    if (altitudeAmS != 0)
                    {
                        try { Altit.Text = altitudeAmS.ToString("#.00"); }
                        catch { await DisplayAlert("Message", "Não foi possível ler o valor '" + Altit.Placeholder + "'", "Ok"); }
                    }
                    if (precisaohAmS != 0)
                    {
                        try { PrecisH.Text = precisaohAmS.ToString("#.00"); }
                        catch { await DisplayAlert("Message", "Não foi possível ler o valor '" + PrecisH.Placeholder + "'", "Ok"); }
                    }
                    if (precisaovAmS != 0)
                    {
                        try { PrecisV.Text = precisaovAmS.ToString("#.00"); }
                        catch { await DisplayAlert("Message", "Não foi possível ler o valor '" + PrecisV.Placeholder + "'", "Ok"); }
                    }
                }
                else
                {
                    //Cancela a captura de coordenadas
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro: ", $"{ex.Message}.\n\nCaso o erro esteja relacionado com a " +
                    $"não autorização do uso do geolocalizador, siga as instruções a seguir:" +
                    $"\n\nVá em Configurar\nApps e Notificações" +
                    $"\nAvançado\nPermissões do App\nLocalização, \nem seguida, role até localizar o SIGNUS " +
                    $"e acione o comando para ativá-lo.", "Ok");
            }

        }

        //pgAddParcelasMedArv BtnMostrar = new pgAddParcelasMedArv(null, null, null, null, null);
        private void btnMostrarNoMapaClick(object sender, EventArgs e)
        {
            //BtnMostrar.btnMostrarNoMapaClick(sender, e);
            //Este codigo utiliza o plugin Xam.Plugin.ExternalMaps
            try
            {
                CrossExternalMaps.Current.NavigateTo("Am. solos", latitudeAmS, longitudeAmS);
            }
            catch (Exception ex)
            {
                DisplayAlert("Erro: ", ex.Message, "Ok");
            }

        }

        pgAddParcelasMedArv TextCh1 = new pgAddParcelasMedArv(null, null, null, null, null);
        private void TextChanged1(object sender, TextChangedEventArgs e)
        {
            TextCh1.TextChanged1(sender, e);
        }
    }
}