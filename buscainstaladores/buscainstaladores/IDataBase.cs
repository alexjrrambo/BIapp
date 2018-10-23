using System;
using SQLite;

namespace buscainstaladores
{
    public interface IDatabase
    {
        SQLiteConnection GetConnection();
    }
}
