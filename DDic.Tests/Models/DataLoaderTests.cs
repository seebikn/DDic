using System.Data;
using Xunit;
using DDic.Models;
using System.Diagnostics;

namespace DDic.Tests.Models
{
    public class DataLoaderTests : IDisposable
    {
        private readonly string testTablesFilePath;
        private readonly string testColumnsFilePath;
        public DataLoaderTests()
        {
            // テスト用の一時TSVファイルパスを設定
            testTablesFilePath = Path.Combine(Path.GetTempPath(), "test_tables.tsv");
            testColumnsFilePath = Path.Combine(Path.GetTempPath(), "test_columns.tsv");
        }

        #region " table "
        /// <summary>
        /// TSV ファイルからデータを読み込み、DataTable に正しくデータが格納されることを検証します。
        /// </summary>
        [Fact]
        public void LoadTables_ShouldLoadDataIntoDataTable()
        {
            // Arrange
            var tsvContent = "Header1\tHeader2\tHeader3\r\n" + 
                             "TableID1\tTableName1\tDescription1\r\n" +
                             "TableID2\tTableName2\tDescription2\r\n";
            File.WriteAllText(testTablesFilePath, tsvContent);
            string projectName = "projectName";

            // Act
            DataTable result = DataLoader.LoadTables(testTablesFilePath, projectName);

            // Assert
            Assert.Equal(4, result.Columns.Count);
            Assert.Equal(2, result.Rows.Count);

            Assert.Equal(projectName, result.Rows[0][Constants.TableColumns.ProjectName]);
            Assert.Equal("TableID1", result.Rows[0][Constants.TableColumns.TableID]);
            Assert.Equal("TableName1", result.Rows[0][Constants.TableColumns.TableName]);
            Assert.Equal("Description1", result.Rows[0][Constants.TableColumns.Description]);

            Assert.Equal(projectName, result.Rows[1][Constants.TableColumns.ProjectName]);
            Assert.Equal("TableID2", result.Rows[1][Constants.TableColumns.TableID]);
            Assert.Equal("TableName2", result.Rows[1][Constants.TableColumns.TableName]);
            Assert.Equal("Description2", result.Rows[1][Constants.TableColumns.Description]);
        }

        /// <summary>
        /// 空のファイルを読み込んだ場合、カラムは定義されるが、行が追加されないことを確認します。
        /// </summary>
        [Fact]
        public void LoadTables_ShouldHandleEmptyFile()
        {
            // Arrange
            File.WriteAllText(testTablesFilePath, string.Empty);
            string projectName = "projectName";

            // Act
            DataTable result = DataLoader.LoadTables(testTablesFilePath, projectName);

            // Assert
            Assert.Equal(4, result.Columns.Count);
            Assert.Empty(result.Rows);
        }

        /// <summary>
        /// ヘッダーのみのファイルを読み込んだ場合、カラムは定義されるが、行が追加されないことを確認します。
        /// </summary>
        [Fact]
        public void LoadTables_ShouldHandleHeaderOnly()
        {
            // Arrange
            var tsvContent = "Header1\tHeader2\tHeader3\r\n";
            File.WriteAllText(testTablesFilePath, tsvContent);
            string projectName = "projectName";

            // Act
            DataTable result = DataLoader.LoadTables(testTablesFilePath, projectName);

            // Assert
            Assert.Equal(4, result.Columns.Count);
            Assert.Empty(result.Rows);
        }

        /// <summary>
        /// TSV ファイルの列数が期待値（3列）と一致しない場合、InvalidDataException がスローされることを確認します。
        /// </summary>
        [Fact]
        public void LoadTables_ShouldThrowExceptionForInvalidColumnCount()
        {
            // Arrange
            var tsvContent = "Header1\tHeader2\tHeader3\r\n" + 
                                "TableID1\tTableName1\r\n"; // 列数が不足している
            File.WriteAllText(testTablesFilePath, tsvContent);
            string projectName = "projectName";

            // Act & Assert
            Assert.Throws<InvalidDataException>(() => DataLoader.LoadTables(testTablesFilePath, projectName));
        }
        #endregion

        #region " columns "
        /// <summary>
        /// TSV ファイルからデータを読み込み、DataTable に正しくデータが格納されることを検証します。
        /// </summary>
        [Fact]
        public void LoadColumns_ShouldLoadDataIntoDataTable()
        {
            string projectName = "TestProject";

            // Arrange
            var tablesContent = "Header1\tHeader2\tHeader3\r\n" +
                             "TableID1\tTableName1\tDescription1\r\n" +
                             "TableID2\tTableName2\tDescription2\r\n";
            File.WriteAllText(testTablesFilePath, tablesContent);

            // Act
            DataTable tablesResult = DataLoader.LoadTables(testTablesFilePath, projectName);

            // Arrange
            var tsvContent = "Header1\t1\tHeader2\tHeader3\tHeader4\tHeader5\tHeader6\tHeader7\tHeader8\tHeader\r\n" +
                             "TableID1\t1\tLogicalName1\tPhysicalName1\t1\t1\tDataType1\tDataSize1\tRemarks1\r\n" +
                             "TableID2\t2\tLogicalName2\tPhysicalName2\t0\t0\tDataType2\tDataSize2\tRemarks2\r\n";
            File.WriteAllText(testColumnsFilePath, tsvContent);

            // Act
            DataTable result = DataLoader.LoadColumns(testColumnsFilePath, projectName, tablesResult);

            // Assert
            Assert.Equal(11, result.Columns.Count);
            Assert.Equal(2, result.Rows.Count);

            Assert.Equal(projectName, result.Rows[0][Constants.ColumnColumns.ProjectName]);
            Assert.Equal("TableID1", result.Rows[0][Constants.ColumnColumns.TableID]);
            Assert.Equal("TableName1", result.Rows[0][Constants.ColumnColumns.TableName]);
            Assert.Equal(1, result.Rows[0][Constants.ColumnColumns.ColumnNo]);
            Assert.Equal("LogicalName1", result.Rows[0][Constants.ColumnColumns.LogicalName]);
            Assert.Equal("PhysicalName1", result.Rows[0][Constants.ColumnColumns.PhysicalName]);
            Assert.Equal("1" == "1" ? 1 : 0, result.Rows[0][Constants.ColumnColumns.PrimaryKey]);
            Assert.Equal("1" == "1" ? 1 : 0, result.Rows[0][Constants.ColumnColumns.NotNullable]);
            Assert.Equal("DataType1", result.Rows[0][Constants.ColumnColumns.DataType]);
            Assert.Equal("DataSize1", result.Rows[0][Constants.ColumnColumns.DataSize]);
            Assert.Equal("Remarks1", result.Rows[0][Constants.ColumnColumns.Remarks]);

            Assert.Equal(projectName, result.Rows[1][Constants.ColumnColumns.ProjectName]);
            Assert.Equal("TableID2", result.Rows[1][Constants.ColumnColumns.TableID]);
            Assert.Equal("TableName2", result.Rows[1][Constants.ColumnColumns.TableName]);
            Assert.Equal(2, result.Rows[1][Constants.ColumnColumns.ColumnNo]);
            Assert.Equal("LogicalName2", result.Rows[1][Constants.ColumnColumns.LogicalName]);
            Assert.Equal("PhysicalName2", result.Rows[1][Constants.ColumnColumns.PhysicalName]);
            Assert.Equal("0" == "1" ? 1 : 0, result.Rows[1][Constants.ColumnColumns.PrimaryKey]);
            Assert.Equal("0" == "1" ? 1 : 0, result.Rows[1][Constants.ColumnColumns.NotNullable]);
            Assert.Equal("DataType2", result.Rows[1][Constants.ColumnColumns.DataType]);
            Assert.Equal("DataSize2", result.Rows[1][Constants.ColumnColumns.DataSize]);
            Assert.Equal("Remarks2", result.Rows[1][Constants.ColumnColumns.Remarks]);
        }

        /// <summary>
        /// 空のファイルを読み込んだ場合、カラムは定義されるが、行が追加されないことを確認します。
        /// </summary>
        [Fact]
        public void LoadColumns_ShouldHandleEmptyFile()
        {
            string projectName = "TestProject";

            // Arrange
            var tablesContent = "Header1\tHeader2\tHeader3\r\n" +
                             "TableID1\tTableName1\tDescription1\r\n" +
                             "TableID2\tTableName2\tDescription2\r\n";
            File.WriteAllText(testTablesFilePath, tablesContent);

            // Act
            DataTable tablesResult = DataLoader.LoadTables(testTablesFilePath, projectName);

            // Arrange
            File.WriteAllText(testColumnsFilePath, string.Empty);

            // Act
            DataTable result = DataLoader.LoadColumns(testColumnsFilePath, projectName, tablesResult);

            // Assert
            Assert.Equal(11, result.Columns.Count);
            Assert.Empty(result.Rows);
        }

        /// <summary>
        /// ヘッダーのみのファイルを読み込んだ場合、カラムは定義されるが、行が追加されないことを確認します。
        /// </summary>
        [Fact]
        public void LoadColumns_ShouldHandleHeaderOnly()
        {
            string projectName = "TestProject";

            // Arrange
            var tablesContent = "Header1\tHeader2\tHeader3\r\n" +
                             "TableID1\tTableName1\tDescription1\r\n" +
                             "TableID2\tTableName2\tDescription2\r\n";
            File.WriteAllText(testTablesFilePath, tablesContent);

            // Act
            DataTable tablesResult = DataLoader.LoadTables(testTablesFilePath, projectName);

            // Arrange
            var tsvContent = "Header1\t1\tHeader2\tHeader3\tHeader4\tHeader5\tHeader6\tHeader7\tHeader8\tHeader\r\n";
            File.WriteAllText(testColumnsFilePath, tsvContent);

            // Act
            DataTable result = DataLoader.LoadColumns(testColumnsFilePath, projectName, tablesResult);

            // Assert
            Assert.Equal(11, result.Columns.Count);
            Assert.Empty(result.Rows);
        }

        /// <summary>
        /// TSV ファイルの列数が期待値（11列）と一致しない場合、InvalidDataException がスローされることを確認します。
        /// </summary>
        [Fact]
        public void LoadColumns_ShouldThrowExceptionForInvalidColumnCount()
        {
            string projectName = "TestProject";

            // Arrange
            var tablesContent = "Header1\tHeader2\tHeader3\r\n" +
                             "TableID1\tTableName1\tDescription1\r\n" +
                             "TableID2\tTableName2\tDescription2\r\n";
            File.WriteAllText(testTablesFilePath, tablesContent);

            // Act
            DataTable tablesResult = DataLoader.LoadTables(testTablesFilePath, projectName);

            // Arrange
            var tsvContent = "Header1\t1\tHeader2\tHeader3\tHeader4\tHeader5\tHeader6\tHeader7\tHeader8\tHeader9\r\n" + 
                             "TableID1\t1\tLogicalName1\r\n"; // 列数が不足している
            File.WriteAllText(testColumnsFilePath, tsvContent);

            // Act & Assert
            Assert.Throws<InvalidDataException>(() => DataLoader.LoadColumns(testColumnsFilePath, projectName, tablesResult));
        }
        #endregion

        public void Dispose()
        {
            // テスト終了後に一時TSVファイルを削除
            if (File.Exists(testTablesFilePath))
            {
                File.Delete(testTablesFilePath);
            }
            if (File.Exists(testColumnsFilePath))
            {
                File.Delete(testColumnsFilePath);
            }
        }
    }
}
