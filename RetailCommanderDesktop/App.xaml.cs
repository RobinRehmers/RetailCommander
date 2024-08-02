using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RetailCommanderLibrary.Data;
using RetailCommanderLibrary.Database;
using RetailCommanderDesktop.Forms;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;

namespace RetailCommanderDesktop
{
    public partial class App : Application
    {
        public static ServiceProvider serviceProvider;

        /// <summary>
        /// Initializes a new instance of the App class. Adds services to the service collection and builds the service provider.
        /// </summary>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();
            services.AddTransient<MainWindow>();
            services.AddTransient<AddEmployeeForm>();
            services.AddTransient<DeleteEmployeeForm>();
            services.AddTransient<ConfigurationForm>();
            services.AddTransient<ISqliteDataAccess, SqliteDataAccess>();

            services.AddSingleton<SqliteData>();
            services.AddSingleton<ITranslationManager, TranslationManager>();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration config = builder.Build();

            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string dbPath = Path.Combine(projectDirectory, "RetailCommanderDB.db");
            string ConnectionStringName = config.GetConnectionString("SqliteDb").Replace("{DB_PATH}", dbPath);

            var configurationSection = config as IConfigurationRoot;
            configurationSection["ConnectionStrings:SqliteDb"] = ConnectionStringName;

            Resources["AppConfig"] = config;
            services.AddSingleton(config);

            serviceProvider = services.BuildServiceProvider();
            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow.Show();

        }

    }
}
