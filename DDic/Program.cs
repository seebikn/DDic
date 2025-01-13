// MIT License
// Copyright (c) 2025 seebikn   
// See LICENSE file in the project root for full license information.

using DDic.Controllers;

namespace DDic
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            string iniFilePath = Path.Combine(exePath, "Resources", "config.ini");

            var controller = new MainController(iniFilePath);
            controller.Run();
        }

    }
}