using SQLite;

namespace MauiAppMinhasCompras.Models
{
    // Classe que representa um produto no sistema e no banco de dados
    public class Produto
    {
        // Campos privados para armazenar os valores internamente
        string _descricao;
        double _quantidade;
        double _preco;
        string _categoria;

        // Define a chave primária no banco e auto incremento
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        // Propriedade pública para descrição do produto
        public string Descricao
        {
            // Retorna o valor armazenado
            get => _descricao;

            // Define o valor com validação
            set
            {
                // Impede valores nulos, vazios ou apenas com espaços
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new Exception("Por favor, preencha a descrição.");
                }

                // Armazena o valor válido
                _descricao = value;
            }
        }

        // Propriedade para quantidade do produto
        public double Quantidade
        {
            get => _quantidade;

            set
            {
                // Valida se a quantidade é maior que zero
                if (value <= 0)
                {
                    throw new Exception("A quantidade deve ser maior que zero.");
                }

                _quantidade = value;
            }
        }

        // Propriedade para preço do produto
        public double Preco
        {
            get => _preco;

            set
            {
                // Valida se o preço é maior que zero
                if (value <= 0)
                {
                    throw new Exception("O preço deve ser maior que zero.");
                }

                _preco = value;
            }
        }

        // Propriedade calculada (somente leitura)
        // Não é armazenada no banco, é calculada em tempo de execução
        public double Total
        {
            get => Quantidade * Preco;
        }

        public string Categoria
        {
            get => _categoria;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new Exception("Por favor, selecione uma categoria.");
                }
                _categoria = value;
            }
        }
    }
}