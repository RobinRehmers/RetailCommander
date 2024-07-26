﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RetailCommanderLibrary.Data;
using RetailCommanderLibrary.Database;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;

namespace RetailCommanderDesktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ServiceProvider serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();
            services.AddTransient<MainWindow>();
            services.AddTransient<AddEmployeeForm>();
            services.AddTransient<ISqliteDataAccess, SqliteDataAccess>();
            services.AddSingleton<SqliteData>();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfiguration config = builder.Build();

            services.AddSingleton(config);



            serviceProvider = services.BuildServiceProvider();
            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow.Show();

        }

    }
}
