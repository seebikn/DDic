using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Linq;
using System.Data;
using System.Text.RegularExpressions;

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

            var lines = File.ReadAllLines(filePath).Skip(1); // ヘッダーをスキップ
            foreach (var line in lines)
            {
                var columns = ParseTsvLine(line);
                if (columns.Length >= 3)
                {
                    // DataTableに行を追加
                    var row = dataTable.NewRow();
                    row[Constants.TableColumns.ProjectName] = projectName;
                    row[Constants.TableColumns.TableID] = columns[0];
                    row[Constants.TableColumns.TableName] = columns[1];
                    row[Constants.TableColumns.Description] = columns[2];
                    dataTable.Rows.Add(row);
                }
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

            var lines = File.ReadAllLines(filePath).Skip(1);
            foreach (var line in lines)
            {
                var columns = ParseTsvLine(line);
                if (columns.Length >= 3)
                {
                    var tableName = parent.AsEnumerable()
                        .Where(x => x[Constants.TableColumns.ProjectName].Equals(projectName))
                        .Where(x => x[Constants.TableColumns.TableID].Equals(columns[0]))
                        .ToList()
                        .FirstOrDefault();

                    // DataTableに行を追加
                    var row = dataTable.NewRow();
                    row[Constants.ColumnColumns.ProjectName] = projectName;
                    row[Constants.ColumnColumns.TableID] = columns[0];
                    row[Constants.ColumnColumns.TableName] = tableName == null ? "" : tableName[Constants.TableColumns.TableName];
                    row[Constants.ColumnColumns.ColumnNo] = columns[1];
                    row[Constants.ColumnColumns.LogicalName] = columns[2];
                    row[Constants.ColumnColumns.PhysicalName] = columns[3];
                    row[Constants.ColumnColumns.PrimaryKey] = columns[4] == "1" ? 1 : 0;
                    row[Constants.ColumnColumns.NotNullable] = columns[5] == "1" ? 1 : 0;
                    row[Constants.ColumnColumns.DataType] = columns[6];
                    row[Constants.ColumnColumns.DataSize] = columns[7];
                    row[Constants.ColumnColumns.Remarks] = columns[8];
                    dataTable.Rows.Add(row);
                }
            }
            return dataTable;
        }

        private static string[] ParseTsvLine(string line)
        {
            // TSV行をパースするロジック (ダブルクォート対応など)
            return line.Split('\t').Select(field => Regex.Unescape (field.Trim('"'))).ToArray();
        }
    }
}
