using MauiAppMinhasCompras.Helpers;

namespace MauiAppMinhasCompras
{
    public partial class App : Application
    {
        //definindo a propriedade que armazena a conexao
        static SQLiteDatabaseHelper _db; //campo
        
        public static SQLiteDatabaseHelper Db //propriedade
        {
            get 
            { 
                if(_db == null) //verifica se já existe um objeto dentro do campo _db
                {   //define o caminho até o arquivo do SQlite
                    string path = Path.Combine(
                        Environment.GetFolderPath(
                            Environment.SpecialFolder.LocalApplicationData), 
                        "banco_sqlite_compras.db3");

                    _db = new SQLiteDatabaseHelper(path);
                }
                return _db; //retorna o arquivo no campo _db
            }
        }
        public App()
        {
            InitializeComponent();

            //MainPage = new AppShell();
            MainPage = new NavigationPage(new Views.ListaProduto());
        }
    }
}
