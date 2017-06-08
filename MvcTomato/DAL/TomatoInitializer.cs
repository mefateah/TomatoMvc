using System.Data.Entity;

namespace MvcTomato.DAL
{
    public class TomatoInitializer : DropCreateDatabaseIfModelChanges<TomatoContext>
    {
        
    }
}