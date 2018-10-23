using System;
using System.Collections.Generic;
using System.Linq;
using buscainstaladores.Models;
using SQLite;
using Xamarin.Forms;

namespace buscainstaladores.Database
{
    public class UsuarioDataAccess
    {
        private SQLiteConnection _database;

        public UsuarioDataAccess()
        {
            _database = DependencyService.Get<IDatabase>().GetConnection();
            _database.CreateTable<Usuario>();
        }

        public List<Usuario> GetUsuarioList()
        {
            return _database.Table<Usuario>().ToList();
        }

        public int InserirUsuario(Usuario usuario)
        {
            return _database.Insert(usuario);
        }

        public int ExcluirUsuario(Usuario usuario)
        {
            return _database.Delete(usuario);
        }

        public int AtualizarUsuario(Usuario usuario)
        {
            return _database.Update(usuario);
        }
    }
}
