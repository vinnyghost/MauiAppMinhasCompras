using MauiAppMinhasCompras.Models;
using SQLite;

namespace MauiAppMinhasCompras.Helpers
{
    public class SQLiteDatabaseHelper
    {
        // campo para armazenar a conexão async
        readonly SQLiteAsyncConnection _conn;

        //implementa o metodo construtor
        
        public SQLiteDatabaseHelper(string path) //path é o caminho até o arquivo de texto
        {
            // _conn recebe um novo objeto que será uma conexao com o arquivo de texto
            _conn = new SQLiteAsyncConnection(path);

            //cria a tabela no banco de dados, caso ainda nao exista
            _conn.CreateTableAsync<Produto>().Wait();
        }
         //declaracao dos metodos CRUD
        public Task<int> Insert(Produto p)
        {
            return _conn.InsertAsync(p);
        }
        public Task<List<Produto>> Update(Produto p)
        {
            // Adicionei a Categoria no comando SQL
            string sql = "UPDATE Produto SET Descricao=?, Quantidade=?, Preco=?, Categoria=? WHERE Id=?";

            return _conn.QueryAsync<Produto>(
                sql, p.Descricao, p.Quantidade, p.Preco, p.Categoria, p.Id
            );
        }
        public Task<int> Delete(int id)
        {    
            return _conn.Table<Produto>().DeleteAsync(i => i.Id == id); 
        }
        public Task<List<Produto>> GetAll()
        {
            //retorna uma lista de todos os produtos 
            return _conn.Table<Produto>().ToListAsync();
        }
        public Task<List<Produto>> Search(string q)
        {
            //utiliza um SELECT em todos os campos do produto com LIKE para fazer uma busca por alguma parte do nome, antes ou depois
            string sql = "SELECT * FROM Produto WHERE descricao LIKE '%" + q + "%'";

            //retorna uma busca instantanea
            return _conn.QueryAsync<Produto>(sql);
        }

    }
} 
