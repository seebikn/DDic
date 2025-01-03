using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDic.Models;

namespace DDic.Controllers
{
    internal class MainController
    {
        public DataTable Tables { get; private set; } = new DataTable();
        public DataTable Columns { get; private set; } = new DataTable();

        public void LoadData(string directoryPath)
        {
            var tableFiles = Directory.GetFiles(directoryPath, "table*.tsv");
            var columnFiles = Directory.GetFiles(directoryPath, "column*.tsv");

            foreach (var file in tableFiles)
            {
                string projectName = GetProjectName(file, "table-");
                Tables.Merge(DataLoader.LoadTables(file, projectName));
            }

            foreach (var file in columnFiles)
            {
                string projectName = GetProjectName(file, "column-");
                Columns.Merge(DataLoader.LoadColumns(file, projectName, Tables));
            }
        }

        private string GetProjectName(string filePath, string baseName)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            return fileName == baseName ? " " : fileName.Replace($"{baseName}", "");
        }
    }
}
