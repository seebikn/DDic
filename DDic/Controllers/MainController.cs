using System.Data;
using System.Reflection;
using System.Text;
using DDic.Models;

namespace DDic.Controllers
{
    internal class MainController
    {
        private MainForm view;
        private IniController iniController;
        private BindingSource tableBindingSource = new BindingSource();
        private BindingSource columnBindingSource = new BindingSource();

        public MainController(string iniFilePath)
        {
            view = new MainForm();
            view.OnHandleTableSelected += HandleTableSelected;
            view.OnHandleApplyFilters += HandleApplyFilters;
            view.OnHandleHandleSelectionDataToClipboard += HandleSelectionDataToClipboard;
            view.OnHandleSelectStatementToClipboard += HandleSelectStatementToClipboard;
            view.OnHandleSelectStatementToClipboardA5 += HandleSelectStatementToClipboardA5;
            view.FormClosing += HandleSaveWindowSettings;
            view.FormClosing += HandleSaveGridSettings;
            view.Load += HandleRestoreWindowSettings;
            view.Load += HandleRestoreGridSettings;

            iniController = new IniController(iniFilePath);
        }

        public void Run()
        {
            {
                // アセンブリ名とバージョンを取得
                Assembly assembly = Assembly.GetExecutingAssembly();
                AssemblyName assemblyName = assembly.GetName();
                string appName = assemblyName.Name!;
                Version version = assemblyName.Version!;
                string majorVersion = version.Major.ToString();
                string minorVersion = version.Minor.ToString();

                // フォームのタイトルを設定
                view.Text = $"{appName} - Version {majorVersion}.{minorVersion}";
            }
            {
                // iniファイルを読み込み
                this.LoadSettings();
            }
            {
                // tsvファイルからテーブル一覧、カラム一覧を読み込み、viewに設定
                this.LoadTables();
            }

            Application.Run(view);
        }

        private void LoadTables()
        {
            // Resourcesからtsvデータの読み込み
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string resourcesDirectory = Path.Combine(appDirectory, Constants.resources);
            DataTable tables = new DataTable();
            DataTable columns = new DataTable();
            LoadData(resourcesDirectory, tables, columns);

            // データバインド
            tableBindingSource.DataSource = tables;
            columnBindingSource.DataSource = columns;
            view.SetDataSource(tableBindingSource, columnBindingSource);

            void LoadData(string directoryPath, DataTable tables, DataTable columns)
            {
                // talbeファイル
                var tableFiles = Directory.GetFiles(directoryPath, "table*.tsv");
                foreach (var file in tableFiles)
                {
                    string projectName = GetProjectName(file, "table-");
                    tables.Merge(DataLoader.LoadTables(file, projectName));
                }

                // columnファイル
                var columnFiles = Directory.GetFiles(directoryPath, "column*.tsv");
                foreach (var file in columnFiles)
                {
                    string projectName = GetProjectName(file, "column-");
                    columns.Merge(DataLoader.LoadColumns(file, projectName, tables));
                }

                // ファイルパスからプロジェクト名を取得
                string GetProjectName(string filePath, string baseName)
                {
                    string fileName = Path.GetFileNameWithoutExtension(filePath);
                    return fileName == baseName ? " " : fileName.Replace($"{baseName}", "");
                }
            }
        }

        private void LoadSettings()
        {
            iniController.InitializeFile();

            // テーブル一覧 右クリックメニューの表示設定
            view.SetMenuTablesVisible(Constants.MenuTables.Copy, 
                iniController.Get(Constants.IniTableGrid.section, Constants.IniTableGrid.copyVisible, true));

            // カラム一覧 右クリックメニューの表示設定
            view.SetMenuColumnsVisible(Constants.MenuColumns.Copy, 
                iniController.Get(Constants.IniColumnGrid.section, Constants.IniColumnGrid.copyVisible, true));            
            view.SetMenuColumnsVisible(Constants.MenuColumns.SqlSelect, 
                iniController.Get(Constants.IniColumnGrid.section, Constants.IniColumnGrid.createSqlVisible, true));
            view.SetMenuColumnsVisible(Constants.MenuColumns.SqlSelectA5, 
                iniController.Get(Constants.IniColumnGrid.section, Constants.IniColumnGrid.createSqlA5m2Visible, true));
        }

        #region " テーブル一覧で選択 "
        /// <summary>
        /// テーブル一覧で選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleTableSelected(object? sender, EventArgs e)
        {
            var grid = sender as DataGridView;
            if (grid == null) return;
            if (grid.CurrentRow == null) return;

            // 選択したテーブルの概要を概要欄に表示
            var selectedDescription = grid.CurrentRow.Cells[Constants.TableColumns.Description].Value.ToString();
            view.SetTextTableDescription(selectedDescription!);

            // 選択したテーブルのカラムに絞り込み
            var selectedProjectName = grid.CurrentRow.Cells[Constants.TableColumns.ProjectName].Value.ToString();
            var selectedTableID = grid.CurrentRow.Cells[Constants.TableColumns.TableID].Value.ToString();
            columnBindingSource.Filter = $"{Constants.ColumnColumns.ProjectName} = '{selectedProjectName}' and {Constants.ColumnColumns.TableID} = '{selectedTableID}'";
        }
        #endregion

        #region " 検索 "
        private void HandleApplyFilters(object? sender, EventArgs e)
        {
            var projectName = view.GetTextProjectNameValue();
            var tableName = view.GetTextTableNameValue().Trim();
            var columnName = view.GetTextColumnNameValue().Trim();
            var columnDetail = view.GetTextColumnDetailValue();

            List<string> searchTable = new List<string>();
            List<string> searchColumn = new List<string>();

            if (!string.IsNullOrEmpty(projectName))
            {
                if (projectName == " ")
                {
                    searchTable.Add($"{Constants.TableColumns.ProjectName} like ' '");
                    searchColumn.Add($"{Constants.ColumnColumns.ProjectName} like ' '");
                }
                else
                {
                    searchTable.Add($"{Constants.TableColumns.ProjectName} like '%{projectName}%'");
                    searchColumn.Add($"{Constants.ColumnColumns.ProjectName} like '%{projectName}%'");
                }
            }

            if (!string.IsNullOrEmpty(tableName))
            {
                searchTable.Add($"({Constants.TableColumns.TableID} like '%{tableName}%' or {Constants.TableColumns.TableName} like '%{tableName}%')");
                searchColumn.Add($"({Constants.ColumnColumns.TableID} like '%{tableName}%' or {Constants.ColumnColumns.TableName} like '%{tableName}%')");
            }

            if (!string.IsNullOrEmpty(columnName))
            {
                searchColumn.Add($"({Constants.ColumnColumns.LogicalName} like '%{columnName}%' or {Constants.ColumnColumns.PhysicalName} like '%{columnName}%')");
            }

            if (!string.IsNullOrEmpty(columnDetail))
            {
                searchColumn.Add($"{Constants.ColumnColumns.Remarks} like '%{columnDetail}%'");
            }

            tableBindingSource.Filter = string.Join(" and ", searchTable);
            columnBindingSource.Filter = string.Join(" and ", searchColumn);
        }
        #endregion

        #region " 右クリック：選択項目をクリップボードへコピー "
        private void HandleSelectionDataToClipboard(object? sender, EventArgs e)
        {
            var grid = sender as DataGridView;
            if (grid == null) return;
            if (!(grid.SelectedCells.Count > 0)) return;

            // 選択されたセルを行単位でグループ化
            var groupedRows = grid.SelectedCells
                .Cast<DataGridViewCell>()
                .GroupBy(cell => cell.RowIndex) // 行ごとにグループ化
                .OrderBy(group => group.Key)   // 行の順序を保証
                .Select(rowGroup =>
                    rowGroup.OrderBy(cell => cell.ColumnIndex) // 列順に並び替え
                            .Select(cell => cell.Value?.ToString() ?? "") // セルの値を取得
                            .ToList()
                );

            // 行ごとにタブ区切り、全体を改行区切りで結合
            var clipboardText = string.Join(
                Environment.NewLine,
                groupedRows.Select(row => string.Join("\t", row))
            );

            // クリップボードにコピー
            Clipboard.SetText(clipboardText);
        }
        #endregion

        #region " 右クリック：Select文をクリップボードへコピー "

        private void HandleSelectStatementToClipboard(object? sender, EventArgs e)
        {
            HandleSelectStatementToClipboardCommon(sender, e, false);
        }

        private void HandleSelectStatementToClipboardA5(object? sender, EventArgs e)
        {
            HandleSelectStatementToClipboardCommon(sender, e, true);
        }

        private void HandleSelectStatementToClipboardCommon(object? sender, EventArgs e, bool a5m2)
        {
            var grid = sender as DataGridView;
            if (grid == null) return;
            if (grid.SelectedCells.Count == 0) return;

            var projectName = grid.CurrentRow.Cells[Constants.ColumnColumns.ProjectName].Value.ToString() ?? String.Empty;
            var tableId = grid.CurrentRow.Cells[Constants.ColumnColumns.TableID].Value.ToString() ?? String.Empty;
            var tableName = grid.CurrentRow.Cells[Constants.ColumnColumns.TableName].Value.ToString() ?? String.Empty;
            var tableAlias = view.GetTextTableAliasValue().Trim();
            var omitColumns = GetOmitSqlColumns();

            // SQLのSelect句から除外する列名
            string[] GetOmitSqlColumns()
            {
                string columns = iniController.Get(Constants.IniColumnGrid.section, Constants.IniColumnGrid.omitSqlColumns, String.Empty);
                return columns.Split(',');
            }

            // カラム一覧の物理名と論理名を取得
            var columns = grid.Rows
                .Cast<DataGridViewRow>()
                .Where(row => row.Cells[Constants.ColumnColumns.ProjectName].Value?.ToString() == projectName
                    && row.Cells[Constants.ColumnColumns.TableID].Value?.ToString() == tableId)
                .Where(row => !omitColumns.Contains(row.Cells[Constants.ColumnColumns.PhysicalName].Value?.ToString()))
                .Select(row => new
                {
                    PhysicalName = row.Cells[Constants.ColumnColumns.PhysicalName].Value?.ToString() ?? String.Empty,
                    LogicalName = row.Cells[Constants.ColumnColumns.LogicalName].Value?.ToString() ?? String.Empty
                })
                .ToList();

            if (!columns.Any())
            {
                return;
            }

            // SELECT文を作成
            var selectStatement = new StringBuilder();
            if (a5m2)
            {
                selectStatement.AppendLine($"--* DataTitle '{tableName}'");
                selectStatement.AppendLine("--* CaptionFromComment");
            }
            selectStatement.AppendLine("SELECT");

            for (int i = 0; i < columns.Count; i++)
            {
                var column = columns[i];
                var line = "    "
                    + (i > 0 ? "," : " ")
                    + (string.IsNullOrWhiteSpace(tableAlias) ? "" : $"{tableAlias}.")
                    + $"{column.PhysicalName.PadRight(30)}-- {column.LogicalName}";
                selectStatement.AppendLine(line);
            }

            selectStatement.AppendLine("FROM");
            selectStatement.AppendLine($"    {tableId} {tableAlias!.PadRight(30)}-- {tableName}");

            // クリップボードにコピー
            Clipboard.SetText(selectStatement.ToString());
        }
        #endregion

        #region " ウィンドウサイズ・位置の保存 "
        private void HandleSaveWindowSettings(object? sender, EventArgs e)
        {
            // viewの位置とサイズを保存
            iniController.Set(Constants.IniMain.section, Constants.IniMain.x, view.Location.X);
            iniController.Set(Constants.IniMain.section, Constants.IniMain.y, view.Location.Y);
            iniController.Set(Constants.IniMain.section, Constants.IniMain.width, view.Width);
            iniController.Set(Constants.IniMain.section, Constants.IniMain.height, view.Height);
            iniController.Set(Constants.IniMain.section, Constants.IniMain.maximized, view.WindowState == FormWindowState.Maximized);
        }

        private void HandleSaveGridSettings(object? sender, EventArgs e)
        {
            SaveGridTableSettingsCommon(sender, e, view.GetGridTables(), Constants.IniTableGrid.section);
            SaveGridTableSettingsCommon(sender, e, view.GetGridColumns(), Constants.IniColumnGrid.section);
        }

        private void SaveGridTableSettingsCommon(object? sender, EventArgs e, DataGridView view, string baseSection)
        {
            foreach (DataGridViewColumn column in view.Columns)
            {
                string section = baseSection + Constants.IniGridSetting.addSection;
                string indexKey = $"{column.Name}{Constants.IniGridSetting.index}";
                string visibleKey = $"{column.Name}{Constants.IniGridSetting.visible}";
                string widthKey = $"{column.Name}{Constants.IniGridSetting.width}";

                iniController.Set(section, indexKey, column.DisplayIndex);
                iniController.Set(section, visibleKey, column.Visible);

                if (column.AutoSizeMode == DataGridViewAutoSizeColumnMode.Fill)
                {
                    iniController.Set(section, widthKey, -1);
                }
                else
                {
                    iniController.Set(section, widthKey, column.Width);
                }
            }
        }
        #endregion

        #region " ウィンドウサイズ・位置の読込 "
        private void HandleRestoreWindowSettings(object? sender, EventArgs e)
        {
            // viewの位置とサイズ
            int x = iniController.Get(Constants.IniMain.section, Constants.IniMain.x, 50);
            int y = iniController.Get(Constants.IniMain.section, Constants.IniMain.y, 50);
            int width = iniController.Get(Constants.IniMain.section, Constants.IniMain.width, 1274);
            int height = iniController.Get(Constants.IniMain.section, Constants.IniMain.height, 668);
            bool isMaximized = iniController.Get(Constants.IniMain.section, Constants.IniMain.maximized, false);

            if (isMaximized)
            {
                // 最大化
                view.WindowState = FormWindowState.Maximized;
            } 
            else
            {
                view.StartPosition = FormStartPosition.Manual;
                view.Location = new System.Drawing.Point(x, y);
                view.Size = new System.Drawing.Size(width, height);
            }
        }

        private void HandleRestoreGridSettings(object? sender, EventArgs e)
        {
            RestoreGridSettingsCommon(sender, e, view.GetGridTables(), Constants.IniTableGrid.section);
            RestoreGridSettingsCommon(sender, e, view.GetGridColumns(), Constants.IniColumnGrid.section);
        }

        private void RestoreGridSettingsCommon(object? sender, EventArgs e, DataGridView view, string baseSection)
        {
            for (int i = 0; i < view.Columns.Count; i++)
            {
                var column = view.Columns[i];
                string section = baseSection + Constants.IniGridSetting.addSection;
                string indexKey = $"{column.Name}{Constants.IniGridSetting.index}";
                string visibleKey = $"{column.Name}{Constants.IniGridSetting.visible}";
                string widthKey = $"{column.Name}{Constants.IniGridSetting.width}";

                int index = iniController.Get(section, indexKey, column.Index);
                bool visible = iniController.Get(section, visibleKey, column.Visible);
                int width = iniController.Get(section, widthKey, column.Width);

                column.DisplayIndex = index;
                column.Visible = visible;
        
                if (width == -1)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                else
                {
                    column.Width = width;
                }
            }
        }
        #endregion

    }
}
