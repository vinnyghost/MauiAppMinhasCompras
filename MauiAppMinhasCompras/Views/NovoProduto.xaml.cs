using MauiAppMinhasCompras.Models;
using System.Threading.Tasks;

namespace MauiAppMinhasCompras.Views;

public partial class NovoProduto : ContentPage
{
	public NovoProduto()
	{
		InitializeComponent();
	}

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            //declara uma variavel p do tipo Produto que recebe um novo objeto do tipo produto
            Produto p = new Produto
            {
                //propriedades para preenchimento
                Descricao = txt_descricao.Text,
                Quantidade = Convert.ToDouble(txt_quantidade.Text),
                Preco = Convert.ToDouble(txt_preco.Text)
            };

            //diretiva await
            //Faz o Insert no SQlite
            await App.Db.Insert(p);
            //mensagem de sucesso
            await DisplayAlert("Sucesso!", "Registro Inserido", "OK");

        } catch (Exception ex)
        {
           await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}