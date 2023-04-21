using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SignusMobile20.Models;
using SQLite;
using Syncfusion.SfDataGrid.XForms;
using System;
using System.Collections.ObjectModel;


namespace SignusMobile20
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class pgAddMedicaoArvoreGrd : ContentPage
    {
        MedicaoArvore1 detalheMedicaoArvore1;
        private SQLiteConnection conn;
        public MedicaoArvore1 MedicaoArvore1;
        private string pkamostragem;
        public string pkparcelasmedarv;
        public string idfazenda;
        public string idtalhao;
        public string idamostragem;
        public string idparcela;
        public string filtroparc;
        private int numreg;
        string msg;
        private string sqlParcelas;
        private string sqlArvores;
        public ObservableCollection<ListaParcelas> LstParcelas { get; set; }
        public ObservableCollection<ListaParcelas> ParcelasSelec;


        //Dar uma olhada nisso:
        //https://docs.microsoft.com/pt-br/xamarin/xamarin-forms/user-interface/listview/data-and-databinding
        //https://www.youtube.com/watch?v=rdPStxCSy5w&t=190s

        public pgAddMedicaoArvoreGrd(MedicaoArvore1 details, string pkamost, string pkparc, string idfaz, string idtalh, string idamost, string idparc)
        {
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTM3Njg3QDMxMzcyZTMyMmUzMFhoRVRPYmZrZzBsNHFocEc1SVNrWEE2Q3kvQlFOR09ocEw2K1A3MHNrZzQ9");
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTU0OTAzQDMxMzkyZTM0MmUzMGhGOWRUZEN4NDBVSGx2OFFub2E4QjAySERLTHFvZmxYODUzUlU0d1hzUm89");

            InitializeComponent();
            //Retornar estas duas linhas abaixo caso dê algo errado
            conn = DependencyService.Get<ISQLite>().GetConnection();
            //conn.DropTable<MedicaoArvore1>();
            conn.CreateTable<MedicaoArvore1>();
            pkamostragem = pkamost;
            pkparcelasmedarv = pkparc;
            idfazenda = idfaz;
            idtalhao = idtalh;
            idamostragem = idamost;
            idparcela = idparc;
            filtroparc = "";

            IdFazenda.Text = "Fazenda/Proj.: " + idfaz;
            IdTalhao.Text = "Talhão: " + idtalh;
            IdAmostragemMN.Text = "Amostragem: " + idamost;
            IdParcelasMedArv.Text = "Parcela: " + idparcela;

            //Consulta na tabela ParcelasMedArv a lista de parcelas da Amostragem atual
            sqlParcelas = $"SELECT PKParcelasMedArv AS PkParcela, IdParcela, 0 AS Check1 FROM ParcelasMedArv " +
                        $"WHERE PKAmostragemMN = {pkamostragem} " +
                        $"ORDER BY PKParcelasMedArv";

            LstParcelas = new ObservableCollection<ListaParcelas>();
            LstParcelas.Clear();

            var parcls = conn.Query<ListaParcelas>(sqlParcelas);
            if (parcls.Count > 0)
            {
                foreach (var prc in parcls)
                {
                    if (pkparcelasmedarv == prc.PkParcela.ToString())
                    {
                        LstParcelas.Add(new ListaParcelas { PkParcela = prc.PkParcela, IdParcela = prc.IdParcela, Check1 = true });
                    }
                    else
                    {
                        LstParcelas.Add(new ListaParcelas { PkParcela = prc.PkParcela, IdParcela = prc.IdParcela, Check1 = false });
                    }
                }
                clvParcelas.ItemsSource = LstParcelas;
            }

            ////Atualiza a lista de medições das árvores
            sqlArvores = $"SELECT MedArv.PKMedicaoArv1, Parc.IdParcela, MedArv.PKParcelasMedArv, MedArv.ArvNum, " +
                $"MedArv.FusteNum, MedArv.TipoMedD, MedArv.HTotal, MedArv.MedD, MedArv.HDom, MedArv.HFuste, MedArv.DCopa, " +
                $"MedArv.HCopa, MedArv.Observacoes, MedArv.Selec FROM MedicaoArvore1 MedArv JOIN ParcelasMedArv Parc " +
                $"ON MedArv.PKParcelasMedArv = Parc.PKParcelasMedArv ";
            if (filtroparc != "") { sqlArvores += $"WHERE ({filtroparc}) "; }
            else { sqlArvores += $"WHERE (MedArv.PKParcelasMedArv = {pkparcelasmedarv}) "; }
            sqlArvores += $"ORDER BY MedArv.PKParcelasMedArv, MedArv.ArvNum";

            var data = conn.Query<MedicaoArvore1>(sqlArvores);
            dataGrid.ItemsSource = data;
            btnDelete.IsEnabled = false;
            numreg = data.Count;

            ParcelasSelec = new ObservableCollection<ListaParcelas>();
        }

        //List<string> parcelas = new List<string> { };

        /*
        protected override void OnAppearing()
        {

            sqlParcelas = $"SELECT PKParcelasMedArv AS PkParcela, IdParcela, 0 AS Check1 FROM ParcelasMedArv " +
                        $"WHERE PKAmostragemMN = {pkamostragem} " +
                        $"ORDER BY PKParcelasMedArv";


            var parcls = conn.Query<ListaParcelas>(sqlParcelas);
            if (parcls.Count > 0)
            {
                foreach (var prc in parcls)
                {
                    LstParcelas.Add(new ListaParcelas { PkParcela = prc.PkParcela, IdParcela = prc.IdParcela, Check1 = false });
                }
                csvParcelas.ItemsSource = LstParcelas;
            }
        }
        */

        private void SaveMedicaoArvore(object sender, EventArgs e)
        {
            dataGrid.EndEdit();

            string sql = "";
            string sql1 = "";
            string codreg = "";
            conn.BeginTransaction();

            //Atualiza o registro
            try
            {
                for (int linha = 1; linha <= numreg; linha++)
                {
                    sql = $"UPDATE MedicaoArvore1 SET ";
                    sql1 = "";

                    string cellValue = null;
                    foreach (var column in dataGrid.Columns)
                    {
                        if (column.MappingName == "PKMedicaoArv1")
                        {
                            try
                            {
                                var rowData = dataGrid.GetRecordAtRowIndex(linha);
                                cellValue = dataGrid.GetCellValue(rowData, column.MappingName).ToString();
                                if ((cellValue != "") && (cellValue != null)) { codreg = cellValue; }
                            }
                            catch { }
                        }
                        if (column.MappingName == "IdParcela")
                        {
                            try
                            {
                                var rowData = dataGrid.GetRecordAtRowIndex(linha);
                                cellValue = dataGrid.GetCellValue(rowData, column.MappingName).ToString();
                                if ((cellValue != "") && (cellValue != null)) { sql1 += $" IdParcela = '{cellValue}'"; }
                                else { sql1 += $" IdParcela = NULL"; }
                            }
                            catch { }
                        }
                        if (column.MappingName == "ArvNum")
                        {
                            try
                            {
                                var rowData = dataGrid.GetRecordAtRowIndex(linha);
                                cellValue = dataGrid.GetCellValue(rowData, column.MappingName).ToString();
                                if ((cellValue != "") && (cellValue != null)) { sql1 += $", ArvNum = {cellValue}"; }
                                else { sql1 += $", ArvNum = NULL"; }
                            }
                            catch { }
                        }
                        if (column.MappingName == "FusteNum")
                        {
                            try
                            {
                                var rowData = dataGrid.GetRecordAtRowIndex(linha);
                                cellValue = dataGrid.GetCellValue(rowData, column.MappingName).ToString();
                                if ((cellValue != "") && (cellValue != null)) { sql1 += $", FusteNum = {cellValue}"; }
                                else { sql1 += $", FusteNum = NULL"; }
                            }
                            catch { }
                        }
                        if (column.MappingName == "TipoMedD")
                        {
                            try
                            {
                                var rowData = dataGrid.GetRecordAtRowIndex(linha);
                                cellValue = dataGrid.GetCellValue(rowData, column.MappingName).ToString();
                                if ((cellValue != "") && (cellValue != null)) { sql1 += $", TipoMedD = '{cellValue}'"; }
                                else { sql1 += $", TipoMedD = NULL"; }
                            }
                            catch { }
                        }
                        if (column.MappingName == "HTotal")
                        {
                            try
                            {
                                var rowData = dataGrid.GetRecordAtRowIndex(linha);
                                cellValue = dataGrid.GetCellValue(rowData, column.MappingName).ToString();
                                if ((cellValue != "") && (cellValue != null)) { sql1 += $", HTotal = {Math.Round(double.Parse(cellValue) * 100)}*1.0/100"; }
                                else { sql1 += $", HTotal = NULL"; }
                            }
                            catch { }
                        }
                        if (column.MappingName == "MedD")
                        {
                            try
                            {
                                var rowData = dataGrid.GetRecordAtRowIndex(linha);
                                cellValue = dataGrid.GetCellValue(rowData, column.MappingName).ToString();
                                if ((cellValue != "") && (cellValue != null)) { sql1 += $", MedD = {Math.Round(float.Parse(cellValue) * 100)}*1.0/100"; }
                                else { sql1 += $", MedD = NULL"; }
                            }
                            catch { }
                        }
                        if (column.MappingName == "HDom")
                        {
                            try
                            {
                                var rowData = dataGrid.GetRecordAtRowIndex(linha);
                                cellValue = dataGrid.GetCellValue(rowData, column.MappingName).ToString();
                                if ((cellValue != "") && (cellValue != null)) { sql1 += $", HDom = {Math.Round(float.Parse(cellValue) * 100)}*1.0/100"; }
                                else { sql1 += $", HDom = NULL"; }
                            }
                            catch { }
                        }
                        if (column.MappingName == "HFuste")
                        {
                            try
                            {
                                var rowData = dataGrid.GetRecordAtRowIndex(linha);
                                cellValue = dataGrid.GetCellValue(rowData, column.MappingName).ToString();
                                if ((cellValue != "") && (cellValue != null)) { sql1 += $", HFuste = {Math.Round(float.Parse(cellValue) * 100)}*1.0/100"; }
                                else { sql1 += $", HFuste = NULL"; }
                            }
                            catch { }
                        }
                        if (column.MappingName == "DCopa")
                        {
                            try
                            {
                                var rowData = dataGrid.GetRecordAtRowIndex(linha);
                                cellValue = dataGrid.GetCellValue(rowData, column.MappingName).ToString();
                                if ((cellValue != "") && (cellValue != null)) { sql1 += $", DCopa = {Math.Round(float.Parse(cellValue) * 100)}*1.0/100"; }
                                else { sql1 += $", DCopa = NULL"; }
                            }
                            catch { }
                        }
                        if (column.MappingName == "HCopa")
                        {
                            try
                            {
                                var rowData = dataGrid.GetRecordAtRowIndex(linha);
                                cellValue = dataGrid.GetCellValue(rowData, column.MappingName).ToString();
                                if ((cellValue != "") && (cellValue != null)) { sql1 += $", HCopa = {Math.Round(float.Parse(cellValue) * 100)}*1.0/100"; }
                                else { sql1 += $", HCopa = NULL"; }
                            }
                            catch { }
                        }
                        if (column.MappingName == "Observacoes")
                        {
                            try
                            {
                                var rowData = dataGrid.GetRecordAtRowIndex(linha);
                                cellValue = dataGrid.GetCellValue(rowData, column.MappingName).ToString();
                                sql1 += $", Observacoes = '{cellValue}'";
                            }
                            catch { }
                        }
                    }

                    if ((sql1 != "") && (codreg != ""))
                    {
                        sql += sql1;
                        sql += $" WHERE PKMedicaoArv1 = {codreg}";
                        try
                        {
                            conn.Execute(sql);
                            //conn.Commit();
                            CancelBtn.Text = "Voltar";
                        }
                        catch (Exception ex)
                        {
                            conn.Rollback();
                            DisplayAlert("Erro: ", ex.Message, "Ok");
                        }

                        //catch
                        //{
                        //    conn.Rollback();
                        //    DisplayAlert("Message", "Não foi possível atualizar. Verifique o valor digitado.", "Ok");
                        //}
                    }
                }
            }
            finally
            {
                conn.Commit();
            }
            ////Atualiza a lista
            ///
            /*
            sql = $"SELECT PKMedicaoArv1, PKParcelasMedArv, IdParcela, ArvNum, FusteNum, TipoMedD, HTotal, MedD, HDom, " +
                $"HFuste, DCopa, HCopa, Observacoes, Selec FROM MedicaoArvore1 WHERE PKParcelasMedArv = {pkparcelasmedarv} " +
                $"ORDER BY ArvNum";
            var data = conn.Query<MedicaoArvore1>(sql);
            dataGrid.ItemsSource = data;
            btnDelete.IsEnabled = false;
            */


            sql = $"SELECT MedArv.PKMedicaoArv1, Parc.IdParcela, MedArv.PKParcelasMedArv, MedArv.ArvNum, " +
                $"MedArv.FusteNum, MedArv.TipoMedD, MedArv.HTotal, MedArv.MedD, MedArv.HDom, MedArv.HFuste, MedArv.DCopa, " +
                $"MedArv.HCopa, MedArv.Observacoes, MedArv.Selec FROM MedicaoArvore1 MedArv JOIN ParcelasMedArv Parc " +
                $"ON MedArv.PKParcelasMedArv = Parc.PKParcelasMedArv ";
            if (filtroparc != "") { sql += $"WHERE ({filtroparc}) "; }
            else { sql += $"WHERE (MedArv.PKParcelasMedArv = {pkparcelasmedarv}) "; }
            sql += $"ORDER BY MedArv.PKParcelasMedArv, MedArv.ArvNum";

            var data = conn.Query<MedicaoArvore1>(sql);
            dataGrid.ItemsSource = data;
            btnDelete.IsEnabled = false;
            //numreg = data.Count;

        }

        async void CancelarEdicao(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void NovaMedicaoArvore1_Clicked(object sender, EventArgs e)
        {

            //Navigation.PushAsync(new pgAddMedicaoArvore1(null, pkparcelasmedarv, idfazenda, idtalhao, idamostragem, idparcela));
            ////Navigation.PushAsync(new pgAddMedicaoArvoreGrd(null, pkparcela, idfazenda, idtalhoes, idamostragem, idparcela));
            ////Adicionar código do tipo Insert into

            //Verifica o número da última árvore (ArvNum)
            int NumMaxArv = 0;
            var qryMaxArvNum = conn.Query<MaxArvNum>($"SELECT MAX(ArvNum) AS MaxArvoreNum FROM MedicaoArvore1 WHERE PKParcelasMedArv = {pkparcelasmedarv}");

            if (qryMaxArvNum.Count() > 0)
            {
                qryMaxArvNum.First();
                foreach (var campo in qryMaxArvNum)
                {
                    try { NumMaxArv = campo.MaxArvoreNum; }
                    catch (Exception ex)
                    {
                        DisplayAlert("Erro: ", ex.Message, "Ok");
                    }
                }
            }

            conn.BeginTransaction();
            string sql = $"INSERT INTO MedicaoArvore1(PKParcelasMedArv, ArvNum) values({pkparcelasmedarv}, {NumMaxArv + 1})";
            try
            {
                conn.Execute(sql);
                conn.Commit();
                numreg += 1;
            }
            catch
            {
                conn.Rollback();
                DisplayAlert("Message", "Não foi possível inserir o registro. Houve um erro.", "Ok");
            }


            //Salvar a edição atual
            SaveMedicaoArvore(sender, e);

            ////Atualiza a lista
            //sql = $"SELECT PKMedicaoArv1, PKParcelasMedArv, ArvNum, FusteNum, TipoMedD, HTotal, MedD, HDom, " +
            //    $"HFuste, DCopa, HCopa, Observacoes, Selec FROM MedicaoArvore1 WHERE PKParcelasMedArv = {pkparcelasmedarv} " +
            //    $"ORDER BY ArvNum";
            //var data = conn.Query<MedicaoArvore1>(sql);
            //dataGrid.ItemsSource = data;
            //btnDelete.IsEnabled = false;
        }

        private void RemoverSelec_Clicked(object sender, EventArgs e)
        {

            int selecindex = dataGrid.SelectedIndex;
            int codreg = 0;
            conn.BeginTransaction();

            try
            {
                var rowData = dataGrid.GetRecordAtRowIndex(selecindex);
                codreg = Convert.ToInt32(dataGrid.GetCellValue(rowData, "PKMedicaoArv1"));
            }
            catch { }

            if (codreg > 0)
            {
                string sql = $"DELETE FROM MedicaoArvore1 WHERE PKMedicaoArv1 = {codreg}";
                try
                {
                    conn.Execute(sql);
                    conn.Commit();
                    CancelBtn.Text = "Voltar";
                }
                catch
                {
                    conn.Rollback();
                    DisplayAlert("Message", "O registro não foi excluído.", "Ok");
                }
                //DeleteMedicaoArvore(0, codreg);
                //Salvar a edição atual
                SaveMedicaoArvore(sender, e);
            }
            else
            {
                DisplayAlert("SIGNUS", "Para excluir um registro, selecione-o clicando sobre ele e, em seguida, acione o comando 'REMOVER LINHA'.", "Ok");
                conn.Rollback();
            }
        }

        private void Informacoes_Clicked(object sender, EventArgs e)
        {
            msg = $"DADOS DE MEDIÇÕES DE ÁRVORES\n\n" +
                $"Esta tela permite a edição dos dados de medições das árvores por parcela.\n\n" +
                $"Os comandos são os seguintes:\n\nNOVA MEDIÇÃO: incluir novo registro;\n\n" +
                $"REMOVER SELEC.: remove um registro selecionado (seleciona-se o registro utilizando-se " +
                $"a caixa de seleção na coluna 'Selecionar')\n\n" +
                $"SALVAR: salva a edição\n\n" +
                $"CANCELAR: cancela a edição\n\n" +
                $"Os campos da tabela são os seguintes:\n\nPK Medição: chave primária gerada automaticamente pelo SIGNUS;\n\n" +
                $"Árvore nº: número da árvore (é gerado automaticamente quando já está definido o número de árvores);\n\n" +
                $"Fuste nº: número do fuste (necessário quando houver árvores bifurcadas ou mais de um broto na cepa);\n\n" +
                $"Tipo med. D: tipo de medição diamétrica (D para DAP ou C para CAP);\n\n" +
                $"Alt. total: altura total (m);\n\n" +
                $"DAP/CAP: medida do DAP ou do CAP (cm);\n\n" +
                $"Alt. dom.: altura dominante (m);\n\n" +
                $"Alt. fuste: altura desde o solo até o ponto de inserção da copa (início da copa) (m);\n\n" +
                $"Diâm. copa: diâmetro da copa (m);\n\n" +
                $"Alt. copa: altura da copa (m);\n\n" +
                $"Observações: observações adicionais;\n\n" +
                $"Selecionar: caixa de seleção (utiliza-se esse campo para selecionar registros a serem deletados);\n\n" +
                $"OBSERVAÇÕES:\n\n" +
                $"É possível mover as colunas clicando-se nos títulos e arrastando-os lateralmente. \n\n" +
                $"É possível também mover as linhas clicando-se sobre elas e arrastando-as para cima ou para baixo.\n\n" +
                $"A seta localizada no canto superior esquerdo da tela permite retornar à tela anterior.";
            DisplayAlert("SIGNUS", msg, "Ok");
        }

        private void OnSelectionChange(object sender, GridSelectionChangedEventArgs e)
        {
            int selecindex = 0;
            selecindex = dataGrid.SelectedIndex;
            if (selecindex > 0)
            {
                btnDelete.IsEnabled = true;
            }
            else
            {
                btnDelete.IsEnabled = false;
            }
        }

        private void lstFazendas_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //
        }

        private void OnCheckedChangedParc(object sender, CheckedChangedEventArgs e)
        {
            filtroparc = "";

            ParcelasSelec = new ObservableCollection<ListaParcelas>();

            //Inclui registro selecionano à lista ListaParcelas
            for (int i = 0; i < LstParcelas.Count; i++)
            {
                ListaParcelas item = LstParcelas[i];
                if (item.Check1)
                {
                    ParcelasSelec.Add(item);
                    //ParcelasSelec.Add(new ListaParcelas { PkParcela = prc.PkParcela, IdParcela = prc.IdParcela, Check1 = true });
                }
            }

            if (ParcelasSelec.Count > 0)
            {
                //Caso exista alguma parcela selecionada
                for (int i = 0; i < ParcelasSelec.Count; i++)
                {
                    ListaParcelas item1 = ParcelasSelec[i];
                    if (i == 0) filtroparc += $"(MedArv.PkParcelasMedArv = {item1.PkParcela})";
                    else filtroparc += $" OR (MedArv.PkParcelasMedArv = {item1.PkParcela})";
                    pkparcelasmedarv = item1.PkParcela.ToString();
                }

                ////Atualiza a lista de medições das árvores
                sqlArvores = $"SELECT MedArv.PKMedicaoArv1, Parc.IdParcela, MedArv.PKParcelasMedArv, MedArv.ArvNum, " +
                    $"MedArv.FusteNum, MedArv.TipoMedD, MedArv.HTotal, MedArv.MedD, MedArv.HDom, MedArv.HFuste, MedArv.DCopa, " +
                    $"MedArv.HCopa, MedArv.Observacoes, MedArv.Selec FROM MedicaoArvore1 MedArv JOIN ParcelasMedArv Parc " +
                    $"ON MedArv.PKParcelasMedArv = Parc.PKParcelasMedArv ";
                if (filtroparc != "") { sqlArvores += $"WHERE ({filtroparc}) "; }
                else { sqlArvores += $"WHERE (MedArv.PKParcelasMedArv = {pkparcelasmedarv}) "; }
                sqlArvores += $"ORDER BY MedArv.PKParcelasMedArv, MedArv.ArvNum";

                var data = conn.Query<MedicaoArvore1>(sqlArvores);
                dataGrid.ItemsSource = data;
            }
            else
            {
                //Caso não exista alguma parcela selecionada
                dataGrid.ItemsSource = null;
            }
        }

    }
}