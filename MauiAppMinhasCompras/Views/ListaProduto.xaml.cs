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

    // Evento disparado ao trocar a categoria no Picker de filtro
    private async void pck_filtro_categoria_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string categoriaSelecionada = pck_filtro_categoria.SelectedItem?.ToString();

            lista.Clear();

            // Pega todos os produtos
            List<Produto> tmp = await App.Db.GetAll();

            // Se a categoria for diferente de "Todas", aplicamos o filtro
            if (!string.IsNullOrEmpty(categoriaSelecionada) && categoriaSelecionada != "Todas")
            {
                tmp = tmp.Where(p => p.Categoria == categoriaSelecionada).ToList();
            }

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    // Evento disparado ao clicar no botão Relatório
    private async void ToolbarItem_Relatorio_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Busca todos os produtos do banco
            List<Produto> todosProdutos = await App.Db.GetAll();

            // Agrupa os produtos por Categoria e soma o total de cada grupo
            var relatorio = todosProdutos
                .GroupBy(p => p.Categoria)
                .Select(g => new {
                    Categoria = string.IsNullOrEmpty(g.Key) ? "Sem Categoria" : g.Key,
                    TotalGasto = g.Sum(p => p.Total)
                })
                .ToList();

            // Monta a mensagem de texto do relatório
            string mensagem = "";
            foreach (var item in relatorio)
            {
                mensagem += $"{item.Categoria}: {item.TotalGasto:C2}\n";
            }

            if (string.IsNullOrEmpty(mensagem))
                mensagem = "Nenhum produto cadastrado para gerar relatório.";

            // Exibe o relatório num alerta na tela
            await DisplayAlert("Gasto por Categoria", mensagem, "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}