using System;
using System.IO;
using buscainstaladores.Droid;
using SQLite;

[assembly: Xamarin.Forms.Dependency(typeof(Database_Android))]
namespace buscainstaladores.Droid
{
    public class Database_Android : IDatabase
    {
        public SQLiteConnection GetConnection()
        {
            var nomeDB = "usuario_logado.db3";

            var caminhoDB = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) ,nomeDB);


            return new SQLiteConnection(caminhoDB);
        }
    }
}
