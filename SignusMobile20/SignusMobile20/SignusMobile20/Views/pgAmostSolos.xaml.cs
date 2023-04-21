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
	public partial class pgAmostSolos : ContentPage
	{
        private SQLiteConnection conn;
        public AmostSolos amostsolos;
        public string pktalhoescr;
        public string pkamostragem;
        public string pkparcela;
        public string idfazenda;
        public string idtalhoescr;
        public string idamostragemmn;
        public string idparcela;
        public int numamostrascomp;
        public int numcamadas;
        string msg;

        public pgAmostSolos (string pktlh, string pkamost, string pkparc, string idfaz, string idtlh, string idamostrmn, string idparc, int namcomp, int ncmd)
		{
            InitializeComponent();
            conn = DependencyService.Get<ISQLite>().GetConnection();
            //conn.CreateTable<ParcelasMedArv>();
            conn.CreateTable<AmostSolos>();
            pktalhoescr = pktlh;
            pkamostragem = pkamost;
            pkparcela = pkparc;
            idfazenda = idfaz;
            idtalhoescr = idtlh;
            idamostragemmn = idamostrmn;
            idparcela = idparc;
            numamostrascomp = namcomp;
            numcamadas = ncmd;
            IdFazenda.Text = "Fazenda/Proj.: " + idfaz;
            IdTalhao.Text = "Talhão: " + idtlh;
            IdAmostragem.Text = "Amostragem: " + idamostrmn;
            if (idparcela != "") { IdParcela.Text = "Parcela: " + idparcela; }
            else
            { IdParcela.Text = "Amostra(s) composta(s)"; }
        }

        protected override void OnAppearing()
        {
            int num = Convert.ToInt32(pkamostragem);
            //PopulateListaAmostSolos();
            var data = conn.Table<AmostSolos>().Where(x => x.PKAmostragemMN == num).ToList();
            lstAmostSolos.ItemsSource = data;
        }


        //private void PopulateDetails(AmostragemMN details)
        //{
        //    try { PKAmostragemMN.Text = details.PKAmostragemMN.ToString(); }
        //    catch { DisplayAlert("Message", "Não foi possível ler o valor 'PKAmostragemMN'", "Ok"); }
        //}
        private void Mostrar_Clicked(object sender, EventArgs e)
        {
            var data = (from amsolo in conn.Table<AmostSolos>() select amsolo);
            lstAmostSolos.ItemsSource = data;
        }

        private void NovaAmostSolos_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new pgAddAmostSolos(null, pkamostragem, idfazenda, idtalhoescr, idamostragemmn, idparcela));
        }

        private void EditAmostSolos(object sender, ItemTappedEventArgs e)
        {
            AmostSolos details = e.Item as AmostSolos;
            if (details != null)
            {
                Navigation.PushAsync(new pgAddAmostSolos(details, pkamostragem, idfazenda, idtalhoescr, idamostragemmn, idparcela));
            }
        }

        public async void AlternaDetalhes(object sender, ItemTappedEventArgs e)
        {
            //Alterna entre visível e invisível a lista de detalhes
            AmostSolos details = e.Item as AmostSolos;
            string sql = "";

            if (details.Visible.ToString() == "True")
            {
                sql = $"UPDATE AmostSolos SET Visible = 0, Imagem = 'Down.png' ";
            }
            else
            {
                sql = $"UPDATE AmostSolos SET Visible = 1, Imagem = 'Up.png' ";
            }

            sql += $"WHERE PKAmostSolos = {details.PKAmostSolos}";
            try { conn.Execute(sql); }
            catch { await DisplayAlert("Message", "Houve um erro. Não foi possível atualizar.", "Ok"); }

            sql = $"SELECT * FROM AmostSolos WHERE PKAmostragemMN = {pkamostragem}";

            //Atualiza a lista
            var data = conn.Query<AmostSolos>(sql);
            lstAmostSolos.ItemsSource = data;
        }

        private async void DeleteAmostSolos(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var amsolos = mi.CommandParameter as AmostSolos;
            if (amsolos != null)
            {
                var opcao = await DisplayAlert("Excluir registro", "Confirma a exclusão do registro " + amsolos.PKAmostSolos.ToString() + "? ", "Ok", "Cancel");
                if (opcao)
                {
                    string sql = $"DELETE FROM AmostSolos WHERE PKAmostSolos = {amsolos.PKAmostSolos}";

                    try { conn.Execute(sql); }
                    catch { await DisplayAlert("Message", "Houve um erro, não foi possível excluir o registro.", "Ok"); }

                    sql = $"SELECT * FROM AmostSolos WHERE PKAmostragemMN = {pkamostragem}";

                    //Atualiza a lista
                    var data = conn.Query<AmostSolos>(sql);
                    lstAmostSolos.ItemsSource = data;
                }
            }
        }

        private async void AlterarAmostSolos(object sender, EventArgs e)
        {
            try
            {
                var mi = ((MenuItem)sender);
                var amsolos = mi.CommandParameter as AmostSolos;
                if (amsolos != null)
                {
                    await Navigation.PushAsync(new pgAddAmostSolos(amsolos, pkamostragem, idfazenda, idtalhoescr, idamostragemmn, idparcela));
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
                var amsolos = mi.CommandParameter as AmostSolos;
                if (amsolos != null)
                {
                    //                   await Navigation.PushAsync(new pgAddAmostSolos1(medarv, AmostSolos.PKAmostragemMN.ToString()));
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro: ", ex.Message, "Ok");
            }

        }

        private void Informacoes_Clicked(object sender, EventArgs e)
        {
            msg = $"Esta tela exibe a lista de amostras de solos já realizadas para o monitoramento nutricional e permite a edição.\n\n" +
                $"IMPORTANTE:\nAnote na embalagem a identificação da amostra gerado pelo SIGNUS e acrescente a " +
                $"profundidade (0-20 ou 20-40).\n\n" +
                $"Os comandos desta tela são os seguintes:\n\nNOVA AMOSTRA DE SOLOS: incluir novo registro por meio de digitação direta no SIGNUS;\n\n" +
                $"Um toque rápido sobre um registro de amostra expande exibe os detalhes do mesmo;\n\n" +
                $"Para editar um registro já existente, pressione-o até aparecerem as opções na parte superior " +
                $"da tela.\n\n" +
                $"A seta localizada no canto superior esquerdo da tela permite retornar à tela anterior.";
            DisplayAlert("SIGNUS", msg, "Ok");
        }

    }
}