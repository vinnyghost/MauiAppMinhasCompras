using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

// Página que exibe a lista de produtos
public partial class ListaProduto : ContentPage
{
    // Coleção observável que atualiza automaticamente a interface quando alterada
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

    // Construtor da página
    public ListaProduto()
    {
        InitializeComponent();

        // Define a fonte de dados da ListView
        lst_produtos.ItemsSource = lista;
    }

    // Método executado sempre que a tela aparece
    protected async override void OnAppearing()
    {
        try
        {
            // Limpa a lista antes de recarregar os dados do banco
            lista.Clear();

            // Busca todos os produtos no banco de dados
            List<Produto> tmp = await App.Db.GetAll();

            // Adiciona cada produto na lista observável
            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            // Exibe mensagem de erro
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    // Evento do botão "Adicionar" (Toolbar)
    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Navega para a tela de cadastro de novo produto
            Navigation.PushAsync(new Views.NovoProduto());

        }
        catch (Exception ex)
        {
            // Exibe mensagem de erro
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    // Evento disparado dinamicante ao digitar no campo de busca
    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            // Texto digitado pelo usuário
            string q = e.NewTextValue;

            // Limpa a lista atual
            lista.Clear();

            // Busca no banco os produtos que correspondem ao texto
            List<Produto> tmp = await App.Db.Search(q);

            // Adiciona os resultados na lista
            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            // Exibe mensagem de erro
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    // Evento do botão "Somar" (Toolbar)
    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        // Soma o total de todos os produtos da lista
        double soma = lista.Sum(i => i.Total);

        // Formata a mensagem como moeda
        string msg = $"O total é {soma:C}";

        // Exibe o resultado
        DisplayAlert("Total dos Produtos", msg, "OK");
    }

    // Evento do menu de contexto (botão "Remover")
    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Obtém o MenuItem que foi clicado
            MenuItem selecionado = (MenuItem)sender;

            // Recupera o produto associado a esse item
            Produto p = selecionado.BindingContext as Produto;

            // Solicita confirmação do usuário antes de excluir
            bool confirmacao = await DisplayAlert(
                "Confirmação",
                $"Tem certeza que deseja remover o item '{p.Descricao}'?",
                "Sim", "Não");

            if (confirmacao)
            {
                // Remove o produto do banco de dados
                await App.Db.Delete(p.Id);

                // Remove o produto da lista (atualiza a interface automaticamente)
                lista.Remove(p);
            }
        }
        catch (Exception ex)
        {
            // Exibe mensagem de erro
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    // Evento ao selecionar um item da lista
    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            // Recupera o produto selecionado
            Produto p = e.SelectedItem as Produto;

            // Navega para a tela de edição, passando o produto como contexto
            Navigation.PushAsync(new Views.EditarProduto
            {
                BindingContext = p,
            });

        }
        catch (Exception ex)
        {
            // Exibe mensagem de erro
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}