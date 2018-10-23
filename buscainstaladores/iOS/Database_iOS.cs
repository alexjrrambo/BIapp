using System;
using System.IO;
using buscainstaladores.iOS;
using SQLite;

[assembly: Xamarin.Forms.Dependency(typeof(Database_iOS))]
namespace buscainstaladores.iOS
{
    public class Database_iOS : IDatabase
    {

        public SQLiteConnection GetConnection()
        {
            var nomeDB = "usuario_logado.db3";
            var personalFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

            //o dois pontos serve para voltar 1 diretorio
            string LibraryFolder = Path.Combine(personalFolder, "..", "Library");

            string caminhoDB = Path.Combine(LibraryFolder, nomeDB);

            return new SQLiteConnection(caminhoDB);
        }
    }
}
