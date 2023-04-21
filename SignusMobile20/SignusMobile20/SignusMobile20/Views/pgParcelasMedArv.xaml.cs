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
	public partial class pgParcelasMedArv : ContentPage
	{
        private SQLiteConnection conn;
        public ParcelasMedArv parcelasmedarv;
        public string pktalhoescr;
        public string pkamostragem;
        public string pkparcela;
        public string idfazenda;
        public string idtalhoescr;
        public string idamostragem;
        public int numparc;
        public int numarvparc;
        string msg;

        public pgParcelasMedArv (string pktlh, string pkamost, string pkparc, string idfaz, string idtlh, string idamostr, int nparc, int narvparc)
		{
			InitializeComponent ();
            conn = DependencyService.Get<ISQLite>().GetConnection();
            conn.CreateTable<ParcelasMedArv>();
            pktalhoescr = pktlh;
            pkamostragem = pkamost;
            pkparcela = pkparc;
            idfazenda = idfaz;
            idtalhoescr = idtlh;
            idamostragem = idamostr;
            numparc = nparc;
            numarvparc = narvparc;
            Fazenda.Text = "Fazenda/Proj.: " + idfaz;
            IdTalhao.Text = "Talhão: " + idtlh;
            IdAmostragem.Text = "Amostragem: " + idamostr;

        }

        protected override void OnAppearing()
        {
            int num = Convert.ToInt32(pkamostragem);
            //PopulateListaParcelasMedArv();
            var data = conn.Table<ParcelasMedArv>().Where(x => x.PKAmostragemMN == num).ToList();
            if (pkparcela != "")
            {
                num = Convert.ToInt32(pkparcela);
                data = conn.Table<ParcelasMedArv>().Where(x => x.PKParcelasMedArv == num).ToList();
            }
            lstParcelasMedArv.ItemsSource = data;
        }

        private void Mostrar_Clicked(object sender, EventArgs e)
        {
            var data = (from parcmedarv in conn.Table<ParcelasMedArv>() select parcmedarv);
            lstParcelasMedArv.ItemsSource = data;
        }

        private void NovaParcelaMedArv_Clicked(object sender, EventArgs e)
        {
//            Navigation.PushAsync(new pgAddParcelasMedArv(null, pkamostragem, idfazenda, idtalhoescr, idamostragem));
        }

        private void EditParcelasMedArv(object sender, ItemTappedEventArgs e)
        {
            ParcelasMedArv details = e.Item as ParcelasMedArv;
            if (details != null)
            {
//                Navigation.PushAsync(new pgAddParcelasMedArv(details, pkamostragem, idfazenda, idtalhoescr, idamostragem));
            }
        }

        public async void AlternaDetalhes(object sender, ItemTappedEventArgs e)
        {
            //Alterna entre visível e invisível a lista de detalhes
            ParcelasMedArv details = e.Item as ParcelasMedArv;
            string sql = "";

            if (details.Visible.ToString() == "True")
            {
                sql = $"UPDATE ParcelasMedArv SET Visible = 0, Imagem = 'Down.png' ";
            }
            else
            {
                sql = $"UPDATE ParcelasMedArv SET Visible = 1, Imagem = 'Up.png' ";
            }

            sql += $"WHERE PKParcelasMedArv = {details.PKParcelasMedArv}";
            try { conn.Execute(sql); }
            catch { await DisplayAlert("Message", "Houve um erro. Não foi possível atualizar.", "Ok"); }

            sql = $"SELECT * FROM ParcelasMedArv WHERE PKAmostragemMN = {pkamostragem}";

            //Atualiza a lista
            var data = conn.Query<ParcelasMedArv>(sql);
            lstParcelasMedArv.ItemsSource = data;
        }

        private async void DeleteParcelas(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var parcmedarv = mi.CommandParameter as ParcelasMedArv;
            if (parcmedarv != null)
            {
                var opcao = await DisplayAlert("Excluir registro", "Confirma a exclusão do registro de código " + parcmedarv.PKParcelasMedArv + " e de todos os outros (de medição) associados? ", "Ok", "Cancel");
                if (opcao)
                {
                    //Deleta todos os registros de medição associados à parcela atual
                    string sql = $"DELETE FROM MedicaoArvore1 WHERE PKParcelasMedArv = {parcmedarv.PKParcelasMedArv}";
                    try { conn.Execute(sql); }
                    catch
                    {
                        await DisplayAlert("Message", "Houve um erro, não foi possível excluir os registros de medição associados a este registro de parcela. " +
                        "Possivelmente, a tabela de medições ainda não foi criada.", "Ok");
                    }

                    //Deleta o registro de amostragem
                    sql = $"DELETE FROM ParcelasMedArv WHERE PKParcelasMedArv = {parcmedarv.PKParcelasMedArv}";
                    try { conn.Execute(sql); }
                    catch { await DisplayAlert("Message", "Houve um erro, não foi possível excluir o registro de parcela.", "Ok"); }

                    //Atualiza a lista
                    sql = $"SELECT * FROM ParcelasMedArv WHERE PKAmostragemMN = {pkamostragem}";
                    var data = conn.Query<ParcelasMedArv>(sql);
                    lstParcelasMedArv.ItemsSource = data;
                }
            }
        }

        private async void AlterarParcelasMedArv(object sender, EventArgs e)
        {
            try
            {
                var mi = ((MenuItem)sender);
                var parcmedarv = mi.CommandParameter as ParcelasMedArv;
                if (parcmedarv != null)
                {
//                    await Navigation.PushAsync(new pgAddParcelasMedArv(parcmedarv, pkamostragem, idfazenda, idtalhoescr, idamostragem));
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro: ", ex.Message, "Ok");
            }
        }

        private async void AvaliarQualidade(object sender, EventArgs e)
        {
            try
            {
                var mi = ((MenuItem)sender);
                var parcmedarv = mi.CommandParameter as ParcelasMedArv;
                if (parcmedarv != null)
                {
//                    await Navigation.PushAsync(new pgAddParcelasMedArv(parcmedarv, pkamostragem, idfazenda, idtalhoescr, idamostragem));
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro: ", ex.Message, "Ok");
            }

        }

        private void Informacoes_Clicked(object sender, EventArgs e)
        {
            msg = $"COMANDO EM FASE DE TESTES\n\n" +
                $"Esta tela exibe a lista de parcelas de medições de árvores para o monitoramento nutricional e peprmite a edição.\n\n" +
                $"Os comandos são os seguintes:\n\nNOVA PARCELA: incluir novo registro por meio de digitação direta no SIGNUS;\n\n" +
                //$"A partir do menu (três pontinhos) no cato superior direito da tela, há a opção de exportar " +
                //$"os dados das amostragens para uma planilha Excel. O arquivo gerado conterá os dados dos talhões" +
                //$"para os quais as amostragens foram realizadas, os dados das amostragens e das medições. O arquivo é " +
                //$"disponibilizado na pasta Signus com o nome iniciado por 'Monit_' seguido da data e hora.\n\n" +
                $"Um toque rápido sobre um registro de parcela expande exibe os detalhes do mesmo;\n\n" +
                $"Para editar um registro já existente, pressione-o até aparecerem as opções na parte superior " +
                $"da tela.\n\n" +
                $"Para editar as medições de árvores, pressione o registro até aparecer o menu (três pontinhos) " +
                $"no canto superior direito da tela. A partir deste menu, é possível abrir o formulário para cadastro das medições.\n\n" +
                $"A seta localizada no canto superior esquerdo da tela permite retornar à tela anterior.";
            DisplayAlert("SIGNUS", msg, "Ok");
        }

        //Este código é o que estava anteriormente, funcionando normalmente
        //private async void EditarMedicoes(object sender, EventArgs e)
        //{
        //    var mi = ((MenuItem)sender);
        //    var parcmedarv = mi.CommandParameter as ParcelasMedArv;
        //    if (parcmedarv != null)
        //    {
        //        //string sql = $"SELECT * FROM MedicaoArvore WHERE PKAmostragemMN = {amostragem.PKAmostragemMN}";
        //        //try { conn.Query<MedicaoArvore>(sql); }
        //        //catch { await DisplayAlert("Message", "Não foi possível abrir a consulta. Verifique o valor digitado.", "Ok"); }
        //        await Navigation.PushAsync(new pgMedicaoArvore1(pktalhoescr, pkamostragem, parcmedarv.PKParcelasMedArv.ToString(), idfazenda, idtalhoescr, idamostragem, parcmedarv.IdParcela, numparc, numarvparc));
        //    }

        //}

        //private async void EditarMedicoes(object sender, EventArgs e)
        //{
        /*
        var mi = ((MenuItem)sender);
        var parcmedarv = mi.CommandParameter as ParcelasMedArv;
        if (parcmedarv != null)
        {
            int narv = numarvparc;
            int narvtb = 0;
            pkparcela = parcmedarv.PKParcelasMedArv.ToString();
            //Verifica o número de árvores já existentes na parcela e, caso o número seja inferior ao informado, 
            //acrescenta mais árvores
            try
            {
                var qryArvores = conn.Query<MedicaoArvore1>($"SELECT PKMedicaoArv1 FROM MedicaoArvore1 WHERE PKParcelasMedArv = {pkparcela}");
                if (qryArvores.Count() > 0) { narvtb = qryArvores.Count(); }
            }
            catch { }
            if (narvtb < narv)
            {
                narv -= narvtb;

                for (int arv = 1; arv <= narv; arv++)
                {
                    conn.BeginTransaction();
                    string sql = $"INSERT INTO MedicaoArvore1(PKParcelasMedArv, ArvNum) values({pkparcela}, {narvtb + arv})";
                    try
                    {
                        conn.Execute(sql);
                        conn.Commit();
                        //nparc += 1;
                    }
                    catch
                    {
                        conn.Rollback();
                        await DisplayAlert("Message", "Não foi possível inserir o registro. Houve um erro.", "Ok");
                    }
                }
            }

            //Abre o formulário para edição das medições.
            await Navigation.PushAsync(new pgAddMedicaoArvoreGrd(null, pkamostragem, parcmedarv.PKParcelasMedArv.ToString(), idfazenda, idtalhoescr, idamostragem, parcmedarv.IdParcela));
        }
        */
        //}

        //private async void EditarAmostSolos(object sender, EventArgs e)
        //{
        //    var mi = ((MenuItem)sender);
        //    var parcmedarv = mi.CommandParameter as ParcelasMedArv;
        //    if (parcmedarv != null)
        //    {
        //        await Navigation.PushAsync(new pgAmostSolos(pktalhoescr,
        //            pkamostragem,
        //            pkparcela,
        //            idfazenda,
        //            idtalhoescr,
        //            idamostragem,
        //            parcmedarv.IdParcela,
        //            1, 2));
        //    }
        //}

        //private async void EditarAmostTecidos(object sender, EventArgs e)
        //{
        //    var mi = ((MenuItem)sender);
        //    var parcmedarv = mi.CommandParameter as ParcelasMedArv;
        //    if (parcmedarv != null)
        //    {
        //        await Navigation.PushAsync(new pgAmostTecidos(pktalhoescr,
        //            pkamostragem,
        //            idfazenda,
        //            idtalhoescr,
        //            idamostragem,
        //            parcmedarv.IdParcela,
        //            1, 1));
        //    }
        //}

    }
}