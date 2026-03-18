using SQLite;

namespace MauiAppMinhasCompras.Models
{
    public class Produto
    {
        // Campos privados para armazenar os valores
        string _descricao;
        double _quantidade;
        double _preco;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Descricao
        {
            get => _descricao;
            set
            {
                // validação para impedir que o usuário tente salvar apenas inserindo um "espaço"
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new Exception("Por favor, preencha a descrição.");
                }
                _descricao = value;
            }
        }

        public double Quantidade
        {
            get => _quantidade;
            set
            {
                if (value <= 0)
                {
                    throw new Exception("A quantidade deve ser maior que zero.");
                }
                _quantidade = value;
            }
        }

        public double Preco
        {
            get => _preco;
            set
            {
                if (value <= 0)
                {
                    throw new Exception("O preço deve ser maior que zero.");
                }
                _preco = value;
            }
        }

        // Propriedade calculada (apenas leitura)
        public double Total { get => Quantidade * Preco; }
    }
}