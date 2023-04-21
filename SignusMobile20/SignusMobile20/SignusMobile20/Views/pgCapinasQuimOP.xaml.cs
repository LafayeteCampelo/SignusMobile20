using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SignusMobile20.Models;
using SQLite;
using Syncfusion.XlsIO;
using System.IO;
using System.Reflection;
using Color = Syncfusion.Drawing.Color;


namespace SignusMobile20
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class pgCapinasQuimOP : ContentPage
	{
        private SQLiteConnection conn;
        public CapinasQuimOP capinasquimop;
        public CapinasQuimCQ capinasquimcq;
        public string pktlh;
        string msg;

        public pgCapinasQuimOP (string pktalhoescr)
		{
			InitializeComponent ();
            conn = DependencyService.Get<ISQLite>().GetConnection();
            //conn.DropTable<CapinasQuimOP>();
            //conn.DropTable<CapinasQuimCQ>();
            conn.CreateTable<CapinasQuimOP>();
            conn.CreateTable<CapinasQuimCQ>();

            pktlh = pktalhoescr;
            PKTalhoesCR.Text = "Código do talhão: " + pktlh;

        }

        protected override void OnAppearing()
        {
            int num = Convert.ToInt32(pktlh);

            var data = conn.Table<CapinasQuimOP>().Where(x => x.PKTalhoesCR == num).ToList();
            lstCapinasQuimOP.ItemsSource = data;
        }

        private void Mostrar_Clicked(object sender, EventArgs e)
        {
            var data = (from capinasquim in conn.Table<CapinasQuimOP>() select capinasquim);
            lstCapinasQuimOP.ItemsSource = data;
        }

        private void NovoCapinasQuimOP_Clicked(object sender, EventArgs e)
        {
//            Navigation.PushAsync(new pgAddCapinasQuimOP(null, pktlh));
        }

        private void EditCapinasQuimOP(object sender, ItemTappedEventArgs e)
        {
            CapinasQuimOP details = e.Item as CapinasQuimOP;
            if (details != null)
            {
//                Navigation.PushAsync(new pgAddCapinasQuimOP(details, pktlh));
            }
        }

        public async void AlternaDetalhes(object sender, ItemTappedEventArgs e)
        {
            //Alterna entre visível e invisível a lista de detalhes
            CapinasQuimOP details = e.Item as CapinasQuimOP;
            string sql = "";

            if (details.Visible.ToString() == "True")
            {
                sql = $"UPDATE CapinasQuimOP SET Visible = 0, Imagem = 'Down.png' ";
            }
            else
            {
                sql = $"UPDATE CapinasQuimOP SET Visible = 1, Imagem = 'Up.png' ";
            }

            sql += $"WHERE PKCapinasQuimOP = {details.PKCapinasQuimOP}";
            try { conn.Execute(sql); }
            catch { await DisplayAlert("Message", "Houve um erro. Não foi possível atualizar.", "Ok"); }

            sql = $"SELECT * FROM CapinasQuimOP WHERE PKTalhoesCR = {pktlh}";

            //Atualiza a lista
            var data = conn.Query<CapinasQuimOP>(sql);
            lstCapinasQuimOP.ItemsSource = data;
        }

        private async void DeleteCapinasQuimOP(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var operacao = mi.CommandParameter as CapinasQuimOP;
            if (operacao != null)
            {
                var opcao = await DisplayAlert("Excluir operação", "Confirma a exclusão da operação de capina química de código " + operacao.PKCapinasQuimOP + "" +
                    " e de todos os outros (de controle de qualidade) associados? ", "Ok", "Cancel");
                if (opcao)
                {
                    //Deleta todos os registros de controle de qualidade associados à operação atual
                    string sql = $"DELETE FROM CapinasQuimCQ WHERE PKCapinasQuimOP = {operacao.PKCapinasQuimOP}";
                    try { conn.Execute(sql); }
                    catch
                    {
                        await DisplayAlert("Message", "Houve um erro, não foi possível excluir os registros de controle de qualidade associados a este registro de operação de capina química. " +
                            "Possivelmente, a tabela de controle de qualidade ainda não foi criada.", "Ok");
                    }

                    //Deleta o registro de operação
                    sql = $"DELETE FROM CapinasQuimOP WHERE PKCapinasQuimOP = {operacao.PKCapinasQuimOP}";

                    try { conn.Execute(sql); }
                    catch { await DisplayAlert("Message", "Houve um erro, não foi possível excluir o registro de operação de capina química.", "Ok"); }

                    sql = $"SELECT * FROM CapinasQuimOP WHERE PKTalhoesCR = {pktlh}";
                    conn.Query<CapinasQuimOP>(sql);

                    //Atualiza a lista
                    var data = conn.Query<CapinasQuimOP>(sql);
                    lstCapinasQuimOP.ItemsSource = data;
                }
            }
        }

        private async void AlterarCapinasQuimOP(object sender, EventArgs e)
        {
            try
            {
                var mi = ((MenuItem)sender);
                var operacao = mi.CommandParameter as CapinasQuimOP;
                if (operacao != null)
                {
//                    await Navigation.PushAsync(new pgAddCapinasQuimOP(operacao, pktlh));
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro: ", ex.Message, "Ok");
            }
        }

        private async void AvaliarQualidade(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var operacao = mi.CommandParameter as CapinasQuimOP;
            if (operacao != null)
            {
                //string sql = $"SELECT * FROM CapinasQuimCQ WHERE PKCapinasQuimOP = {operacao.PKCapinasQuimOP}";
                //try { conn.Query<CapinasQuimCQ>(sql); }
                //catch { await DisplayAlert("Message", "Houve um erro. Não foi possível abrir a consulta.", "Ok"); }
                //Atualiza a página
//                await Navigation.PushAsync(new pgCapinasQuimCQ(pktlh, operacao.PKCapinasQuimOP.ToString()));
            }
        }

        private void ExportarCapQuimica_Clicked(object sender, EventArgs e)
        {

            DisplayAlert("Informação", message: "Este comando exporta todos os dados das operações de capinas químicas e os respectivos controles de qualidade.", "Ok");

            //Create an instance of ExcelEngine.
            using (ExcelEngine excelEngine = new ExcelEngine())
            {

                IApplication application = excelEngine.Excel;

                application.DefaultVersion = ExcelVersion.Excel2016;

                //Create a workbook with a worksheet
                IWorkbook workbook = excelEngine.Excel.Workbooks.Create(3);

                //Access first worksheet from the workbook instance.
                IWorksheet worksheet = workbook.Worksheets[0];
                worksheet.Name = "Talhões";

                //Assembly executingAssembly = typeof(MainPage).GetTypeInfo().Assembly;
                Assembly executingAssembly = typeof(pgTalhoesCR).GetTypeInfo().Assembly;

                Stream inputStream = executingAssembly.GetManifestResourceStream("GettingStarted.AdventureCycles-Logo.png");

                //Add a picture
                //Este comando não deu certo
                //IPictureShape shape = worksheet.Pictures.AddPicture(1, 1, inputStream);

                //Disable gridlines in the worksheet
                //worksheet.IsGridLinesVisible = false;

                //Enter values to the cells from A3 to A5
                worksheet.Range["A1"].Text = "PKTalhoes"; //PKTalhoesCR
                worksheet.Range["B1"].Text = "CodTalhoesCRSR";//CodTalhoesCRSR
                worksheet.Range["C1"].Text = "Código do local";//CodInform_Local
                worksheet.Range["D1"].Text = "CodCadastro";//CodCadastro
                worksheet.Range["E1"].Text = "Empresa";//Empresa
                worksheet.Range["F1"].Text = "Unidade";//Unidade
                worksheet.Range["G1"].Text = "Fazenda/Projeto";//Fazenda
                worksheet.Range["H1"].Text = "Talhão";//Talhao
                worksheet.Range["I1"].Text = "Subtalhão";//Subtalhao
                worksheet.Range["J1"].Text = "Ciclo";//Ciclo
                worksheet.Range["K1"].Text = "Rotacão";//Rotacao
                worksheet.Range["L1"].Text = "Espaç entre linhas";//EspEL
                worksheet.Range["M1"].Text = "Espaç entre plantas";//EspEP
                worksheet.Range["N1"].Text = "DataIniRot/Data de plantio";//DataIniRot
                worksheet.Range["O1"].Text = "Área do talhão (ha)";//AreaTalh
                worksheet.Range["P1"].Text = "AreaSubtalh";//AreaSubtalh
                worksheet.Range["Q1"].Text = "Espécie";//Especie
                worksheet.Range["R1"].Text = "MatGen";//MatGen
                worksheet.Range["S1"].Text = "Tipo propagação";//Propag
                worksheet.Range["T1"].Text = "Classe de solo";//SoloClasse
                worksheet.Range["U1"].Text = "Cód da Unid manejo";//UnidMan
                worksheet.Range["V1"].Text = "Latitude (G)";//LatitudeLocG
                worksheet.Range["W1"].Text = "Latitude (M)";//LatitudeLocM
                worksheet.Range["X1"].Text = "Latitude (S)";//LatitudeLocS
                worksheet.Range["Y1"].Text = "Longitude (G)";//LongitudeLocG
                worksheet.Range["Z1"].Text = "Longitude (M)";//LongitudeLocM
                worksheet.Range["AA1"].Text = "Longitude (S)";//LongitudeLocS
                worksheet.Range["AB1"].Text = "UTM N (m)";//UTMN
                worksheet.Range["AC1"].Text = "UTM E (m)";//UTME
                worksheet.Range["AD1"].Text = "Datum";//Datum
                worksheet.Range["AE1"].Text = "Zona";//Zona
                worksheet.Range["AF1"].Text = "Lat (N/S)";//Lat
                worksheet.Range["AG1"].Text = "Long (E/W)";//Lon
                worksheet.Range["AH1"].Text = "Observações";//Observacoes

                //Formata fonte da linha de títulos
                worksheet.Range["A1:AH1"].CellStyle.Font.Bold = true;
                worksheet.Range["A1:AH1"].CellStyle.Font.Italic = true;
                worksheet.Range["A1:D1"].CellStyle.Font.RGBColor = Color.FromArgb(200, 50, 50);

                //Colore as células A1 a D1
                worksheet.Range["A1:D1"].CellStyle.Color = Color.FromArgb(150, 150, 150);

                //Borda inferior da linha de títulos
                worksheet.Range["A1:AH1"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range["A1:AH1"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].Color = ExcelKnownColors.Black;

                //Formatação de números
                worksheet.Range["L1:M1"].NumberFormat = "$.00";
                worksheet.Range["O1:P1"].NumberFormat = "$.00";
                worksheet.Range["X1:X1"].NumberFormat = "$.00";
                worksheet.Range["AA1:AA1"].NumberFormat = "$.00";
                worksheet.Range["AB1:AC1"].NumberFormat = "$.00";

                //Ajusta a largura das colunas
                worksheet.Range["A1:AH1"].AutofitColumns();

                string sql = $"SELECT * FROM TalhoesCR";
                try { conn.Query<TalhoesCR>(sql); }
                catch { DisplayAlert("Message", "Não foi possível abrir a tabela TalhoesCR.", "Ok"); }
                sql.First();

                var qryCapinasQuimOP = conn.Query<CapinasQuimOP>("SELECT PKTalhoesCR FROM CapinasQuimOP GROUP BY PKTalhoesCR ORDER BY PKTalhoesCR");
                //var ListaAmostragem = conn.Table<AmostragemMN>();
                if (qryCapinasQuimOP.Count() > 0)
                {
                    qryCapinasQuimOP.First();
                    int i = 1;
                    foreach (var campoam in qryCapinasQuimOP)
                    {
                        i = i + 1;
                        int cdtalh = campoam.PKTalhoesCR;
                        if (cdtalh > 0)
                        {
                            var qryTalhoes = conn.Query<TalhoesCR>("SELECT * FROM TalhoesCR WHERE PKTalhoesCR = " + cdtalh.ToString() + "");
                            //var ListaTalhoes = conn.Table<TalhoesCR>();
                            if (qryTalhoes.Count() > 0)
                            {
                                qryTalhoes.First();
                                foreach (var campotalh in qryTalhoes)
                                {

                                    try { worksheet.Range["A" + i].Number = campotalh.PKTalhoesCR; }
                                    catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PKTalhoesCR'", "Ok"); }
                                    try { worksheet.Range["B" + i].Number = campotalh.CodTalhoesCRSR; }
                                    catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodTalhoesCRSR'", "Ok"); }
                                    try { worksheet.Range["C" + i].Number = campotalh.CodInform_Local; }
                                    catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodInform_Local'", "Ok"); }
                                    try { worksheet.Range["D" + i].Number = campotalh.CodCadastro; }
                                    catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodCadastro'", "Ok"); }
                                    if (campotalh.Empresa != null)
                                    {
                                        try { worksheet.Range["E" + i].Text = campotalh.Empresa; }
                                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Empresa'", "Ok"); }
                                    }
                                    if (campotalh.Unidade != null)
                                    {
                                        try { worksheet.Range["F" + i].Text = campotalh.Unidade; }
                                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Unidade'", "Ok"); }
                                    }
                                    if (campotalh.Fazenda != null)
                                    {
                                        try { worksheet.Range["G" + i].Text = campotalh.Fazenda; }
                                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Fazenda'", "Ok"); }
                                    }
                                    if (campotalh.Talhao != null)
                                    {
                                        try { worksheet.Range["H" + i].Text = campotalh.Talhao; }
                                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Talhao'", "Ok"); }
                                    }
                                    if (campotalh.Subtalhao != null)
                                    {
                                        try { worksheet.Range["I" + i].Text = campotalh.Subtalhao; }
                                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Subtalhao'", "Ok"); }
                                    }
                                    try { worksheet.Range["J" + i].Number = campotalh.Ciclo; }
                                    catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Ciclo'", "Ok"); }
                                    try { worksheet.Range["K" + i].Number = campotalh.Rotacao; }
                                    catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Rotacao'", "Ok"); }
                                    try { worksheet.Range["L" + i].Number = campotalh.EspEL; }
                                    catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'EspEL'", "Ok"); }
                                    try { worksheet.Range["M" + i].Number = campotalh.EspEP; }
                                    catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'EspEP'", "Ok"); }
                                    if (campotalh.DataIniRot != null)
                                    {
                                        try { worksheet.Range["N" + i].Value = campotalh.DataIniRot; }
                                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'DataIniRot'", "Ok"); }
                                    }
                                    try { worksheet.Range["O" + i].Number = campotalh.AreaTalh; }
                                    catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'AreaTalh'", "Ok"); }
                                    try { worksheet.Range["P" + i].Number = campotalh.AreaSubtalh; }
                                    catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'AreaSubtalh'", "Ok"); }
                                    if (campotalh.Especie != null)
                                    {
                                        try { worksheet.Range["Q" + i].Text = campotalh.Especie; }
                                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Especie'", "Ok"); }
                                    }
                                    if (campotalh.MatGen != null)
                                    {
                                        try { worksheet.Range["R" + i].Text = campotalh.MatGen; }
                                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'MatGen'", "Ok"); }
                                    }
                                    if (campotalh.SoloClasse != null)
                                    {
                                        try { worksheet.Range["T" + i].Text = campotalh.SoloClasse; }
                                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'SoloClasse'", "Ok"); }
                                    }
                                    if (campotalh.UnidMan != null)
                                    {
                                        try { worksheet.Range["U" + i].Text = campotalh.UnidMan; }
                                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'UnidMan'", "Ok"); }
                                    }
                                    try { worksheet.Range["V" + i].Number = campotalh.LatitudeLocG; }
                                    catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'LatitudeLocG'", "Ok"); }
                                    try { worksheet.Range["W" + i].Number = campotalh.LatitudeLocM; }
                                    catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'LatitudeLocM'", "Ok"); }
                                    try { worksheet.Range["X" + i].Number = campotalh.LatitudeLocS; }
                                    catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'LatitudeLocS'", "Ok"); }
                                    try { worksheet.Range["Y" + i].Number = campotalh.LongitudeLocG; }
                                    catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'LongitudeLocG'", "Ok"); }
                                    try { worksheet.Range["Z" + i].Number = campotalh.LongitudeLocM; }
                                    catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'LongitudeLocM'", "Ok"); }
                                    try { worksheet.Range["AA" + i].Number = campotalh.LongitudeLocS; }
                                    catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'LongitudeLocS'", "Ok"); }
                                    try { worksheet.Range["AB" + i].Number = campotalh.UTMN; }
                                    catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'UTMN'", "Ok"); }
                                    try { worksheet.Range["AC" + i].Number = campotalh.UTME; }
                                    catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'UTME'", "Ok"); }
                                    if (campotalh.Datum != null)
                                    {
                                        try { worksheet.Range["AD" + i].Text = campotalh.Datum; }
                                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Datum'", "Ok"); }
                                    }
                                    if (campotalh.Zona != null)
                                    {
                                        try { worksheet.Range["AE" + i].Text = campotalh.Zona; }
                                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Zona'", "Ok"); }
                                    }
                                    if (campotalh.Lat != null)
                                    {
                                        try { worksheet.Range["AF" + i].Text = campotalh.Lat; }
                                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Lat'", "Ok"); }
                                    }
                                    if (campotalh.Lon != null)
                                    {
                                        try { worksheet.Range["AG" + i].Text = campotalh.Lon; }
                                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Lon'", "Ok"); }
                                    }
                                    if (campotalh.Observacoes != null)
                                    {
                                        try { worksheet.Range["AH" + i].Text = campotalh.Observacoes; }
                                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Observacoes'", "Ok"); }
                                    }
                                }
                            }
                        }
                    }
                }

                //Acrescenta a planilha de CapinasQuim_OP.
                IWorksheet worksheet1 = workbook.Worksheets[1];
                worksheet1.Name = "CapinasQuim_OP";

                //Enter values to the cells from A3 to A5
                worksheet1.Range["A1"].Text = "Código da operação (PK)";//int PKCapinasQuimOP
                worksheet1.Range["B1"].Text = "Código do talhão (PK)";//int PKTalhoesCR
                worksheet1.Range["C1"].Text = "Código do trato cultural";//int CodTrCulturaisOP
                worksheet1.Range["D1"].Text = "Código do local (talhão)";//int CodLocal
                worksheet1.Range["E1"].Text = "Nome da operação";//string IdentifOperacao
                worksheet1.Range["F1"].Text = "Proced. operacional";//int ProcedOperac
                worksheet1.Range["G1"].Text = "Tipo de trato cultural";//string TipoTC
                worksheet1.Range["H1"].Text = "Produto utilizado";//string ProdutoUtilizado
                worksheet1.Range["I1"].Text = "Dose do produto";//float DoseProd
                worksheet1.Range["J1"].Text = "Dose da solução";//float DoseSoluc
                worksheet1.Range["K1"].Text = "Unid. dose produto";//string UnidDoseProd
                worksheet1.Range["L1"].Text = "Unid. dose soluç.";//string UnidDoseSoluc
                worksheet1.Range["M1"].Text = "Método de aplicação";//string MetodoAplic
                worksheet1.Range["N1"].Text = "Equipamento";//string Equipamento
                worksheet1.Range["O1"].Text = "Marca do equipamento";//string MarcaEquip
                worksheet1.Range["P1"].Text = "Modelo do equipamento";//string ModeloEquip
                worksheet1.Range["Q1"].Text = "Data da operação";//string DataOper
                worksheet1.Range["R1"].Text = "Núm. de horas";//float NumHoras
                worksheet1.Range["S1"].Text = "Rendimento";//float Rendimento
                worksheet1.Range["T1"].Text = "Empresa resp.";//string EmpresaResp
                worksheet1.Range["U1"].Text = "Resp. pela operação";//string ResponsavelOper
                worksheet1.Range["V1"].Text = "Observações";//string Observacoes

                //Formata fonte da linha de títulos
                worksheet1.Range["A1:V1"].CellStyle.Font.Bold = true;
                worksheet1.Range["A1:V1"].CellStyle.Font.Italic = true;
                worksheet1.Range["A1:D1"].CellStyle.Font.RGBColor = Color.FromArgb(200, 50, 50);

                //Colore as células A1 a D1
                worksheet1.Range["A1:D1"].CellStyle.Color = Color.FromArgb(150, 150, 150);

                //Borda inferior da linha de títulos
                worksheet1.Range["A1:V1"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                worksheet1.Range["A1:V1"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].Color = ExcelKnownColors.Black;

                //Ajusta a largura das colunas
                worksheet1.Range["A1:V1"].AutofitColumns();

                sql = $"SELECT * FROM CapinasQuimOP";
                try { conn.Query<CapinasQuimOP>(sql); }
                catch { DisplayAlert("Message", "Não foi possível abrir a tabela CapinasQuimOP.", "Ok"); }
                sql.First();

                var qryCapinasQuim = conn.Query<CapinasQuimOP>("SELECT * FROM CapinasQuimOP");

                var ListaCapinasQuim = conn.Table<CapinasQuimOP>();

                if (ListaCapinasQuim.Count() > 0)
                {
                    int i = 1;
                    foreach (var campo in ListaCapinasQuim)
                    {
                        i = i + 1;

                        try { worksheet1.Range["A" + i].Number = campo.PKCapinasQuimOP; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PKCapinasQuimOP'", "Ok"); }
                        try { worksheet1.Range["B" + i].Number = campo.PKTalhoesCR; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PKTalhoesCR'", "Ok"); }
                        try { worksheet1.Range["C" + i].Number = campo.CodTrCulturaisOP; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodTrCulturaisOP'", "Ok"); }
                        try { worksheet1.Range["D" + i].Number = campo.CodLocal; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodLocal'", "Ok"); }
                        if (campo.IdentifOperacao != null)
                        {
                            try { worksheet1.Range["E" + i].Text = campo.IdentifOperacao; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'IdentifOperacao'", "Ok"); }
                        }
                        try { worksheet1.Range["F" + i].Number = campo.ProcedOperac; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'ProcedOperac'", "Ok"); }
                        if (campo.TipoTC != null)
                        {
                            try { worksheet1.Range["G" + i].Text = campo.TipoTC; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'TipoTC'", "Ok"); }
                        }
                        if (campo.ProdutoUtilizado != null)
                        {
                            try { worksheet1.Range["H" + i].Text = campo.ProdutoUtilizado; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'ProdutoUtilizado'", "Ok"); }
                        }
                        try { worksheet1.Range["O" + i].Number = campo.DoseProd; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'DoseProd'", "Ok"); }
                        try { worksheet1.Range["O" + i].Number = campo.DoseSoluc; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'DoseSoluc'", "Ok"); }
                        if (campo.UnidDoseProd != null)
                        {
                            try { worksheet1.Range["I" + i].Text = campo.UnidDoseProd; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'UnidDoseProd'", "Ok"); }
                        }
                        if (campo.UnidDoseSoluc != null)
                        {
                            try { worksheet1.Range["J" + i].Text = campo.UnidDoseSoluc; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'UnidDoseSoluc'", "Ok"); }
                        }
                        if (campo.MetodoAplic != null)
                        {
                            try { worksheet1.Range["K" + i].Text = campo.MetodoAplic; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'MetodoAplic'", "Ok"); }
                        }
                        if (campo.Equipamento != null)
                        {
                            try { worksheet1.Range["L" + i].Text = campo.Equipamento; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Equipamento'", "Ok"); }
                        }
                        if (campo.MarcaEquip != null)
                        {
                            try { worksheet1.Range["L" + i].Text = campo.MarcaEquip; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'MarcaEquip'", "Ok"); }
                        }
                        if (campo.ModeloEquip != null)
                        {
                            try { worksheet1.Range["L" + i].Text = campo.ModeloEquip; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'ModeloEquip'", "Ok"); }
                        }
                        if (campo.DataOper != null)
                        {
                            try { worksheet1.Range["M" + i].Value = campo.DataOper; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'DataOper'", "Ok"); }
                        }
                        try { worksheet1.Range["O" + i].Number = campo.NumHoras; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'NumHoras'", "Ok"); }
                        try { worksheet1.Range["P" + i].Number = campo.Rendimento; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Rendimento'", "Ok"); }
                        if (campo.EmpresaResp != null)
                        {
                            try { worksheet1.Range["Q" + i].Text = campo.EmpresaResp; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'EmpresaResp'", "Ok"); }
                        }
                        if (campo.ResponsavelOper != null)
                        {
                            try { worksheet1.Range["R" + i].Text = campo.ResponsavelOper; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'ResponsavelOper'", "Ok"); }
                        }
                        if (campo.Observacoes != null)
                        {
                            try { worksheet1.Range["S" + i].Text = campo.Observacoes; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Observacoes'", "Ok"); }
                        }
                    }
                }

                //Acrescenta a planilha de Controle de qualidade.
                IWorksheet worksheet2 = workbook.Worksheets[2];
                worksheet2.Name = "CapinasQuim_CQ";

                //Acrescenta os títulos das colulas
                worksheet2.Range["A1"].Text = "Código do cont. de qual. (PK)"; //int PKCapinasQuimCQ
                worksheet2.Range["B1"].Text = "Código da operação (PK)"; //int PKCapinasQuimOP
                worksheet2.Range["C1"].Text = "Código do cont. de qual."; //int CodCapQuimicasCQ
                worksheet2.Range["D1"].Text = "Código da operação"; //int CodTrCulturaisOP
                worksheet2.Range["E1"].Text = "Código do local (talhão)"; //int CodLocal
                worksheet2.Range["F1"].Text = "Repetição"; //int Repeticao
                worksheet2.Range["G1"].Text = "Área aplicada"; //float AreaAplic
                worksheet2.Range["H1"].Text = "Vol. saída 1"; //float VolBico1
                worksheet2.Range["I1"].Text = "Vol. saída 2"; //float VolBico2
                worksheet2.Range["J1"].Text = "Vol. saída 3"; //float VolBico3
                worksheet2.Range["K1"].Text = "Vol. saída 4"; //float VolBico4
                worksheet2.Range["L1"].Text = "Vol. saída 5"; //float VolBico5
                worksheet2.Range["M1"].Text = "Vol. saída 6"; //float VolBico6
                worksheet2.Range["N1"].Text = "Vol. saída 7"; //float VolBico7
                worksheet2.Range["O1"].Text = "Vol. saída 8"; //float VolBico8
                worksheet2.Range["P1"].Text = "Vol. saída 9"; //float VolBico9
                worksheet2.Range["Q1"].Text = "Vol. saída 10"; //float VolBico10
                worksheet2.Range["R1"].Text = "Vol. total"; //float VolTotal
                worksheet2.Range["S1"].Text = "Plantas avaliadas (n)"; //int NPtsAval
                worksheet2.Range["T1"].Text = "Vazão (média)"; //float VazaoMed
                worksheet2.Range["U1"].Text = "Vazão (desvio)"; //float VazaoDesv
                worksheet2.Range["V1"].Text = "Vazão inf. mín. (n)"; //int VazaoInfMin
                worksheet2.Range["W1"].Text = "Vazão inf. méd. (n)"; //int VazaoInfMed
                worksheet2.Range["X1"].Text = "Vazão sup. méd. (n)"; //int VazaoSupMed
                worksheet2.Range["Y1"].Text = "Vazão sup. máx. (n)"; //int VazaoSupMax
                worksheet2.Range["Z1"].Text = "Pressão (média)"; //float PressaoMed
                worksheet2.Range["AA1"].Text = "Pressão (desvio)"; //float PressaoDesv
                worksheet2.Range["AB1"].Text = "Pressão inf. mín. (n)"; //int PressaoInfMin
                worksheet2.Range["AC1"].Text = "Pressão inf. méd. (n)"; //int PressaoInfMed
                worksheet2.Range["AD1"].Text = "Pressão sup. méd. (n)"; //int PressaoSupMed
                worksheet2.Range["AE1"].Text = "Pressão sup. máx. (n)"; //int PressaoSupMax
                worksheet2.Range["AF1"].Text = "Inconform. cobertura (n)"; //int InconfCobert
                worksheet2.Range["AG1"].Text = "Inconform. deriva (n)"; //int InconfDeriva
                worksheet2.Range["AH1"].Text = "Avaliador"; //string Avaliador
                worksheet2.Range["AI1"].Text = "Data da avaliação"; //string DataAval
                worksheet2.Range["AJ1"].Text = "Observações"; //string Observacoes

                //Formata fonte da linha de títulos
                worksheet2.Range["A1:AJ1"].CellStyle.Font.Bold = true;
                worksheet2.Range["A1:AJ1"].CellStyle.Font.Italic = true;
                worksheet2.Range["A1:E1"].CellStyle.Font.RGBColor = Color.FromArgb(200, 50, 50);

                //Colore as células A1 a D1
                worksheet2.Range["A1:E1"].CellStyle.Color = Color.FromArgb(150, 150, 150);

                //Borda inferior da linha de títulos
                worksheet2.Range["A1:AJ1"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                worksheet2.Range["A1:AJ1"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].Color = ExcelKnownColors.Black;

                //Ajusta a largura das colunas
                worksheet2.Range["A1:AJ1"].AutofitColumns();

                sql = $"SELECT * FROM CapinasQuimCQ";
                try { conn.Query<CapinasQuimCQ>(sql); }
                catch { DisplayAlert("Message", "Não foi possível abrir a tabela CapinasQuimCQ.", "Ok"); }
                sql.First();

                var qryCapinasQuimCQ = conn.Query<CapinasQuimCQ>("SELECT * FROM CapinasQuimCQ");

                var ListaCapinasQuimCQ = conn.Table<CapinasQuimCQ>();

                if (ListaCapinasQuimCQ.Count() > 0)
                {
                    int i = 1;
                    foreach (var campo in ListaCapinasQuimCQ)
                    {
                        i = i + 1;
                        try { worksheet2.Range["A" + i].Number = campo.PKCapinasQuimCQ; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PKCapinasQuimCQ'", "Ok"); }
                        try { worksheet2.Range["B" + i].Number = campo.PKCapinasQuimOP; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PKCapinasQuimOP'", "Ok"); }
                        try { worksheet2.Range["C" + i].Number = campo.CodCapQuimicasCQ; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodCapQuimicasCQ'", "Ok"); }
                        try { worksheet2.Range["D" + i].Number = campo.CodTrCulturaisOP; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodTrCulturaisOP'", "Ok"); }
                        try { worksheet2.Range["E" + i].Number = campo.CodLocal; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodLocal'", "Ok"); }
                        try { worksheet2.Range["F" + i].Number = campo.Repeticao; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Repeticao'", "Ok"); }
                        try { worksheet2.Range["G" + i].Number = campo.AreaAplic; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'AreaAplic'", "Ok"); }
                        try { worksheet2.Range["H" + i].Number = campo.VolBico1; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'VolBico1'", "Ok"); }
                        try { worksheet2.Range["I" + i].Number = campo.VolBico2; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'VolBico2'", "Ok"); }
                        try { worksheet2.Range["J" + i].Number = campo.VolBico3; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'VolBico3'", "Ok"); }
                        try { worksheet2.Range["K" + i].Number = campo.VolBico4; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'VolBico4'", "Ok"); }
                        try { worksheet2.Range["L" + i].Number = campo.VolBico5; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'VolBico5'", "Ok"); }
                        try { worksheet2.Range["M" + i].Number = campo.VolBico6; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'VolBico6'", "Ok"); }
                        try { worksheet2.Range["N" + i].Number = campo.VolBico7; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'VolBico7'", "Ok"); }
                        try { worksheet2.Range["O" + i].Number = campo.VolBico8; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'VolBico8'", "Ok"); }
                        try { worksheet2.Range["P" + i].Number = campo.VolBico9; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'VolBico9'", "Ok"); }
                        try { worksheet2.Range["Q" + i].Number = campo.VolBico10; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'VolBico10'", "Ok"); }
                        try { worksheet2.Range["R" + i].Number = campo.VolTotal; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'VolTotal'", "Ok"); }
                        try { worksheet2.Range["S" + i].Number = campo.NPtsAval; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'NPtsAval'", "Ok"); }
                        try { worksheet2.Range["T" + i].Number = campo.VazaoMed; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'VazaoMed'", "Ok"); }
                        try { worksheet2.Range["U" + i].Number = campo.VazaoDesv; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'VazaoDesv'", "Ok"); }
                        try { worksheet2.Range["V" + i].Number = campo.VazaoInfMin; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'VazaoInfMin'", "Ok"); }
                        try { worksheet2.Range["W" + i].Number = campo.VazaoInfMed; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'VazaoInfMed'", "Ok"); }
                        try { worksheet2.Range["X" + i].Number = campo.VazaoSupMed; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'VazaoSupMed'", "Ok"); }
                        try { worksheet2.Range["Y" + i].Number = campo.VazaoSupMax; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'VazaoSupMax'", "Ok"); }
                        try { worksheet2.Range["Z" + i].Number = campo.PressaoMed; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PressaoMed'", "Ok"); }
                        try { worksheet2.Range["AA" + i].Number = campo.PressaoDesv; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PressaoDesv'", "Ok"); }
                        try { worksheet2.Range["AB" + i].Number = campo.PressaoInfMin; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PressaoInfMin'", "Ok"); }
                        try { worksheet2.Range["AC" + i].Number = campo.PressaoInfMed; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PressaoInfMed'", "Ok"); }
                        try { worksheet2.Range["AD" + i].Number = campo.PressaoSupMed; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PressaoSupMed'", "Ok"); }
                        try { worksheet2.Range["AE" + i].Number = campo.PressaoSupMax; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PressaoSupMax'", "Ok"); }
                        try { worksheet2.Range["AF" + i].Number = campo.InconfCobert; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'InconfCobert'", "Ok"); }
                        try { worksheet2.Range["AG" + i].Number = campo.InconfDeriva; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'InconfDeriva'", "Ok"); }
                        if (campo.Avaliador != null)
                        {
                            try { worksheet2.Range["AH" + i].Text = campo.Avaliador; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Avaliador'", "Ok"); }
                        }
                        if (campo.DataAval != null)
                        {
                            try { worksheet1.Range["AI" + i].Value = campo.DataAval; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'DataAval'", "Ok"); }
                        }
                        if (campo.Observacoes != null)
                        {
                            try { worksheet2.Range["AJ" + i].Text = campo.Observacoes; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Observacoes'", "Ok"); }
                        }
                    }
                }

                //Save the workbook to stream in xlsx format. 
                MemoryStream stream = new MemoryStream();
                workbook.SaveAs(stream);

                string data = DateTime.Now.ToString(("yyyy-MM-dd hh:mm:ss"));

                workbook.Close();

                //Save the stream as a file in the device and invoke it for viewing
                Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("CapinasQuim_" + data + ".xlsx", "application/msexcel", stream);
            }
        }

        private void Informacoes_Clicked(object sender, EventArgs e)
        {
            msg = $"Esta tela exibe a lista de operações de capinas químicas realizadas no talhão e peprmite a edição.\n\n" +
                $"Os comandos são os seguintes:\n\nNOVA CAPINA QUÍMICA: incluir novo registro por meio de digitação direta no SIGNUS;\n\n" +
                $"A partir do menu (três pontinhos) no cato superior direito da tela, há a opção de exportar " +
                $"os dados de capinas químicas para uma planilha Excel. O arquivo gerado conterá os dados dos talhões" +
                $"para os quais há operações de capinas químicas cadastradas, os dados das operações e das avaliações de " +
                $"controle de qualidade realizadas. O arquivo é disponibilizado na pasta Signus com o nome iniciado por " +
                $"'CapinasQuim_' seguido da data e hora.\n\n" +
                $"Um toque rápido sobre um registro de operação de capina química expande exibe os detalhes do mesmo;\n\n" +
                $"Para editar um registro já existente, pressione-o até aparecerem as opções na parte superior da tela.\n\n" +
                $"Para editar as avaliações de controle de qualidade, pressione o registro até aparecer o menu (três pontinhos) " +
                $"no canto superior direito da tela. A partir deste menu, é possível abrir o formulário para cadastro dessas avaliações.\n\n" +
                $"A seta localizada no canto superior esquerdo da tela permite retornar à tela anterior.";
            DisplayAlert("SIGNUS", msg, "Ok");
        }

    }
}