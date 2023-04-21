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
	public partial class pgAddTesteGPS : ContentPage
	{
        private SQLiteConnection conn;
        public TesteGPS testegps;

        public pgAddTesteGPS (string pktalhoescr)
		{
			InitializeComponent ();
            conn = DependencyService.Get<ISQLite>().GetConnection();
            conn.CreateTable<TesteGPS>();

        }
        private void PopulateDetails(TesteGPS details)
        {
            try { PKTalhoesCR.Text = details.PKTalhoesCR.ToString(); }
            catch { DisplayAlert("Message", "Não foi possível ler o valor 'Código do talhão'", "Ok"); }

            //try { Empresa.Text = details.Empresa; }
            //catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Empresa.Placeholder + "'", "Ok"); }
            //try { Unidade.Text = details.Unidade; }
            //catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Unidade.Placeholder + "'", "Ok"); }
            //try { Fazenda.Text = details.Fazenda; }
            //catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Fazenda.Placeholder + "'", "Ok"); }
            //try { Talhao.Text = details.Talhao; }
            //catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Talhao.Placeholder + "'", "Ok"); }
            //try { Subtalhao.Text = details.Subtalhao; }
            //catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Subtalhao.Placeholder + "'", "Ok"); }
            //try { Ciclo.Text = details.Ciclo.ToString(); }
            //catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Ciclo.Placeholder + "'", "Ok"); }
            //try { Rotacao.Text = details.Rotacao.ToString(); }
            //catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Rotacao.Placeholder + "'", "Ok"); }
            //try { EspEL.Text = details.EspEL.ToString(); }
            //catch { DisplayAlert("Message", "Não foi possível ler o valor '" + EspEL.Placeholder + "'", "Ok"); }
            //try { EspEP.Text = details.EspEP.ToString(); }
            //catch { DisplayAlert("Message", "Não foi possível ler o valor '" + EspEP.Placeholder + "'", "Ok"); }
            ////DisplayAlert("Message", "details.DataInicio", "Ok");
            //try { DataIniRot.Date = DateTime.ParseExact(details.DataIniRot, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture); }
            //catch { DisplayAlert("Message", "Não foi possível ler o valor 'Data de início da rotação'", "Ok"); }
            //try { AreaTalh.Text = details.AreaTalh.ToString(); }
            //catch { DisplayAlert("Message", "Não foi possível ler o valor '" + AreaTalh.Placeholder + "'", "Ok"); }
            //try { AreaSubtalh.Text = details.AreaSubtalh.ToString(); }
            //catch { DisplayAlert("Message", "Não foi possível ler o valor '" + AreaSubtalh.Placeholder + "'", "Ok"); }
            //try { Especie.Text = details.Especie; }
            //catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Especie.Placeholder + "'", "Ok"); }
            //try { MatGen.Text = details.MatGen; }
            //catch { DisplayAlert("Message", "Não foi possível ler o valor '" + MatGen.Placeholder + "'", "Ok"); }
            //try { Propag.Text = details.Propag; }
            //catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Propag.Placeholder + "'", "Ok"); }
            //try { SoloClasse.Text = details.SoloClasse; }
            //catch { DisplayAlert("Message", "Não foi possível ler o valor '" + SoloClasse.Placeholder + "'", "Ok"); }
            //try { UnidMan.Text = details.UnidMan; }
            //catch { DisplayAlert("Message", "Não foi possível ler o valor '" + UnidMan.Placeholder + "'", "Ok"); }
            //try { LatitudeLocG.Text = details.LatitudeLocG.ToString(); }
            //catch { DisplayAlert("Message", "Não foi possível ler o valor '" + LatitudeLocG.Placeholder + "'", "Ok"); }
            //try { LatitudeLocM.Text = details.LatitudeLocM.ToString(); }
            //catch { DisplayAlert("Message", "Não foi possível ler o valor '" + LatitudeLocM.Placeholder + "'", "Ok"); }
            //try { LatitudeLocS.Text = details.LatitudeLocS.ToString(); }
            //catch { DisplayAlert("Message", "Não foi possível ler o valor '" + LatitudeLocS.Placeholder + "'", "Ok"); }
            //try { LongitudeLocG.Text = details.LongitudeLocG.ToString(); }
            //catch { DisplayAlert("Message", "Não foi possível ler o valor '" + LongitudeLocG.Placeholder + "'", "Ok"); }
            //try { LongitudeLocM.Text = details.LongitudeLocM.ToString(); }
            //catch { DisplayAlert("Message", "Não foi possível ler o valor '" + LongitudeLocM.Placeholder + "'", "Ok"); }
            //try { LongitudeLocS.Text = details.LongitudeLocS.ToString(); }
            //catch { DisplayAlert("Message", "Não foi possível ler o valor '" + LongitudeLocS.Placeholder + "'", "Ok"); }
            //try { UTMN.Text = details.UTMN.ToString(); }
            //catch { DisplayAlert("Message", "Não foi possível ler o valor '" + UTMN.Placeholder + "'", "Ok"); }
            //try { UTME.Text = details.UTME.ToString(); }
            //catch { DisplayAlert("Message", "Não foi possível ler o valor '" + UTME.Placeholder + "'", "Ok"); }
            //try { Datum.Text = details.Datum; }
            //catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Datum.Placeholder + "'", "Ok"); }
            //try { Zona.Text = details.Zona; }
            //catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Zona.Placeholder + "'", "Ok"); }
            //try { Lat.Text = details.Lat; }
            //catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Lat.Placeholder + "'", "Ok"); }
            //try { Lon.Text = details.Lon; }
            //catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Lon.Placeholder + "'", "Ok"); }
            //try { Observacoes.Text = details.Observacoes; }
            //catch { DisplayAlert("Message", "Não foi possível ler o valor '" + Observacoes.Placeholder + "'", "Ok"); }



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

        private void CancelarEdicao(object sender, EventArgs e)
        {
            //
        }

    }
}