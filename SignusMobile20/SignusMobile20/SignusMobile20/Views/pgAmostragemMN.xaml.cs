using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignusMobile20.Models;
using SQLite;
using Syncfusion.XlsIO;
using System.IO;
using System.Reflection;
using Color = Syncfusion.Drawing.Color;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SignusMobile20
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class pgAmostragemMN : ContentPage
	{
        private SQLiteConnection conn;
        public AmostragemMN amostragemmn;
        public string pktlhoescr;
        public string idfazenda;
        public string idtalhao;

        //public string pkparcela;
        public int numparc;
        public int numarvparc;

        string msg;

        public pgAmostragemMN (string pktalhcr, string faz, string idtalh)
		{
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTU0OTAzQDMxMzkyZTM0MmUzMGhGOWRUZEN4NDBVSGx2OFFub2E4QjAySERLTHFvZmxYODUzUlU0d1hzUm89");

            InitializeComponent();
            conn = DependencyService.Get<ISQLite>().GetConnection();
            //conn.DropTable<AmostragemMN>();
            conn.CreateTable<AmostragemMN>();
            //conn.DropTable<ParcelasMedArv>();
            conn.CreateTable<ParcelasMedArv>();
            conn.CreateTable<MedicaoArvore1>();
            conn.CreateTable<AmostSolos>();
            conn.CreateTable<AmostTecidos>();
            pktlhoescr = pktalhcr;
            idfazenda = faz;
            idtalhao = idtalh;
            Fazenda.Text = "Fazenda/Proj.: " + faz;
            IdTalhao.Text = "Talhão: " + idtalh;


        }

        protected override void OnAppearing()
        {
            int num = Convert.ToInt32(pktlhoescr);
            //PopulateListaAmostragemMN();
            var data = conn.Table<AmostragemMN>().Where(x => x.PKTalhoesCR == num).ToList();
            lstAmostragemMN.ItemsSource = data;
        }

        private void Mostrar_Clicked(object sender, EventArgs e)
        {
            var data = (from amostmn in conn.Table<AmostragemMN>() select amostmn);
            lstAmostragemMN.ItemsSource = data;
        }

        private void NovoAmostragemMN_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new pgAddAmostragemMN(null, pktlhoescr, idfazenda, idtalhao));
        }

        private void EditAmostragemMN(object sender, ItemTappedEventArgs e)
        {
            AmostragemMN details = e.Item as AmostragemMN;
            if (details != null)
            {
                Navigation.PushAsync(new pgAddAmostragemMN(details, pktlhoescr, idfazenda, idtalhao));
            }
        }

        public async void AlternaDetalhes(object sender, ItemTappedEventArgs e)
        {
            //Alterna entre visível e invisível a lista de detalhes
            AmostragemMN details = e.Item as AmostragemMN;
            string sql = "";

            if (details.Visible.ToString() == "True")
            {
                sql = $"UPDATE AmostragemMN SET Visible = 0, Imagem = 'Down.png' ";
            }
            else
            {
                sql = $"UPDATE AmostragemMN SET Visible = 1, Imagem = 'Up.png' ";
            }

            sql += $"WHERE PKAmostragemMN = {details.PKAmostragemMN}";
            try { conn.Execute(sql); }
            catch { await DisplayAlert("Message", "Houve um erro. Não foi possível atualizar.", "Ok"); }

            sql = $"SELECT * FROM AmostragemMN WHERE PKTalhoesCR = {pktlhoescr}";

            //Atualiza a lista
            var data = conn.Query<AmostragemMN>(sql);
            lstAmostragemMN.ItemsSource = data;
        }

        private async void DeleteAmostr(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var amostragem = mi.CommandParameter as AmostragemMN;
            if (amostragem != null)
            {
                int nparc = 0;
                int namtec = 0;
                int namsol = 0;

                try
                {
                    //verifica o número de parcelas existem na amostratem atual
                    var qryParcelas = conn.Query<ParcelasMedArv>($"SELECT PKParcelasMedArv FROM ParcelasMedArv WHERE PKAmostragemMN = {amostragem.PKAmostragemMN}");
                    if (qryParcelas.Count() > 0)
                    {
                        nparc = qryParcelas.Count();
                    }
                }
                catch { nparc = 0; }

                try
                {
                    var qryAmostTec = conn.Query<AmostTecidos>($"SELECT PKAmostTecidos FROM AmostTecidos WHERE PKAmostragemMN = {amostragem.PKAmostragemMN}");
                    if (qryAmostTec.Count() > 0)
                    {
                        namtec = qryAmostTec.Count();
                    }
                }
                catch { namtec = 0; }

                try
                {
                    var qryAmostSolos = conn.Query<AmostSolos>($"SELECT PKAmostSolos FROM AmostSolos WHERE PKAmostragemMN = {amostragem.PKAmostragemMN}");
                    if (qryAmostSolos.Count() > 0)
                    {
                        namsol = qryAmostSolos.Count();
                    }
                }
                catch { namsol = 0; }

                if ((nparc > 0) || (namtec > 0) || (namsol > 0))
                {
                    await DisplayAlert("Message", $"Antes de excluir este registro, exclua todos os registros associados " +
                        $"de parcelas ({nparc} registros), amostras de tecidos ({namtec} registros) e/ou amostras de solos ({namsol} registros).", "Ok");
                }
                else
                {
                    var opcao = await DisplayAlert("Excluir registro", $"Confirma a exclusão do registro de código {amostragem.PKAmostragemMN}? ", "Ok", "Cancel");
                    if (opcao)
                    {
                        //Deleta o registro de amostragem
                        string sqlamost2 = $"DELETE FROM AmostragemMN WHERE PKAmostragemMN = {amostragem.PKAmostragemMN}";
                        try { conn.Execute(sqlamost2); }
                        catch { await DisplayAlert("Message", "Houve um erro, não foi possível excluir o(s) registro(s) de amostragens associados ao talhão.", "Ok"); }

                        /*
                                                //Deleta todos os arquivos de medição para todas as parcelas de uma determinada amostragem
                                                string sqlparc = $"SELECT * FROM ParcelasMedArv WHERE PKAmostragemMN = {amostragem.PKAmostragemMN}";
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
                                                string sqlparc2 = $"DELETE FROM ParcelasMedArv WHERE PKAmostragemMN = {amostragem.PKAmostragemMN}";
                                                try { conn.Execute(sqlparc2); }
                                                catch { await DisplayAlert("Message", "Houve um erro, não foi possível excluir o(s) registro(s) de parcela(s) associados à amostragem.", "Ok"); }

                        */
                        //Atualiza a lista
                        string sqlam = $"SELECT * FROM AmostragemMN WHERE PKAmostragemMN = {amostragem.PKAmostragemMN}";
                        var data = conn.Query<AmostragemMN>(sqlam);
                        lstAmostragemMN.ItemsSource = data;

                        //Atualiza a página
                        //var UpdatedPage = new pgAmostragemMN(amostragem.PKTalhoesCR.ToString());
                        //Navigation.InsertPageBefore(UpdatedPage, this);
                        //await Navigation.PopAsync();
                    }
                }
                /*
                //Deleta todos os registros de árvore associados à amostragem atual
                string sql = $"DELETE FROM MedicaoArvore WHERE PKAmostragemMN = {amostragem.PKAmostragemMN}";
                try { conn.Execute(sql); }
                catch { await DisplayAlert("Message", "Houve um erro, não foi possível excluir os registros de árvore associados a este registro de amostragem.", "Ok"); }

            //Deleta o registro de amostragem
            sql = $"DELETE FROM AmostragemMN WHERE PKAmostragemMN = {amostragem.PKAmostragemMN}";
            try { conn.Execute(sql); }
            catch { await DisplayAlert("Message", "Houve um erro, não foi possível excluir o registro de amostragem.", "Ok"); }
            sql = $"SELECT * FROM AmostragemMN WHERE PKTalhoesCR = {pktlhoescr}";

            //Atualiza a lista
            var data = conn.Query<AmostragemMN>(sql);
            lstAmostragemMN.ItemsSource = data;

            //Atualiza a página
            //var UpdatedPage = new pgAmostragemMN(amostragem.PKTalhoesCR.ToString());
            //Navigation.InsertPageBefore(UpdatedPage, this);
            //await Navigation.PopAsync();
            */
            }
        }

        //pgParcelasMedArv pgparcelas = new pgParcelasMedArv(null, null, null, null, null, null, 0, 0);
        //public async void DeletarAmostragemMN(int pkamostr, int pktalh)
        //{
        /*
        if (pktalh > 0)
        {
            //Deleta todos os arquivos de parcelas para todas as amostragens de um determinado talhão
            string sqlamost1 = $"SELECT * FROM AmostragemMN WHERE PKTalhoesCR = {pktalh}";
            var amostragem = conn.Query<AmostragemMN>(sqlamost1);

            if (amostragem.Count() > 0)
            {
                foreach (var campo in amostragem)
                {
                    try { pgparcelas.DeleteParcelasMedArv(0, campo.PKAmostragemMN); }
                    catch
                    {
                        await DisplayAlert("Message", $"Houve um erro, não foi possível excluir os registros de parcelas associados a este talhão ({campo.PKTalhoesCR}). " +
                        "Possivelmente, a tabela de medições ainda não foi criada.", "Ok");
                    }
                }
            }
            //Deleta o registro da amostragem
            string sqlamost2 = $"DELETE FROM AmostragemMN WHERE PKTalhoesCR = {pktalh}";
            try { conn.Execute(sqlamost2); }
            catch { await DisplayAlert("Message", "Houve um erro, não foi possível excluir o(s) registro(s) de amostragens associados ao talhão.", "Ok"); }
        }
        else
        {
            //Deleta registros da tabela Amostragem com base no código de chave primária da tabela (PKAmostragemMN)
            var opcao = await DisplayAlert("Excluir registro", $"Confirma a exclusão do registro de código {pkamostr} e de todos os outros (de medição) associados? ", "Ok", "Cancel");
            if (opcao)
            {
                //Deleta todos os registros de medição associados à parcela atual
                try { pgparcelas.DeleteParcelasMedArv(0, pkamostr); }
                catch
                {
                    await DisplayAlert("Message", "Houve um erro, não foi possível excluir os registros de parcelas associados a esta amostragem. " +
                    "Possivelmente, a tabela de parcelas ainda não foi criada.", "Ok");
                }
            }
            //Deleta o registro da amostragem
            string sqlamost2 = $"DELETE FROM AmostragemMN WHERE PKAmostragemMN = {pkamostr}";
            try { conn.Execute(sqlamost2); }
            catch { await DisplayAlert("Message", "Houve um erro, não foi possível excluir o registro de amostragem.", "Ok"); }
        }
        */
        //}

        private async void AlterarAmostragemMN(object sender, EventArgs e)
        {
            try
            {
                var mi = ((MenuItem)sender);
                var amostragem = mi.CommandParameter as AmostragemMN;
                if (amostragem != null)
                {
                    await Navigation.PushAsync(new pgAddAmostragemMN(amostragem, pktlhoescr, idfazenda, idtalhao));
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro: ", ex.Message, "Ok");
            }
        }

        //private async void EditarMedicoes(object sender, EventArgs e)
        //{
        //var mi = ((MenuItem)sender);
        //var amostragem = mi.CommandParameter as AmostragemMN;
        //if (amostragem != null)
        //{
        //    //string sql = $"SELECT * FROM MedicaoArvore WHERE PKAmostragemMN = {amostragem.PKAmostragemMN}";
        //    //try { conn.Query<MedicaoArvore>(sql); }
        //    //catch { await DisplayAlert("Message", "Não foi possível abrir a consulta. Verifique o valor digitado.", "Ok"); }
        //    await Navigation.PushAsync(new pgMedicaoArvore(amostragem.PKTalhoesCR.ToString(), amostragem.PKAmostragemMN.ToString(), amostragem.NParcelas, amostragem.NArvParc));
        //}

        //}

        private async void EditarParcelasMedArv(object sender, EventArgs e)
        {
            //Abre a tela de edição de parcelas
            //antes de abrir, verifica se há o número de parcelas definido no registro de amostragem
            //Caso o número de parcelas for inferior ao número definido, insere as parcelas conforme o número que foi definido

            var mi = ((MenuItem)sender);
            var amostragem = mi.CommandParameter as AmostragemMN;

            if (amostragem != null)
            {
                int nparc = amostragem.NParcelas;
                int nparctb = 0;
                string pkamost = amostragem.PKAmostragemMN.ToString();
                //Verifica o número de parcelas já existentes na amostragem e, caso o número seja inferior ao informado, 
                //acrescenta mais parcelas
                try
                {
                    var qryParcelas = conn.Query<ParcelasMedArv>($"SELECT PKParcelasMedArv FROM ParcelasMedArv WHERE PKAmostragemMN = {pkamost}");
                    if (qryParcelas.Count() > 0) { nparctb = qryParcelas.Count(); }
                }
                catch { }
                if (nparctb < nparc)
                {
                    nparc -= nparctb;

                    for (int parc = 1; parc <= nparc; parc++)
                    {
                        conn.BeginTransaction();
                        string sql = $"INSERT INTO ParcelasMedArv(PKAmostragemMN, IdParcela) values({pkamost}, 'P{nparctb + parc}')";
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


                //string sql = $"SELECT * FROM MedicaoArvore WHERE PKAmostragemMN = {amostragem.PKAmostragemMN}";
                //try { conn.Query<MedicaoArvore>(sql); }
                //catch { await DisplayAlert("Message", "Não foi possível abrir a consulta. Verifique o valor digitado.", "Ok"); }

                await Navigation.PushAsync(new pgParcelasMedArv(amostragem.PKTalhoesCR.ToString(),
                    amostragem.PKAmostragemMN.ToString(),
                    "",
                    idfazenda,
                    idtalhao,
                    amostragem.IdAmostragem,
                    amostragem.NParcelas,
                    amostragem.NArvParc)); ; ;

            }
        }

        private async void EditarMedicaoArvores(object sender, EventArgs e)
        {

            //Abre a tela de edição de medições das árvores
            //antes de abrir, verifica se há o número de parcelas definido no registro de amostragem
            //Caso o número de parcelas for inferior ao número definido, insere as parcelas conforme o número que foi definido
            //
            var mi = ((MenuItem)sender);
            var amostragem = mi.CommandParameter as AmostragemMN;

            ///////////////////////////////////////////////////////////////////////////////////////
            int pkparc1 = 0;
            string pkamost = amostragem.PKAmostragemMN.ToString();
            string idparc1 = "";
            ///////////////////////////////////////////////////////////////////////////////////////

            if (amostragem != null)
            {
                int nparcam = amostragem.NParcelas;//número de parcelas informado na tabela Amostragem
                int nparctb = 0;//número de parcelas existentes na tabela de Parcelas
                //Verifica o número de parcelas já existentes na amostragem e colhe o código da primeira parcela da lista
                try
                {
                    //Verifica o número de parcelas na amostragem
                    var qryParcelas = conn.Query<ParcelasMedArv>($"SELECT PKParcelasMedArv FROM ParcelasMedArv WHERE PKAmostragemMN = {pkamost}");
                    if (qryParcelas.Count() > 0)
                    {
                        nparctb = qryParcelas.Count();

                        //Captura o código e a identificação da primeira parcela da lista
                        var qryParcela1 = conn.Query<ParcelasMedArv>($"SELECT MIN(PKParcelasMedArv) AS PKParcelasMedArv FROM ParcelasMedArv WHERE PKAmostragemMN = {pkamost}");
                        qryParcela1.First();
                        foreach (var campoparc1 in qryParcela1)
                        {
                            pkparc1 = campoparc1.PKParcelasMedArv;
                            idparc1 = campoparc1.IdParcela;
                        }
                    }
                    else
                    {
                        //Se o número de parcelas for igual a zero
                        await DisplayAlert("Message", "Antes de inserir medições, é necessário criar uma parcela.", "Ok");
                    }

                }
                catch { }

                //Caso o número de parcelas contidas na tabela ParcelasMedArv for inferior ao número de parcelas informado,
                //insere as parcelas faltantes
                if (nparctb < nparcam)
                {
                    nparcam -= nparctb;

                    for (int parc = 1; parc <= nparcam; parc++)
                    {
                        conn.BeginTransaction();
                        string sql = $"INSERT INTO ParcelasMedArv(PKAmostragemMN, IdParcela) values({pkamost}, 'P{nparctb + parc}')";
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
            }


            ////////////////////////////////////////////////////////////////////////////////////////////
            ///Obs.: O procedimento está acrescentando apenas uma parcela (a primeira)               ///
            ///Deve mudar para acrescentar todas as parcelas da amostragem e todas as árvores também.///
            ///Abrir novamente a tabela parcelas e inserir o número de árvores para cada parcela,    ///
            ///conforme o número de árvores por parcela informado na tabela da Amostragem.           ///
            ////////////////////////////////////////////////////////////////////////////////////////////

            //Abre 
            //int numparc = amostragem.NParcelas;
            //int nparctb = 0;
            int pkparc = 0;
            int numarvparc = amostragem.NArvParc;
            string idamostragem = amostragem.IdAmostragem;

            var qryParcelas2 = conn.Query<ParcelasMedArv>($"SELECT PKParcelasMedArv FROM ParcelasMedArv WHERE PKAmostragemMN = {pkamost}");
            foreach (var campoparc2 in qryParcelas2)
            {
                pkparc = campoparc2.PKParcelasMedArv;


                if (pkparc > 0)
                {

                    int narvam = numarvparc;//número de árvores por parcela informado na tabela de Amostragem
                    int narvtb = 0;//número de árvores por parcela existentes já cadastrados

                    //Verifica o número de árvores já existentes na amostragem e, caso o número seja inferior ao informado, 
                    //acrescenta mais árvores
                    try
                    {
                        var qryArvores = conn.Query<MedicaoArvore1>($"SELECT PKMedicaoArv1 FROM MedicaoArvore1 WHERE PKParcelasMedArv = {pkparc}");
                        if (qryArvores.Count() > 0) { narvtb = qryArvores.Count(); }
                    }
                    catch { }
                    if (narvtb < narvam)
                    {
                        narvam -= narvtb;

                        for (int arv = 1; arv <= narvam; arv++)
                        {
                            conn.BeginTransaction();
                            string sql = $"INSERT INTO MedicaoArvore1(PKParcelasMedArv, ArvNum) values({pkparc}, {narvtb + arv})";
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
                }
            }
            if (pkparc1 > 0)
            {
                //Abre o formulário para edição das medições.
                await Navigation.PushAsync(new pgAddMedicaoArvoreGrd(null, pkamost, pkparc1.ToString(), idfazenda, idtalhao, idamostragem, idparc1));
            }
        }

        private async void EditarAmostSolos(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var amostragem = mi.CommandParameter as AmostragemMN;
            if (amostragem != null)
            {
                int nparc = amostragem.NParcelas;
                int nrepsolotalh = amostragem.NRepSoloTalh;
                int nrepsoloparc = amostragem.NRepSoloParc;
                int ncamadas = amostragem.NCamadasSolo;
                int nregexistentes = 0;

                //Verifica o número de registros já existentes de amostragem e, caso o número seja igual a zero, 
                //acrescenta as parcelas. Caso já exista registros, não acrescenta
                string pkamost = amostragem.PKAmostragemMN.ToString();
                try
                {
                    var qryAmSolos = conn.Query<AmostSolos>($"SELECT PKAmostSolos FROM AmostSolos WHERE PKAmostragemMN = {pkamost}");
                    if (qryAmSolos.Count() > 0) { nregexistentes = qryAmSolos.Count(); }
                }
                catch { }

                //Calcula o número de amostras de solos a acrescentar
                if ((nparc > 0) && (nregexistentes == 0))
                {
                    if (nrepsoloparc > 0)
                    {
                        for (int parc = 1; parc <= nparc; parc++)
                        {
                            for (int repparc = 1; repparc <= nrepsoloparc; repparc++)
                            {
                                for (int ncam = 1; ncam <= ncamadas; ncam++)
                                {
                                    conn.BeginTransaction();
                                    string sql = $"INSERT INTO AmostSolos(PKAmostragemMN, IdAmostra) values({pkamost}, '{idfazenda}/T{idtalhao}/P{parc}/R{repparc}/H{ncam}')";
                                    try { conn.Execute(sql); conn.Commit(); }
                                    catch
                                    {
                                        conn.Rollback();
                                        await DisplayAlert("Message", "Não foi possível inserir o registro. Houve um erro.", "Ok");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int reptalh = 1; reptalh <= nrepsolotalh; reptalh++)
                        {
                            for (int ncam = 1; ncam <= ncamadas; ncam++)
                            {
                                conn.BeginTransaction();
                                string sql = $"INSERT INTO AmostSolos(PKAmostragemMN, IdAmostra) values({pkamost}, '{idfazenda}/T{idtalhao}/R{reptalh}/H{ncam}')";
                                try { conn.Execute(sql); conn.Commit(); }
                                catch
                                {
                                    conn.Rollback();
                                    await DisplayAlert("Message", "Não foi possível inserir o registro. Houve um erro.", "Ok");
                                }
                            }
                        }
                    }
                }

                //string sql = $"SELECT * FROM MedicaoArvore WHERE PKAmostragemMN = {amostragem.PKAmostragemMN}";
                //try { conn.Query<MedicaoArvore>(sql); }
                //catch { await DisplayAlert("Message", "Não foi possível abrir a consulta. Verifique o valor digitado.", "Ok"); }

                await Navigation.PushAsync(new pgAmostSolos(amostragem.PKTalhoesCR.ToString(),
                    amostragem.PKAmostragemMN.ToString(),
                    "",
                    idfazenda,
                    idtalhao,
                    amostragem.IdAmostragem,
                    "",
                    1, 2));

            }
        }

        private async void EditarAmostTecidos(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var amostragem = mi.CommandParameter as AmostragemMN;
            if (amostragem != null)
            {
                int nparc = amostragem.NParcelas;
                int nreptectalh = amostragem.NRepTecidosTalh;
                int nreptecparc = amostragem.NRepTecidosParc;
                int nregexistentes = 0;
                bool folha = amostragem.Folhas;
                bool galho = amostragem.Galhos;
                bool casca = amostragem.Casca;
                bool lenho = amostragem.Lenho;
                bool raiz = amostragem.Raizes;

                //Verifica o número de registros já existentes de amostragem e, caso o número seja igual a zero, 
                //acrescenta as parcelas. Caso já exista registros, não acrescenta
                string pkamost = amostragem.PKAmostragemMN.ToString();
                try
                {
                    var qryAmTecidos = conn.Query<AmostTecidos>($"SELECT PKAmostTecidos FROM AmostTecidos WHERE PKAmostragemMN = {pkamost}");
                    if (qryAmTecidos.Count() > 0) { nregexistentes = qryAmTecidos.Count(); }
                }
                catch { }

                //Calcula o número de amostras de tecidos a acrescentar
                if ((nparc > 0) && (nregexistentes == 0))
                {
                    if (nreptecparc > 0)
                    {
                        for (int parc = 1; parc <= nparc; parc++)
                        {
                            for (int repparc = 1; repparc <= nreptecparc; repparc++)
                            {
                                if (folha)
                                {
                                    conn.BeginTransaction();
                                    string sql = $"INSERT INTO AmostTecidos(PKAmostragemMN, IdAmostra) values({pkamost}, '{idfazenda}/T{idtalhao}/P{parc}/R{repparc}/F')";
                                    try { conn.Execute(sql); conn.Commit(); }
                                    catch
                                    {
                                        conn.Rollback();
                                        await DisplayAlert("Message", "Não foi possível inserir o registro. Houve um erro.", "Ok");
                                    }
                                }
                                if (galho)
                                {
                                    conn.BeginTransaction();
                                    string sql = $"INSERT INTO AmostTecidos(PKAmostragemMN, IdAmostra) values({pkamost}, '{idfazenda}/T{idtalhao}/P{parc}/R{repparc}/G')";
                                    try { conn.Execute(sql); conn.Commit(); }
                                    catch
                                    {
                                        conn.Rollback();
                                        await DisplayAlert("Message", "Não foi possível inserir o registro. Houve um erro.", "Ok");
                                    }
                                }
                                if (casca)
                                {
                                    conn.BeginTransaction();
                                    string sql = $"INSERT INTO AmostTecidos(PKAmostragemMN, IdAmostra) values({pkamost}, '{idfazenda}/T{idtalhao}/P{parc}/R{repparc}/C')";
                                    try { conn.Execute(sql); conn.Commit(); }
                                    catch
                                    {
                                        conn.Rollback();
                                        await DisplayAlert("Message", "Não foi possível inserir o registro. Houve um erro.", "Ok");
                                    }
                                }
                                if (lenho)
                                {
                                    conn.BeginTransaction();
                                    string sql = $"INSERT INTO AmostTecidos(PKAmostragemMN, IdAmostra) values({pkamost}, '{idfazenda}/T{idtalhao}/P{parc}/R{repparc}/L')";
                                    try { conn.Execute(sql); conn.Commit(); }
                                    catch
                                    {
                                        conn.Rollback();
                                        await DisplayAlert("Message", "Não foi possível inserir o registro. Houve um erro.", "Ok");
                                    }
                                }
                                if (raiz)
                                {
                                    conn.BeginTransaction();
                                    string sql = $"INSERT INTO AmostTecidos(PKAmostragemMN, IdAmostra) values({pkamost}, '{idfazenda}/T{idtalhao}/P{parc}/R{repparc}/R')";
                                    try { conn.Execute(sql); conn.Commit(); }
                                    catch
                                    {
                                        conn.Rollback();
                                        await DisplayAlert("Message", "Não foi possível inserir o registro. Houve um erro.", "Ok");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int reptalh = 1; reptalh <= nreptectalh; reptalh++)
                        {
                            if (folha)
                            {
                                conn.BeginTransaction();
                                string sql = $"INSERT INTO AmostTecidos(PKAmostragemMN, IdAmostra) values({pkamost}, '{idfazenda}/T{idtalhao}/R{reptalh}/F')";
                                try { conn.Execute(sql); conn.Commit(); }
                                catch
                                {
                                    conn.Rollback();
                                    await DisplayAlert("Message", "Não foi possível inserir o registro. Houve um erro.", "Ok");
                                }
                            }
                            if (galho)
                            {
                                conn.BeginTransaction();
                                string sql = $"INSERT INTO AmostTecidos(PKAmostragemMN, IdAmostra) values({pkamost}, '{idfazenda}/T{idtalhao}/R{reptalh}/G')";
                                try { conn.Execute(sql); conn.Commit(); }
                                catch
                                {
                                    conn.Rollback();
                                    await DisplayAlert("Message", "Não foi possível inserir o registro. Houve um erro.", "Ok");
                                }
                            }
                            if (casca)
                            {
                                conn.BeginTransaction();
                                string sql = $"INSERT INTO AmostTecidos(PKAmostragemMN, IdAmostra) values({pkamost}, '{idfazenda}/T{idtalhao}/R{reptalh}/C')";
                                try { conn.Execute(sql); conn.Commit(); }
                                catch
                                {
                                    conn.Rollback();
                                    await DisplayAlert("Message", "Não foi possível inserir o registro. Houve um erro.", "Ok");
                                }
                            }
                            if (lenho)
                            {
                                conn.BeginTransaction();
                                string sql = $"INSERT INTO AmostTecidos(PKAmostragemMN, IdAmostra) values({pkamost}, '{idfazenda}/T{idtalhao}/R{reptalh}/L')";
                                try { conn.Execute(sql); conn.Commit(); }
                                catch
                                {
                                    conn.Rollback();
                                    await DisplayAlert("Message", "Não foi possível inserir o registro. Houve um erro.", "Ok");
                                }
                            }
                            if (raiz)
                            {
                                conn.BeginTransaction();
                                string sql = $"INSERT INTO AmostTecidos(PKAmostragemMN, IdAmostra) values({pkamost}, '{idfazenda}/T{idtalhao}/R{reptalh}/R')";
                                try { conn.Execute(sql); conn.Commit(); }
                                catch
                                {
                                    conn.Rollback();
                                    await DisplayAlert("Message", "Não foi possível inserir o registro. Houve um erro.", "Ok");
                                }
                            }
                        }
                    }
                }


                //string sql = $"SELECT * FROM MedicaoArvore WHERE PKAmostragemMN = {amostragem.PKAmostragemMN}";
                //try { conn.Query<MedicaoArvore>(sql); }
                //catch { await DisplayAlert("Message", "Não foi possível abrir a consulta. Verifique o valor digitado.", "Ok"); }

                await Navigation.PushAsync(new pgAmostTecidos(amostragem.PKTalhoesCR.ToString(),
                    amostragem.PKAmostragemMN.ToString(),
                    idfazenda,
                    idtalhao,
                    amostragem.IdAmostragem,
                    "",
                    1, 1));

            }
        }

        public async void ExportarAmostragemMN_Clicked(object sender, EventArgs e)
        {
            //await DisplayAlert("Informação", message: "Este comando exporta todos os dados de amostragens e as respectivas medições.", "Ok");

            //Create an instance of ExcelEngine.
            using (ExcelEngine excelEngine = new ExcelEngine())
            {

                IApplication application = excelEngine.Excel;

                application.DefaultVersion = ExcelVersion.Excel2016;

                //Create a workbook with a worksheet
                IWorkbook workbook = excelEngine.Excel.Workbooks.Create(6);

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

                //Formatação de números
                worksheet.Range["M1:N1"].NumberFormat = "$.00";
                worksheet.Range["P1:Q1"].NumberFormat = "$.00";
                worksheet.Range["Y1:Y1"].NumberFormat = "$.00";
                worksheet.Range["AB1:AB1"].NumberFormat = "$.00";
                worksheet.Range["AC1:AD1"].NumberFormat = "$.00";

                //Ajusta a largura das colunas
                worksheet.Range["A1:AI1"].AutofitColumns();

                string sql = $"SELECT * FROM TalhoesCR";
                try { conn.Query<TalhoesCR>(sql); }
                catch { await DisplayAlert("Message", "Não foi possível abrir a tabela TalhoesCR.", "Ok"); }
                sql.First();

                var qryAmostragemMN = conn.Query<AmostragemMN>("SELECT PKTalhoesCR FROM AmostragemMN GROUP BY PKTalhoesCR ORDER BY PKTalhoesCR");
                //var ListaAmostragem = conn.Table<AmostragemMN>();
                if (qryAmostragemMN.Count() > 0)
                {
                    qryAmostragemMN.First();
                    int i = 1;
                    foreach (var campoam in qryAmostragemMN)
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
                                    catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PKTalhoesCR'", "Ok"); }
                                    try { worksheet.Range["B" + i].Number = campotalh.CodTalhoesCRSR; }
                                    catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodTalhoesCRSR'", "Ok"); }
                                    try { worksheet.Range["C" + i].Number = campotalh.CodInform_Local; }
                                    catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodInform_Local'", "Ok"); }
                                    try { worksheet.Range["D" + i].Number = campotalh.CodCadastro; }
                                    catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodCadastro'", "Ok"); }
                                    if (campotalh.CodEPTCR != null)
                                    {
                                        try { worksheet.Range["E" + i].Text = campotalh.CodEPTCR; }
                                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodEPTCR'", "Ok"); }
                                    }
                                    if (campotalh.Empresa != null)
                                    {
                                        try { worksheet.Range["F" + i].Text = campotalh.Empresa; }
                                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Empresa'", "Ok"); }
                                    }
                                    if (campotalh.Unidade != null)
                                    {
                                        try { worksheet.Range["G" + i].Text = campotalh.Unidade; }
                                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Unidade'", "Ok"); }
                                    }
                                    if (campotalh.Fazenda != null)
                                    {
                                        try { worksheet.Range["H" + i].Text = campotalh.Fazenda; }
                                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Fazenda'", "Ok"); }
                                    }
                                    if (campotalh.Talhao != null)
                                    {
                                        try { worksheet.Range["I" + i].Text = campotalh.Talhao; }
                                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Talhao'", "Ok"); }
                                    }
                                    if (campotalh.Subtalhao != null)
                                    {
                                        try { worksheet.Range["J" + i].Text = campotalh.Subtalhao; }
                                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Subtalhao'", "Ok"); }
                                    }
                                    try { worksheet.Range["K" + i].Number = campotalh.Ciclo; }
                                    catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Ciclo'", "Ok"); }
                                    try { worksheet.Range["L" + i].Number = campotalh.Rotacao; }
                                    catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Rotacao'", "Ok"); }
                                    try { worksheet.Range["M" + i].Number = double.Parse(campotalh.EspEL.ToString("#.00")); }
                                    catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'EspEL'", "Ok"); }
                                    try { worksheet.Range["N" + i].Number = double.Parse(campotalh.EspEP.ToString("#.00")); }
                                    catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'EspEP'", "Ok"); }
                                    if (campotalh.DataIniRot != null)
                                    {
                                        try { worksheet.Range["O" + i].Value = campotalh.DataIniRot; }
                                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'DataIniRot'", "Ok"); }
                                    }
                                    try { worksheet.Range["P" + i].Number = double.Parse(campotalh.AreaTalh.ToString("#.00")); }
                                    catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'AreaTalh'", "Ok"); }
                                    try { worksheet.Range["Q" + i].Number = double.Parse(campotalh.AreaSubtalh.ToString("#.00")); }
                                    catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'AreaSubtalh'", "Ok"); }
                                    if (campotalh.Especie != null)
                                    {
                                        try { worksheet.Range["R" + i].Text = campotalh.Especie; }
                                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Especie'", "Ok"); }
                                    }
                                    if (campotalh.MatGen != null)
                                    {
                                        try { worksheet.Range["S" + i].Text = campotalh.MatGen; }
                                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'MatGen'", "Ok"); }
                                    }
                                    if (campotalh.Propag != null)
                                    {
                                        try { worksheet.Range["T" + i].Text = campotalh.Propag; }
                                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Tipo Propagação'", "Ok"); }
                                    }
                                    if (campotalh.SoloClasse != null)
                                    {
                                        try { worksheet.Range["U" + i].Text = campotalh.SoloClasse; }
                                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'SoloClasse'", "Ok"); }
                                    }
                                    if (campotalh.UnidMan != null)
                                    {
                                        try { worksheet.Range["V" + i].Text = campotalh.UnidMan; }
                                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'UnidMan'", "Ok"); }
                                    }
                                    try { worksheet.Range["W" + i].Number = campotalh.LatitudeLocG; }
                                    catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'LatitudeLocG'", "Ok"); }
                                    try { worksheet.Range["X" + i].Number = campotalh.LatitudeLocM; }
                                    catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'LatitudeLocM'", "Ok"); }
                                    try { worksheet.Range["Y" + i].Number = double.Parse(campotalh.LatitudeLocS.ToString("#.00")); }
                                    catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'LatitudeLocS'", "Ok"); }
                                    try { worksheet.Range["Z" + i].Number = campotalh.LongitudeLocG; }
                                    catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'LongitudeLocG'", "Ok"); }
                                    try { worksheet.Range["AA" + i].Number = campotalh.LongitudeLocM; }
                                    catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'LongitudeLocM'", "Ok"); }
                                    try { worksheet.Range["AB" + i].Number = double.Parse(campotalh.LongitudeLocS.ToString("#.00")); }
                                    catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'LongitudeLocS'", "Ok"); }
                                    try { worksheet.Range["AC" + i].Number = double.Parse(campotalh.UTMN.ToString("#.00")); }
                                    catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'UTMN'", "Ok"); }
                                    try { worksheet.Range["AD" + i].Number = double.Parse(campotalh.UTME.ToString("#.00")); }
                                    catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'UTME'", "Ok"); }
                                    if (campotalh.Datum != null)
                                    {
                                        try { worksheet.Range["AE" + i].Text = campotalh.Datum; }
                                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Datum'", "Ok"); }
                                    }
                                    if (campotalh.Zona != null)
                                    {
                                        try { worksheet.Range["AF" + i].Text = campotalh.Zona; }
                                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Zona'", "Ok"); }
                                    }
                                    if (campotalh.Lat != null)
                                    {
                                        try { worksheet.Range["AG" + i].Text = campotalh.Lat; }
                                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Lat'", "Ok"); }
                                    }
                                    if (campotalh.Lon != null)
                                    {
                                        try { worksheet.Range["AH" + i].Text = campotalh.Lon; }
                                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Lon'", "Ok"); }
                                    }
                                    if (campotalh.Observacoes != null)
                                    {
                                        try { worksheet.Range["AI" + i].Text = campotalh.Observacoes; }
                                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Observacoes'", "Ok"); }
                                    }
                                }
                            }
                        }
                    }
                }

                //Acrescenta a planilha de amostragem.
                IWorksheet worksheet1 = workbook.Worksheets[1];
                worksheet1.Name = "Amostragem_MN";

                //Enter values to the cells from A3 to A5
                worksheet1.Range["A1"].Text = "PKAmostragemMN"; //PKAmostragemMN
                worksheet1.Range["B1"].Text = "PKTalhoesCR";//PKTalhoesCR
                worksheet1.Range["C1"].Text = "Código da amostragem SR";//CodAmostragemMNSR
                worksheet1.Range["D1"].Text = "Código da amostragem MN";//CodAmostragemMNMN
                worksheet1.Range["E1"].Text = "Identif. amostragem";//IdAmostragem
                worksheet1.Range["F1"].Text = "Data da amostragem";//DataAmost
                worksheet1.Range["G1"].Text = "Objetivo";//Objetivo
                worksheet1.Range["H1"].Text = "Núm de parcelas";//NParcelas
                worksheet1.Range["I1"].Text = "Núm de árvores/parc.";//NArvParc
                worksheet1.Range["J1"].Text = "Responsável";//Responsavel
                worksheet1.Range["K1"].Text = "Observações";//Observacoes

                //Formata fonte da linha de títulos
                worksheet1.Range["A1:K1"].CellStyle.Font.Bold = true;
                worksheet1.Range["A1:K1"].CellStyle.Font.Italic = true;
                worksheet1.Range["A1:D1"].CellStyle.Font.RGBColor = Color.FromArgb(200, 50, 50);

                //Colore as células A1 a D1
                worksheet1.Range["A1:D1"].CellStyle.Color = Color.FromArgb(150, 150, 150);

                //Borda inferior da linha de títulos
                worksheet1.Range["A1:K1"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                worksheet1.Range["A1:K1"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].Color = ExcelKnownColors.Black;

                //Ajusta a largura das colunas
                worksheet1.Range["A1:K1"].AutofitColumns();

                sql = $"SELECT * FROM AmostragemMN";
                try { conn.Query<AmostragemMN>(sql); }
                catch { await DisplayAlert("Message", "Não foi possível abrir a tabela AmostragemMN.", "Ok"); }
                sql.First();

                var qryAmostragem = conn.Query<AmostragemMN>("SELECT * FROM AmostragemMN");

                var ListaAmostragem = conn.Table<AmostragemMN>();

                if (ListaAmostragem.Count() > 0)
                {
                    int i = 1;
                    foreach (var campo in ListaAmostragem)
                    {
                        i = i + 1;

                        try { worksheet1.Range["A" + i].Number = campo.PKAmostragemMN; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PKAmostragemMN'", "Ok"); }
                        try { worksheet1.Range["B" + i].Number = campo.PKTalhoesCR; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PKTalhoesCR'", "Ok"); }
                        try { worksheet1.Range["C" + i].Number = campo.CodAmostragemMNSR; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodAmostragemMNSR'", "Ok"); }
                        try { worksheet1.Range["D" + i].Number = campo.CodAmostragemMNMN; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodAmostragemMNMN'", "Ok"); }
                        if (campo.IdAmostragem != null)
                        {
                            try { worksheet1.Range["E" + i].Text = campo.IdAmostragem; }
                            catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'IdAmostragem'", "Ok"); }
                        }
                        if (campo.DataAmost != null)
                        {
                            try { worksheet1.Range["F" + i].Value = campo.DataAmost; }
                            catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'DataAmost'", "Ok"); }
                        }
                        if (campo.Objetivo != null)
                        {
                            try { worksheet1.Range["G" + i].Text = campo.Objetivo; }
                            catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Objetivo'", "Ok"); }
                        }
                        try { worksheet1.Range["H" + i].Number = campo.NParcelas; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'NParcelas'", "Ok"); }
                        try { worksheet1.Range["I" + i].Number = campo.NArvParc; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'NArvParc'", "Ok"); }
                        if (campo.Responsavel != null)
                        {
                            try { worksheet1.Range["J" + i].Text = campo.Responsavel; }
                            catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Responsavel'", "Ok"); }
                        }
                        if (campo.Observacoes != null)
                        {
                            try { worksheet1.Range["K" + i].Text = campo.Observacoes; }
                            catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Observacoes'", "Ok"); }
                        }
                    }
                }

                /*******************Faz parte do código antigo
                 * 
                //Acrescenta a planilha de medições.
                IWorksheet worksheet2 = workbook.Worksheets[2];
                worksheet2.Name = "Medições";

                //Enter values to the cells from A3 to A5
                worksheet2.Range["A1"].Text = "PKMedicaoArv"; //PKMedicaoArv
                worksheet2.Range["B1"].Text = "Código da amostragem MN";//PKAmostragemMN
                worksheet2.Range["C1"].Text = "Código da medição SR";//CodMedicaoArvSR
                worksheet2.Range["D1"].Text = "Código da medição MN";//CodMedicaoArvMN
                worksheet2.Range["E1"].Text = "Núm. da árvore";//ArvNum
                worksheet2.Range["F1"].Text = "Núm. do fuste";//FusteNum
                worksheet2.Range["G1"].Text = "Tipo med. diâmétrica";//TipoMedD
                worksheet2.Range["H1"].Text = "Altura total (m)";//HTotal
                worksheet2.Range["I1"].Text = "DAP/CAP (cm)";//MedD
                worksheet2.Range["J1"].Text = "Altura dom. (m)";//HDom
                worksheet2.Range["K1"].Text = "Altura do fuste (m)";//HFuste
                worksheet2.Range["L1"].Text = "Altura do fuste (m)";//HFuste
                worksheet2.Range["M1"].Text = "Altura do fuste (m)";//HFuste
                worksheet2.Range["N1"].Text = "Observações";//Observacoes

                //Formata fonte da linha de títulos
                worksheet2.Range["A1:N1"].CellStyle.Font.Bold = true;
                worksheet2.Range["A1:N1"].CellStyle.Font.Italic = true;
                worksheet2.Range["A1:D1"].CellStyle.Font.RGBColor = Color.FromArgb(200, 50, 50);

                //Colore as células A1 a D1
                worksheet2.Range["A1:D1"].CellStyle.Color = Color.FromArgb(150, 150, 150);

                //Borda inferior da linha de títulos
                worksheet2.Range["A1:N1"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                worksheet2.Range["A1:N1"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].Color = ExcelKnownColors.Black;

                //Ajusta a largura das colunas
                worksheet2.Range["A1:N1"].AutofitColumns();

                sql = $"SELECT * FROM MedicaoArvore";
                try { conn.Query<MedicaoArvore>(sql); }
                catch { await DisplayAlert("Message", "Não foi possível abrir a tabela MedicaoArvore.", "Ok"); }
                sql.First();

                var qryMedicoes = conn.Query<MedicaoArvore>("SELECT * FROM MedicaoArvore");
                //qryMedicoes.First();
                //if (qryMedicoes.Count() > 0)
                //{
                //    //
                //}

                var ListaMedicoes = conn.Table<MedicaoArvore>();

                if (ListaMedicoes.Count() > 0)
                {
                    int i = 1;
                    foreach (var campo in ListaMedicoes)
                    {
                        i = i + 1;

                        try { worksheet2.Range["A" + i].Number = campo.PKMedicaoArv; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PKMedicaoArv'", "Ok"); }
                        try { worksheet2.Range["B" + i].Number = campo.PKAmostragemMN; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PKAmostragemMN'", "Ok"); }
                        try { worksheet2.Range["C" + i].Number = campo.CodMedicaoArvSR; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodMedicaoArvSR'", "Ok"); }
                        try { worksheet2.Range["D" + i].Number = campo.CodMedicaoArvMN; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodMedicaoArvMN'", "Ok"); }
                        try { worksheet2.Range["E" + i].Number = campo.ArvNum; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'ArvNum'", "Ok"); }
                        try { worksheet2.Range["F" + i].Number = campo.FusteNum; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'FusteNum'", "Ok"); }
                        if (campo.TipoMedD != null)
                        {
                            try { worksheet2.Range["G" + i].Text = campo.TipoMedD; }
                            catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'TipoMedD'", "Ok"); }
                        }
                        try { worksheet.Range["H" + i].Number = campo.HTotal; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'HTotal'", "Ok"); }
                        try { worksheet.Range["I" + i].Number = campo.MedD; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'MedD'", "Ok"); }
                        try { worksheet.Range["J" + i].Number = campo.HDom; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'HDom'", "Ok"); }
                        try { worksheet.Range["K" + i].Number = campo.HFuste; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'HFuste'", "Ok"); }
                        try { worksheet.Range["L" + i].Number = campo.DCopa; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'DCopa'", "Ok"); }
                        try { worksheet.Range["M" + i].Number = campo.HCopa; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'HCopa'", "Ok"); }
                        if (campo.Observacoes != null)
                        {
                            try { worksheet2.Range["N" + i].Text = campo.Observacoes; }
                            catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Observacoes'", "Ok"); }
                        }
                    }
                }
                */

                //Acrescenta a planilha de parcelas.
                IWorksheet worksheet2 = workbook.Worksheets[2];
                worksheet2.Name = "Parcelas";

                //Enter values to the cells from A3 to A5
                worksheet2.Range["A1"].Text = "PKParcelasMedArv"; //PKParcelasMedArv
                worksheet2.Range["B1"].Text = "Código da amostragem MN";//PKAmostragemMN
                worksheet2.Range["C1"].Text = "Código da parcela SR";//CodParcelasMedArvSR
                worksheet2.Range["D1"].Text = "Código da parcela MN";//CodParcelasMedArvMN
                worksheet2.Range["E1"].Text = "Identif. da parcela";//IdParcela
                worksheet2.Range["F1"].Text = "Largura (m)";//Largura
                worksheet2.Range["G1"].Text = "Comprimento (m)";//Comprimento
                worksheet2.Range["H1"].Text = "Área (m2)";//Area
                worksheet2.Range["I1"].Text = "Número de covas";//NumCovas
                worksheet2.Range["J1"].Text = "Latitude (GD)";//Latit
                worksheet2.Range["K1"].Text = "Longitude (GD)";//Longit
                worksheet2.Range["L1"].Text = "Observações";//Observacoes

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

                sql = $"SELECT * FROM ParcelasMedArv";
                try { conn.Query<ParcelasMedArv>(sql); }
                catch { await DisplayAlert("Message", "Não foi possível abrir a tabela ParcelasMedArv.", "Ok"); }
                sql.First();

                var qryParcelas = conn.Query<ParcelasMedArv>("SELECT * FROM ParcelasMedArv");
                //qryParcelasMedArv.First();
                //if (qryParcelasMedArv.Count() > 0)
                //{
                //    //
                //}

                var ListaParcelas = conn.Table<ParcelasMedArv>();

                if (ListaParcelas.Count() > 0)
                {
                    int i = 1;
                    foreach (var campo in ListaParcelas)
                    {
                        i = i + 1;

                        try { worksheet2.Range["A" + i].Number = campo.PKParcelasMedArv; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PKParcelasMedArv'", "Ok"); }
                        try { worksheet2.Range["B" + i].Number = campo.PKAmostragemMN; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PKAmostragemMN'", "Ok"); }
                        try { worksheet2.Range["C" + i].Number = campo.CodParcelasMedArvSR; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodParcelasMedArvSR'", "Ok"); }
                        try { worksheet2.Range["D" + i].Number = campo.CodParcelasMedArvMN; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodParcelasMedArvMN'", "Ok"); }
                        if (campo.IdParcela != null)
                        {
                            try { worksheet2.Range["E" + i].Text = campo.IdParcela; }
                            catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'IdParcela'", "Ok"); }
                        }
                        try { worksheet2.Range["F" + i].Number = double.Parse(campo.Largura.ToString("#.00")); }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Largura'", "Ok"); }
                        try { worksheet2.Range["G" + i].Number = double.Parse(campo.Comprimento.ToString("#.00")); }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Comprimento'", "Ok"); }
                        try { worksheet2.Range["H" + i].Number = double.Parse(campo.Area.ToString("#.00")); }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Area'", "Ok"); }
                        try { worksheet2.Range["I" + i].Number = campo.NumCovas; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'NumCovas'", "Ok"); }
                        try { worksheet2.Range["J" + i].Number = double.Parse(campo.Latit.ToString("#.0000000")); }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Latit'", "Ok"); }
                        try { worksheet2.Range["K" + i].Number = double.Parse(campo.Longit.ToString("#.0000000")); }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Longit'", "Ok"); }
                        if (campo.Observacoes != null)
                        {
                            try { worksheet2.Range["L" + i].Text = campo.Observacoes; }
                            catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Observacoes'", "Ok"); }
                        }
                    }
                }


                //Acrescenta a planilha de medições.
                IWorksheet worksheet3 = workbook.Worksheets[3];
                worksheet3.Name = "Medições";

                //Enter values to the cells from A3 to A5
                worksheet3.Range["A1"].Text = "PKMedicaoArv1"; //PKMedicaoArv1
                worksheet3.Range["B1"].Text = "Código da parcela";//PKParcelasMedArv
                worksheet3.Range["C1"].Text = "Código da medição SR";//CodMedicaoArv1SR
                worksheet3.Range["D1"].Text = "Código da medição MN";//CodMedicaoArv1MN
                worksheet3.Range["E1"].Text = "Parcela";//IdParcela
                worksheet3.Range["F1"].Text = "Árvore";//ArvNum
                worksheet3.Range["G1"].Text = "Fuste";//FusteNum
                worksheet3.Range["H1"].Text = "DAP/CAP";//TipoMedD
                worksheet3.Range["I1"].Text = "Altura total (m)";//HTotal
                worksheet3.Range["J1"].Text = "Medida diamétrica";//MedD
                worksheet3.Range["K1"].Text = "Altura dominante";//HDom
                worksheet3.Range["L1"].Text = "Altura do fuste (m)";//HFuste
                worksheet3.Range["M1"].Text = "Diâmetro da copa (m)";//DCopa
                worksheet3.Range["N1"].Text = "Altura da copa (m)";//HCopa
                worksheet3.Range["O1"].Text = "Observações";//Observacoes
                //Formata fonte da linha de títulos
                worksheet3.Range["A1:O1"].CellStyle.Font.Bold = true;
                worksheet3.Range["A1:O1"].CellStyle.Font.Italic = true;
                worksheet3.Range["A1:D1"].CellStyle.Font.RGBColor = Color.FromArgb(200, 50, 50);

                //Colore as células A1 a D1
                worksheet3.Range["A1:D1"].CellStyle.Color = Color.FromArgb(150, 150, 150);

                //Borda inferior da linha de títulos
                worksheet3.Range["A1:O1"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                worksheet3.Range["A1:O1"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].Color = ExcelKnownColors.Black;

                //Ajusta a largura das colunas
                worksheet3.Range["A1:O1"].AutofitColumns();


                sql = $"SELECT MedArv.PKMedicaoArv1, Parc.IdParcela, MedArv.PKParcelasMedArv, MedArv.ArvNum, " +
                                $"MedArv.FusteNum, MedArv.TipoMedD, MedArv.HTotal, MedArv.MedD, MedArv.HDom, MedArv.HFuste, MedArv.DCopa, " +
                                $"MedArv.HCopa, MedArv.Observacoes, MedArv.Selec FROM MedicaoArvore1 MedArv JOIN ParcelasMedArv Parc " +
                                $"ON MedArv.PKParcelasMedArv = Parc.PKParcelasMedArv " +
                                $"ORDER BY MedArv.PKParcelasMedArv, MedArv.ArvNum";

                try { conn.Query<MedicaoArvore1>(sql); }
                catch { await DisplayAlert("Message", "Não foi possível abrir a tabela MedicaoArvore1.", "Ok"); }

                sql.First();


                var ListaMedicoes = conn.Query<MedicaoArvore1>(sql);

                if (ListaMedicoes.Count() > 0)
                {
                    int i = 1;
                    foreach (var campo in ListaMedicoes)
                    {
                        i = i + 1;

                        try { worksheet3.Range["A" + i].Number = campo.PKMedicaoArv1; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PKMedicaoArv1'", "Ok"); }
                        try { worksheet3.Range["B" + i].Number = campo.PKParcelasMedArv; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PKParcelasMedArv'", "Ok"); }
                        try { worksheet3.Range["C" + i].Number = campo.CodMedicaoArv1SR; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodMedicaoArv1SR'", "Ok"); }
                        try { worksheet3.Range["D" + i].Number = campo.CodMedicaoArv1MN; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodMedicaoArv1MN'", "Ok"); }
                        if (campo.IdParcela != null)
                        {
                            try { worksheet3.Range["E" + i].Text = campo.IdParcela; }
                            catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'IdParcela'", "Ok"); }
                        }
                        try { worksheet3.Range["F" + i].Number = campo.ArvNum; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'ArvNum'", "Ok"); }
                        try { worksheet3.Range["G" + i].Number = campo.FusteNum; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'FusteNum'", "Ok"); }
                        if (campo.TipoMedD != null)
                        {
                            try { worksheet3.Range["H" + i].Text = campo.TipoMedD; }
                            catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'TipoMedD'", "Ok"); }
                        }
                        try { worksheet3.Range["I" + i].Number = double.Parse(campo.HTotal.ToString("#.00")); }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'HTotal'", "Ok"); }
                        try { worksheet3.Range["J" + i].Number = double.Parse(campo.MedD.ToString("#.00")); }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'MedD'", "Ok"); }
                        try { worksheet3.Range["K" + i].Number = double.Parse(campo.HDom.ToString("#.00")); }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'HDom'", "Ok"); }
                        try { worksheet3.Range["L" + i].Number = double.Parse(campo.HFuste.ToString("#.00")); }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'HFuste'", "Ok"); }
                        try { worksheet3.Range["M" + i].Number = double.Parse(campo.DCopa.ToString("#.00")); }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'DCopa'", "Ok"); }
                        try { worksheet3.Range["N" + i].Number = double.Parse(campo.HCopa.ToString("#.00")); }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'HCopa'", "Ok"); }
                        if (campo.Observacoes != null)
                        {
                            try { worksheet3.Range["O" + i].Text = campo.Observacoes; }
                            catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Observacoes'", "Ok"); }
                        }
                    }
                }


                //Acrescenta a planilha de amostragens de tecidos.
                IWorksheet worksheet4 = workbook.Worksheets[4];
                worksheet4.Name = "Amost. tecidos";

                //Enter values to the cells from A3 to A5
                worksheet4.Range["A1"].Text = "PKAmostTecidos"; //PKAmostTecidos
                worksheet4.Range["B1"].Text = "Código da amostragem";//PKAmostragemMN
                worksheet4.Range["C1"].Text = "Código da am. tecidos SR";//CodAmostTecidosSR
                worksheet4.Range["D1"].Text = "Código da am. tecidos MN";//CodAmostTecidosMN
                worksheet4.Range["E1"].Text = "Identif. da amostra";//IdAmostra
                worksheet4.Range["F1"].Text = "Data da amostragem";//DataAmostra
                worksheet4.Range["G1"].Text = "Repetição";//Repeticao
                worksheet4.Range["H1"].Text = "Componente";//Componente
                worksheet4.Range["I1"].Text = "Objetivo";//Objetivo
                worksheet4.Range["J1"].Text = "Núm. de am. simples";//NAmSimples
                worksheet4.Range["K1"].Text = "Responsável";//Responsavel
                worksheet4.Range["L1"].Text = "Observações";//Observacoes
                //Formata fonte da linha de títulos
                worksheet4.Range["A1:L1"].CellStyle.Font.Bold = true;
                worksheet4.Range["A1:L1"].CellStyle.Font.Italic = true;
                worksheet4.Range["A1:D1"].CellStyle.Font.RGBColor = Color.FromArgb(200, 50, 50);

                //Colore as células A1 a D1
                worksheet4.Range["A1:D1"].CellStyle.Color = Color.FromArgb(150, 150, 150);

                //Borda inferior da linha de títulos
                worksheet4.Range["A1:L1"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                worksheet4.Range["A1:L1"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].Color = ExcelKnownColors.Black;

                //Ajusta a largura das colunas
                worksheet4.Range["A1:L1"].AutofitColumns();

                sql = $"SELECT * FROM AmostTecidos";
                try { conn.Query<AmostTecidos>(sql); }
                catch
                {
                    await DisplayAlert("Message", "Não foi possível abrir a tabela AmostTecidos.", "Ok");
                }
                sql.First();

                var qryAmTecidos = conn.Query<AmostTecidos>("SELECT * FROM AmostTecidos");
                //qryParcelasMedArv.First();
                //if (qryParcelasMedArv.Count() > 0)
                //{
                //    //
                //}

                var ListaAmTecidos = conn.Table<AmostTecidos>();

                if (ListaAmTecidos.Count() > 0)
                {
                    int i = 1;
                    foreach (var campo in ListaAmTecidos)
                    {
                        i = i + 1;

                        try { worksheet4.Range["A" + i].Number = campo.PKAmostTecidos; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PKAmostTecidos'", "Ok"); }
                        try { worksheet4.Range["B" + i].Number = campo.PKAmostragemMN; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PKAmostragemMN'", "Ok"); }
                        try { worksheet4.Range["C" + i].Number = campo.CodAmostTecidosSR; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodAmostTecidosSR'", "Ok"); }
                        try { worksheet4.Range["D" + i].Number = campo.CodAmostTecidosMN; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodAmostTecidosMN'", "Ok"); }
                        if (campo.IdAmostra != null)
                        {
                            try { worksheet4.Range["E" + i].Text = campo.IdAmostra; }
                            catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'IdAmostra'", "Ok"); }
                        }
                        if (campo.DataAmostra != null)
                        {
                            try { worksheet4.Range["F" + i].Value = campo.DataAmostra; }
                            catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'DataAmostra'", "Ok"); }
                        }
                        try { worksheet4.Range["G" + i].Number = campo.Repeticao; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Repeticao'", "Ok"); }
                        if (campo.Componente != null)
                        {
                            try { worksheet4.Range["H" + i].Text = campo.Componente; }
                            catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Componente'", "Ok"); }
                        }
                        if (campo.Objetivo != null)
                        {
                            try { worksheet4.Range["I" + i].Text = campo.Objetivo; }
                            catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Objetivo'", "Ok"); }
                        }
                        try { worksheet4.Range["J" + i].Number = campo.NAmSimples; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'NAmSimples'", "Ok"); }
                        if (campo.Responsavel != null)
                        {
                            try { worksheet4.Range["K" + i].Text = campo.Responsavel; }
                            catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Responsavel'", "Ok"); }
                        }
                        if (campo.Observacoes != null)
                        {
                            try { worksheet4.Range["L" + i].Text = campo.Observacoes; }
                            catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Observacoes'", "Ok"); }
                        }
                    }
                }


                //Acrescenta a planilha de amostragens de solos.
                IWorksheet worksheet5 = workbook.Worksheets[5];
                worksheet5.Name = "Amost. solos";

                //Enter values to the cells from A3 to A5
                worksheet5.Range["A1"].Text = "PKAmostSolos"; //PKAmostSolos
                worksheet5.Range["B1"].Text = "Código da amostragem";//PKAmostragemMN
                worksheet5.Range["C1"].Text = "Código da am. solos SR";//CodAmostSolosSR
                worksheet5.Range["D1"].Text = "Código da am. solos MN";//CodAmostSolosMN
                worksheet5.Range["E1"].Text = "Identif. da amostra";//IdAmostra
                worksheet5.Range["F1"].Text = "Data da amostragem";//DataAmostra
                worksheet5.Range["G1"].Text = "Repetição";//Repeticao
                worksheet5.Range["H1"].Text = "Prof. inic.";//ProfIni
                worksheet5.Range["I1"].Text = "Prof. fin.";//ProfFin
                worksheet5.Range["J1"].Text = "Objetivo";//Objetivo
                worksheet5.Range["K1"].Text = "Núm. de am. simples";//NAmSimples
                worksheet5.Range["L1"].Text = "Responsável";//Responsavel
                worksheet5.Range["M1"].Text = "Observações";//Observacoes
                //Formata fonte da linha de títulos
                worksheet5.Range["A1:M1"].CellStyle.Font.Bold = true;
                worksheet5.Range["A1:M1"].CellStyle.Font.Italic = true;
                worksheet5.Range["A1:D1"].CellStyle.Font.RGBColor = Color.FromArgb(200, 50, 50);

                //Colore as células A1 a D1
                worksheet5.Range["A1:D1"].CellStyle.Color = Color.FromArgb(150, 150, 150);

                //Borda inferior da linha de títulos
                worksheet5.Range["A1:M1"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                worksheet5.Range["A1:M1"].CellStyle.Borders[ExcelBordersIndex.EdgeBottom].Color = ExcelKnownColors.Black;

                //Ajusta a largura das colunas
                worksheet5.Range["A1:M1"].AutofitColumns();

                sql = $"SELECT * FROM AmostSolos";
                try { conn.Query<AmostSolos>(sql); }
                catch { await DisplayAlert("Message", "Não foi possível abrir a tabela AmostSolos.", "Ok"); }
                sql.First();

                var qryAmSolos = conn.Query<AmostSolos>("SELECT * FROM AmostSolos");
                //qryParcelasMedArv.First();
                //if (qryParcelasMedArv.Count() > 0)
                //{
                //    //
                //}

                var ListaAmSolos = conn.Table<AmostSolos>();

                if (ListaAmSolos.Count() > 0)
                {
                    int i = 1;
                    foreach (var campo in ListaAmSolos)
                    {
                        i = i + 1;

                        try { worksheet5.Range["A" + i].Number = campo.PKAmostSolos; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PKAmostSolos'", "Ok"); }
                        try { worksheet5.Range["B" + i].Number = campo.PKAmostragemMN; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'PKAmostragemMN'", "Ok"); }
                        try { worksheet5.Range["C" + i].Number = campo.CodAmostSolosSR; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodAmostSolosSR'", "Ok"); }
                        try { worksheet5.Range["D" + i].Number = campo.CodAmostSolosMN; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'CodAmostSolosMN'", "Ok"); }
                        if (campo.IdAmostra != null)
                        {
                            try { worksheet5.Range["E" + i].Text = campo.IdAmostra; }
                            catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'IdAmostra'", "Ok"); }
                        }
                        if (campo.DataAmostra != null)
                        {
                            try { worksheet5.Range["F" + i].Value = campo.DataAmostra; }
                            catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'DataAmostra'", "Ok"); }
                        }
                        try { worksheet5.Range["G" + i].Number = campo.Repeticao; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Repeticao'", "Ok"); }
                        try { worksheet5.Range["H" + i].Number = double.Parse(campo.ProfIni.ToString("#.00")); }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'ProfIni'", "Ok"); }
                        try { worksheet5.Range["I" + i].Number = double.Parse(campo.ProfFin.ToString("#.00")); }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'ProfFin'", "Ok"); }
                        if (campo.Objetivo != null)
                        {
                            try { worksheet5.Range["J" + i].Text = campo.Objetivo; }
                            catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Objetivo'", "Ok"); }
                        }
                        try { worksheet5.Range["K" + i].Number = campo.NAmSimples; }
                        catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'NAmSimples'", "Ok"); }
                        if (campo.Responsavel != null)
                        {
                            try { worksheet5.Range["L" + i].Text = campo.Responsavel; }
                            catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Responsavel'", "Ok"); }
                        }
                        if (campo.Observacoes != null)
                        {
                            try { worksheet5.Range["M" + i].Text = campo.Observacoes; }
                            catch { await DisplayAlert("Message", "Não foi possível transferir o valor do campo 'Observacoes'", "Ok"); }
                        }
                    }
                }

                //Save the workbook to stream in xlsx format. 
                MemoryStream stream = new MemoryStream();
                workbook.SaveAs(stream);

                string data = DateTime.Now.ToString(("yyyy-MM-dd hh:mm:ss"));

                workbook.Close();

                //Save the stream as a file in the device and invoke it for viewing
                await Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("Monit_" + data + ".xlsx", "application/msexcel", stream);
            }
        }

        private void Informacoes_Clicked(object sender, EventArgs e)
        {
            msg = $"Esta tela exibe a lista de amostragens realizadas para fins do monitoramento nutricional e peprmite a edição.\n\n" +
                $"Os comandos são os seguintes:\n\nNOVA AMOSTRAGEM MN: incluir novo registro por meio de digitação direta no SIGNUS;\n\n" +
                $"A partir do menu (três pontinhos) no cato superior direito da tela, há a opção de exportar " +
                $"os dados das amostragens para uma planilha Excel. O arquivo gerado conterá os dados dos talhões" +
                $"para os quais as amostragens foram realizadas, os dados das amostragens e das medições. O arquivo é " +
                $"disponibilizado na pasta Signus com o nome iniciado por 'Monit_' seguido da data e hora.\n\n" +
                $"Um toque rápido sobre um registro de amostragem expande exibe os detalhes do mesmo;\n\n" +
                $"Para editar um registro já existente, pressione-o até aparecerem as opções na parte superior " +
                $"da tela.\n\n" +
                $"Para editar as medições realizadas por ocasião da amostragem, pressione o registro até aparecer o menu (três pontinhos) " +
                $"no canto superior direito da tela. A partir deste menu, é possível abrir o formulário para cadastro das medições.\n\n" +
                $"A seta localizada no canto superior esquerdo da tela permite retornar à tela anterior.";
            DisplayAlert("SIGNUS", msg, "Ok");
        }


    }
}