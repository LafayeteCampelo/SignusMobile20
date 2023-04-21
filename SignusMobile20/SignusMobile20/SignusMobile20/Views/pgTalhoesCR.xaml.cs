using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;
using Syncfusion.XlsIO;
using Color = Syncfusion.Drawing.Color;
using SignusMobile20.Models;


namespace SignusMobile20
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class pgTalhoesCR : ContentPage
	{

        private SQLite.SQLiteConnection conn;
        public TalhoesCR talhoescr;
        public AmostragemMN amostragemmn;
        public MedicaoArvore medicaoarvore;
        public CFormigasOP cformidasop;
  
        public string fazenda;
        public bool filtrado;
        string msg;


        public pgTalhoesCR ()
		{
			InitializeComponent ();

            conn = DependencyService.Get<ISQLite>().GetConnection();
            //conn.DropTable<TalhoesCR>();
            //conn.DropTable<AmostragemMN>();
            conn.CreateTable<TalhoesCR>();
            //conn.CreateTable<AmostragemMN>();
            //conn.CreateTable<CFormigasOP>();
            //conn.CreateTable<MedicaoArvore>();

            fazenda = "";
            filtrado = false;
            //var qryAmostragem = conn.Query<AmostragemMN>("SELECT * FROM AmostragemMN Where PKTalhoesCR = 1");

        }

        public interface IExportFilesToLocation
        {
            string GetFolderLocation();
        }

        private void Informacoes_Clicked(object sender, EventArgs e)
        {

            DisplayAlert("SIGNUS", msg, "Ok");

        }

        protected override void OnAppearing()
        {

            //Carrega a lista de talhões
            //var data = (from talhoescr in conn.Table<TalhoesCR>() select talhoescr);
            //lstTalhoesCR.ItemsSource = data;
            if (fazenda != "")
            {
                var data = conn.Query<TalhoesCR>($"SELECT * FROM TalhoesCR Where Fazenda = '{fazenda}'");
                lstTalhoesCR.ItemsSource = data;
                ToolBarItem2.IconImageSource = "ic_action_unsearch.png";
                if (data.Count == 0)
                {
                    msg = $"Não há registros para serem exibidos para esta fazenda. ";
                    DisplayAlert("SIGNUS", $"{msg}", "Ok");
                }
            }
            else
            {
                var data = conn.Query<TalhoesCR>($"SELECT * FROM TalhoesCR");
                lstTalhoesCR.ItemsSource = data;
                ToolBarItem2.IconImageSource = "ic_action_search.png";
                if (data.Count == 0)
                {
                    msg = $"Não há registros para serem exibidos. Entre em contato com a " +
                        $"T&M Consultoria Agroflorestal LTDA +55(31)99209-5166 ou +55(19)99977-2505. Enviaremos " +
                        $"um banco de dados fictício para demonstração completa do SIGNUS.";
                }
                else
                {
                    msg = $"Esta tela exibe a lista de todos os talhões ou glebas cadastradas e permite a edição dos mesmos.\n\n" +
                        $"Os comandos são os seguintes:\n\nNOVO TALHÃO: incluir novo registro por meio de digitação direta no SIGNUS;\n\n" +
                        $"IMPORTAR TALHÕES: permite importar dados de uma planilha excel com formatação específica;\n\n" +
                        $"Pressionando-se o ícone de lupa, é possível filtrar os registros por fazenda.\n\n" +
                        $"A partir do menu (três pontinhos) no cato superior direito da tela, há a opção de exportar " +
                        $"os dados de talhões para uma planilha Excel. O arquivo gerado é disponibilizado na pasta Signus " +
                        $"com o nome iniciado por 'Talhoes_' seguido da data e hora.\n\n" +
                        $"Um toque rápido sobre um registro de talhão expande exibe os detalhes do mesmo;\n\n" +
                        $"Para editar um registro já existente, pressione-o até aparecerem as opções na parte superior " +
                        $"da tela.\n\n" +
                        $"Para editar operações realizadas no talhão, pressione-o até aparecer o menu (três pontinhos) " +
                        $"no canto superior direito da tela. A partir deste menu, é possível abrir as páginas para cadastro de " +
                        $"amostragens do monitoramento nutricional e para cadastro de operações diversas realizadas no talhão.";
                }
                //DisplayAlert("SIGNUS", $"{msg}", "Ok");
            }

        }

        List<string> fazendas = new List<string> { };
        private void Search_Clicked(object sender, EventArgs e)
        {

            //Alterna lstFazendas/lstTalhoesCR
            if (filtrado == false)
            {
                //Estabelecer filtragem
                //Preenche a lista de fazendas, caso exista
                fazendas.Clear();
                var listaFazendas = conn.Query<TalhoesCR>("SELECT Fazenda FROM TalhoesCR GROUP BY Fazenda HAVING (Fazenda IS NOT NULL) ORDER BY Fazenda");
                if (listaFazendas.Count() > 0)
                {
                    foreach (var campo in listaFazendas) { fazendas.Add(campo.Fazenda); }
                }
                lstFazendas.ItemsSource = fazendas;
                SearchConteudo.IsVisible = true;
                lstFazendas.IsVisible = true;
                btnNovoTalhao.IsVisible = false;
                ImportarTalhoes.IsVisible = false;
                lstTalhoesCR.IsVisible = false;
                filtrado = true;
                ToolBarItem2.IconImageSource = "ic_action_unsearch.png";
            }
            else
            {
                //Desfazer filtragem
                var listat = conn.Query<TalhoesCR>("SELECT * FROM TalhoesCR ");
                lstTalhoesCR.ItemsSource = listat;
                SearchConteudo.IsVisible = false;
                lstFazendas.IsVisible = false;
                btnNovoTalhao.IsVisible = true;
                ImportarTalhoes.IsVisible = true;
                lstTalhoesCR.IsVisible = true;
                filtrado = false;
                ToolBarItem2.IconImageSource = "ic_action_search.png";
            }

        }

        private void ExportarTalhoesCR_Clicked(object sender, EventArgs e)
        {

            DisplayAlert("Informação", message: "Este comando exporta apenas os dados dos talhões.", "Ok");




            /*
             * 
             * Teste de acesso a dados remotos utilizando uma conexão com o MySql
                        List<MyObject> result = new List<MyObject>();

                        string connectionString = "connection string";
                        var connection = new MySqlConnection(connectionString); // C# 8!
                        connection.Open();

                        var command = connection.CreateCommand();
                        command.CommandText = "SELECT id, name FROM fooBar WHERE deleted = 0"; // whatever

                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            result.Add(new MyObject
                            {
                                Id = reader.GetUInt64("id"), // the parameter is the name of the column
                                Name = reader.GetString("name"),
                            });

                        }

                        return result;

            */


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
                worksheet.Range["E1"].Text = "CodEPTCR";//CodEPTCR
                worksheet.Range["F1"].Text = "Empresa";//Empresa
                worksheet.Range["G1"].Text = "Unidade";//Unidade
                worksheet.Range["H1"].Text = "Fazenda/Projeto";//Fazenda
                worksheet.Range["I1"].Text = "Talhão";//Talhao
                worksheet.Range["J1"].Text = "Subtalhão";//Subtalhao
                worksheet.Range["K1"].Text = "Ciclo";//Ciclo
                worksheet.Range["L1"].Text = "Rotacão";//Rotacao
                worksheet.Range["M1"].Text = "Espaç entre linhas";//EspEL
                worksheet.Range["N1"].Text = "Espaç entre plantas";//EspEP
                worksheet.Range["O1"].Text = "DataIniRot/Data de plantio";//DataIniRot
                worksheet.Range["P1"].Text = "Área do talhão (ha)";//AreaTalh
                worksheet.Range["Q1"].Text = "AreaSubtalh";//AreaSubtalh
                worksheet.Range["R1"].Text = "Espécie";//Especie
                worksheet.Range["S1"].Text = "MatGen";//MatGen
                worksheet.Range["T1"].Text = "Tipo propagação";//Propag
                worksheet.Range["U1"].Text = "Classe de solo";//SoloClasse
                worksheet.Range["V1"].Text = "Cód da Unid manejo";//UnidMan
                worksheet.Range["W1"].Text = "Latitude (G)";//LatitudeLocG
                worksheet.Range["X1"].Text = "Latitude (M)";//LatitudeLocM
                worksheet.Range["Y1"].Text = "Latitude (S)";//LatitudeLocS
                worksheet.Range["Z1"].Text = "Longitude (G)";//LongitudeLocG
                worksheet.Range["AA1"].Text = "Longitude (M)";//LongitudeLocM
                worksheet.Range["AB1"].Text = "Longitude (S)";//LongitudeLocS
                worksheet.Range["AC1"].Text = "UTM N (m)";//UTMN
                worksheet.Range["AD1"].Text = "UTM E (m)";//UTME
                worksheet.Range["AE1"].Text = "Datum";//Datum
                worksheet.Range["AF1"].Text = "Zona";//Zona
                worksheet.Range["AG1"].Text = "Lat (N/S)";//Lat
                worksheet.Range["AH1"].Text = "Long (E/W)";//Lon
                worksheet.Range["AI1"].Text = "Observações";//Observacoes

                //Formata fonte da linha de títulos
                worksheet.Range["A1:AI1"].CellStyle.Font.Bold = true;
                worksheet.Range["A1:AI1"].CellStyle.Font.Italic = true;
                worksheet.Range["A1:E1"].CellStyle.Font.RGBColor = Color.FromArgb(200, 50, 50);

                //Colore as células A1 a D1
                worksheet.Range["A1:E1"].CellStyle.Color = Color.FromArgb(150, 150, 150);

                //Borda inferior da linha de títulos
                worksheet.Range["A1:AI1"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range["A1:AI1"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].Color = ExcelKnownColors.Black;

                //Formatação de números reais
                worksheet.Range["M1:N1"].NumberFormat = "$.00";
                worksheet.Range["P1:Q1"].NumberFormat = "$.00";
                worksheet.Range["Y1:Y1"].NumberFormat = "$.00";
                worksheet.Range["AB1:AB1"].NumberFormat = "$.00";
                worksheet.Range["AC1:AD1"].NumberFormat = "$.00";

                //Ajusta a largura das colunas
                worksheet.Range["A1:AI1"].AutofitColumns();

                //string sql = $"SELECT * FROM TalhoesCR";
                string sql = "";
                if (fazenda != "")
                {
                    sql = $"SELECT * FROM TalhoesCR Where Fazenda = '" + fazenda + "'";
                    ToolBarItem2.IconImageSource = "ic_action_unsearch.png";
                }
                else
                {
                    sql = $"SELECT * FROM TalhoesCR";
                    ToolBarItem2.IconImageSource = "ic_action_search.png";
                }

                try { conn.Query<TalhoesCR>(sql); }
                catch { DisplayAlert("Message", "Não foi possível abrir a tabela TalhoesCR.", "Ok"); }
                sql.First();

                //if (fazenda != "")
                //{
                //    var qryTalhoes = conn.Query<TalhoesCR>("SELECT * FROM TalhoesCR Where Fazenda = '" + fazenda + "'");
                //    ToolBarItem2.IconImageSource = "ic_action_unsearch.png";
                //}
                //else
                //{
                //    var qryTalhoes = conn.Query<TalhoesCR>("SELECT * FROM TalhoesCR");
                //    ToolBarItem2.IconImageSource = "ic_action_search.png";
                //}




                //var ListaTalhoes = conn.Table<TalhoesCR>();
                var ListaTalhoes = conn.Query<TalhoesCR>(sql);

                if (ListaTalhoes.Count() > 0)
                {
                    int i = 1;
                    foreach (var campo in ListaTalhoes)
                    {
                        i = i + 1;

                        try { worksheet.Range["A" + i].Number = campo.PKTalhoesCR; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PKTalhoesCR'", "Ok"); }
                        try { worksheet.Range["B" + i].Number = campo.CodTalhoesCRSR; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodTalhoesCRSR'", "Ok"); }
                        try { worksheet.Range["C" + i].Number = campo.CodInform_Local; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodInform_Local'", "Ok"); }
                        try { worksheet.Range["D" + i].Number = campo.CodCadastro; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodCadastro'", "Ok"); }
                        if (campo.CodEPTCR != null)
                        {
                            try { worksheet.Range["E" + i].Text = campo.CodEPTCR; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodEPTCR'", "Ok"); }
                        }
                        if (campo.Empresa != null)
                        {
                            try { worksheet.Range["F" + i].Text = campo.Empresa; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Empresa'", "Ok"); }
                        }
                        if (campo.Unidade != null)
                        {
                            try { worksheet.Range["G" + i].Text = campo.Unidade; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Unidade'", "Ok"); }
                        }
                        if (campo.Fazenda != null)
                        {
                            try { worksheet.Range["H" + i].Text = campo.Fazenda; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Fazenda'", "Ok"); }
                        }
                        if (campo.Talhao != null)
                        {
                            try { worksheet.Range["I" + i].Text = campo.Talhao; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Talhao'", "Ok"); }
                        }
                        if (campo.Subtalhao != null)
                        {
                            try { worksheet.Range["J" + i].Text = campo.Subtalhao; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Subtalhao'", "Ok"); }
                        }
                        try { worksheet.Range["K" + i].Number = campo.Ciclo; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Ciclo'", "Ok"); }
                        try { worksheet.Range["L" + i].Number = campo.Rotacao; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Rotacao'", "Ok"); }
                        try { worksheet.Range["M" + i].Number = double.Parse(campo.EspEL.ToString("#.00")); }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'EspEL'", "Ok"); }
                        try { worksheet.Range["N" + i].Number = double.Parse(campo.EspEP.ToString("#.00")); }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'EspEP'", "Ok"); }
                        if (campo.DataIniRot != null)
                        {
                            try { worksheet.Range["O" + i].Value = campo.DataIniRot; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'DataIniRot'", "Ok"); }
                        }
                        try { worksheet.Range["P" + i].Number = double.Parse(campo.AreaTalh.ToString("#.00")); }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'AreaTalh'", "Ok"); }
                        try { worksheet.Range["Q" + i].Number = double.Parse(campo.AreaSubtalh.ToString("#.00")); }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'AreaSubtalh'", "Ok"); }
                        if (campo.Especie != null)
                        {
                            try { worksheet.Range["R" + i].Text = campo.Especie; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Especie'", "Ok"); }
                        }
                        if (campo.MatGen != null)
                        {
                            try { worksheet.Range["S" + i].Text = campo.MatGen; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'MatGen'", "Ok"); }
                        }
                        if (campo.SoloClasse != null)
                        {
                            try { worksheet.Range["T" + i].Text = campo.Propag; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Propag'", "Ok"); }
                        }
                        if (campo.SoloClasse != null)
                        {
                            try { worksheet.Range["U" + i].Text = campo.SoloClasse; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'SoloClasse'", "Ok"); }
                        }
                        if (campo.UnidMan != null)
                        {
                            try { worksheet.Range["V" + i].Text = campo.UnidMan; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'UnidMan'", "Ok"); }
                        }
                        try { worksheet.Range["W" + i].Number = campo.LatitudeLocG; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'LatitudeLocG'", "Ok"); }
                        try { worksheet.Range["X" + i].Number = campo.LatitudeLocM; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'LatitudeLocM'", "Ok"); }
                        try { worksheet.Range["Y" + i].Number = double.Parse(campo.LatitudeLocS.ToString("#.00")); }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'LatitudeLocS'", "Ok"); }
                        try { worksheet.Range["Z" + i].Number = campo.LongitudeLocG; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'LongitudeLocG'", "Ok"); }
                        try { worksheet.Range["AA" + i].Number = campo.LongitudeLocM; }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'LongitudeLocM'", "Ok"); }
                        try { worksheet.Range["AB" + i].Number = double.Parse(campo.LongitudeLocS.ToString("#.00")); }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'LongitudeLocS'", "Ok"); }
                        try { worksheet.Range["AC" + i].Number = double.Parse(campo.UTMN.ToString("#.00")); }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'UTMN'", "Ok"); }
                        try { worksheet.Range["AD" + i].Number = double.Parse(campo.UTME.ToString("#.00")); }
                        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'UTME'", "Ok"); }
                        if (campo.Datum != null)
                        {
                            try { worksheet.Range["AE" + i].Text = campo.Datum; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Datum'", "Ok"); }
                        }
                        if (campo.Zona != null)
                        {
                            try { worksheet.Range["AF" + i].Text = campo.Zona; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Zona'", "Ok"); }
                        }
                        if (campo.Lat != null)
                        {
                            try { worksheet.Range["AG" + i].Text = campo.Lat; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Lat'", "Ok"); }
                        }
                        if (campo.Lon != null)
                        {
                            try { worksheet.Range["AH" + i].Text = campo.Lon; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Lon'", "Ok"); }
                        }
                        if (campo.Observacoes != null)
                        {
                            try { worksheet.Range["AI" + i].Text = campo.Observacoes; }
                            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Observacoes'", "Ok"); }
                        }
                    }
                }

                ////Acrescenta a planilha de amostragem.
                //IWorksheet worksheet1 = workbook.Worksheets[1];
                //worksheet1.Name = "Amostragem";

                ////Enter values to the cells from A3 to A5
                //worksheet1.Range["A1"].Text = "PKAmostragemMN"; //PKAmostragemMN
                //worksheet1.Range["B1"].Text = "PKTalhoesCR";//PKTalhoesCR
                //worksheet1.Range["C1"].Text = "Código da amostragem SR";//CodAmostragemMNSR
                //worksheet1.Range["D1"].Text = "Código da amostragem MN";//CodAmostragemMNMN
                //worksheet1.Range["E1"].Text = "Identif. amostragem";//IdAmostragem
                //worksheet1.Range["F1"].Text = "Data da amostragem";//DataAmost
                //worksheet1.Range["G1"].Text = "Objetivo";//Objetivo
                //worksheet1.Range["H1"].Text = "Núm de parcelas";//NParcelas
                //worksheet1.Range["I1"].Text = "Núm de árvores/parc.";//NArvParc
                //worksheet1.Range["J1"].Text = "Responsável";//Responsavel
                //worksheet1.Range["K1"].Text = "Observações";//Observacoes

                ////Formata fonte da linha de títulos
                //worksheet1.Range["A1:K1"].CellStyle.Font.Bold = true;
                //worksheet1.Range["A1:K1"].CellStyle.Font.Italic = true;
                //worksheet1.Range["A1:D1"].CellStyle.Font.RGBColor = Color.FromArgb(200, 50, 50);

                ////Colore as células A1 a D1
                //worksheet1.Range["A1:D1"].CellStyle.Color = Color.FromArgb(150, 150, 150);

                ////Borda inferior da linha de títulos
                //worksheet1.Range["A1:K1"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                //worksheet1.Range["A1:K1"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].Color = ExcelKnownColors.Black;

                ////Ajusta a largura das colunas
                //worksheet1.Range["A1:K1"].AutofitColumns();

                //sql = $"SELECT * FROM AmostragemMN";
                //try { conn.Query<AmostragemMN>(sql); }
                //catch { DisplayAlert("Message", "Não foi possível abrir a tabela AmostragemMN.", "Ok"); }
                //sql.First();

                //var qryAmostragem = conn.Query<AmostragemMN>("SELECT * FROM AmostragemMN");
                ////qryAmostragem.First();
                ////if (qryAmostragem.Count() > 0)
                ////{
                ////    //
                ////}

                //var ListaAmostragem = conn.Table<AmostragemMN>();

                //if (ListaAmostragem.Count() > 0)
                //{
                //    int i = 1;
                //    foreach (var campo in ListaAmostragem)
                //    {
                //        i = i + 1;

                //        try { worksheet1.Range["A" + i].Number = campo.PKAmostragemMN; }
                //        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PKAmostragemMN'", "Ok"); }
                //        try { worksheet1.Range["B" + i].Number = campo.PKTalhoesCR; }
                //        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PKTalhoesCR'", "Ok"); }
                //        try { worksheet1.Range["C" + i].Number = campo.CodAmostragemMNSR; }
                //        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodAmostragemMNSR'", "Ok"); }
                //        try { worksheet1.Range["D" + i].Number = campo.CodAmostragemMNMN; }
                //        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodAmostragemMNMN'", "Ok"); }
                //        if (campo.IdAmostragem != null)
                //        {
                //            try { worksheet1.Range["E" + i].Text = campo.IdAmostragem; }
                //            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'IdAmostragem'", "Ok"); }
                //        }
                //        if (campo.DataAmost != null)
                //        {
                //            try { worksheet1.Range["F" + i].Value = campo.DataAmost; }
                //            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'DataAmost'", "Ok"); }
                //        }
                //        if (campo.Objetivo != null)
                //        {
                //            try { worksheet1.Range["G" + i].Text = campo.Objetivo; }
                //            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Objetivo'", "Ok"); }
                //        }
                //        try { worksheet1.Range["H" + i].Number = campo.NParcelas; }
                //        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'NParcelas'", "Ok"); }
                //        try { worksheet1.Range["I" + i].Number = campo.NArvParc; }
                //        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'NArvParc'", "Ok"); }
                //        if (campo.Responsavel != null)
                //        {
                //            try { worksheet1.Range["J" + i].Text = campo.Responsavel; }
                //            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Responsavel'", "Ok"); }
                //        }
                //        if (campo.Observacoes != null)
                //        {
                //            try { worksheet1.Range["K" + i].Text = campo.Observacoes; }
                //            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Observacoes'", "Ok"); }
                //        }
                //    }
                //}


                ////Acrescenta a planilha de medições.
                //IWorksheet worksheet2 = workbook.Worksheets[2];
                //worksheet2.Name = "Medições";

                ////Enter values to the cells from A3 to A5
                //worksheet2.Range["A1"].Text = "PKMedicaoArv"; //PKMedicaoArv
                //worksheet2.Range["B1"].Text = "Código da amostragem MN";//PKAmostragemMN
                //worksheet2.Range["C1"].Text = "Código da medição SR";//CodMedicaoArvSR
                //worksheet2.Range["D1"].Text = "Código da medição MN";//CodMedicaoArvMN
                //worksheet2.Range["E1"].Text = "Núm. da árvore";//ArvNum
                //worksheet2.Range["F1"].Text = "Tipo med. diâmétrica";//TipoMedD
                //worksheet2.Range["G1"].Text = "Altura total (m)";//HTotal
                //worksheet2.Range["H1"].Text = "DAP/CAP (cm)";//MedD
                //worksheet2.Range["I1"].Text = "Altura dom. (m)";//HDom
                //worksheet2.Range["J1"].Text = "Altura do fuste (m)";//HFuste
                //worksheet2.Range["K1"].Text = "Observações";//Observacoes

                ////Formata fonte da linha de títulos
                //worksheet2.Range["A1:K1"].CellStyle.Font.Bold = true;
                //worksheet2.Range["A1:K1"].CellStyle.Font.Italic = true;
                //worksheet2.Range["A1:D1"].CellStyle.Font.RGBColor = Color.FromArgb(200, 50, 50);

                ////Colore as células A1 a D1
                //worksheet2.Range["A1:D1"].CellStyle.Color = Color.FromArgb(150, 150, 150);

                ////Borda inferior da linha de títulos
                //worksheet2.Range["A1:K1"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                //worksheet2.Range["A1:K1"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].Color = ExcelKnownColors.Black;

                ////Ajusta a largura das colunas
                //worksheet2.Range["A1:K1"].AutofitColumns();



                //sql = $"SELECT * FROM MedicaoArvore";
                //try { conn.Query<MedicaoArvore>(sql); }
                //catch { DisplayAlert("Message", "Não foi possível abrir a tabela MedicaoArvore.", "Ok"); }
                //sql.First();

                //var qryMedicoes = conn.Query<MedicaoArvore>("SELECT * FROM MedicaoArvore");
                ////qryMedicoes.First();
                ////if (qryMedicoes.Count() > 0)
                ////{
                ////    //
                ////}

                //var ListaMedicoes = conn.Table<MedicaoArvore>();

                //if (ListaMedicoes.Count() > 0)
                //{
                //    int i = 1;
                //    foreach (var campo in ListaMedicoes)
                //    {
                //        i = i + 1;

                //        try { worksheet2.Range["A" + i].Number = campo.PKMedicaoArv; }
                //        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PKMedicaoArv'", "Ok"); }
                //        try { worksheet2.Range["B" + i].Number = campo.PKAmostragemMN; }
                //        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PKAmostragemMN'", "Ok"); }
                //        try { worksheet2.Range["C" + i].Number = campo.CodMedicaoArvSR; }
                //        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodMedicaoArvSR'", "Ok"); }
                //        try { worksheet2.Range["D" + i].Number = campo.CodMedicaoArvMN; }
                //        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodMedicaoArvMN'", "Ok"); }
                //        try { worksheet2.Range["E" + i].Number = campo.ArvNum; }
                //        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'ArvNum'", "Ok"); }
                //        if (campo.TipoMedD != null)
                //        {
                //            try { worksheet2.Range["F" + i].Text = campo.TipoMedD; }
                //            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'TipoMedD'", "Ok"); }
                //        }
                //        try { worksheet.Range["G" + i].Number = campo.HTotal; }
                //        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'HTotal'", "Ok"); }
                //        try { worksheet.Range["H" + i].Number = campo.MedD; }
                //        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'MedD'", "Ok"); }
                //        try { worksheet.Range["I" + i].Number = campo.HDom; }
                //        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'HDom'", "Ok"); }
                //        try { worksheet.Range["J" + i].Number = campo.HFuste; }
                //        catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'HFuste'", "Ok"); }
                //        if (campo.Observacoes != null)
                //        {
                //            try { worksheet2.Range["K" + i].Text = campo.Observacoes; }
                //            catch { DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Observacoes'", "Ok"); }
                //        }
                //    }
                //}

                //Save the workbook to stream in xlsx format. 
                MemoryStream stream = new MemoryStream();
                workbook.SaveAs(stream);

                string data = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                workbook.Close();

                //Save the stream as a file in the device and invoke it for viewing
                Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("Talhoes_" + data + ".xlsx", "application/msexcel", stream);
                //Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("Talhoes.xlsx", "application/msexcel", stream);

            }
        }

        private void SearchConteudo_TextChanged(object sender, TextChangedEventArgs e)
        {
            var keyword = SearchConteudo.Text;
            if (keyword.Length >= 1)
            {
                //var sugestao = fazendas.Where(c => c.ToLower().Contains(keyword.ToLower()));
                var sugestao = fazendas.Where(c => c.ToLower().StartsWith(keyword.ToLower()));
                lstFazendas.ItemsSource = sugestao;
                lstFazendas.IsVisible = true;
            }
            else
            {
                lstFazendas.ItemsSource = fazendas;
            }
        }

        private void NovoTalhoesCR_Clicked(object sender, EventArgs e)
        {

            //if (filtrado == true)
            //{
            //    var opcao = await DisplayAlert("Novo talhão", "O filtro encontra-se ativado, antes de inserir novo talhão, o filtro será desativado. " +
            //        "Confirma a desativação do filtro? ", "Ok", "Cancel");
            //    if (opcao)
            //    {
            //        await Navigation.PushAsync(new pgAddTalhoesCR(null));
            //        var qryTalhoes = conn.Query<TalhoesCR>("SELECT * FROM TalhoesCR");
            //        ToolBarItem2.IconImageSource = "ic_action_search.png";
            //        filtrado = false;
            //    }
            //}
            //else
            //{
            Navigation.PushAsync(new pgAddTalhoesCR(null));
            //}

        }

        private void ImportarTalhoesCR_Clicked(object sender, EventArgs e)
        {
/*
            try
            {
                //Instantiate the spreadsheet creation engine
                using (ExcelEngine excelEngine = new ExcelEngine())
                {


                    //Initialize application
                    IApplication app = excelEngine.Excel;

                    //Set default application version as Excel 2016
                    app.DefaultVersion = ExcelVersion.Excel2016;

                    //var backingFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "count.txt");

                    //string diretorioraiz = Android.OS.Environment.ExternalStorageDirectory.ToString();

                    //string diretorioraiz = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).ToString();

                    //string diretorioraiz = Android.App.Application.Context.GetExternalFilesDir("").AbsolutePath +"/Signus";
                    string diretorioraiz = Android.App.Application.Context.GetExternalFilesDir("").AbsolutePath;

                    //string diretorioraiz = "/storage/emulated/0/Android/media";
                    //string diretorioraiz = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).ToString();

                    //string file = Path.Combine(diretorio, "Cadastro.xlsx");

                    //Verifica a existência do diretório externo do sistema
                    Java.IO.File diretoriosistema = new Java.IO.File(diretorioraiz);
                    //Java.IO.File diretoriosistema = new Java.IO.File(diretorioraiz + "/Signus");
                    if (diretoriosistema.Exists() == false)
                    {
                        //Cria o diretório externo do sistema caso ele não exista
                        diretoriosistema.Mkdir();
                    }
                    Java.IO.File file = new Java.IO.File(diretorioraiz + "/", "Cadastro.xlsx");
                    //Java.IO.File file = new Java.IO.File(diretorioraiz + "/Signus/", "Cadastro.xlsx");

                    //string caminho = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                    //DisplayAlert("Message 1", caminho, "Ok");

                    //if (backingFile == null || !File.Exists(backingFile))
                    //{
                    //    return 0;
                    //}

                    if (file.Exists())
                    {
                        //Obs.: foi necessário instalar um aplicativo (File Manager) no celular para identificar o caminho do arquivo.
                        //Esta linha abaixo não está sendo permitida no Android 11



                        FileStream fileStream = new FileStream(diretorioraiz + "/Cadastro.xlsx", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        //FileStream fileStream = new FileStream(diretorioraiz + "/Signus/Cadastro.xlsx", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        IWorkbook workbook = app.Workbooks.Open(fileStream);

                        if (Android.OS.Environment.MediaMounted.Equals(diretorioraiz))
                        {
                            //
                            //DisplayAlert("Message", "Teste..........", "Ok");
                        }
                        else
                        {
                            //Snackbar.Make(FindViewById(Android.Resource.Id.Content), "Permission needed!", Snackbar.LengthIndefinite)
                            //        .SetAction("Settings", new MyOnClickListener(this)).Show();
                        }

                        //Access the first worksheet
                        IWorksheet worksheet = workbook.Worksheets[0];

                        int n = 2;
                        if (worksheet["D" + n].Value.ToString() == "")
                        {
                            DisplayAlert("Message", "É necessário preencher a coluna CodCadastro.", "Ok");
                            return;
                        }
                        while (worksheet["D" + n].Value.ToString() != "")
                        {
                            //Realiza a leitura da planilha linha por linha
                            int PKTalhoesCR = (int)worksheet["A" + n].Number;
                            int CodTalhoesCRSR = (int)worksheet["B" + n].Number;
                            int CodLocal = (int)worksheet["C" + n].Number;
                            int CodCadastro = (int)worksheet["D" + n].Number;
                            string CodEPTCR = worksheet["E" + n].Value.ToString();
                            string Empresa = worksheet["F" + n].Value.ToString();
                            string Unidade = worksheet["G" + n].Value.ToString();
                            string Fazenda = worksheet["H" + n].Value.ToString();
                            string Talhao = worksheet["I" + n].Value.ToString();
                            string SubTalhao = worksheet["J" + n].Value.ToString();
                            int Ciclo = (int)worksheet["K" + n].Number;
                            int Rotacao = (int)worksheet["L" + n].Number;
                            float EspEL = (float)worksheet["M" + n].Number;
                            float EspEP = (float)worksheet["N" + n].Number;
                            DateTime DataIniRot = worksheet["O" + n].DateTime;
                            float AreaTalhao = (float)worksheet["P" + n].Number;
                            float AreaSubTalhao = (float)worksheet["Q" + n].Number;
                            string Especie = worksheet["R" + n].Value.ToString();
                            string MatGen = worksheet["S" + n].Value.ToString();
                            string TipoPropag = worksheet["T" + n].Value.ToString();
                            string ClasseSolo = worksheet["U" + n].Value.ToString();
                            int CodUnidManejo = (int)worksheet["V" + n].Number;
                            int LatitG = (int)worksheet["W" + n].Number;
                            int LatitM = (int)worksheet["X" + n].Number;
                            float LatitS = (float)worksheet["Y" + n].Number;
                            int LongitG = (int)worksheet["Z" + n].Number;
                            int LongitM = (int)worksheet["AA" + n].Number;
                            float LongitS = (float)worksheet["AB" + n].Number;
                            float UTM_N = (float)worksheet["AC" + n].Number;
                            float UTM_E = (float)worksheet["AD" + n].Number;
                            string Datum = worksheet["AE" + n].Value.ToString();
                            string Zona = worksheet["AF" + n].Value.ToString();
                            string Lat_NS = worksheet["AG" + n].Value.ToString();
                            string Lon_EW = worksheet["AH" + n].Value.ToString();
                            string Observacoes = worksheet["AI" + n].Value.ToString();
                            if (PKTalhoesCR > 0)
                            {
                                //Verifica se o registro já existe no BD do celular
                                var codReg = conn.Query<TalhoesCR>($"SELECT PKTalhoesCR FROM TalhoesCR WHERE PKTalhoesCR = '{PKTalhoesCR}'");
                                if (codReg.Count() == 0)
                                {
                                    //Insere registro com novo código PKTalhoesCR, se ele não existir
                                    string sql = $"INSERT INTO TalhoesCR (" +
                                        $"PKTalhoesCR" +
                                        $", CodTalhoesCRSR" +
                                        $", CodInform_Local" +
                                        $", CodCadastro" +
                                        $", CodEPTCR" +
                                        $", Empresa" +
                                        $", Unidade" +
                                        $", Fazenda" +
                                        $", Talhao" +
                                        $", Subtalhao" +
                                        $", Ciclo" +
                                        $", Rotacao";
                                    if (float.IsNaN(EspEL) == false) { sql += $", EspEL"; }
                                    if (float.IsNaN(EspEP) == false) { sql += $", EspEP"; }
                                    sql += $", DataIniRot";
                                    if (float.IsNaN(AreaTalhao) == false) { sql += $", AreaTalh"; }
                                    if (float.IsNaN(AreaSubTalhao) == false) { sql += $", AreaSubtalh"; }
                                    sql += $", Especie" +
                                        $", MatGen" +
                                        $", Propag" +
                                        $", SoloClasse" +
                                        $", UnidMan" +
                                        $", LatitudeLocG" +
                                        $", LatitudeLocM";
                                    if (float.IsNaN(LatitS) == false) { sql += $", LatitudeLocS"; }
                                    sql += $", LongitudeLocG" +
                                        $", LongitudeLocM";
                                    if (float.IsNaN(LongitS) == false) { sql += $", LongitudeLocS"; }
                                    if (float.IsNaN(UTM_N) == false) { sql += $", UTMN"; }
                                    if (float.IsNaN(UTM_E) == false) { sql += $", UTME"; }
                                    sql += $", Datum" +
                                        $", Zona" +
                                        $", Lat" +
                                        $", Lon" +
                                        $", Observacoes" +
                                        $") VALUES (" +
                                        $"{PKTalhoesCR}" +
                                        $", {CodTalhoesCRSR}" +
                                        $", {CodLocal}" +
                                        $", {CodCadastro}" +
                                        $", '{CodEPTCR}'" +
                                        $", '{Empresa}'" +
                                        $", '{Unidade}'" +
                                        $", '{Fazenda}'" +
                                        $", '{Talhao}'" +
                                        $", '{SubTalhao}'" +
                                        $", {Ciclo}" +
                                        $", {Rotacao}";
                                    if (float.IsNaN(EspEL) == false) { sql += $", {Math.Round(EspEL * 100)}*1.0/100"; }
                                    if (float.IsNaN(EspEP) == false) { sql += $", {Math.Round(EspEP * 100)}*1.0/100"; }
                                    sql += $", '{DataIniRot.Date:dd/MM/yyyy}'";
                                    if (float.IsNaN(AreaTalhao) == false) { sql += $", {Math.Round(AreaTalhao * 100)}*1.0/100"; }
                                    if (float.IsNaN(AreaSubTalhao) == false) { sql += $", {Math.Round(AreaSubTalhao * 100)}*1.0/100"; }
                                    sql += $", '{Especie}'" +
                                        $", '{MatGen}'" +
                                        $", '{TipoPropag}'" +
                                        $", '{ClasseSolo}'" +
                                        $", '{CodUnidManejo}'" +
                                        $", {LatitG}" +
                                        $", {LatitM}";
                                    if (float.IsNaN(LatitS) == false) { sql += $", {Math.Round(LatitS * 1000)}*1.0/1000"; }
                                    sql += $", {LongitG}" +
                                        $", {LongitM}";
                                    if (float.IsNaN(LongitS) == false) { sql += $", {Math.Round(LongitS * 1000)}*1.0/1000"; }
                                    if (float.IsNaN(UTM_N) == false) { sql += $", {Math.Round(UTM_N * 1000)}*1.0/1000"; }
                                    if (float.IsNaN(UTM_E) == false) { sql += $", {Math.Round(UTM_E * 1000)}*1.0/1000"; }
                                    sql += $", '{Datum}'" +
                                        $", '{Zona}'" +
                                        $", '{Lat_NS}'" +
                                        $", '{Lon_EW}'" +
                                        $", '{Observacoes}'";
                                    sql += $") ";
                                    try { conn.Execute(sql); }
                                    catch
                                    {
                                        DisplayAlert("Message", "Não foi possível inserir o registro de código:" + PKTalhoesCR.ToString() + ",  da linha " + n.ToString() + " da planilha. Verifique a planilha Cadastro.", "Ok");
                                    }
                                }
                                else
                                {
                                    //Registro já está no banco de dados
                                    //Atualiza o registro com mesmo código PKTalhoesCR
                                    string sql = $"UPDATE TalhoesCR SET " +
                                        $"CodTalhoesCRSR = {CodTalhoesCRSR}" +
                                        $", CodInform_Local = {CodLocal}" +
                                        $", CodCadastro = {CodCadastro}" +
                                        $", CodEPTCR = '{CodEPTCR}'" +
                                        $", Empresa = '{Empresa}'" +
                                        $", Unidade ='{Unidade}'" +
                                        $", Fazenda = '{Fazenda}'" +
                                        $", Talhao = '{Talhao}'" +
                                        $", Subtalhao = '{SubTalhao}'" +
                                        $", Ciclo = {Ciclo}" +
                                        $", Rotacao = {Rotacao}";
                                    if (float.IsNaN(EspEL) == false) { sql += $", EspEL = {Math.Round(EspEL * 100)}*1.0/100"; }
                                    if (float.IsNaN(EspEP) == false) { sql += $", EspEP = {Math.Round(EspEP * 100)}*1.0/100"; }
                                    sql += $", DataIniRot = '{DataIniRot.Date:dd/MM/yyyy}'";
                                    if (float.IsNaN(AreaTalhao) == false) { sql += $", AreaTalh = {Math.Round(AreaTalhao * 100)}*1.0/100"; }
                                    if (float.IsNaN(AreaSubTalhao) == false) { sql += $", AreaSubtalh = {Math.Round(AreaSubTalhao * 100)}*1.0/100"; }
                                    sql += $", Especie = '{Especie}'" +
                                        $", MatGen = '{MatGen}'" +
                                        $", Propag = '{TipoPropag}'" +
                                        $", SoloClasse = '{ClasseSolo}'" +
                                        $", UnidMan = '{CodUnidManejo}'" +
                                        $", LatitudeLocG = {LatitG}" +
                                        $", LatitudeLocM = {LatitM}";
                                    if (float.IsNaN(LatitS) == false) { sql += $", LatitudeLocS = {Math.Round(LatitS * 1000)}*1.0/1000"; }
                                    sql += $", LongitudeLocG = {LongitG}" +
                                        $", LongitudeLocM = {LongitM}";
                                    if (float.IsNaN(LongitS) == false) { sql += $", LongitudeLocS = {Math.Round(LongitS * 1000)}*1.0/1000"; }
                                    if (float.IsNaN(UTM_N) == false) { sql += $", UTMN = ({Math.Round(UTM_N * 1000)})*1.0/1000"; }
                                    if (float.IsNaN(UTM_E) == false) { sql += $", UTME = ({Math.Round(UTM_E * 1000)})*1.0/1000"; }
                                    sql += $", Datum = '{Datum}'" +
                                        $", Zona = '{Zona}'" +
                                        $", Lat = '{Lat_NS}'" +
                                        $", Lon = '{Lon_EW}'" +
                                        $", Observacoes = '{Observacoes}'" +
                                        $" WHERE PKTalhoesCR = {PKTalhoesCR}";
                                    try { conn.Execute(sql); }
                                    catch
                                    {
                                        DisplayAlert("Message 2", "Não foi possível atualizar o regisgro de código:" + PKTalhoesCR.ToString() + ",  da linha " + n.ToString() + " da planilha. Verifique o valor digitado na planilha Cadastro.", "Ok");
                                    }
                                }
                            }
                            else
                            {
                                //Insere registro com novo código PKTalhoesCR
                                string sql = $"INSERT INTO TalhoesCR (" +
                                        $"CodTalhoesCRSR" +
                                        $", CodInform_Local" +
                                        $", CodCadastro" +
                                        $", CodEPTCR" +
                                        $", Empresa" +
                                        $", Unidade" +
                                        $", Fazenda" +
                                        $", Talhao" +
                                        $", Subtalhao" +
                                        $", Ciclo" +
                                        $", Rotacao";
                                if (float.IsNaN(EspEL) == false) { sql += $", EspEL"; }
                                if (float.IsNaN(EspEP) == false) { sql += $", EspEP"; }
                                sql += $", DataIniRot";
                                if (float.IsNaN(AreaTalhao) == false) { sql += $", AreaTalh"; }
                                if (float.IsNaN(AreaSubTalhao) == false) { sql += $", AreaSubtalh"; }
                                sql += $", Especie" +
                                    $", MatGen" +
                                    $", Propag" +
                                    $", SoloClasse" +
                                    $", UnidMan" +
                                    $", LatitudeLocG" +
                                    $", LatitudeLocM";
                                if (float.IsNaN(LatitS) == false) { sql += $", LatitudeLocS"; }
                                sql += $", LongitudeLocG" +
                                    $", LongitudeLocM";
                                if (float.IsNaN(LongitS) == false) { sql += $", LongitudeLocS"; }
                                if (float.IsNaN(UTM_N) == false) { sql += $", UTMN"; }
                                if (float.IsNaN(UTM_E) == false) { sql += $", UTME"; }
                                sql += $", Datum" +
                                    $", Zona" +
                                    $", Lat" +
                                    $", Lon" +
                                    $", Observacoes" +
                                    $") VALUES(" +
                                    $"{CodTalhoesCRSR}" +
                                    $", {CodLocal}" +
                                    $", {CodCadastro}" +
                                    $", '{CodEPTCR}'" +
                                    $", '{Empresa}'" +
                                    $", '{Unidade}'" +
                                    $", '{Fazenda}'" +
                                    $", '{Talhao}'" +
                                    $", '{SubTalhao}'" +
                                    $", {Ciclo}" +
                                    $", {Rotacao}";
                                if (float.IsNaN(EspEL) == false) { sql += $", {Math.Round(EspEL * 100)}*1.0/100"; }
                                if (float.IsNaN(EspEP) == false) { sql += $", {Math.Round(EspEP * 100)}*1.0/100"; }
                                sql += $", '{DataIniRot.Date:dd/MM/yyyy}'";
                                if (float.IsNaN(AreaTalhao) == false) { sql += $", {Math.Round(AreaTalhao * 100)}*1.0/100"; }
                                if (float.IsNaN(AreaSubTalhao) == false) { sql += $", {Math.Round(AreaSubTalhao * 100)}*1.0/100"; }
                                sql += $", '{Especie}'" +
                                    $", '{MatGen}'" +
                                    $", '{TipoPropag}'" +
                                    $", '{ClasseSolo}'" +
                                    $", '{CodUnidManejo}'" +
                                    $", {LatitG}" +
                                    $", {LatitM}";
                                if (float.IsNaN(LatitS) == false) { sql += $", {Math.Round(LatitS * 1000)}*1.0/1000"; }
                                sql += $", {LongitG}" +
                                    $", {LongitM}";
                                if (float.IsNaN(LongitS) == false) { sql += $", {Math.Round(LongitS * 1000)}*1.0/1000"; }
                                if (float.IsNaN(UTM_N) == false) { sql += $", {Math.Round(UTM_N * 1000)}*1.0/1000"; }
                                if (float.IsNaN(UTM_E) == false) { sql += $", {Math.Round(UTM_E * 1000)}*1.0/1000"; }
                                sql += $", '{Datum}'" +
                                    $", '{Zona}'" +
                                    $", '{Lat_NS}'" +
                                    $", '{Lon_EW}'" +
                                    $", '{Observacoes}'";
                                sql += $") ";
                                try { conn.Execute(sql); }
                                catch
                                {
                                    DisplayAlert("Message 3", "Não foi possível inserir novo registro (Fazenda: " + Fazenda + "; Talhão: " + Talhao + "),  da linha " + n.ToString() + " da planilha. Verifique a planilha Cadastro.", "Ok");
                                }
                            }
                            n++;
                        }
                        DisplayAlert("Message", "Importação concluída.", "Ok");
                    }
                    else
                    {
                        DisplayAlert("Message", "O arquivo Cadastro.xlsx não foi encontrado. Copie o arquivo para a pasta " +
                            "Android/data/com.signus.signus/files e tente novamente. Entre em contato com a " +
                            "T&M Consultoria Agroflorestal LTDA para mais informações.", "Ok");
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Erro: ", ex.Message, "Ok");
            }
*/
        }

        private void lstFazendas_ItemTapped(object sender, ItemTappedEventArgs e)
        {

            if (e.Item as string == null)
            {
                return;
            }
            else
            {
                lstFazendas.ItemsSource = fazendas.Where(c => c.Equals(e.Item as string));
                lstFazendas.IsVisible = true;
                SearchConteudo.Text = e.Item as string;
                fazenda = e.Item as string;
                var listat = conn.Query<TalhoesCR>("SELECT * FROM TalhoesCR Where Fazenda = '" + fazenda + "'");
                ToolBarItem2.IconImageSource = "ic_action_unsearch.png";
                lstTalhoesCR.ItemsSource = listat;
                SearchConteudo.IsVisible = false;
                lstFazendas.IsVisible = false;
                btnNovoTalhao.IsVisible = true;
                ImportarTalhoes.IsVisible = true;
                lstTalhoesCR.IsVisible = true;
                filtrado = true;
                ToolBarItem2.IconImageSource = "ic_action_unsearch.png";
            }

        }

        private async void DeleteTalhoesCR(object sender, EventArgs e)
        {

            var mi = ((MenuItem)sender);
            var talhao = mi.CommandParameter as TalhoesCR;
            if (talhao != null)
            {
                int namostmn = 0;
                int ncontform = 0;
                int nplantio = 0;
                int nprepsolo = 0;
                int ncapquim = 0;
                int nfertiliz = 0;

                try
                {
                    var qryAmostMN = conn.Query<AmostragemMN>($"SELECT * FROM AmostragemMN WHERE PKTalhoesCR = {talhao.PKTalhoesCR}");
                    if (qryAmostMN.Count() > 0)
                    {
                        var opcao = await DisplayAlert("Excluir amostragens", $"Confirma a exclusão de registros de amostragens associados a este talhão? ", "Ok", "Cancel");
                        if (opcao)
                        {
                            //pgamostragem.DeletarAmostragemMN(0, talhao.PKTalhoesCR);
                            //Deleta todos os arquivos de parcelas para todas as amostragens de um determinado talhão
                            foreach (var campoam in qryAmostMN)
                            {
                                //pgparcelas.DeleteParcelasMedArv(0, campo.PKAmostragemMN); 
                                //Deleta todos os arquivos de medição para todas as parcelas de uma determinada amostragem
                                string sqlparc = $"SELECT * FROM ParcelasMedArv WHERE PKAmostragemMN = {campoam.PKAmostragemMN}";
                                var qryParcelas = conn.Query<ParcelasMedArv>(sqlparc);
                                if (qryParcelas.Count() > 0)
                                {
                                    foreach (var campopar in qryParcelas)
                                    {
                                        //Deleta registros da tabela MedicaoArvore1 com base no código da parcela (PKParcelasMedArv)
                                        string sql = $"DELETE FROM MedicaoArvore1 WHERE PKParcelasMedArv = {campopar.PKParcelasMedArv}";
                                        try { conn.Execute(sql); }
                                        catch { await DisplayAlert("Message", "Houve um erro. Não foi possível excluir o(s) registro(s) de medição associados à parcela.", "Ok"); }
                                    }
                                }
                                //Deleta o registro de parcela
                                string sqlparc2 = $"DELETE FROM ParcelasMedArv WHERE PKAmostragemMN = {campoam.PKAmostragemMN}";
                                try { conn.Execute(sqlparc2); }
                                catch { await DisplayAlert("Message", "Houve um erro, não foi possível excluir o(s) registro(s) de parcela(s) associados à amostragem.", "Ok"); }
                            }
                            //Deleta o registro da amostragem
                            string sqlamost2 = $"DELETE FROM AmostragemMN WHERE PKTalhoesCR = {talhao.PKTalhoesCR}";
                            try { conn.Execute(sqlamost2); }
                            catch { await DisplayAlert("Message", "Houve um erro, não foi possível excluir o(s) registro(s) de amostragens associados ao talhão.", "Ok"); }
                        }
                    }
                    //Deleta o registro de talhão
                    //string sqltalh = $"DELETE FROM TalhoesCR WHERE PKTalhoesCR = {talhao.PKTalhoesCR}";
                    //try { conn.Execute(sqltalh); }
                    //catch { await DisplayAlert("Message", "Houve um erro, não foi possível excluir o registro de talhão.", "Ok"); }
                }
                catch { namostmn = 0; };

                try
                {
                    var qryAmostMN = conn.Query<AmostragemMN>($"SELECT * FROM AmostragemMN WHERE PKTalhoesCR = {talhao.PKTalhoesCR}");
                    if (qryAmostMN.Count() > 0) { namostmn = qryAmostMN.Count(); }
                }
                catch { namostmn = 0; };

                try
                {
                    var qryContForm = conn.Query<CFormigasOP>($"SELECT PKCFormigasOP FROM CFormigasOP WHERE PKTalhoesCR = {talhao.PKTalhoesCR}");
                    if (qryContForm.Count() > 0) { ncontform = qryContForm.Count(); }
                }
                catch { ncontform = 0; };

                try
                {
                    var qryPlantio = conn.Query<PlantioOP>($"SELECT PKPlantioOP FROM PlantioOP WHERE PKTalhoesCR = {talhao.PKTalhoesCR}");
                    if (qryPlantio.Count() > 0) { nplantio = qryPlantio.Count(); }
                }
                catch { nplantio = 0; };

                try
                {
                    var qryPrepSolo = conn.Query<PrepSoloOP>($"SELECT PKPrepSoloOP FROM PrepSoloOP WHERE PKTalhoesCR = {talhao.PKTalhoesCR}");
                    if (qryPrepSolo.Count() > 0) { nprepsolo = qryPrepSolo.Count(); }
                }
                catch { nprepsolo = 0; };

                try
                {
                    var qryCapQuim = conn.Query<CapinasQuimOP>($"SELECT PKCapinasQuimOP FROM CapinasQuimOP WHERE PKTalhoesCR = {talhao.PKTalhoesCR}");
                    if (qryCapQuim.Count() > 0) { ncapquim = qryCapQuim.Count(); }
                }
                catch { ncapquim = 0; };

                try
                {
                    var qryFertiliz = conn.Query<FertilizacoesOP>($"SELECT PKFertilizacoesOP FROM FertilizacoesOP WHERE PKTalhoesCR = {talhao.PKTalhoesCR}");
                    if (qryFertiliz.Count() > 0) { nfertiliz = qryFertiliz.Count(); }
                }
                catch { nfertiliz = 0; };

                if ((namostmn > 0) || (ncontform > 0) || (nplantio > 0) || (nprepsolo > 0) || (ncapquim > 0) || (nfertiliz > 0))
                {
                    await DisplayAlert("Message", "Antes de excluir este registro, exclua todos os registros associados de amostragens, contr. de formigas, plantio, etc.", "Ok");
                }
                else
                {
                    var opcao = await DisplayAlert("Excluir registro", $"Confirma a exclusão do talhão {talhao.Talhao}? ", "Ok", "Cancel");
                    if (opcao)
                    {
                        string sql = $"DELETE FROM TalhoesCR WHERE PKTalhoesCR = {talhao.PKTalhoesCR}";
                        try { conn.Execute(sql); }
                        catch { await DisplayAlert("Message", "Não foi possível atualizar. Verifique o valor digitado.", "Ok"); }

                        //sql = $"SELECT * FROM TalhoesCR";
                        if (fazenda != "")
                        {
                            sql = $"SELECT * FROM TalhoesCR Where Fazenda = '{fazenda}'";
                            ToolBarItem2.IconImageSource = "ic_action_unsearch.png";
                        }
                        else
                        {
                            sql = $"SELECT * FROM TalhoesCR ";
                            ToolBarItem2.IconImageSource = "ic_action_search.png";
                        }

                        var data = conn.Query<TalhoesCR>(sql);
                        lstTalhoesCR.ItemsSource = data;
                    }
                }
            }

        }

        private async void AlterarTalhoesCR(object sender, EventArgs e)
        {

            try
            {
                var mi = ((MenuItem)sender);
                var talhao = mi.CommandParameter as TalhoesCR;
                if (talhao != null)
                {
                    await Navigation.PushAsync(new pgAddTalhoesCR(talhao));
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro: ", ex.Message, "Ok");
            }

        }

        private async void EditarAmostragemMN(object sender, EventArgs e)
        {

            var mi = ((MenuItem)sender);
            var talhao = mi.CommandParameter as TalhoesCR;
            if (talhao != null)
            {
                //var opcao = await DisplayAlert("Editar amostragem", "Abrir edição de amostragem para o talhão " + talhao.PKTalhoesCR.ToString() + "? ", "Ok", "Cancel");
                //if (opcao)
                //{
                //string sql = $"SELECT * FROM AmostragemMN WHERE PKTalhoesCR = {talhao.PKTalhoesCR}";
                //try { conn.Query<AmostragemMN>(sql); }
                //catch 
                //{ await DisplayAlert("Message", $"Houve um erro. Não foi possível abrir a consulta.", "Ok"); }
                await Navigation.PushAsync(new pgAmostragemMN(talhao.PKTalhoesCR.ToString(), talhao.Fazenda, talhao.Talhao));
            }

        }

        private async void EditarContForm(object sender, EventArgs e)
        {

            var mi = ((MenuItem)sender);
            var talhao = mi.CommandParameter as TalhoesCR;
            if (talhao != null)
            {
                //string sql = $"SELECT * FROM CFormigasOP WHERE PKTalhoesCR = {talhao.PKTalhoesCR}";

                //try { conn.Query<CFormigasOP>(sql); }
                //catch 
                //{ await DisplayAlert("Message", $"Houve um erro. Não foi possível abrir a consulta.", "Ok"); }

                //Atualiza a página
                //Navigation.InsertPageBefore(new pgAmostragemMN(talhao.PKTalhoesCR.ToString()), this);
                //await Navigation.PopAsync();
                await Navigation.PushAsync(new pgCFormigasOP(talhao.PKTalhoesCR.ToString()));
            }

        }

        private async void EditarPlantio(object sender, EventArgs e)
        {

            var mi = ((MenuItem)sender);
            var talhao = mi.CommandParameter as TalhoesCR;
            if (talhao != null)
            {
                //string sql = $"SELECT * FROM PlantioOP WHERE PKTalhoesCR = {talhao.PKTalhoesCR}";

                //try { conn.Query<PlantioOP>(sql); }
                //catch 
                //{ await DisplayAlert("Message", $"Houve um erro. Não foi possível abrir a consulta.", "Ok"); }

                //Atualiza a página
                //Navigation.InsertPageBefore(new pgAmostragemMN(talhao.PKTalhoesCR.ToString()), this);
                //await Navigation.PopAsync();
                await Navigation.PushAsync(new pgPlantioOP(talhao.PKTalhoesCR.ToString()));
            }

        }

        private async void EditarPrepSolo(object sender, EventArgs e)
        {

            var mi = ((MenuItem)sender);
            var talhao = mi.CommandParameter as TalhoesCR;
            if (talhao != null)
            {
                //var opcao = await DisplayAlert("Editar preparo de solo", "Abrir edição de operações de preparo de solo para o talhão " + talhao.PKTalhoesCR.ToString() + "? ", "Ok", "Cancel");
                //if (opcao)
                //{
                //string sql = $"SELECT * FROM PrepSoloOP WHERE PKTalhoesCR = {talhao.PKTalhoesCR}";

                //try { conn.Query<PrepSoloOP>(sql); }
                //catch
                //{ await DisplayAlert("Message", $"Houve um erro. Não foi possível abrir a consulta.", "Ok"); }

                //Atualiza a página
                //Navigation.InsertPageBefore(new pgAmostragemMN(talhao.PKTalhoesCR.ToString()), this);
                //await Navigation.PopAsync();
                await Navigation.PushAsync(new pgPreparoSoloOP(talhao.PKTalhoesCR.ToString()));
                //}
            }

        }

        private async void EditarCapQuim(object sender, EventArgs e)
        {

            var mi = ((MenuItem)sender);
            var talhao = mi.CommandParameter as TalhoesCR;
            if (talhao != null)
            {
                //var opcao = await DisplayAlert("Editar capinas químicas", "Abrir edição de operações de capinas químicas para o talhão " + talhao.PKTalhoesCR.ToString() + "? ", "Ok", "Cancel");
                //if (opcao)
                //{
                //string sql = $"SELECT * FROM CapinasQuimOP WHERE PKTalhoesCR = {talhao.PKTalhoesCR}";

                //try { conn.Query<CapinasQuimOP>(sql); }
                //catch 
                //{ await DisplayAlert("Message", $"Houve um erro. Não foi possível abrir a consulta.", "Ok"); }

                //Atualiza a página
                //Navigation.InsertPageBefore(new pgAmostragemMN(talhao.PKTalhoesCR.ToString()), this);
                //await Navigation.PopAsync();
                await Navigation.PushAsync(new pgCapinasQuimOP(talhao.PKTalhoesCR.ToString()));
                //}
            }

        }

        private async void EditarFertilizaoes(object sender, EventArgs e)
        {

            var mi = ((MenuItem)sender);
            var talhao = mi.CommandParameter as TalhoesCR;
            if (talhao != null)
            {
                //var opcao = await DisplayAlert("Editar fertilizações", "Abrir edição de operações de fertilizações para o talhão " + talhao.PKTalhoesCR.ToString() + "? ", "Ok", "Cancel");
                //if (opcao)
                //{
                //string sql = $"SELECT * FROM FertilizacoesOP WHERE PKTalhoesCR = {talhao.PKTalhoesCR}";

                //try { conn.Query<CapinasQuimOP>(sql); }
                //catch 
                //{ await DisplayAlert("Message", $"Ainda não há registros de capinas químicas para este talhão.", "Ok"); }

                //Atualiza a página
                //Navigation.InsertPageBefore(new pgAmostragemMN(talhao.PKTalhoesCR.ToString()), this);
                //await Navigation.PopAsync();
                await Navigation.PushAsync(new pgFertilizacoesOP(talhao.PKTalhoesCR.ToString()));
                //}
            }

        }

        private async void AlternaDetalhes(object sender, ItemTappedEventArgs e)
        {

            //Alterna entre visível e invisível a lista de detalhes
            TalhoesCR details = e.Item as TalhoesCR;
            string sql;
            if (details.Visible.ToString() == "True")
            {
                sql = $"UPDATE TalhoesCR SET Visible = 0, Imagem = 'Down.png' ";
            }
            else
            {
                sql = $"UPDATE TalhoesCR SET Visible = 1, Imagem = 'Up.png' ";
            }

            sql += $"WHERE PKTalhoesCR = {details.PKTalhoesCR}";
            try { conn.Execute(sql); }
            catch { await DisplayAlert("Message", "Não foi possível atualizar. Verifique o valor digitado.", "Ok"); }

            //sql = $"SELECT * FROM TalhoesCR";
            if (fazenda != "")
            {
                sql = $"SELECT * FROM TalhoesCR Where Fazenda = '" + fazenda + "'";
                ToolBarItem2.IconImageSource = "ic_action_unsearch.png";
            }
            else
            {
                sql = $"SELECT * FROM TalhoesCR";
                ToolBarItem2.IconImageSource = "ic_action_search.png";
            }

            var data = conn.Query<TalhoesCR>(sql);

            //Atualiza a lista
            lstTalhoesCR.ItemsSource = data;




            //ListView lstv = this.FindByName<ListView>("lstAmostragem");

            //ListView lstv = this.FindByName<ListView>("lstAmostragem");

            //ListView lstv = (ListView)this.FindByName("lstAmostragem");
            //lstv.ItemsSource = data;


            //Label mpEmpresa = (Label)Master.FindControl("lblEmpresa");

            //Testar:
            //https://forums.xamarin.com/discussion/51073/how-to-create-a-sub-listview-inside-a-listview-with-data-binding
            //https://www.youtube.com/watch?v=f1sVcfT_UBo

        }

        private async void EditPoligono(object sender, EventArgs e)
        {

            //Abre a tela de edição de polígono do talhão
            try
            {
                var bt = (Button)sender;
                var talhao = bt.CommandParameter as TalhoesCR;
                if (talhao != null)
                {
                    //Abre a tela pgAddTesteGPS com o parâmetro PKTalhoesCR
                    await Navigation.PushAsync(new pgAddTesteGPS(talhao.PKTalhoesCR.ToString()));
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro: ", ex.Message, "Ok");
            }

        }
    }
}