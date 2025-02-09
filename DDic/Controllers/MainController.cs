using System.Data;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using DDic.Models;

namespace DDic.Controllers
{
    internal class MainController
    {
        private readonly MainForm view;
        private readonly IniController iniController;
        private readonly BindingSource tableBindingSource = [];
        private readonly BindingSource columnBindingSource = [];

        public MainController(string iniFilePath)
        {
            view = new MainForm();
            view.OnHandleTableSelected += HandleTableSelected;
            view.OnHandleColumnDoubleClick += HandleColumnDoubleClick;
            view.OnHandleApplyFilters += HandleApplyFilters;
            view.OnHandleHandleFontChange += HandleFontChange;
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

            System.Windows.Forms.Application.Run(view);
        }

        private void LoadTables()
        {
            // Resourcesからtsvデータの読み込み
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string resourcesDirectory = Path.Combine(appDirectory, Constants.resources);
            DataTable tables = new();
            DataTable columns = new();
            LoadData(resourcesDirectory, tables, columns);

            // データバインド
            tableBindingSource.DataSource = tables;
            columnBindingSource.DataSource = columns;
            view.SetDataSource(tableBindingSource, columnBindingSource);

            static void LoadData(string directoryPath, DataTable tables, DataTable columns)
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
                static string GetProjectName(string filePath, string baseName)
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
            if (sender is not DataGridView grid) return;
            if (grid.SelectedCells.Count == 0) return;

            var row = grid.Rows[grid.SelectedCells[0].RowIndex];

            // 選択したテーブルの概要を概要欄に表示
            var selectedDescription = row.Cells[Constants.TableColumns.Description].Value.ToString();
            view.SetTextTableDescription(selectedDescription!);

            // 選択したテーブルのカラムに絞り込み
            var selectedProjectName = row.Cells[Constants.TableColumns.ProjectName].Value.ToString();
            var selectedTableID = row.Cells[Constants.TableColumns.TableID].Value.ToString();
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

            List<string> searchTable = [];
            List<string> searchColumn = [];

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

        #region " 右クリック：フォント変更 "
        private void HandleFontChange(object? sender, EventArgs e)
        {
            var currentFont = view.GetGridTables().Font;

            using (FontDialog fontDialog = new())
            {
                fontDialog.Font = currentFont;
                if (fontDialog.ShowDialog() == DialogResult.OK)
                {
                    ApplyFont(fontDialog.Font);
                }
            }
        }
        private void ApplyFont(Font font)
        {
            view.GetGridTables().Font = font;
            view.GetGridColumns().Font = font;
        }
        #endregion

        #region " 右クリック：選択項目をクリップボードへコピー "
        private void HandleSelectionDataToClipboard(object? sender, EventArgs e)
        {
            if (sender is not DataGridView grid) return;
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

            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                // shiftを押している場合は縦横を入れ替える
                groupedRows = Enumerable.Range(0, groupedRows.Max(row => row.Count))
                        .Select(colIndex => groupedRows.Select(row => colIndex < row.Count ? row[colIndex] : "").ToList()
                    ).ToList();
            }

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
            if (sender is not DataGridView grid) return;
            if (grid.SelectedCells.Count == 0) return;

            var projectName = grid.Rows[grid.SelectedCells[0].RowIndex].Cells[Constants.ColumnColumns.ProjectName].Value.ToString() ?? String.Empty;
            var tableId = grid.Rows[grid.SelectedCells[0].RowIndex].Cells[Constants.ColumnColumns.TableID].Value.ToString() ?? String.Empty;
            var tableName = grid.Rows[grid.SelectedCells[0].RowIndex].Cells[Constants.ColumnColumns.TableName].Value.ToString() ?? String.Empty;
            var tableAlias = view.GetTextTableAliasValue().Trim();

            // SQLのSelect句から除外する列名
            string[] omitColumns = [];
            string[] GetOmitSqlColumns()
            {
                string columns = iniController.Get(Constants.IniColumnGrid.section, Constants.IniColumnGrid.omitSqlColumns, String.Empty);
                return columns.Split(',');
            }
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                // shiftを押している場合は除外しない
            }
            else
            {
                omitColumns = GetOmitSqlColumns();
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

            if (columns.Count == 0)
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

        #region " ウィンドウサイズの保存 "
        private void HandleSaveWindowSettings(object? sender, EventArgs e)
        {
            // viewのサイズを保存
            iniController.Set(Constants.IniMain.section, Constants.IniMain.width, view.Width);
            iniController.Set(Constants.IniMain.section, Constants.IniMain.height, view.Height);
            iniController.Set(Constants.IniMain.section, Constants.IniMain.maximized, view.WindowState == FormWindowState.Maximized);
            iniController.Set(Constants.IniMain.section, Constants.IniMain.fontName, view.GetGridTables().Font.Name);
            iniController.Set(Constants.IniMain.section, Constants.IniMain.fontSize, view.GetGridTables().Font.Size);
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

        #region " ウィンドウサイズの読込 "
        private void HandleRestoreWindowSettings(object? sender, EventArgs e)
        {
            // viewのサイズ
            int width = iniController.Get(Constants.IniMain.section, Constants.IniMain.width, 1274);
            int height = iniController.Get(Constants.IniMain.section, Constants.IniMain.height, 668);
            int splitDistance = iniController.Get(Constants.IniMain.section, Constants.IniMain.splitDistance, 350);
            bool isMaximized = iniController.Get(Constants.IniMain.section, Constants.IniMain.maximized, false);

            string fontName = iniController.Get(Constants.IniMain.section, Constants.IniMain.fontName, "Yu Gothic UI");
            float fontSize = iniController.Get(Constants.IniMain.section, Constants.IniMain.fontSize, 9);

            if (isMaximized)
            {
                // 最大化
                view.WindowState = FormWindowState.Maximized;
            }
            else
            {
                view.StartPosition = FormStartPosition.Manual;
                view.Size = new System.Drawing.Size(width, height);
            }

            view.SplitterDistance = splitDistance;
            ApplyFont(new Font(fontName, fontSize));
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

        #region " カラム一覧でダブルクリック "
        /// <summary>
        /// カラム一覧でダブルクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleColumnDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (sender is not DataGridView grid) return;
            if (grid.CurrentRow == null) return;

            var columnName = grid.Columns[e.ColumnIndex].Name;

            void SearchCellSelect(string colName)
            {
                var cellValue = grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
                if (string.IsNullOrEmpty(cellValue)) return;

                // テーブル一覧該当列を探索して、一致する行を選択状態にする
                foreach (DataGridViewRow row in view.GetGridTables().Rows)
                {
                    var targetCell = row.Cells[colName];
                    if (targetCell?.Value?.ToString() == cellValue)
                    {
                        view.GetGridTables().ClearSelection();
                        targetCell.Selected = true;

                        // 一致した行を表示（スクロールして見える位置にする）
                        view.GetGridTables().FirstDisplayedScrollingRowIndex = row.Index;
                        return;
                    }
                }
            }

            switch (columnName)
            {
                case Constants.ColumnColumns.TableID:
                    SearchCellSelect(Constants.TableColumns.TableID);
                    break;
                default:
                    SearchCellSelect(Constants.TableColumns.TableName);
                    break;
            }

        }
        #endregion

    }
}
