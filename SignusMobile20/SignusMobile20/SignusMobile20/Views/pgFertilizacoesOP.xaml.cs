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
	public partial class pgFertilizacoesOP : ContentPage
	{
        private SQLiteConnection conn;
        public FertilizacoesOP plantioop;
        public FertilizacoesCQ plantiocq;
        public string pktlh;
        string msg;

        public pgFertilizacoesOP (string pktalhoescr)
		{
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTU0OTAzQDMxMzkyZTM0MmUzMGhGOWRUZEN4NDBVSGx2OFFub2E4QjAySERLTHFvZmxYODUzUlU0d1hzUm89");
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTM3Njg3QDMxMzcyZTMyMmUzMFhoRVRPYmZrZzBsNHFocEc1SVNrWEE2Q3kvQlFOR09ocEw2K1A3MHNrZzQ9");

            InitializeComponent();
            conn = DependencyService.Get<ISQLite>().GetConnection();
            //conn.DropTable<FertilizacoesOP>();
            //conn.DropTable<FertilizacoesCQ>();
            conn.CreateTable<FertilizacoesOP>();
            conn.CreateTable<FertilizacoesCQ>();

            pktlh = pktalhoescr;
            PKTalhoesCR.Text = "Código do talhão: " + pktlh;

        }

        protected override void OnAppearing()
        {
            int num = Convert.ToInt32(pktlh);

            //PopulateListaFertilizacoesOP();
            var data = conn.Table<FertilizacoesOP>().Where(x => x.PKTalhoesCR == num).ToList();
            lstFertilizacoesOP.ItemsSource = data;
        }

        private void Mostrar_Clicked(object sender, EventArgs e)
        {
            var data = (from plantio in conn.Table<FertilizacoesOP>() select plantio);
            lstFertilizacoesOP.ItemsSource = data;
        }

        private void NovoFertilizacoesOP_Clicked(object sender, EventArgs e)
        {
//            Navigation.PushAsync(new pgAddFertilizacoesOP(null, pktlh));
        }

        private void EditFertilizacoesOP(object sender, ItemTappedEventArgs e)
        {
            FertilizacoesOP details = e.Item as FertilizacoesOP;
            if (details != null)
            {
//                Navigation.PushAsync(new pgAddFertilizacoesOP(details, pktlh));
            }
        }

        public async void AlternaDetalhes(object sender, ItemTappedEventArgs e)
        {
            //Alterna entre visível e invisível a lista de detalhes
            FertilizacoesOP details = e.Item as FertilizacoesOP;
            string sql = "";

            if (details.Visible.ToString() == "True")
            {
                sql = $"UPDATE FertilizacoesOP SET Visible = 0, Imagem = 'Down.png' ";
            }
            else
            {
                sql = $"UPDATE FertilizacoesOP SET Visible = 1, Imagem = 'Up.png' ";
            }

            sql += $"WHERE PKFertilizacoesOP = {details.PKFertilizacoesOP}";
            try { conn.Execute(sql); }
            catch { await DisplayAlert("Message", "Houve um erro. Não foi possível excluir o registro.", "Ok"); }

            sql = $"SELECT * FROM FertilizacoesOP WHERE PKTalhoesCR = {pktlh}";

            //Atualiza a lista
            var data = conn.Query<FertilizacoesOP>(sql);
            lstFertilizacoesOP.ItemsSource = data;
        }

        private async void DeleteFertilizacoesOP(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var operacao = mi.CommandParameter as FertilizacoesOP;
            if (operacao != null)
            {
                var opcao = await DisplayAlert("Excluir operação", "Confirma a exclusão da operação de fertilização de código " + operacao.PKFertilizacoesOP + "" +
                    " e de todos os outros (de controle de qualidade) associados? ", "Ok", "Cancel");
                if (opcao)
                {
                    //Deleta todos os registros de controle de qualidade associados à operação atual
                    string sql = $"DELETE FROM FertilizacoesCQ WHERE PKFertilizacoesOP = {operacao.PKFertilizacoesOP}";
                    try { conn.Execute(sql); }
                    catch
                    {
                        await DisplayAlert("Message", "Houve um erro, não foi possível excluir os registros de controle de qualidade associados a este registro de operação de fertilização. " +
                            "Possivelmente, a tabela de controle de qualidade ainda não foi criada.", "Ok");
                    }

                    //Deleta o registro de operação
                    sql = $"DELETE FROM FertilizacoesOP WHERE PKFertilizacoesOP = {operacao.PKFertilizacoesOP}";

                    try { conn.Execute(sql); }
                    catch { await DisplayAlert("Message", "Houve um erro, não foi possível excluir o registro de operação de fertilização.", "Ok"); }

                    sql = $"SELECT * FROM FertilizacoesOP WHERE PKTalhoesCR = {pktlh}";
                    conn.Query<FertilizacoesOP>(sql);

                    //Atualiza a lista
                    var data = conn.Query<FertilizacoesOP>(sql);
                    lstFertilizacoesOP.ItemsSource = data;
                }
            }
        }

        private async void AlterarFertilizacoesOP(object sender, EventArgs e)
        {
            try
            {
                var mi = ((MenuItem)sender);
                var operacao = mi.CommandParameter as FertilizacoesOP;
                if (operacao != null)
                {
//                    await Navigation.PushAsync(new pgAddFertilizacoesOP(operacao, pktlh));
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
            var operacao = mi.CommandParameter as FertilizacoesOP;
            if (operacao != null)
            {
                //var opcao = await DisplayAlert("Avaliar qualidade", "Abrir controles de qualidade da operação " + operacao.PKFertilizacoesOP.ToString() + "? ", "Ok", "Cancel");
                //if (opcao)
                //{
                string sql = $"SELECT * FROM FertilizacoesCQ WHERE PKFertilizacoesOP = {operacao.PKFertilizacoesOP.ToString()}";

                try { conn.Query<FertilizacoesCQ>(sql); }
                catch { await DisplayAlert("Message", "Houve um erro. Não foi possível abrir a consulta.", "Ok"); }

                //Atualiza a página
//                await Navigation.PushAsync(new pgFertilizacoesCQ(operacao.PKTalhoesCR.ToString(), operacao.PKFertilizacoesOP.ToString()));
                //}
            }
        }

        public void ExportarFertilizacoes_Clicked(object sender, EventArgs e)
        {

            DisplayAlert("Informação", message: "Este comando exporta todos os dados das operações de fertilizações e os respectivos controles de qualidade.", "Ok");

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

                var qryFertilizacoesOP = conn.Query<FertilizacoesOP>("SELECT PKTalhoesCR FROM FertilizacoesOP GROUP BY PKTalhoesCR ORDER BY PKTalhoesCR");
                if (qryFertilizacoesOP.Count() > 0)
                {
                    qryFertilizacoesOP.First();
                    int i = 1;
                    foreach (var campoam in qryFertilizacoesOP)
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
                                    try { worksheet.Range["L" + i].Number = double.Parse(campotalh.EspEL.ToString("#.00")); }
                                    catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'EspEL'", "Ok"); }
                                    try { worksheet.Range["M" + i].Number = double.Parse(campotalh.EspEP.ToString("#.00")); }
                                    catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'EspEP'", "Ok"); }
                                    if (campotalh.DataIniRot != null)
                                    {
                                        try { worksheet.Range["N" + i].Value = campotalh.DataIniRot; }
                                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'DataIniRot'", "Ok"); }
                                    }
                                    try { worksheet.Range["O" + i].Number = double.Parse(campotalh.AreaTalh.ToString("#.00")); }
                                    catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'AreaTalh'", "Ok"); }
                                    try { worksheet.Range["P" + i].Number = double.Parse(campotalh.AreaSubtalh.ToString("#.00")); }
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
                                    try { worksheet.Range["X" + i].Number = double.Parse(campotalh.LatitudeLocS.ToString("#.00")); }
                                    catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'LatitudeLocS'", "Ok"); }
                                    try { worksheet.Range["Y" + i].Number = campotalh.LongitudeLocG; }
                                    catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'LongitudeLocG'", "Ok"); }
                                    try { worksheet.Range["Z" + i].Number = campotalh.LongitudeLocM; }
                                    catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'LongitudeLocM'", "Ok"); }
                                    try { worksheet.Range["AA" + i].Number = double.Parse(campotalh.LongitudeLocS.ToString("#.00")); }
                                    catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'LongitudeLocS'", "Ok"); }
                                    try { worksheet.Range["AB" + i].Number = double.Parse(campotalh.UTMN.ToString("#.00")); }
                                    catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'UTMN'", "Ok"); }
                                    try { worksheet.Range["AC" + i].Number = double.Parse(campotalh.UTME.ToString("#.00")); }
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

                //Acrescenta a planilha de amostragem.
                IWorksheet worksheet1 = workbook.Worksheets[1];
                worksheet1.Name = "Fertilizacoes_OP";

                //Enter values to the cells from A3 to A5
                worksheet1.Range["A1"].Text = "Código da operação (PK)";//int PKFertilizacoesOP
                worksheet1.Range["B1"].Text = "Código do talhão (PK)";//int PKTalhoesCR
                worksheet1.Range["C1"].Text = "Código da fertilização";//int CodFertilizacoesOP
                worksheet1.Range["D1"].Text = "Código do local (talhão)";//int CodLocal
                worksheet1.Range["E1"].Text = "Nome da operação";//string IdentifOperacao
                worksheet1.Range["F1"].Text = "Tipo de adubação";//string TipoAdubacao
                worksheet1.Range["G1"].Text = "Proced. operacional";//int ProcedOperac
                worksheet1.Range["H1"].Text = "Código do fertilizante";//int CodFertilizante
                worksheet1.Range["I1"].Text = "Dose do fertilizante";//float DoseFertilizante
                worksheet1.Range["J1"].Text = "Unidade dose";//string UnidDose
                worksheet1.Range["K1"].Text = "Distância da planta";//float DistanciaPlt
                worksheet1.Range["L1"].Text = "Profundidade";//float Profundidade
                worksheet1.Range["M1"].Text = "Época de aplicação";//float EpocaAplicFert
                worksheet1.Range["N1"].Text = "Data da aplicação";//string DataAplicacao
                worksheet1.Range["O1"].Text = "Método de aplicação";//string MetodoAplic
                worksheet1.Range["P1"].Text = "Equipamento";//string Equipamento
                worksheet1.Range["Q1"].Text = "Núm. de horas";//float NumHoras
                worksheet1.Range["R1"].Text = "Rendimento";//float Rendimento
                worksheet1.Range["S1"].Text = "Rmpresa responsável";//string EmpresaResp
                worksheet1.Range["T1"].Text = "Respons. pela operação";//string ResponsavelOper
                worksheet1.Range["U1"].Text = "Observações";//string Observacoes
                //Formata fonte da linha de títulos
                worksheet1.Range["A1:U1"].CellStyle.Font.Bold = true;
                worksheet1.Range["A1:U1"].CellStyle.Font.Italic = true;
                worksheet1.Range["A1:D1"].CellStyle.Font.RGBColor = Color.FromArgb(200, 50, 50);

                //Colore as células A1 a D1
                worksheet1.Range["A1:D1"].CellStyle.Color = Color.FromArgb(150, 150, 150);

                //Borda inferior da linha de títulos
                worksheet1.Range["A1:U1"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                worksheet1.Range["A1:U1"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].Color = ExcelKnownColors.Black;

                //Ajusta a largura das colunas
                worksheet1.Range["A1:U1"].AutofitColumns();

                sql = $"SELECT * FROM FertilizacoesOP";
                try { conn.Query<FertilizacoesOP>(sql); }
                catch { DisplayAlert("Message", "Não foi possível abrir a tabela FertilizacoesOP.", "Ok"); }
                sql.First();

                var qryFertilizacoes = conn.Query<FertilizacoesOP>("SELECT * FROM FertilizacoesOP");

                var ListaFertilizacoes = conn.Table<FertilizacoesOP>();

                if (ListaFertilizacoes.Count() > 0)
                {
                    int i = 1;
                    foreach (var campo in ListaFertilizacoes)
                    {
                        i = i + 1;

                        try { worksheet1.Range["A" + i].Number = campo.PKFertilizacoesOP; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PKFertilizacoesOP'", "Ok"); }
                        try { worksheet1.Range["B" + i].Number = campo.PKTalhoesCR; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PKTalhoesCR'", "Ok"); }
                        try { worksheet1.Range["C" + i].Number = campo.CodFertilizacoesOP; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodFertilizacoesOP'", "Ok"); }
                        try { worksheet1.Range["D" + i].Number = campo.CodLocal; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodLocal'", "Ok"); }
                        if (campo.IdentifOperacao != null)
                        {
                            try { worksheet1.Range["E" + i].Text = campo.IdentifOperacao; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'IdentifOperacao'", "Ok"); }
                        }
                        if (campo.TipoAdubacao != null)
                        {
                            try { worksheet1.Range["F" + i].Text = campo.TipoAdubacao; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'TipoAdubacao'", "Ok"); }
                        }
                        try { worksheet1.Range["G" + i].Number = campo.ProcedOperac; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'ProcedOperac'", "Ok"); }
                        try { worksheet1.Range["H" + i].Number = campo.CodFertilizante; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodFertilizante'", "Ok"); }
                        try { worksheet1.Range["I" + i].Number = double.Parse(campo.DoseFertilizante.ToString("#.00")); }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'DoseFertilizante'", "Ok"); }
                        if (campo.UnidDose != null)
                        {
                            try { worksheet1.Range["J" + i].Text = campo.UnidDose; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'UnidDose'", "Ok"); }
                        }
                        try { worksheet1.Range["K" + i].Number = double.Parse(campo.DistanciaPlt.ToString("#.00")); }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'DistanciaPlt'", "Ok"); }
                        try { worksheet1.Range["L" + i].Number = double.Parse(campo.Profundidade.ToString("#.00")); }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Profundidade'", "Ok"); }
                        try { worksheet1.Range["M" + i].Number = double.Parse(campo.EpocaAplicFert.ToString("#.00")); }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'EpocaAplicFert'", "Ok"); }
                        if (campo.DataAplicacao != null)
                        {
                            try { worksheet1.Range["N" + i].Value = campo.DataAplicacao; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'DataAplicacao'", "Ok"); }
                        }
                        if (campo.MetodoAplic != null)
                        {
                            try { worksheet1.Range["O" + i].Text = campo.MetodoAplic; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'MetodoAplic'", "Ok"); }
                        }
                        if (campo.Equipamento != null)
                        {
                            try { worksheet1.Range["P" + i].Text = campo.Equipamento; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Equipamento'", "Ok"); }
                        }
                        try { worksheet1.Range["Q" + i].Number = double.Parse(campo.NumHoras.ToString("#.00")); }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'NumHoras'", "Ok"); }
                        try { worksheet1.Range["R" + i].Number = double.Parse(campo.Rendimento.ToString("#.00")); }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Rendimento'", "Ok"); }
                        if (campo.EmpresaResp != null)
                        {
                            try { worksheet1.Range["S" + i].Text = campo.EmpresaResp; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'EmpresaResp'", "Ok"); }
                        }
                        if (campo.ResponsavelOper != null)
                        {
                            try { worksheet1.Range["T" + i].Text = campo.ResponsavelOper; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'ResponsavelOper'", "Ok"); }
                        }
                        if (campo.Observacoes != null)
                        {
                            try { worksheet1.Range["U" + i].Text = campo.Observacoes; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Observacoes'", "Ok"); }
                        }
                    }
                }

                //Acrescenta a planilha de Fertilizacões_CQ.
                IWorksheet worksheet2 = workbook.Worksheets[2];
                worksheet2.Name = "Fertilizacoes_CQ";

                worksheet2.Range["A1"].Text = "Código do cont. de qual. (PK)"; // int PKFertilizacoesCQ
                worksheet2.Range["B1"].Text = "Código da operação (PK)"; // int PKFertilizacoesOP
                worksheet2.Range["C1"].Text = "Código do cont. de qual."; // int CodFertilizacoesCQ
                worksheet2.Range["D1"].Text = "Código da operação"; // int CodFertilizacoesOP
                worksheet2.Range["E1"].Text = "Código do local"; // int CodLocal
                worksheet2.Range["F1"].Text = "Repetição"; // int Repeticao
                worksheet2.Range["G1"].Text = "Plantas avaliadas (n)"; // int NumPtasAvaliadas
                worksheet2.Range["H1"].Text = "Plantas não adubadas (n)"; // int PtasNaoAdubadas
                worksheet2.Range["I1"].Text = "Dist. da planta (média)"; // float DistPlantaMed
                worksheet2.Range["J1"].Text = "Dist. da planta (DP)"; // float DistPlantaDesv
                worksheet2.Range["K1"].Text = "Profundidade (média)"; // float ProfundMed
                worksheet2.Range["L1"].Text = "Profundidade (DP)"; // float ProfundDesv
                worksheet2.Range["M1"].Text = "Dist. da planta inf. mín. (n)"; // int DistPlantaInfMin
                worksheet2.Range["N1"].Text = "Dist. da planta inf. méd. (n)"; // int DistPlantaInfMed
                worksheet2.Range["O1"].Text = "Dist. da planta sup. méd. (n)"; // int DistPlantaSupMed
                worksheet2.Range["P1"].Text = "Dist. da planta sup. máx. (n)"; // int DistPlantaSupMax
                worksheet2.Range["Q1"].Text = "Profundidade inf. mín. (n)"; // int ProfundInfMin
                worksheet2.Range["R1"].Text = "Profundidade inf. méd. (n)"; // int ProfundInfMed
                worksheet2.Range["S1"].Text = "Profundidade sup. méd. (n)"; // int ProfundSupMed
                worksheet2.Range["T1"].Text = "Profundidade sup. máx. (n)"; // int ProfundSupMax
                worksheet2.Range["U1"].Text = "Distância percorrida (m)"; // float DistPercEquip
                worksheet2.Range["V1"].Text = "Total saída 1"; // float Saida1
                worksheet2.Range["W1"].Text = "Total saída 2"; // float Saida2
                worksheet2.Range["X1"].Text = "Total saída 3"; // float Saida3
                worksheet2.Range["Y1"].Text = "Total saída 4"; // float Saida4
                worksheet2.Range["Z1"].Text = "Total saída 5"; // float Saida5
                worksheet2.Range["AA1"].Text = "Total saídas"; // float TotalSaidas
                worksheet2.Range["AB1"].Text = "Fora da cova (n)"; // int ForaDaCova
                worksheet2.Range["AC1"].Text = "Data da avaliação"; // string DataAval
                worksheet2.Range["AD1"].Text = "Respons. pela avaliação"; // string ResponsavelAval
                worksheet2.Range["AE1"].Text = "Observações"; // string Observacoes

                //Formata fonte da linha de títulos
                worksheet2.Range["A1:AE1"].CellStyle.Font.Bold = true;
                worksheet2.Range["A1:AE1"].CellStyle.Font.Italic = true;
                worksheet2.Range["A1:E1"].CellStyle.Font.RGBColor = Color.FromArgb(200, 50, 50);

                //Colore as células A1 a D1
                worksheet2.Range["A1:E1"].CellStyle.Color = Color.FromArgb(150, 150, 150);

                //Borda inferior da linha de títulos
                worksheet2.Range["A1:AE1"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                worksheet2.Range["A1:AE1"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].Color = ExcelKnownColors.Black;

                //Ajusta a largura das colunas
                worksheet2.Range["A1:AE1"].AutofitColumns();

                sql = $"SELECT * FROM FertilizacoesCQ";
                try { conn.Query<FertilizacoesCQ>(sql); }
                catch { DisplayAlert("Message", "Não foi possível abrir a tabela FertilizacoesCQ.", "Ok"); }
                sql.First();

                var qryFertilizacoesCQ = conn.Query<FertilizacoesCQ>("SELECT * FROM FertilizacoesCQ");

                var ListaFertilizacoesCQ = conn.Table<FertilizacoesCQ>();

                if (ListaFertilizacoesCQ.Count() > 0)
                {
                    int i = 1;
                    foreach (var campo in ListaFertilizacoesCQ)
                    {
                        i = i + 1;
                        try { worksheet2.Range["A" + i].Number = campo.PKFertilizacoesCQ; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PKFertilizacoesCQ'", "Ok"); }
                        try { worksheet2.Range["B" + i].Number = campo.PKFertilizacoesOP; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PKFertilizacoesOP'", "Ok"); }
                        try { worksheet2.Range["C" + i].Number = campo.CodFertilizacoesCQ; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodFertilizacoesCQ'", "Ok"); }
                        try { worksheet2.Range["D" + i].Number = campo.CodFertilizacoesOP; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodFertilizacoesOP'", "Ok"); }
                        try { worksheet2.Range["E" + i].Number = campo.CodLocal; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodLocal'", "Ok"); }
                        try { worksheet2.Range["F" + i].Number = campo.Repeticao; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Repeticao'", "Ok"); }
                        try { worksheet2.Range["G" + i].Number = campo.NumPtasAvaliadas; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'NumPtasAvaliadas'", "Ok"); }
                        try { worksheet2.Range["H" + i].Number = campo.PtasNaoAdubadas; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PtasNaoAdubadas'", "Ok"); }
                        try { worksheet2.Range["I" + i].Number = double.Parse(campo.DistPlantaMed.ToString("#.00")); }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'DistPlantaMed'", "Ok"); }
                        try { worksheet2.Range["J" + i].Number = double.Parse(campo.DistPlantaDesv.ToString("#.00")); }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'DistPlantaDesv'", "Ok"); }
                        try { worksheet2.Range["K" + i].Number = double.Parse(campo.ProfundMed.ToString("#.00")); }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'ProfundMed'", "Ok"); }
                        try { worksheet2.Range["L" + i].Number = double.Parse(campo.ProfundDesv.ToString("#.00")); }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'ProfundDesv'", "Ok"); }
                        try { worksheet2.Range["M" + i].Number = campo.DistPlantaInfMin; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'DistPlantaInfMin'", "Ok"); }
                        try { worksheet2.Range["N" + i].Number = campo.DistPlantaInfMed; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'DistPlantaInfMed'", "Ok"); }
                        try { worksheet2.Range["O" + i].Number = campo.DistPlantaSupMed; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'DistPlantaSupMed'", "Ok"); }
                        try { worksheet2.Range["P" + i].Number = campo.DistPlantaSupMax; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'DistPlantaSupMax'", "Ok"); }
                        try { worksheet2.Range["Q" + i].Number = campo.ProfundInfMin; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'ProfundInfMin'", "Ok"); }
                        try { worksheet2.Range["R" + i].Number = campo.ProfundInfMed; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'ProfundInfMed'", "Ok"); }
                        try { worksheet2.Range["S" + i].Number = campo.ProfundSupMed; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'ProfundSupMed'", "Ok"); }
                        try { worksheet2.Range["T" + i].Number = campo.ProfundSupMax; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'ProfundSupMax'", "Ok"); }
                        try { worksheet2.Range["U" + i].Number = double.Parse(campo.DistPercEquip.ToString("#.00")); }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'DistPercEquip'", "Ok"); }
                        try { worksheet2.Range["V" + i].Number = double.Parse(campo.Saida1.ToString("#.00")); }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Saida1'", "Ok"); }
                        try { worksheet2.Range["W" + i].Number = double.Parse(campo.Saida2.ToString("#.00")); }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Saida2'", "Ok"); }
                        try { worksheet2.Range["X" + i].Number = double.Parse(campo.Saida3.ToString("#.00")); }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Saida3'", "Ok"); }
                        try { worksheet2.Range["Y" + i].Number = double.Parse(campo.Saida4.ToString("#.00")); }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Saida4'", "Ok"); }
                        try { worksheet2.Range["Z" + i].Number = double.Parse(campo.Saida5.ToString("#.00")); }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Saida5'", "Ok"); }
                        try { worksheet2.Range["AA" + i].Number = double.Parse(campo.TotalSaidas.ToString("#.00")); }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'TotalSaidas'", "Ok"); }
                        try { worksheet2.Range["AB" + i].Number = campo.ForaDaCova; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'ForaDaCova'", "Ok"); }
                        if (campo.DataAval != null)
                        {
                            try { worksheet1.Range["AC" + i].Value = campo.DataAval; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'DataAval'", "Ok"); }
                        }
                        if (campo.ResponsavelAval != null)
                        {
                            try { worksheet2.Range["AD" + i].Text = campo.ResponsavelAval; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'ResponsavelAval'", "Ok"); }
                        }
                        if (campo.Observacoes != null)
                        {
                            try { worksheet2.Range["AE" + i].Text = campo.Observacoes; }
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
                Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("Fertilizacoes_" + data + ".xlsx", "application/msexcel", stream);
            }

        }

        private void Informacoes_Clicked(object sender, EventArgs e)
        {
            msg = $"Esta tela exibe a lista de operações de fertilizações realizadas no talhão e peprmite a edição.\n\n" +
                $"Os comandos são os seguintes:\n\nNOVA FERTILIZAÇÃO: incluir novo registro por meio de digitação direta no SIGNUS;\n\n" +
                $"A partir do menu (três pontinhos) no cato superior direito da tela, há a opção de exportar " +
                $"os dados das fertilizações para uma planilha Excel. O arquivo gerado conterá os dados dos talhões" +
                $"para os quais há fertilizações cadastradas, os dados das operações e das avaliações de " +
                $"controle de qualidade realizadas. O arquivo é disponibilizado na pasta Signus com o nome iniciado por " +
                $"'Fertilizacoes_' seguido da data e hora.\n\n" +
                $"Um toque rápido sobre um registro de fertilização expande exibe os detalhes do mesmo;\n\n" +
                $"Para editar um registro já existente, pressione-o até aparecerem as opções na parte superior " +
                $"da tela.\n\n" +
                $"Para editar as avaliações de controle de qualidade, pressione o registro até aparecer o menu (três pontinhos) " +
                $"no canto superior direito da tela. A partir deste menu, é possível abrir o formulário para cadastro dessas avaliações.\n\n" +
                $"A seta localizada no canto superior esquerdo da tela permite retornar à tela anterior.";
            DisplayAlert("SIGNUS", msg, "Ok");

        }

    }
}