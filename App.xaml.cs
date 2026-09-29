using System.Windows;
using GymManager.Data;

namespace GymManager
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Crea la base de datos y sus tablas si aún no existen
            using (var db = new GymDbContext())
            {
                db.Database.EnsureCreated();
            }
        }
    }
}