// MIT License
// Copyright (c) 2025 seebikn   
// See LICENSE file in the project root for full license information.

using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DDic.Controllers;

namespace DDic
{
    public partial class MainForm : Form
    {
        private MainController controller = new MainController();
        private BindingSource tableBindingSource = new BindingSource();
        private BindingSource columnBindingSource = new BindingSource();

        public MainForm()
        {
            InitializeComponent();
            LoadTables();
        }

        private void LoadTables()
        {
            // Resourcesフォルダのパスを取得
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string resourcesDirectory = Path.Combine(appDirectory, Constants.resources);

            controller.LoadData(resourcesDirectory);

            // データをバインド
            tableBindingSource.DataSource = controller.Tables;
            GridTables.DataSource = tableBindingSource;

            columnBindingSource.DataSource = controller.Columns;
            GridColumns.DataSource = columnBindingSource;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // イベント追加
            GridTables.CellBeginEdit += GridTables_CellBeginEdit;
            GridColumns.CellBeginEdit += GridColumns_CellBeginEdit;
            GridTables.SelectionChanged += GridTables_SelectionChanged;
            TextTableName.TextChanged += ApplyFilters;
            TextProjectName.TextChanged += ApplyFilters;
            TextColumnName.TextChanged += ApplyFilters;
            TextColumnDetail.TextChanged += ApplyFilters;
            ButtonClearFiltter.Click += ButtonClearFiltter_Click;

            // DataGridViewのソート停止
            foreach (DataGridViewColumn column in GridTables.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            foreach (DataGridViewColumn column in GridColumns.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            #region " ToolStripMenu コピー "
            // テーブル一覧
            var tsmiTableCopy = new ToolStripMenuItem();
            tsmiTableCopy.Name = "Copy";
            tsmiTableCopy.Text = "コピー";
            MenuTables.Items.AddRange(new ToolStripItem[] { tsmiTableCopy });
            MenuTables.Items[tsmiTableCopy.Name]!.Click += MenuTables_CopyMenuItem_Click;

            // カラム一覧
            var tsmiColumnCopy = new ToolStripMenuItem();
            tsmiColumnCopy.Name = "Copy";
            tsmiColumnCopy.Text = "コピー";
            MenuColumns.Items.AddRange(new ToolStripItem[] { tsmiColumnCopy });
            MenuColumns.Items[tsmiColumnCopy.Name]!.Click += MenuColumns_CopyMenuItem_Click;
            #endregion

            #region " ToolStripMenu Select文 "
            // カラム一覧
            var tsmiFormSelectStatement = new ToolStripMenuItem();
            tsmiFormSelectStatement.Name = "SelectStatement";
            tsmiFormSelectStatement.Text = "Select句作成";
            //tsmiColumnSelectStatement.ShortcutKeys = Keys.Control | Keys.Alt | Keys.D;
            MenuColumns.Items.AddRange(new ToolStripItem[] { tsmiFormSelectStatement });
            MenuColumns.Items[tsmiFormSelectStatement.Name]!.Click += MenuColumns_SelectStatementMenuItem_Click;
            #endregion

            #region " ToolStripMenu Select文(a5m2) "
            // カラム一覧
            var tsmiFormSelectStatementA5 = new ToolStripMenuItem();
            tsmiFormSelectStatementA5.Name = "SelectStatement-a5m2";
            tsmiFormSelectStatementA5.Text = "Select句作成(a5m2)";
            MenuColumns.Items.AddRange(new ToolStripItem[] { tsmiFormSelectStatementA5 });
            MenuColumns.Items[tsmiFormSelectStatementA5.Name]!.Click += MenuColumns_SelectStatementA5m2MenuItem_Click;
            #endregion

        }

        private void GridTables_CellBeginEdit(object? sender, DataGridViewCellCancelEventArgs e)
        {
            e.Cancel = true; // 編集操作をキャンセル
        }

        private void GridColumns_CellBeginEdit(object? sender, DataGridViewCellCancelEventArgs e)
        {
            e.Cancel = true; // 編集操作をキャンセル
        }

        #region " テーブル一覧で行選択 "
        /// <summary>
        /// テーブル一覧で行選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridTables_SelectionChanged(object? sender, EventArgs e)
        {
            if (GridTables.CurrentRow == null)
            {
                return;
            }

            // 選択したテーブルの概要を概要欄に表示
            var selectedDescription = GridTables.CurrentRow.Cells[Constants.TableColumns.Description].Value.ToString();
            TextTableDescription.Text = selectedDescription;

            // 選択したテーブルのカラムに絞り込み
            var selectedProjectName = GridTables.CurrentRow.Cells[Constants.TableColumns.ProjectName].Value.ToString();
            var selectedTableID = GridTables.CurrentRow.Cells[Constants.TableColumns.TableID].Value.ToString();
            columnBindingSource.Filter = $"{Constants.ColumnColumns.ProjectName} = '{selectedProjectName}' and {Constants.ColumnColumns.TableID} = '{selectedTableID}'";
        }
        #endregion

        #region " 検索 "
        private void ApplyFilters(object? sender, EventArgs e)
        {
            var projectName = TextProjectName.Text;
            var tableName = TextTableName.Text.Trim();
            var columnName = TextColumnName.Text.Trim();
            var columnDetail = TextColumnDetail.Text.Trim();

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

        private void ButtonClearFiltter_Click(object? sender, EventArgs e)
        {
            TextProjectName.Text = string.Empty;
            TextTableName.Text = string.Empty;
            TextColumnName.Text = string.Empty;
            TextColumnDetail.Text = string.Empty;
        }
        #endregion

        #region " 「コピー」メニュークリック時の動作 "

        private void MenuTables_CopyMenuItem_Click(object? sender, EventArgs e)
        {
            Common_CopyMenuItem_Click(GridTables, sender, e);
        }
        private void MenuColumns_CopyMenuItem_Click(object? sender, EventArgs e)
        {
            Common_CopyMenuItem_Click(GridColumns, sender, e);
        }

        private void Common_CopyMenuItem_Click(DataGridView target, object? sender, EventArgs e)
        {
            if (target.SelectedCells.Count > 0)
            {
                // 選択されたセルを行単位でグループ化
                var groupedRows = target.SelectedCells
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
        }
        #endregion

        #region " 「Select文」メニュークリック時の動作 "
        private void MenuColumns_SelectStatementMenuItem_Click(object? sender, EventArgs e)
        {
            Column_SelectStatementMenuItem_Click(sender, e, false);
        }

        private void MenuColumns_SelectStatementA5m2MenuItem_Click(object? sender, EventArgs e)
        {
            Column_SelectStatementMenuItem_Click(sender, e, true);
        }

        private void Column_SelectStatementMenuItem_Click(object? sender, EventArgs e, bool a5m2)
        {
            if (GridColumns.SelectedCells.Count == 0)
            {
                return;
            }

            var projectName = GridColumns.CurrentRow.Cells[Constants.ColumnColumns.ProjectName].Value.ToString() ?? String.Empty;
            var tableId = GridColumns.CurrentRow.Cells[Constants.ColumnColumns.TableID].Value.ToString() ?? String.Empty;
            var tableName = GridColumns.CurrentRow.Cells[Constants.ColumnColumns.TableName].Value.ToString() ?? String.Empty;
            var tableAlias = TextTableAlias.Text;

            // カラム一覧の物理名と論理名を取得
            var columns = GridColumns.Rows
                .Cast<DataGridViewRow>()
                .Where(row => row.Cells[Constants.ColumnColumns.ProjectName].Value?.ToString() == projectName
                    && row.Cells[Constants.ColumnColumns.TableID].Value?.ToString() == tableId)
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
                selectStatement.AppendLine($"-- * DataTitle '{tableName}'");
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

    }
}
