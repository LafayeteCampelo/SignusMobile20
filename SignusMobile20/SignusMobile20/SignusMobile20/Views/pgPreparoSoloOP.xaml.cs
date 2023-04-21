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
	public partial class pgPreparoSoloOP : ContentPage
	{
        private SQLiteConnection conn;//Retornar este código caso dê algo errado
        public PrepSoloOP prepsoloop;
        public PrepSoloCQ prepsolocq;
        public string pktlh;
        string msg;

        public pgPreparoSoloOP (string pktalhoescr)
		{
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTU0OTAzQDMxMzkyZTM0MmUzMGhGOWRUZEN4NDBVSGx2OFFub2E4QjAySERLTHFvZmxYODUzUlU0d1hzUm89");
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTM3Njg3QDMxMzcyZTMyMmUzMFhoRVRPYmZrZzBsNHFocEc1SVNrWEE2Q3kvQlFOR09ocEw2K1A3MHNrZzQ9");

            InitializeComponent();
            conn = DependencyService.Get<ISQLite>().GetConnection();
            //conn.DropTable<PrepSoloOP>();
            //conn.DropTable<PrepSoloCQ>();
            conn.CreateTable<PrepSoloOP>();
            conn.CreateTable<PrepSoloCQ>();

            pktlh = pktalhoescr;
            PKTalhoesCR.Text = "Código do talhão: " + pktlh;

        }

        protected override void OnAppearing()
        {
            int num = Convert.ToInt32(pktlh);

            var data = conn.Table<PrepSoloOP>().Where(x => x.PKTalhoesCR == num).ToList();
            lstPrepSoloOP.ItemsSource = data;
        }

        private void Mostrar_Clicked(object sender, EventArgs e)
        {
            var data = (from preps in conn.Table<PrepSoloOP>() select preps);
            lstPrepSoloOP.ItemsSource = data;
        }

        private void NovoPrepSoloOP_Clicked(object sender, EventArgs e)
        {
//            Navigation.PushAsync(new pgAddPreparoSoloOP(null, pktlh));
        }

        private void EditPreparoSoloOP(object sender, ItemTappedEventArgs e)
        {
            PrepSoloOP details = e.Item as PrepSoloOP;
            if (details != null)
            {
//                Navigation.PushAsync(new pgAddPreparoSoloOP(details, pktlh));
            }
        }

        public async void AlternaDetalhes(object sender, ItemTappedEventArgs e)
        {
            //Alterna entre visível e invisível a lista de detalhes
            PrepSoloOP details = e.Item as PrepSoloOP;
            string sql = "";

            if (details.Visible.ToString() == "True")
            {
                sql = $"UPDATE PrepSoloOP SET Visible = 0, Imagem = 'Down.png' ";
            }
            else
            {
                sql = $"UPDATE PrepSoloOP SET Visible = 1, Imagem = 'Up.png' ";
            }

            sql += $"WHERE PKPrepSoloOP = {details.PKPrepSoloOP.ToString()}";
            try { conn.Execute(sql); }
            catch { await DisplayAlert("Message", "Houve um erro. Não foi possível atualizar.", "Ok"); }

            sql = $"SELECT * FROM PrepSoloOP WHERE PKTalhoesCR = {pktlh.ToString()}";

            //Atualiza a lista
            var data = conn.Query<PrepSoloOP>(sql);
            lstPrepSoloOP.ItemsSource = data;
        }

        private async void DeletePreparoSoloOP(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var operacao = mi.CommandParameter as PrepSoloOP;
            if (operacao != null)
            {
                var opcao = await DisplayAlert("Excluir operação", "Confirma a exclusão da operação de preparo de solo de código " + operacao.PKPrepSoloOP + "" +
                    " e de todos os outros (de controle de qualidade) associados? ", "Ok", "Cancel");
                if (opcao)
                {
                    //Deleta todos os registros de controle de qualidade associados à operação atual
                    string sql = $"DELETE FROM PreparoSoloCQ WHERE PKPrepSoloOP = {operacao.PKPrepSoloOP}";
                    try { conn.Execute(sql); }
                    catch
                    {
                        await DisplayAlert("Message", "Houve um erro, não foi possível excluir os registros de controle de qualidade associados a este registro de operação de preparo de solo. " +
                            "Possivelmente, a tabela de controle de qualidade ainda não foi criada.", "Ok");
                    }

                    //Deleta o registro de operação
                    sql = $"DELETE FROM PrepSoloOP WHERE PKPrepSoloOP = {operacao.PKPrepSoloOP}";

                    try { conn.Execute(sql); }
                    catch { await DisplayAlert("Message", "Houve um erro, não foi possível excluir o registro de operação de prepapro de solo.", "Ok"); }

                    sql = $"SELECT * FROM PrepSoloOP WHERE PKTalhoesCR = {pktlh}";
                    conn.Query<PrepSoloOP>(sql);

                    //Atualiza a lista
                    var data = conn.Query<PrepSoloOP>(sql);
                    lstPrepSoloOP.ItemsSource = data;
                }
            }
        }

        private async void AlterarPreparoSoloOP(object sender, EventArgs e)
        {
            try
            {
                var mi = ((MenuItem)sender);
                var operacao = mi.CommandParameter as PrepSoloOP;
                if (operacao != null)
                {
//                    await Navigation.PushAsync(new pgAddPreparoSoloOP(operacao, pktlh));
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
            var operacao = mi.CommandParameter as PrepSoloOP;
            if (operacao != null)
            {
                //var opcao = await DisplayAlert("Avaliar qualidade", "Abrir controles de qualidade da operação " + operacao.PKPrepSoloOP.ToString() + "? ", "Ok", "Cancel");
                //if (opcao)
                //{
                string sql = $"SELECT * FROM PrepSoloCQ WHERE PKPrepSoloOP = {operacao.PKPrepSoloOP.ToString()}";

                try { conn.Query<PrepSoloCQ>(sql); }
                catch { await DisplayAlert("Message", "Houve um erro. Não foi possível abrir a consulta.", "Ok"); }

                //Atualiza a página
//                await Navigation.PushAsync(new pgPreparoSoloCQ(operacao.PKTalhoesCR.ToString(), operacao.PKPrepSoloOP.ToString()));
                //}
            }
        }

        public void ExportarPrepSolo_Clicked(object sender, EventArgs e)
        {

            DisplayAlert("Informação", message: "Este comando exporta todos os dados das operações de preparo de solo e os respectivos controles de qualidade.", "Ok");

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

                var qryPrepSoloOP = conn.Query<PrepSoloOP>("SELECT PKTalhoesCR FROM PrepSoloOP GROUP BY PKTalhoesCR ORDER BY PKTalhoesCR");
                if (qryPrepSoloOP.Count() > 0)
                {
                    qryPrepSoloOP.First();
                    int i = 1;
                    foreach (var campoam in qryPrepSoloOP)
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

                //Acrescenta a planilha de operações de prepado de solo.
                IWorksheet worksheet1 = workbook.Worksheets[1];
                worksheet1.Name = "PrepSolo_OP";

                //Enter values to the cells from A3 to A5
                worksheet1.Range["A1"].Text = "Código da operação (PK)";//int PKPrepSoloOP
                worksheet1.Range["B1"].Text = "Código do talhão (PK)";//int PKTalhoesCR
                worksheet1.Range["C1"].Text = "Código do prep. de solo";//int CodPrepSolo
                worksheet1.Range["D1"].Text = "Código do local (talhão)";//int CodLocal
                worksheet1.Range["E1"].Text = "Nome da operação";//string IdentifOperacao
                worksheet1.Range["F1"].Text = "Proced. operacional";//int ProcedOperac
                worksheet1.Range["G1"].Text = "Equipamento";//string Equipamento
                worksheet1.Range["H1"].Text = "Marca do equip.";//string MarcaEquip
                worksheet1.Range["I1"].Text = "Modelo do equip.";//string ModeloEquip
                worksheet1.Range["J1"].Text = "Aplic. de fertilizante";//string AplicFert
                worksheet1.Range["K1"].Text = "Data início da op.";//string DataInicio
                worksheet1.Range["L1"].Text = "Data final da op.";//string DataFinal
                worksheet1.Range["M1"].Text = "Profundidade (cm)";//float Profundidade
                worksheet1.Range["N1"].Text = "Dist. entre linhas (m)";//float DistanciaEL
                worksheet1.Range["O1"].Text = "Núm. de horas";//float NumHoras
                worksheet1.Range["P1"].Text = "Rendimento";//float Rendimento
                worksheet1.Range["Q1"].Text = "Empresa resp.";//string EmpresaResp
                worksheet1.Range["R1"].Text = "Operador";//string Operador
                worksheet1.Range["S1"].Text = "Observações";//string Observacoes

                //Formata fonte da linha de títulos
                worksheet1.Range["A1:S1"].CellStyle.Font.Bold = true;
                worksheet1.Range["A1:S1"].CellStyle.Font.Italic = true;
                worksheet1.Range["A1:D1"].CellStyle.Font.RGBColor = Color.FromArgb(200, 50, 50);

                //Colore as células A1 a D1
                worksheet1.Range["A1:D1"].CellStyle.Color = Color.FromArgb(150, 150, 150);

                //Borda inferior da linha de títulos
                worksheet1.Range["A1:S1"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                worksheet1.Range["A1:S1"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].Color = ExcelKnownColors.Black;

                //Ajusta a largura das colunas
                worksheet1.Range["A1:S1"].AutofitColumns();

                sql = $"SELECT * FROM PrepSoloOP";
                try { conn.Query<PrepSoloOP>(sql); }
                catch { DisplayAlert("Message", "Não foi possível abrir a tabela PrepSoloOP.", "Ok"); }
                sql.First();

                var ListaPrepSoloOP = conn.Table<PrepSoloOP>();

                if (ListaPrepSoloOP.Count() > 0)
                {
                    int i = 1;
                    foreach (var campo in ListaPrepSoloOP)
                    {
                        i = i + 1;

                        try { worksheet1.Range["A" + i].Number = campo.PKPrepSoloOP; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PKPrepSoloOP'", "Ok"); }
                        try { worksheet1.Range["B" + i].Number = campo.PKTalhoesCR; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PKTalhoesCR'", "Ok"); }
                        try { worksheet1.Range["C" + i].Number = campo.CodPrepSolo; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodPrepSolo'", "Ok"); }
                        try { worksheet1.Range["D" + i].Number = campo.CodLocal; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodLocal'", "Ok"); }
                        if (campo.IdentifOperacao != null)
                        {
                            try { worksheet1.Range["E" + i].Text = campo.IdentifOperacao; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'IdentifOperacao'", "Ok"); }
                        }
                        try { worksheet1.Range["F" + i].Number = campo.ProcedOperac; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'ProcedOperac'", "Ok"); }
                        if (campo.Equipamento != null)
                        {
                            try { worksheet1.Range["G" + i].Text = campo.Equipamento; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Equipamento'", "Ok"); }
                        }
                        if (campo.MarcaEquip != null)
                        {
                            try { worksheet1.Range["H" + i].Text = campo.MarcaEquip; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'MarcaEquip'", "Ok"); }
                        }
                        if (campo.ModeloEquip != null)
                        {
                            try { worksheet1.Range["I" + i].Text = campo.ModeloEquip; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'ModeloEquip'", "Ok"); }
                        }
                        if (campo.AplicFert != null)
                        {
                            try { worksheet1.Range["J" + i].Text = campo.AplicFert; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'AplicFert'", "Ok"); }
                        }
                        if (campo.DataInicio != null)
                        {
                            try { worksheet1.Range["K" + i].Value = campo.DataInicio; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'DataInicio'", "Ok"); }
                        }
                        if (campo.DataFinal != null)
                        {
                            try { worksheet1.Range["L" + i].Value = campo.DataFinal; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'DataFinal'", "Ok"); }
                        }
                        try { worksheet1.Range["M" + i].Number = double.Parse(campo.Profundidade.ToString("#.00")); }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Profundidade'", "Ok"); }
                        try { worksheet1.Range["N" + i].Number = double.Parse(campo.DistanciaEL.ToString("#.00")); }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'DistanciaEL'", "Ok"); }
                        try { worksheet1.Range["O" + i].Number = double.Parse(campo.NumHoras.ToString("#.00")); }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'NumHoras'", "Ok"); }
                        try { worksheet1.Range["P" + i].Number = double.Parse(campo.Rendimento.ToString("#.00")); }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Rendimento'", "Ok"); }
                        if (campo.EmpresaResp != null)
                        {
                            try { worksheet1.Range["Q" + i].Text = campo.EmpresaResp; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'EmpresaResp'", "Ok"); }
                        }
                        if (campo.Operador != null)
                        {
                            try { worksheet1.Range["R" + i].Text = campo.Operador; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Operador'", "Ok"); }
                        }
                        if (campo.Observacoes != null)
                        {
                            try { worksheet1.Range["S" + i].Text = campo.Observacoes; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Observacoes'", "Ok"); }
                        }
                    }
                }

                //Acrescenta a planilha de Avaliações de ontrode de qualidade de preparo de solo.
                IWorksheet worksheet2 = workbook.Worksheets[2];
                worksheet2.Name = "PrepSolo_CQ";

                //Enter values to the cells from A3 to A5
                worksheet2.Range["A1"].Text = "Código do c. qualidade (PK)"; //int PKPrepSoloCQ
                worksheet2.Range["B1"].Text = "Código da operação (PK)"; //int PKPrepSoloOP
                worksheet2.Range["C1"].Text = "Código do C. Q. Prep. solo"; //int CodPrepSoloCQ
                worksheet2.Range["D1"].Text = "Código do prep. solo"; //int CodPrepSolo
                worksheet2.Range["E1"].Text = "Código do local"; //int CodLocal
                worksheet2.Range["F1"].Text = "Repetição"; //int Repeticao
                worksheet2.Range["G1"].Text = "Núm. de observações"; //int NumObs
                worksheet2.Range["H1"].Text = "Profund. do preparo (média)"; //float ProfundMed
                worksheet2.Range["I1"].Text = "Profund. do preparo (desvio)"; //float ProfundDesv
                worksheet2.Range["J1"].Text = "Estrond. lateral (média)"; //float EstrondLatMed
                worksheet2.Range["K1"].Text = "Estrond. lateral (desvio)"; //float EstrondLatDesv
                worksheet2.Range["L1"].Text = "Dist. entre linhas (média)"; //float DistELMed
                worksheet2.Range["M1"].Text = "Dist. entre linhas (desvio)"; //float DistELDesv
                worksheet2.Range["N1"].Text = "Prof. inf. à mínima (n)"; //int ProfundInfMin
                worksheet2.Range["O1"].Text = "Prof. inf. à média (n)"; //int ProfundInfMed
                worksheet2.Range["P1"].Text = "Estrond. lat. inf. ao mínimo (n)"; //int EstrondLatInfMin
                worksheet2.Range["Q1"].Text = "Estrond. lat. inf. ao médio (n)"; //int EstrondLatInfMed
                worksheet2.Range["R1"].Text = "Dist. entre linas inf. à mínima (n)"; //int DistELInfMin
                worksheet2.Range["S1"].Text = "Dist. entre linas inf. à média (n)"; //int DistELInfMed
                worksheet2.Range["T1"].Text = "Dist. entre linas sup. à média (n)"; //int DistELSupMed
                worksheet2.Range["U1"].Text = "Dist. entre linas inf. à máxima (n)"; //int DistELSupMax
                worksheet2.Range["V1"].Text = "Avaliador"; //string ResponsavelAval
                worksheet2.Range["W1"].Text = "Data da avaliação"; //string DataAval
                worksheet2.Range["X1"].Text = "Observações"; //string Observacoes

                //Formata fonte da linha de títulos
                worksheet2.Range["A1:X1"].CellStyle.Font.Bold = true;
                worksheet2.Range["A1:X1"].CellStyle.Font.Italic = true;
                worksheet2.Range["A1:E1"].CellStyle.Font.RGBColor = Color.FromArgb(200, 50, 50);

                //Colore as células A1 a D1
                worksheet2.Range["A1:E1"].CellStyle.Color = Color.FromArgb(150, 150, 150);

                //Borda inferior da linha de títulos
                worksheet2.Range["A1:X1"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                worksheet2.Range["A1:X1"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].Color = ExcelKnownColors.Black;

                //Ajusta a largura das colunas
                worksheet2.Range["A1:X1"].AutofitColumns();

                sql = $"SELECT * FROM PrepSoloCQ";
                try { conn.Query<PrepSoloCQ>(sql); }
                catch { DisplayAlert("Message", "Não foi possível abrir a tabela PrepSoloCQ.", "Ok"); }
                sql.First();

                var qryPrepSoloCQ = conn.Query<PrepSoloCQ>("SELECT * FROM PrepSoloCQ");

                var ListaPrepSoloCQ = conn.Table<PrepSoloCQ>();

                if (ListaPrepSoloCQ.Count() > 0)
                {
                    int i = 1;
                    foreach (var campo in ListaPrepSoloCQ)
                    {
                        i = i + 1;
                        try { worksheet2.Range["A" + i].Number = campo.PKPrepSoloCQ; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PKPrepSoloCQ'", "Ok"); }
                        try { worksheet2.Range["B" + i].Number = campo.PKPrepSoloOP; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PKPrepSoloOP'", "Ok"); }
                        try { worksheet2.Range["C" + i].Number = campo.CodPrepSoloCQ; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodPrepSoloCQ'", "Ok"); }
                        try { worksheet2.Range["D" + i].Number = campo.CodPrepSolo; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodPrepSolo'", "Ok"); }
                        try { worksheet2.Range["E" + i].Number = campo.CodLocal; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodLocal'", "Ok"); }
                        try { worksheet2.Range["F" + i].Number = campo.Repeticao; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Repeticao'", "Ok"); }
                        try { worksheet2.Range["G" + i].Number = campo.NumObs; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'NumObs'", "Ok"); }
                        try { worksheet2.Range["H" + i].Number = double.Parse(campo.ProfundMed.ToString("#.00")); }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'ProfundMed'", "Ok"); }
                        try { worksheet2.Range["I" + i].Number = double.Parse(campo.ProfundDesv.ToString("#.00")); }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'ProfundDesv'", "Ok"); }
                        try { worksheet2.Range["J" + i].Number = double.Parse(campo.EstrondLatMed.ToString("#.00")); }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'EstrondLatMed'", "Ok"); }
                        try { worksheet2.Range["K" + i].Number = double.Parse(campo.EstrondLatDesv.ToString("#.00")); }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'EstrondLatDesv'", "Ok"); }
                        try { worksheet2.Range["L" + i].Number = double.Parse(campo.DistELMed.ToString("#.00")); }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'DistELMed'", "Ok"); }
                        try { worksheet2.Range["M" + i].Number = double.Parse(campo.DistELDesv.ToString("#.00")); }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'DistELDesv'", "Ok"); }
                        try { worksheet2.Range["N" + i].Number = campo.ProfundInfMin; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'ProfundInfMin'", "Ok"); }
                        try { worksheet2.Range["O" + i].Number = campo.ProfundInfMed; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'ProfundInfMed'", "Ok"); }
                        try { worksheet2.Range["P" + i].Number = campo.EstrondLatInfMin; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'EstrondLatInfMin'", "Ok"); }
                        try { worksheet2.Range["Q" + i].Number = campo.EstrondLatInfMed; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'EstrondLatInfMed'", "Ok"); }
                        try { worksheet2.Range["R" + i].Number = campo.DistELInfMin; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'DistELInfMin'", "Ok"); }
                        try { worksheet2.Range["S" + i].Number = campo.DistELInfMed; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'DistELInfMed'", "Ok"); }
                        try { worksheet2.Range["T" + i].Number = campo.DistELSupMed; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'DistELSupMed'", "Ok"); }
                        try { worksheet2.Range["U" + i].Number = campo.DistELSupMax; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'DistELSupMax'", "Ok"); }
                        if (campo.ResponsavelAval != null)
                        {
                            try { worksheet2.Range["V" + i].Text = campo.ResponsavelAval; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'ResponsavelAval'", "Ok"); }
                        }
                        if (campo.DataAval != null)
                        {
                            try { worksheet1.Range["W" + i].Value = campo.DataAval; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'DataAval'", "Ok"); }
                        }
                        if (campo.Observacoes != null)
                        {
                            try { worksheet2.Range["X" + i].Text = campo.Observacoes; }
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
                Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("PrepSolo_" + data + ".xlsx", "application/msexcel", stream);
            }

        }

        private void Informacoes_Clicked(object sender, EventArgs e)
        {
            msg = $"Esta tela exibe a lista de operações de preparo de solo realizadas no talhão e peprmite a edição.\n\n" +
                $"Os comandos são os seguintes:\n\nNOVO PREPARO DE SOLO: incluir novo registro por meio de digitação direta no SIGNUS;\n\n" +
                $"A partir do menu (três pontinhos) no cato superior direito da tela, há a opção de exportar " +
                $"os dados de preparo de solo para uma planilha Excel. O arquivo gerado conterá os dados dos talhões" +
                $"para os quais há operações de preparo de solo cadastradas, os dados das operações e das avaliações de " +
                $"controle de qualidade realizadas. O arquivo é disponibilizado na pasta Signus com o nome iniciado por " +
                $"'PrepSolo_' seguido da data e hora.\n\n" +
                $"Um toque rápido sobre um registro de operação de preparo de solo expande exibe os detalhes do mesmo;\n\n" +
                $"Para editar um registro já existente, pressione-o até aparecerem as opções na parte superior da tela.\n\n" +
                $"Para editar as avaliações de controle de qualidade, pressione o registro até aparecer o menu (três pontinhos) " +
                $"no canto superior direito da tela. A partir deste menu, é possível abrir o formulário para cadastro dessas avaliações.\n\n" +
                $"A seta localizada no canto superior esquerdo da tela permite retornar à tela anterior.";
            DisplayAlert("SIGNUS", msg, "Ok");
        }

    }
}