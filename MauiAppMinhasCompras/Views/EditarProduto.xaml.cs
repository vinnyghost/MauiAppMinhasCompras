using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class EditarProduto : ContentPage
{
    public EditarProduto()
    {
        InitializeComponent();
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Declara a variável produto_anexado e converte o BindingContext da página para o tipo Produto
            Produto produto_anexado = BindingContext as Produto;

            //declara uma variavel p do tipo Produto que recebe um novo objeto do tipo produto
            Produto p = new Produto
            {
                //propriedades para preenchimento
                Id = produto_anexado.Id,
                Descricao = txt_descricao.Text,
                Quantidade = Convert.ToDouble(txt_quantidade.Text),
                Preco = Convert.ToDouble(txt_preco.Text)
            };

            //Faz o Update no SQlite
            await App.Db.Update(p);

            //mensagem de sucesso
            await DisplayAlert("Sucesso!", "Registro Atualizado", "OK");

            // Retorna para a tela de origem
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}