using System.Data;

namespace DDic.Models
{
    internal class DataLoader
    {
        public static DataTable LoadTables(string filePath, string projectName)
        {
            // 新しいDataTableを作成
            var dataTable = new DataTable();

            // DataTableにカラムを定義
            dataTable.Columns.Add(Constants.TableColumns.ProjectName, typeof(string));
            dataTable.Columns.Add(Constants.TableColumns.TableID, typeof(string));
            dataTable.Columns.Add(Constants.TableColumns.TableName, typeof(string));
            dataTable.Columns.Add(Constants.TableColumns.Description, typeof(string));

            var lines = TsvReader.ReadFile(filePath, 3);
            foreach (var line in lines)
            {
                var data = line.Select(x => x.Replace("¥t", "    ")).ToArray();

                // DataTableに行を追加
                var row = dataTable.NewRow();
                row[Constants.TableColumns.ProjectName] = projectName;
                row[Constants.TableColumns.TableID] = data[0];
                row[Constants.TableColumns.TableName] = data[1];
                row[Constants.TableColumns.Description] = data[2];
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        public static DataTable LoadColumns(string filePath, string projectName, DataTable parent)
        {
            // 新しいDataTableを作成
            var dataTable = new DataTable();

            // DataTableにカラムを定義
            dataTable.Columns.Add(Constants.ColumnColumns.ProjectName, typeof(string));
            dataTable.Columns.Add(Constants.ColumnColumns.TableID, typeof(string));
            dataTable.Columns.Add(Constants.ColumnColumns.TableName, typeof(string));
            dataTable.Columns.Add(Constants.ColumnColumns.ColumnNo, typeof(int));
            dataTable.Columns.Add(Constants.ColumnColumns.LogicalName, typeof(string));
            dataTable.Columns.Add(Constants.ColumnColumns.PhysicalName, typeof(string));
            dataTable.Columns.Add(Constants.ColumnColumns.PrimaryKey, typeof(int));
            dataTable.Columns.Add(Constants.ColumnColumns.NotNullable, typeof(int));
            dataTable.Columns.Add(Constants.ColumnColumns.DataType, typeof(string));
            dataTable.Columns.Add(Constants.ColumnColumns.DataSize, typeof(string));
            dataTable.Columns.Add(Constants.ColumnColumns.Remarks, typeof(string));

            var lines = TsvReader.ReadFile(filePath, 9);
            foreach (var line in lines)
            {
                var data = line.Select(x => x.Replace("\t", "    ")).ToArray();

                var tableName = parent.AsEnumerable()
                    .Where(x => x[Constants.TableColumns.ProjectName].Equals(projectName))
                    .Where(x => x[Constants.TableColumns.TableID].Equals(data[0]))
                    .ToList()
                    .FirstOrDefault();

                // DataTableに行を追加
                var row = dataTable.NewRow();
                row[Constants.ColumnColumns.ProjectName] = projectName;
                row[Constants.ColumnColumns.TableID] = data[0];
                row[Constants.ColumnColumns.TableName] = tableName == null ? "" : tableName[Constants.TableColumns.TableName];
                row[Constants.ColumnColumns.ColumnNo] = data[1];
                row[Constants.ColumnColumns.LogicalName] = data[2];
                row[Constants.ColumnColumns.PhysicalName] = data[3];
                row[Constants.ColumnColumns.PrimaryKey] = data[4] == "1" ? 1 : 0;
                row[Constants.ColumnColumns.NotNullable] = data[5] == "1" ? 1 : 0;
                row[Constants.ColumnColumns.DataType] = data[6];
                row[Constants.ColumnColumns.DataSize] = data[7];
                row[Constants.ColumnColumns.Remarks] = data[8];
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
    }
}
