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
	public partial class pgAmostTecidos : ContentPage
	{
        private SQLiteConnection conn;
        public MedicaoArvore medicaoarvore;
        public string pktalhoescr;
        public string pkamostragemmn;
        public string idfazenda;
        public string idtalhoescr;
        public string idamostragemmn;
        public string idparcela;
        public int numamostrascomp;
        public int numcomponentes;
        string msg;
        public pgAmostTecidos(string pktlh, string pkamostmn, string idfaz, string idtlh, string idamostrmn, string idparc, int namcomp, int ncompon)
        {
            InitializeComponent();
            conn = DependencyService.Get<ISQLite>().GetConnection();
            //conn.CreateTable<ParcelasMedArv>();
            conn.CreateTable<AmostTecidos>();
            pktalhoescr = pktlh;
            pkamostragemmn = pkamostmn;
            idfazenda = idfaz;
            idtalhoescr = idtlh;
            idamostragemmn = idamostrmn;
            idparcela = idparc;
            numamostrascomp = namcomp;
            numcomponentes = ncompon;
            IdFazenda.Text = "Fazenda/Proj.: " + idfaz;
            IdTalhao.Text = "Talhão: " + idtlh;
            IdAmostragem.Text = "Amostragem: " + idamostrmn;
            if (idparcela != "") { IdParcela.Text = "Parcela: " + idparcela; }
            else
            { IdParcela.Text = "Amostra composta por talhão"; }
        }

        protected override void OnAppearing()
        {

            int num = Convert.ToInt32(pkamostragemmn);
            //PopulateListaAmostTecidos();
            var data = conn.Table<AmostTecidos>().Where(x => x.PKAmostragemMN == num).ToList();
            lstAmostTecidos.ItemsSource = data;
        }


        //private void PopulateDetails(AmostragemMN details)
        //{
        //    try { PKAmostragemMN.Text = details.PKAmostragemMN.ToString(); }
        //    catch { DisplayAlert("Message", "Não foi possível ler o valor 'PKAmostragemMN'", "Ok"); }
        //}
        private void Mostrar_Clicked(object sender, EventArgs e)
        {
            var data = (from amtecidos in conn.Table<AmostTecidos>() select amtecidos);
            lstAmostTecidos.ItemsSource = data;
        }

        private void NovaAmostraTecidos_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new pgAddAmostTecidos(null, pkamostragemmn, idfazenda, idtalhoescr, idamostragemmn, idparcela));
        }

        private void EditMedicaoArvore(object sender, ItemTappedEventArgs e)
        {
            AmostTecidos details = e.Item as AmostTecidos;
            if (details != null)
            {
                Navigation.PushAsync(new pgAddAmostTecidos(details, pkamostragemmn, idfazenda, idtalhoescr, idamostragemmn, idparcela));
            }
        }

        public async void AlternaDetalhes(object sender, ItemTappedEventArgs e)
        {
            //Alterna entre visível e invisível a lista de detalhes
            AmostTecidos details = e.Item as AmostTecidos;
            string sql = "";

            if (details.Visible.ToString() == "True")
            {
                sql = $"UPDATE AmostTecidos SET Visible = 0, Imagem = 'Down.png' ";
            }
            else
            {
                sql = $"UPDATE AmostTecidos SET Visible = 1, Imagem = 'Up.png' ";
            }

            sql += $"WHERE PKAmostTecidos = {details.PKAmostTecidos}";
            try { conn.Execute(sql); }
            catch { await DisplayAlert("Message", "Houve um erro. Não foi possível atualizar.", "Ok"); }

            sql = $"SELECT * FROM AmostTecidos WHERE PKAmostragemMN = {pkamostragemmn}";

            //Atualiza a lista
            var data = conn.Query<AmostTecidos>(sql);
            lstAmostTecidos.ItemsSource = data;
        }

        private async void DeleteAmostTecidos(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var amtecidos = mi.CommandParameter as AmostTecidos;
            if (amtecidos != null)
            {
                var opcao = await DisplayAlert("Excluir registro", "Confirma a exclusão do registro " + amtecidos.PKAmostTecidos.ToString() + "? ", "Ok", "Cancel");
                if (opcao)
                {
                    string sql = $"DELETE FROM AmostTecidos WHERE PKAmostTecidos = {amtecidos.PKAmostTecidos}";

                    try { conn.Execute(sql); }
                    catch { await DisplayAlert("Message", "Houve um erro, não foi possível excluir o registro.", "Ok"); }

                    sql = $"SELECT * FROM AmostTecidos WHERE PKAmostragemMN = {pkamostragemmn}";

                    //Atualiza a lista
                    var data = conn.Query<AmostTecidos>(sql);
                    lstAmostTecidos.ItemsSource = data;
                }
            }
        }

        private async void AlterarAmostTecidos(object sender, EventArgs e)
        {
            try
            {
                var mi = ((MenuItem)sender);
                var amtecidos = mi.CommandParameter as AmostTecidos;
                if (amtecidos != null)
                {
                    await Navigation.PushAsync(new pgAddAmostTecidos(amtecidos, pkamostragemmn, idfazenda, idtalhoescr, idamostragemmn, idparcela));
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
                var amtecidos = mi.CommandParameter as AmostTecidos;
                if (amtecidos != null)
                {
                    //                   await Navigation.PushAsync(new pgAddMedicaoArvore1(medarv, medicaoarvore.PKAmostragemMN.ToString()));
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro: ", ex.Message, "Ok");
            }

        }

        private void Informacoes_Clicked(object sender, EventArgs e)
        {
            msg = $"Esta tela exibe a lista de amostras de tecidos já realizadas para o monitoramento nutricional e permite a edição.\n\n" +
                $"Os comandos são os seguintes:\n\nNOVA AMOSTRA DE TECIDOS: incluir novo registro por meio de digitação direta no SIGNUS;\n\n" +
                $"Um toque rápido sobre um registro de amostra expande exibe os detalhes do mesmo;\n\n" +
                $"Para editar um registro já existente, pressione-o até aparecerem as opções na parte superior " +
                $"da tela.\n\n" +
                $"A seta localizada no canto superior esquerdo da tela permite retornar à tela anterior.";
            DisplayAlert("SIGNUS", msg, "Ok");
        }


    }
}