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
	public partial class pgCFormigasOP : ContentPage
	{

        private SQLiteConnection conn;
        public CFormigasOP cformigasop;
        public CFormigasCQ cformigascq;
        public string pktlh;
        string msg;

        public pgCFormigasOP (string pktalhoescr)
		{
			InitializeComponent ();
            conn = DependencyService.Get<ISQLite>().GetConnection();
            //conn.DropTable<CFormigasOP>();
            //conn.DropTable<CFormigasCQ>();
            conn.CreateTable<CFormigasOP>();
            conn.CreateTable<CFormigasCQ>();

            pktlh = pktalhoescr;
            PKTalhoesCR.Text = "Código do talhão: " + pktlh;

        }
        protected override void OnAppearing()
        {
            int num = Convert.ToInt32(pktlh);
            //PopulateListaCFormigasOP();

            var data = conn.Table<CFormigasOP>().Where(x => x.PKTalhoesCR == num).ToList();
            //var data = (from cformigas in conn.Table<CFormigasOP>() select cformigas);
            lstCFormigasOP.ItemsSource = data;
        }

        private void Mostrar_Clicked(object sender, EventArgs e)
        {
            var data = (from cformigas in conn.Table<CFormigasOP>() select cformigas);
            lstCFormigasOP.ItemsSource = data;
        }

        private void NovoCFormigasOP_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new pgAddCFormigasOP(null, pktlh));
        }

        private void EditCFormigasOP(object sender, ItemTappedEventArgs e)
        {
            CFormigasOP details = e.Item as CFormigasOP;
            if (details != null)
            {
//                Navigation.PushAsync(new pgAddCFormigasOP(details, pktlh));
            }
        }

        public async void AlternaDetalhes(object sender, ItemTappedEventArgs e)
        {
            //Alterna entre visível e invisível a lista de detalhes
            CFormigasOP details = e.Item as CFormigasOP;
            string sql = "";

            if (details.Visible.ToString() == "True")
            {
                sql = $"UPDATE CFormigasOP SET Visible = 0, Imagem = 'Down.png' ";
            }
            else
            {
                sql = $"UPDATE CFormigasOP SET Visible = 1, Imagem = 'Up.png' ";
            }

            sql += $"WHERE PKCFormigasOP = {details.PKCFormigasOP.ToString()}";
            try { conn.Execute(sql); }
            catch { await DisplayAlert("Message", "Houve um erro. Não foi possível atualizar.", "Ok"); }

            sql = $"SELECT * FROM CFormigasOP WHERE PKTalhoesCR = {pktlh.ToString()}";

            //Atualiza a lista
            var data = conn.Query<CFormigasOP>(sql);
            lstCFormigasOP.ItemsSource = data;
        }


        private async void DeleteCFormigasOP(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var operacao = mi.CommandParameter as CFormigasOP;
            if (operacao != null)
            {
                var opcao = await DisplayAlert("Excluir operação", "Confirma a exclusão da operação de controle de formigas de código " + operacao.PKCFormigasOP + "" +
                    " e de todos os outros (de controle de qualidade) associados? ", "Ok", "Cancel");
                if (opcao)
                {
                    //Deleta todos os registros de controle de qualidade associados à operação atual
                    string sql = $"DELETE FROM CFormigasCQ WHERE PKCFormigasOP = {operacao.PKCFormigasOP}";
                    try { conn.Execute(sql); }
                    catch
                    {
                        await DisplayAlert("Message", "Houve um erro, não foi possível excluir os registros de controle de qualidade associados a este registro de operação de controle de formigas. " +
                            "Possivelmente, a tabela de controle de qualidade ainda não foi criada.", "Ok");
                    }

                    //Deleta o registro de operação
                    sql = $"DELETE FROM CFormigasOP WHERE PKCFormigasOP = {operacao.PKCFormigasOP}";

                    try { conn.Execute(sql); }
                    catch { await DisplayAlert("Message", "Houve um erro, não foi possível excluir o registro de operação de controle de formigas.", "Ok"); }

                    sql = $"SELECT * FROM CFormigasOP WHERE PKTalhoesCR = {pktlh}";
                    conn.Query<CFormigasOP>(sql);

                    //Atualiza a lista
                    var data = conn.Query<CFormigasOP>(sql);
                    lstCFormigasOP.ItemsSource = data;
                }
            }
        }

        private async void AlterarCFormigasOP(object sender, EventArgs e)
        {
            try
            {
                var mi = ((MenuItem)sender);
                var operacao = mi.CommandParameter as CFormigasOP;
                if (operacao != null)
                {
//                    await Navigation.PushAsync(new pgAddCFormigasOP(operacao, pktlh));
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
            var operacao = mi.CommandParameter as CFormigasOP;
            if (operacao != null)
            {
                //var opcao = await DisplayAlert("Avaliar qualidade", "Abrir controles de qualidade da operação " + operacao.PKCFormigasOP.ToString() + "? ", "Ok", "Cancel");
                //if (opcao)
                //{
                string sql = $"SELECT * FROM CFormigasCQ WHERE PKCFormigasOP = {operacao.PKCFormigasOP}";

                try { conn.Query<CFormigasCQ>(sql); }
                catch { await DisplayAlert("Message", "Houve um erro. Não foi possível abrir a consulta.", "Ok"); }

                //Atualiza a página
                //Navigation.InsertPageBefore(new pgCFormigasCQ(operacao.PKCFormigasOP.ToString()), this);
                //await Navigation.PopAsync();
//                await Navigation.PushAsync(new pgCFormigasCQ(operacao.PKTalhoesCR.ToString(), operacao.PKCFormigasOP.ToString()));
                //}
            }
        }

        public void ExportarCFormigas_Clicked(object sender, EventArgs e)
        {

            DisplayAlert("Informação", message: "Este comando exporta todos os dados das operações de combate a formigas e os respectivos controles de qualidade.", "Ok");

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

                var qryCFormigasOP = conn.Query<CFormigasOP>("SELECT PKTalhoesCR FROM CFormigasOP GROUP BY PKTalhoesCR ORDER BY PKTalhoesCR");
                //var ListaAmostragem = conn.Table<AmostragemMN>();
                if (qryCFormigasOP.Count() > 0)
                {
                    qryCFormigasOP.First();
                    int i = 1;
                    foreach (var campoam in qryCFormigasOP)
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

                //Acrescenta a planilha de CFormigasOP.
                IWorksheet worksheet1 = workbook.Worksheets[1];
                worksheet1.Name = "CFormigas_OP";

                //Enter values to the cells from A3 to A5
                worksheet1.Range["A1"].Text = "Código da operação (PK)";//int PKCFormigasOP
                worksheet1.Range["B1"].Text = "Código do talhão (PK)";//int PKTalhoesCR
                worksheet1.Range["C1"].Text = "Código do C. Formigas";//int CodCFormigasOP
                worksheet1.Range["D1"].Text = "Código do local (talhão)";//int CodLocal
                worksheet1.Range["E1"].Text = "Nome da operação";//string IdentifOperacao
                worksheet1.Range["F1"].Text = "Proced. operacional";//int ProcedOperac
                worksheet1.Range["G1"].Text = "Tipo de controle";//string TipoContr
                worksheet1.Range["H1"].Text = "Produto utilizado";//string Produto
                worksheet1.Range["I1"].Text = "Dose";//float Dose
                worksheet1.Range["J1"].Text = "Unidade dose";//string UnidDose
                worksheet1.Range["K1"].Text = "Método de controle";//string MetodoContr
                worksheet1.Range["L1"].Text = "Equipamento utilizado";//string Equipamento
                worksheet1.Range["M1"].Text = "Data do controle";//string DataContr
                worksheet1.Range["N1"].Text = "Núm. de horas";//float NumHoras
                worksheet1.Range["O1"].Text = "Rendimento";//float Rendimento
                worksheet1.Range["P1"].Text = "Empresa responsável";//string EmpresaResp
                worksheet1.Range["Q1"].Text = "Observações";//string Observacoes

                //Formata fonte da linha de títulos
                worksheet1.Range["A1:Q1"].CellStyle.Font.Bold = true;
                worksheet1.Range["A1:Q1"].CellStyle.Font.Italic = true;
                worksheet1.Range["A1:D1"].CellStyle.Font.RGBColor = Color.FromArgb(200, 50, 50);

                //Colore as células A1 a D1
                worksheet1.Range["A1:D1"].CellStyle.Color = Color.FromArgb(150, 150, 150);

                //Borda inferior da linha de títulos
                worksheet1.Range["A1:Q1"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                worksheet1.Range["A1:Q1"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].Color = ExcelKnownColors.Black;

                //Ajusta a largura das colunas
                worksheet1.Range["A1:Q1"].AutofitColumns();

                sql = $"SELECT * FROM CFormigasOP";
                try { conn.Query<CFormigasOP>(sql); }
                catch { DisplayAlert("Message", "Não foi possível abrir a tabela CFormigasOP.", "Ok"); }
                sql.First();

                var qryCFormigas = conn.Query<CFormigasOP>("SELECT * FROM CFormigasOP");

                var ListaCFormigas = conn.Table<CFormigasOP>();

                if (ListaCFormigas.Count() > 0)
                {
                    int i = 1;
                    foreach (var campo in ListaCFormigas)
                    {
                        i = i + 1;

                        try { worksheet1.Range["A" + i].Number = campo.PKCFormigasOP; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PKCFormigasOP'", "Ok"); }
                        try { worksheet1.Range["B" + i].Number = campo.PKTalhoesCR; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PKTalhoesCR'", "Ok"); }
                        try { worksheet1.Range["C" + i].Number = campo.CodCFormigasOP; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodCFormigasOP'", "Ok"); }
                        try { worksheet1.Range["D" + i].Number = campo.CodLocal; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodLocal'", "Ok"); }
                        if (campo.IdentifOperacao != null)
                        {
                            try { worksheet1.Range["E" + i].Text = campo.IdentifOperacao; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'IdentifOperacao'", "Ok"); }
                        }
                        try { worksheet1.Range["F" + i].Number = campo.ProcedOperac; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'ProcedOperac'", "Ok"); }
                        if (campo.TipoContr != null)
                        {
                            try { worksheet1.Range["G" + i].Text = campo.TipoContr; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'TipoContr'", "Ok"); }
                        }
                        if (campo.Produto != null)
                        {
                            try { worksheet1.Range["H" + i].Text = campo.Produto; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Produto'", "Ok"); }
                        }
                        try { worksheet1.Range["I" + i].Number = double.Parse(campo.Dose.ToString("#.00")); }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Dose'", "Ok"); }
                        if (campo.UnidDose != null)
                        {
                            try { worksheet1.Range["J" + i].Text = campo.UnidDose; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'UnidDose'", "Ok"); }
                        }
                        if (campo.MetodoContr != null)
                        {
                            try { worksheet1.Range["K" + i].Text = campo.MetodoContr; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'MetodoContr'", "Ok"); }
                        }
                        if (campo.Equipamento != null)
                        {
                            try { worksheet1.Range["L" + i].Text = campo.Equipamento; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Equipamento'", "Ok"); }
                        }
                        if (campo.DataContr != null)
                        {
                            try { worksheet1.Range["M" + i].Value = campo.DataContr; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'DataContr'", "Ok"); }
                        }
                        try { worksheet1.Range["N" + i].Number = double.Parse(campo.NumHoras.ToString("#.00")); }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'NumHoras'", "Ok"); }
                        try { worksheet1.Range["O" + i].Number = double.Parse(campo.Rendimento.ToString("#.00")); }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Rendimento'", "Ok"); }
                        if (campo.EmpresaResp != null)
                        {
                            try { worksheet1.Range["P" + i].Text = campo.EmpresaResp; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'EmpresaResp'", "Ok"); }
                        }
                        if (campo.Observacoes != null)
                        {
                            try { worksheet1.Range["Q" + i].Text = campo.Observacoes; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Observacoes'", "Ok"); }
                        }
                    }
                }

                //Acrescenta a planilha de Controle de qualidade.
                IWorksheet worksheet2 = workbook.Worksheets[2];
                worksheet2.Name = "CFormigas_CQ";

                //Títulos das colunas
                worksheet2.Range["A1"].Text = "Código do cont. de qual. (PK)";//int PKCFormigasCQ
                worksheet2.Range["B1"].Text = "Código da operação (PK)";//int PKCFormigasOP
                worksheet2.Range["C1"].Text = "Código do cont. de qual.";//int CodCFormigasCQ
                worksheet2.Range["D1"].Text = "Código da operação";//int CodCFormigasOP
                worksheet2.Range["E1"].Text = "Repetição";//int Repeticao
                worksheet2.Range["F1"].Text = "Formigueiros avaliados (n)";//int NumFormAval
                worksheet2.Range["G1"].Text = "Formigueiros não aplicados (n)";//int NumFormNaoAp
                worksheet2.Range["H1"].Text = "Aplic. em local inadequado (n)";//int NumFormLocInad
                worksheet2.Range["I1"].Text = "Aplic. em form não cortadeiras (n)";//int NumAplFormNCort
                worksheet2.Range["J1"].Text = "Responsãvel pela aval.";//string ResponsAval
                worksheet2.Range["K1"].Text = "Data da avaliação";//string DataAval
                worksheet2.Range["L1"].Text = "Observações";//string Observacoes

                //Formata fonte da linha de títulos
                worksheet2.Range["A1:L1"].CellStyle.Font.Bold = true;
                worksheet2.Range["A1:L1"].CellStyle.Font.Italic = true;
                worksheet2.Range["A1:D1"].CellStyle.Font.RGBColor = Color.FromArgb(200, 50, 50);

                //Colore as células A1 a D1
                worksheet2.Range["A1:D1"].CellStyle.Color = Color.FromArgb(150, 150, 150);

                //Borda inferior da linha de títulos
                worksheet2.Range["A1:L1"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                worksheet2.Range["A1:L1"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].Color = ExcelKnownColors.Black;

                //Ajusta a largura das colunas
                worksheet2.Range["A1:L1"].AutofitColumns();

                sql = $"SELECT * FROM CFormigasCQ";
                try { conn.Query<CFormigasCQ>(sql); }
                catch { DisplayAlert("Message", "Não foi possível abrir a tabela CFormigasCQ.", "Ok"); }
                sql.First();

                var qryCFormigasCQ = conn.Query<CFormigasCQ>("SELECT * FROM CFormigasCQ");

                var ListaCFormigasCQ = conn.Table<CFormigasCQ>();

                if (ListaCFormigasCQ.Count() > 0)
                {
                    int i = 1;
                    foreach (var campo in ListaCFormigasCQ)
                    {
                        i = i + 1;
                        try { worksheet2.Range["A" + i].Number = campo.PKCFormigasCQ; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PKCFormigasCQ'", "Ok"); }
                        try { worksheet2.Range["B" + i].Number = campo.PKCFormigasOP; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PKCFormigasOP'", "Ok"); }
                        try { worksheet2.Range["C" + i].Number = campo.CodCFormigasCQ; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodCFormigasCQ'", "Ok"); }
                        try { worksheet2.Range["D" + i].Number = campo.CodCFormigasOP; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodCFormigasOP'", "Ok"); }
                        try { worksheet2.Range["E" + i].Number = campo.Repeticao; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Repeticao'", "Ok"); }
                        try { worksheet2.Range["F" + i].Number = campo.NumFormAval; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'NumFormAval'", "Ok"); }
                        try { worksheet2.Range["G" + i].Number = campo.NumFormNaoAp; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'NumFormNaoAp'", "Ok"); }
                        try { worksheet2.Range["H" + i].Number = campo.NumFormLocInad; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'NumFormLocInad'", "Ok"); }
                        try { worksheet2.Range["I" + i].Number = campo.NumAplFormNCort; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'NumAplFormNCort'", "Ok"); }

                        if (campo.ResponsAval != null)
                        {
                            try { worksheet2.Range["J" + i].Text = campo.ResponsAval; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'ResponsAval'", "Ok"); }
                        }
                        if (campo.DataAval != null)
                        {
                            try { worksheet1.Range["K" + i].Value = campo.DataAval; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'DataAval'", "Ok"); }
                        }
                        if (campo.Observacoes != null)
                        {
                            try { worksheet2.Range["L" + i].Text = campo.Observacoes; }
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
                Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("CFormigas_" + data + ".xlsx", "application/msexcel", stream);
            }

        }

        private void Informacoes_Clicked(object sender, EventArgs e)
        {
            msg = $"Esta tela exibe a lista de operações de controle de formigas realizadas no talhão e peprmite a edição.\n\n" +
                $"Os comandos são os seguintes:\n\nNOVO C. DE FORMIGAS: incluir novo registro por meio de digitação direta no SIGNUS;\n\n" +
                $"A partir do menu (três pontinhos) no cato superior direito da tela, há a opção de exportar " +
                $"os dados dos controles de formigas para uma planilha Excel. O arquivo gerado conterá os dados dos talhões" +
                $"para os quais há controles de formigas cadastrados, os dados das operações e das avaliações de " +
                $"controle de qualidade realizadas. O arquivo é disponibilizado na pasta Signus com o nome iniciado por " +
                $"'CFormigas_' seguido da data e hora.\n\n" +
                $"Um toque rápido sobre um registro de controle de formigas expande exibe os detalhes do mesmo;\n\n" +
                $"Para editar um registro já existente, pressione-o até aparecerem as opções na parte superior " +
                $"da tela.\n\n" +
                $"Para editar as avaliações de controle de qualidade, pressione o registro até aparecer o menu (três pontinhos) " +
                $"no canto superior direito da tela. A partir deste menu, é possível abrir o formulário para cadastro dessas avaliações.\n\n" +
                $"A seta localizada no canto superior esquerdo da tela permite retornar à tela anterior.";
            DisplayAlert("SIGNUS", msg, "Ok");
        }

    }
}