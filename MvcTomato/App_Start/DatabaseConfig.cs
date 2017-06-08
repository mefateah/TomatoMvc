using System.Data.Entity;
using MvcTomato.DAL;

namespace MvcTomato
{
    public static class DatabaseConfig
    {
        public static void Register()
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<TomatoContext>());
        }
    }
}