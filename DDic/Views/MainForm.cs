// MIT License
// Copyright (c) 2025 seebikn   
// See LICENSE file in the project root for full license information.

namespace DDic
{
    public partial class MainForm : Form
    {
        public event EventHandler? OnHandleTableSelected;
        public event DataGridViewCellEventHandler? OnHandleColumnDoubleClick;
        public event EventHandler? OnHandleApplyFilters;
        public event EventHandler? OnHandleHandleSelectionDataToClipboard;
        public event EventHandler? OnHandleSelectStatementToClipboard;
        public event EventHandler? OnHandleSelectStatementToClipboardA5;

        public MainForm()
        {
            InitializeComponent();

            // イベント追加
            GridTables.CellBeginEdit += GridTables_CellBeginEdit;
            GridColumns.CellBeginEdit += GridColumns_CellBeginEdit;
            ButtonClearFiltter.Click += ButtonClearFiltter_Click;

            GridTables.SelectionChanged += (s, e) => OnHandleTableSelected?.Invoke(s, e);
            GridColumns.CellDoubleClick += (s, e) => OnHandleColumnDoubleClick?.Invoke(s, e);
            TextTableName.TextChanged += (s, e) => OnHandleApplyFilters?.Invoke(s, e);
            TextProjectName.TextChanged += (s, e) => OnHandleApplyFilters?.Invoke(s, e);
            TextColumnName.TextChanged += (s, e) => OnHandleApplyFilters?.Invoke(s, e);
            TextColumnDetail.TextChanged += (s, e) => OnHandleApplyFilters?.Invoke(s, e);

            // DataGridViewのソート停止
            foreach (DataGridViewColumn column in GridTables.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            foreach (DataGridViewColumn column in GridColumns.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            #region " テーブル一覧　右クリックメニュー設定 "
            // テーブル一覧
            var tsmiTableCopy = new ToolStripMenuItem();
            tsmiTableCopy.Name = Constants.MenuTables.Copy;
            tsmiTableCopy.Text = Constants.MenuTables.CopyText;
            MenuTables.Items.AddRange(new ToolStripItem[] { tsmiTableCopy });
            MenuTables.Items[tsmiTableCopy.Name]!.Click += (s, e) => OnHandleHandleSelectionDataToClipboard?.Invoke(this.GridTables, EventArgs.Empty);
            #endregion

            #region " カラム一覧　右クリックメニュー設定 "
            // カラム一覧
            var tsmiColumnCopy = new ToolStripMenuItem
            {
                Name = Constants.MenuColumns.Copy,
                Text = Constants.MenuColumns.CopyText,
            };
            MenuColumns.Items.AddRange(new ToolStripItem[] { tsmiColumnCopy });
            MenuColumns.Items[tsmiColumnCopy.Name]!.Click += (s, e) => OnHandleHandleSelectionDataToClipboard?.Invoke(this.GridColumns, EventArgs.Empty);

            // カラム一覧
            var tsmiFormSelectStatement = new ToolStripMenuItem
            {
                Name = Constants.MenuColumns.SqlSelect,
                Text = Constants.MenuColumns.SqlSelectText,
            };
            MenuColumns.Items.AddRange(new ToolStripItem[] { tsmiFormSelectStatement });
            MenuColumns.Items[tsmiFormSelectStatement.Name]!.Click += (s, e) => OnHandleSelectStatementToClipboard?.Invoke(this.GridColumns, EventArgs.Empty);

            // カラム一覧
            var tsmiFormSelectStatementA5 = new ToolStripMenuItem
            {
                Name = Constants.MenuColumns.SqlSelectA5,
                Text = Constants.MenuColumns.SqlSelectA5Text,
            };
            MenuColumns.Items.AddRange(new ToolStripItem[] { tsmiFormSelectStatementA5 });
            MenuColumns.Items[tsmiFormSelectStatementA5.Name]!.Click += (s, e) => OnHandleSelectStatementToClipboardA5?.Invoke(this.GridColumns, EventArgs.Empty);
            #endregion
        }

        #region " コントローラー用 set/get "
        public DataGridView GetGridTables() { return this.GridTables; }

        public DataGridView GetGridColumns() { return this.GridColumns; }

        public void SetDataSource(BindingSource table, BindingSource columns)
        {
            GridTables.DataSource = table;
            GridColumns.DataSource = columns;
        }

        public void SetTextTableDescription(string str)
        {
            this.TextTableDescription.Text = str;
        }

        public string GetTextProjectNameValue()
        {
            return TextProjectName.Text;
        }

        public string GetTextTableNameValue()
        {
            return TextTableName.Text;
        }

        public string GetTextColumnNameValue()
        {
            return TextColumnName.Text;
        }

        public string GetTextColumnDetailValue()
        {
            return TextColumnDetail.Text;
        }

        public string GetTextTableAliasValue()
        {
            return TextTableAlias.Text;
        }

        public void SetMenuTablesVisible(string name, bool value)
        {
            MenuTables.Items[name]!.Visible = value;
        }

        public void SetMenuColumnsVisible(string name, bool value)
        {
            MenuColumns.Items[name]!.Visible = value;
        }

        public int SplitterDistance
        {
            get => splitContainer1.SplitterDistance;
            set => splitContainer1.SplitterDistance = value;
        }


        #endregion

        private void GridTables_CellBeginEdit(object? sender, DataGridViewCellCancelEventArgs e)
        {
            e.Cancel = true; // 編集操作をキャンセル
        }

        private void GridColumns_CellBeginEdit(object? sender, DataGridViewCellCancelEventArgs e)
        {
            e.Cancel = true; // 編集操作をキャンセル
        }

        private void ButtonClearFiltter_Click(object? sender, EventArgs e)
        {
            TextProjectName.Text = string.Empty;
            TextTableName.Text = string.Empty;
            TextColumnName.Text = string.Empty;
            TextColumnDetail.Text = string.Empty;
        }

    }
}
