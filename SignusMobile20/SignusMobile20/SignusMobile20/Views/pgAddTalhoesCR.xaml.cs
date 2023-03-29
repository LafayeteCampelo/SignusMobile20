using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignusMobile20.Models;
using SQLite;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SignusMobile20
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class pgAddTalhoesCR : ContentPage
	{
        TalhoesCR detalheTalhoesCR;
        private SQLiteConnection conn;
        public TalhoesCR talhoescr;
        public pgAddTalhoesCR (TalhoesCR details)
		{
			InitializeComponent ();
            //Retornar estas duas linhas abaixo caso dê algo errado
            conn = DependencyService.Get<ISQLite>().GetConnection();
            conn.CreateTable<TalhoesCR>();
            if (details != null)
            {
                detalheTalhoesCR = details;
                PopulateDetails(detalheTalhoesCR);
            }

        }

        private void PopulateDetails(TalhoesCR details)
        {
            try { PKTalhoesCR.Text = details.PKTalhoesCR.ToString(); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor 'Código do talhão'", "Ok"); }
            try { Empresa.Text = details.Empresa; }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Empresa.Placeholder + "'", "Ok"); }
            try { Unidade.Text = details.Unidade; }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Unidade.Placeholder + "'", "Ok"); }
            try { Fazenda.Text = details.Fazenda; }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Fazenda.Placeholder + "'", "Ok"); }
            try { Talhao.Text = details.Talhao; }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Talhao.Placeholder + "'", "Ok"); }
            try { Subtalhao.Text = details.Subtalhao; }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Subtalhao.Placeholder + "'", "Ok"); }
            if (details.Ciclo != 0)
            {
                try { Ciclo.Text = details.Ciclo.ToString(); }
                catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Ciclo.Placeholder + "'", "Ok"); }
            }
            if (details.Rotacao != 0)
            {
                try { Rotacao.Text = details.Rotacao.ToString(); }
                catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Rotacao.Placeholder + "'", "Ok"); }
            }
            if (details.EspEL != 0)
            {
                try { EspEL.Text = details.EspEL.ToString("#.00"); }
                catch { DisplayAlert("Message", "Não foi possível ler o valor '" + EspEL.Placeholder + "'", "Ok"); }
            }
            if (details.EspEP != 0)
            {
                try { EspEP.Text = (details.EspEP).ToString("#.00"); }
                catch { DisplayAlert("Message", "Não foi possível ler o valor '" + EspEP.Placeholder + "'", "Ok"); }
            }
            if (details.DataIniRot != null)
            {
                //DisplayAlert("Message", "details.DataInicio", "Ok");
                try { DataIniRot.Date = DateTime.ParseExact(details.DataIniRot, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture); }
                catch { DisplayAlert("Message", "Não foi possível ler o valor 'Data de início da rotação'", "Ok"); }
            }
            if (details.AreaTalh != 0)
            {
                try { AreaTalh.Text = details.AreaTalh.ToString("#.00"); }
                catch { DisplayAlert("Message", "Não foi possível ler o valor '" + AreaTalh.Placeholder + "'", "Ok"); }
            }
            if (details.AreaSubtalh != 0)
            {
                try { AreaSubtalh.Text = details.AreaSubtalh.ToString("#.00"); }
                catch { DisplayAlert("Message", "Não foi possível ler o valor '" + AreaSubtalh.Placeholder + "'", "Ok"); }
            }
            try { Especie.Text = details.Especie; }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Especie.Placeholder + "'", "Ok"); }
            try { MatGen.Text = details.MatGen; }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + MatGen.Placeholder + "'", "Ok"); }
            try { Propag.Text = details.Propag; }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Propag.Placeholder + "'", "Ok"); }
            try { SoloClasse.Text = details.SoloClasse; }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + SoloClasse.Placeholder + "'", "Ok"); }
            try { UnidMan.Text = details.UnidMan; }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + UnidMan.Placeholder + "'", "Ok"); }
            if (details.LatitudeLocG != 0)
            {
                try { LatitudeLocG.Text = details.LatitudeLocG.ToString(); }
                catch { DisplayAlert("Message", "Não foi possível ler o valor '" + LatitudeLocG.Placeholder + "'", "Ok"); }
            }
            if (details.LatitudeLocM != 0)
            {
                try { LatitudeLocM.Text = details.LatitudeLocM.ToString(); }
                catch { DisplayAlert("Message", "Não foi possível ler o valor '" + LatitudeLocM.Placeholder + "'", "Ok"); }
            }
            if (details.LatitudeLocS != 0)
            {
                try { LatitudeLocS.Text = details.LatitudeLocS.ToString("#.00"); }
                catch { DisplayAlert("Message", "Não foi possível ler o valor '" + LatitudeLocS.Placeholder + "'", "Ok"); }
            }
            if (details.LongitudeLocG != 0)
            {
                try { LongitudeLocG.Text = details.LongitudeLocG.ToString(); }
                catch { DisplayAlert("Message", "Não foi possível ler o valor '" + LongitudeLocG.Placeholder + "'", "Ok"); }
            }
            if (details.LongitudeLocM != 0)
            {
                try { LongitudeLocM.Text = details.LongitudeLocM.ToString(); }
                catch { DisplayAlert("Message", "Não foi possível ler o valor '" + LongitudeLocM.Placeholder + "'", "Ok"); }
            }
            if (details.LongitudeLocS != 0)
            {
                try { LongitudeLocS.Text = details.LongitudeLocS.ToString("#.00"); }
                catch { DisplayAlert("Message", "Não foi possível ler o valor '" + LongitudeLocS.Placeholder + "'", "Ok"); }
            }
            if (details.UTMN != 0)
            {
                try { UTMN.Text = details.UTMN.ToString(); }
                catch { DisplayAlert("Message", "Não foi possível ler o valor '" + UTMN.Placeholder + "'", "Ok"); }
            }
            if (details.UTME != 0)
            {
                try { UTME.Text = details.UTME.ToString(); }
                catch { DisplayAlert("Message", "Não foi possível ler o valor '" + UTME.Placeholder + "'", "Ok"); }
            }
            try { Datum.Text = details.Datum; }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Datum.Placeholder + "'", "Ok"); }
            try { Zona.Text = details.Zona; }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Zona.Placeholder + "'", "Ok"); }
            try { Lat.Text = details.Lat; }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Lat.Placeholder + "'", "Ok"); }
            try { Lon.Text = details.Lon; }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Lon.Placeholder + "'", "Ok"); }
            try { Observacoes.Text = details.Observacoes; }
            catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Observacoes.Placeholder + "'", "Ok"); }


            //if (details.PKTalhoesCR.ToString() != "")
            //{
            //    try { PKTalhoesCR.Text = details.PKTalhoesCR.ToString(); }
            //    catch { DisplayAlert("Message", "Não foi possível ler o valor 'PKTalhoesCRSQLite'", "Ok"); }
            //}
            //if (details.Empresa.ToString() != "")
            //{
            //    try { Empresa.Text = details.Empresa.ToString(); }
            //    catch { DisplayAlert("Message", "Não foi possível ler o valor 'Operação'", "Ok"); }
            //}
            //if (details.Unidade.ToString() != "")
            //{
            //    try { Unidade.Text = details.Unidade.ToString(); }
            //    catch { DisplayAlert("Message", "Não foi possível ler o valor 'Código do P.O.'", "Ok"); }
            //}
            //if (details.Fazenda.ToString() != "")
            //{
            //    try { Fazenda.Text = details.Fazenda.ToString(); }
            //    catch { DisplayAlert("Message", "Não foi possível ler o valor 'Tipo de trato cultural'", "Ok"); }
            //}
            //if (details.Talhao.ToString() != "")
            //{
            //    try { Talhao.Text = details.Talhao.ToString(); }
            //    catch { DisplayAlert("Message", "Não foi possível ler o valor 'Produto utilizado'", "Ok"); }
            //}
            //if (details.Subtalhao.ToString() != "")
            //{
            //    try { Subtalhao.Text = details.Subtalhao.ToString(); }
            //    catch { DisplayAlert("Message", "Não foi possível ler o valor 'Dose do produto'", "Ok"); }
            //}
            //if (details.Ciclo.ToString() != "")
            //{
            //    try { Ciclo.Text = details.Ciclo.ToString(); }
            //    catch { DisplayAlert("Message", "Não foi possível ler o valor 'Dose da solução'", "Ok"); }
            //}
            //if (details.Rotacao.ToString() != "")
            //{
            //    try { Rotacao.Text = details.Rotacao.ToString(); }
            //    catch { DisplayAlert("Message", "Não foi possível ler o valor 'Unid. dose produto'", "Ok"); }
            //}
            //if (details.EspEL.ToString() != "")
            //{
            //    try { EspEL.Text = details.EspEL.ToString(); }
            //    catch { DisplayAlert("Message", "Não foi possível ler o valor 'Unid. dose solução'", "Ok"); }
            //}
            //if (details.EspEP.ToString() != "")
            //{
            //    try { EspEP.Text = details.EspEP.ToString(); }
            //    catch { DisplayAlert("Message", "Não foi possível ler o valor 'Método de aplicação'", "Ok"); }
            //}
            ////DisplayAlert("Message", "details.DataInicio", "Ok");
            //try { DataIniRot.Date = DateTime.ParseExact(details.DataIniRot, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture); }
            //catch { DisplayAlert("Message", "Não foi possível ler o valor 'Data da operação'", "Ok"); }

            //if (details.AreaTalh.ToString() != "")
            //{
            //    try { AreaTalh.Text = details.AreaTalh.ToString(); }
            //    catch { DisplayAlert("Message", "Não foi possível ler o valor 'Núm. de horas'", "Ok"); }
            //}
            //if (details.AreaSubtalh.ToString() != "")
            //{
            //    try { AreaSubtalh.Text = details.AreaSubtalh.ToString(); }
            //    catch { DisplayAlert("Message", "Não foi possível ler o valor 'Rendimento'", "Ok"); }
            //}
            //if (details.Especie.ToString() != "")
            //{
            //    try { Especie.Text = details.Especie.ToString(); }
            //    catch { DisplayAlert("Message", "Não foi possível ler o valor 'Empresa resp.'", "Ok"); }
            //}
            //if (details.MatGen.ToString() != "")
            //{
            //    try { MatGen.Text = details.MatGen.ToString(); }
            //    catch { DisplayAlert("Message", "Não foi possível ler o valor 'Responsável pela op.'", "Ok"); }
            //}
            //if (details.Propag.ToString() != "")
            //{
            //    try { Propag.Text = details.Propag.ToString(); }
            //    catch { DisplayAlert("Message", "Não foi possível ler o valor 'Responsável pela op.'", "Ok"); }
            //}
            //if (details.SoloClasse != "")
            //{
            //    try { SoloClasse.Text = details.SoloClasse; }
            //    catch { DisplayAlert("Message", "Não foi possível ler o valor 'Responsável pela op.'", "Ok"); }
            //}
            //if (details.UnidMan != "")
            //{
            //    try { UnidMan.Text = details.UnidMan; }
            //catch { DisplayAlert("Message", "Não foi possível ler o valor 'Responsável pela op.'", "Ok"); }
            //}
            //if (details.LatitudeLocG.ToString() != "")
            //{
            //    try { LatitudeLocG.Text = details.LatitudeLocG.ToString(); }
            //    catch { DisplayAlert("Message", "Não foi possível ler o valor 'Responsável pela op.'", "Ok"); }
            //}
            //if (details.LatitudeLocM.ToString() != "")
            //{
            //    try { LatitudeLocM.Text = details.LatitudeLocM.ToString(); }
            //    catch { DisplayAlert("Message", "Não foi possível ler o valor 'Responsável pela op.'", "Ok"); }
            //}
            //if (details.LatitudeLocS.ToString() != "")
            //{
            //    try { LatitudeLocS.Text = details.LatitudeLocS.ToString(); }
            //    catch { DisplayAlert("Message", "Não foi possível ler o valor 'Responsável pela op.'", "Ok"); }
            //}
            //if (details.LongitudeLocG.ToString() != "")
            //{
            //    try { LongitudeLocG.Text = details.LongitudeLocG.ToString(); }
            //    catch { DisplayAlert("Message", "Não foi possível ler o valor 'Responsável pela op.'", "Ok"); }
            //}
            //if (details.LongitudeLocM.ToString() != "")
            //{
            //    try { LongitudeLocM.Text = details.LongitudeLocM.ToString(); }
            //    catch { DisplayAlert("Message", "Não foi possível ler o valor 'Responsável pela op.'", "Ok"); }
            //}
            //if (details.LongitudeLocS.ToString() != "")
            //{
            //    try { LongitudeLocS.Text = details.LongitudeLocS.ToString(); }
            //    catch { DisplayAlert("Message", "Não foi possível ler o valor 'Responsável pela op.'", "Ok"); }
            //}
            //if (details.UTMN.ToString() != "")
            //{
            //    try { UTMN.Text = details.UTMN.ToString(); }
            //    catch { DisplayAlert("Message", "Não foi possível ler o valor 'Responsável pela op.'", "Ok"); }
            //}
            //if (details.UTME.ToString() != "")
            //{
            //    try { UTME.Text = details.UTME.ToString(); }
            //    catch { DisplayAlert("Message", "Não foi possível ler o valor 'Responsável pela op.'", "Ok"); }
            //}
            //if (details.Datum.ToString() != "")
            //{
            //    try { Datum.Text = details.Datum.ToString(); }
            //    catch { DisplayAlert("Message", "Não foi possível ler o valor 'Responsável pela op.'", "Ok"); }
            //}
            //if (details.Zona.ToString() != "")
            //{
            //    try { Zona.Text = details.Zona.ToString(); }
            //    catch { DisplayAlert("Message", "Não foi possível ler o valor 'Responsável pela op.'", "Ok"); }
            //}
            //if (details.Lat.ToString() != "")
            //{
            //    try { Lat.Text = details.Lat.ToString(); }
            //    catch { DisplayAlert("Message", "Não foi possível ler o valor 'Responsável pela op.'", "Ok"); }
            //}
            //if (details.Lon.ToString() != "")
            //{
            //    try { Lon.Text = details.Lon.ToString(); }
            //    catch { DisplayAlert("Message", "Não foi possível ler o valor 'Responsável pela op.'", "Ok"); }
            //}
            //if (details.Observacoes != "")
            //{
            //    try { Observacoes.Text = details.Observacoes; }
            //    catch { DisplayAlert("Message", "Não foi possível ler o valor 'Observacoes'", "Ok"); }
            //}
            //Device.BeginInvokeOnMainThread(async () => { await DisplayAlert("Alert", "No internet connection", "Ok"); });
            SaveBtn.Text = "Atualizar";
            this.Title = "Editar Talhão";
        }

        private void SaveTalhoesCR(object sender, EventArgs e)
        {
            if (SaveBtn.Text == "Salvar")
            {
                //conn.BeginTransaction();
                //TalhoesCR talhoescr = new TalhoesCR();
                talhoescr = new TalhoesCR();
                try { talhoescr.Empresa = Empresa.Text; }
                catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + Empresa.Placeholder + "'", "Ok"); }
                try { talhoescr.Unidade = Unidade.Text; }
                catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + Unidade.Placeholder + "'", "Ok"); }
                try { talhoescr.Fazenda = Fazenda.Text; }
                catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + Fazenda.Placeholder + "'", "Ok"); }
                try { talhoescr.Talhao = Talhao.Text; }
                catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + Talhao.Placeholder + "'", "Ok"); }
                try { talhoescr.Subtalhao = Subtalhao.Text; }
                catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + Subtalhao.Placeholder + "'", "Ok"); }
                if (Ciclo.Text != null)
                {
                    try { talhoescr.Ciclo = Convert.ToInt32(Ciclo.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + Ciclo.Placeholder + "'", "Ok"); }
                }
                if (Rotacao.Text != null)
                {
                    try { talhoescr.Rotacao = Convert.ToInt32(Rotacao.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + Rotacao.Placeholder + "'", "Ok"); }
                }
                if (EspEL.Text != null)
                {
                    try { talhoescr.EspEL = float.Parse(EspEL.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + EspEL.Placeholder + "'", "Ok"); }
                }
                if (EspEP.Text != null)
                {
                    try { talhoescr.EspEP = float.Parse(EspEP.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + EspEP.Placeholder + "'", "Ok"); }
                }
                try { talhoescr.DataIniRot = DataIniRot.Date.ToString("dd/MM/yyyy"); }
                catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo 'Data do plantio/início da rotação'", "Ok"); }
                if (AreaTalh.Text != null)
                {
                    try { talhoescr.AreaTalh = float.Parse(AreaTalh.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + AreaTalh.Placeholder + "'", "Ok"); }
                }
                if (AreaSubtalh.Text != null)
                {
                    try { talhoescr.AreaSubtalh = float.Parse(AreaSubtalh.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + AreaSubtalh.Placeholder + "'", "Ok"); }
                }
                try { talhoescr.MatGen = MatGen.Text; }
                catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + MatGen.Placeholder + "'", "Ok"); }
                try { talhoescr.Propag = Propag.Text; }
                catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + Propag.Placeholder + "'", "Ok"); }
                try { talhoescr.SoloClasse = SoloClasse.Text; }
                catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + SoloClasse.Placeholder + "'", "Ok"); }
                try { talhoescr.UnidMan = UnidMan.Text; }
                catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + UnidMan.Placeholder + "'", "Ok"); }
                if (LatitudeLocG.Text != null)
                {
                    try { talhoescr.LatitudeLocG = Convert.ToInt32(LatitudeLocG.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + LatitudeLocG.Placeholder + "'", "Ok"); }
                }
                if (LatitudeLocM.Text != null)
                {
                    try { talhoescr.LatitudeLocM = Convert.ToInt32(LatitudeLocM.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + LatitudeLocM.Placeholder + "'", "Ok"); }
                }
                if (LatitudeLocS.Text != null)
                {
                    try { talhoescr.LatitudeLocS = float.Parse(LatitudeLocS.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + LatitudeLocS.Placeholder + "'", "Ok"); }
                }
                if (LongitudeLocG.Text != null)
                {
                    try { talhoescr.LongitudeLocG = Convert.ToInt32(LongitudeLocG.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + LongitudeLocG.Placeholder + "'", "Ok"); }
                }
                if (LongitudeLocM.Text != null)
                {
                    try { talhoescr.LongitudeLocM = Convert.ToInt32(LongitudeLocM.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + LongitudeLocM.Placeholder + "'", "Ok"); }
                }
                if (LongitudeLocS.Text != null)
                {
                    try { talhoescr.LongitudeLocS = float.Parse(LongitudeLocS.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + LongitudeLocS.Placeholder + "'", "Ok"); }
                }
                if (UTMN.Text != null)
                {
                    try { talhoescr.UTMN = float.Parse(UTMN.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + UTMN.Placeholder + "'", "Ok"); }
                }
                if (UTME.Text != null)
                {
                    try { talhoescr.UTME = float.Parse(UTME.Text); }
                    catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + UTME.Placeholder + "'", "Ok"); }
                }
                try { talhoescr.Datum = Datum.Text; }
                catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + Datum.Placeholder + "'", "Ok"); }
                try { talhoescr.Zona = Zona.Text; }
                catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + Zona.Placeholder + "'", "Ok"); }
                try { talhoescr.Lat = Lat.Text; }
                catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + Lat.Placeholder + "'", "Ok"); }
                try { talhoescr.Lon = Lon.Text; }
                catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + Lon.Placeholder + "'", "Ok"); }
                try { talhoescr.Observacoes = Observacoes.Text; }
                catch { DisplayAlert("Message", "Não foi possível salvar. Verifique o valor digitado no campo '" + Observacoes.Placeholder + "'", "Ok"); }
                try { talhoescr.Visible = false; }
                catch { DisplayAlert("Message", "Houve um erro. Não foi possível atualizar o campo Visible", "Ok"); }
                try { talhoescr.Imagem = "Down.png"; }
                catch { DisplayAlert("Message", "Houve um erro. Não foi possível atualizar o campo Imagem", "Ok"); }
                conn.Insert(talhoescr);
                //conn.Commit();
                SaveBtn.IsEnabled = false;
                CancelBtn.Text = "Voltar";

            }
            else
            {
                //txch = false;
                conn.BeginTransaction();
                //talhoescr = new TalhoesCR();
                string sql = $"UPDATE TalhoesCR SET" +
                    $" Empresa = '{Empresa.Text}'" +
                    $", Unidade = '{Unidade.Text}'" +
                    $", Fazenda = '{Fazenda.Text}'" +
                    $", Talhao = '{Talhao.Text}'" +
                    $", Subtalhao = '{Subtalhao.Text}'";
                if ((Ciclo.Text != "") && (Ciclo.Text != null)) { sql += $", Ciclo = {Ciclo.Text}"; }
                else { sql += $", Ciclo = NULL"; }
                if ((Rotacao.Text != "") && (Rotacao.Text != null)) { sql += $", Rotacao = {Rotacao.Text}"; }
                else { sql += $", Rotacao = NULL"; }
                if ((EspEL.Text != "") && (EspEL.Text != null)) { sql += $", EspEL = {Math.Round(float.Parse(EspEL.Text) * 100)}*1.0/100"; }
                else { sql += $", EspEL = NULL"; }
                if ((EspEP.Text != "") && (EspEP.Text != null)) { sql += $", EspEP = {Math.Round(float.Parse(EspEP.Text) * 100)}*1.0/100"; }
                else { sql += $", EspEP = NULL"; }
                sql += $", DataIniRot = '{DataIniRot.Date:dd/MM/yyyy}'";
                if ((AreaTalh.Text != "") && (AreaTalh.Text != null)) { sql += $", AreaTalh = {Math.Round(float.Parse(AreaTalh.Text) * 100)}*1.0/100"; }
                else { sql += $", AreaTalh = NULL"; }
                if ((AreaSubtalh.Text != "") && (AreaSubtalh.Text != null)) { sql += $", AreaSubtalh = {Math.Round(float.Parse(AreaSubtalh.Text) * 100)}*1.0/100"; }
                else { sql += $", AreaSubtalh = NULL"; }
                sql += $", MatGen = '{MatGen.Text}'" +
                    $", Propag = '{Propag.Text}'" +
                    $", SoloClasse = '{SoloClasse.Text}'" +
                    $", UnidMan = '{UnidMan.Text}'";
                if ((LatitudeLocG.Text != "") && (LatitudeLocG.Text != null)) { sql += $", LatitudeLocG = {LatitudeLocG.Text}"; }
                else { sql += $", LatitudeLocG = NULL"; }
                if ((LatitudeLocM.Text != "") && (LatitudeLocM.Text != null)) { sql += $", LatitudeLocM = {LatitudeLocM.Text}"; }
                else { sql += $", LatitudeLocM = NULL"; }
                if ((LatitudeLocS.Text != "") && (LatitudeLocS.Text != null)) { sql += $", LatitudeLocS = {Math.Round(float.Parse(LatitudeLocS.Text) * 100)}*1.0/100"; }
                else { sql += $", LatitudeLocS = NULL"; }
                if ((LongitudeLocG.Text != "") && (LongitudeLocG.Text != null)) { sql += $", LongitudeLocG = {LongitudeLocG.Text}"; }
                else { sql += $", LongitudeLocG = NULL"; }
                if ((LongitudeLocM.Text != "") && (LongitudeLocM.Text != null)) { sql += $", LongitudeLocM = {LongitudeLocM.Text}"; }
                else { sql += $", LongitudeLocM = NULL"; }
                if ((LongitudeLocS.Text != "") && (LongitudeLocS.Text != null)) { sql += $", LongitudeLocS = {Math.Round(float.Parse(LongitudeLocS.Text) * 100)}*1.0/100"; }
                else { sql += $", LongitudeLocS = NULL"; }
                if ((UTMN.Text != "") && (UTMN.Text != null)) { sql += $", UTMN = {Math.Round(float.Parse(UTMN.Text) * 100)}*1.0/100"; }
                else { sql += $", UTMN = NULL"; }
                if ((UTME.Text != "") && (UTME.Text != null)) { sql += $", UTME = {Math.Round(float.Parse(UTME.Text) * 100)}*1.0/100"; }
                else { sql += $", UTME = NULL"; }
                sql += $", Datum = '{Datum.Text}'" +
                    $", Zona = '{Zona.Text}'" +
                    $", Lat = '{Lat.Text}'" +
                    $", Lon = '{Lon.Text}'" +
                    $", Observacoes = '{Observacoes.Text}'" +
                    $" WHERE PKTalhoesCR = {PKTalhoesCR.Text}";
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

        public void TextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Entry)sender;
            var ev = e as TextChangedEventArgs;

            //Código que estava funcionando anteriormente

            if (ev.NewTextValue != ev.OldTextValue)
            {
                double newvalor = 0;
                if ((ev.NewTextValue != null) && (ev.NewTextValue != "") && (ev.NewTextValue != "0,00"))
                //if ((ev.NewTextValue != null) && (ev.NewTextValue != "") && (ev.NewTextValue != "0,00"))
                {
                    //código antigo que estava funcionando
                    newvalor = float.Parse(Regex.Replace(ev.NewTextValue, @"[^0-9]", ""));
                    //newvalor = float.Parse(ev.NewTextValue);
                }
                double oldvalor = 0;
                if ((ev.OldTextValue != null) && (ev.OldTextValue != "") && (ev.OldTextValue != "0,00"))
                {
                    //oldvalor = float.Parse(ev.OldTextValue);
                    oldvalor = float.Parse(Regex.Replace(ev.OldTextValue, @"[^0-9]", ""));
                }
                if (newvalor != (oldvalor * 100))
                {
                    //entry.Text = (newvalor * 1.0 / 100).ToString("F2", CultureInfo.InvariantCulture);
                    entry.Text = (newvalor * 1.0 / 100).ToString("0.00");
                }

                //if ((newvalor != (oldvalor * 100)) && ((newvalor * 10) != oldvalor))
                //{
                //    entry.Text = (newvalor * 1.0 / 100).ToString("#.#0");
                //    //double num = newvalor * 1.0 / 100;
                //    //string numstr = (newvalor * 1.0 / 100).ToString();
                //}

            }
        }

    }
}