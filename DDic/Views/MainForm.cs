// MIT License
// Copyright (c) 2025 seebikn   
// See LICENSE file in the project root for full license information.

using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using DDic.Controllers;

namespace DDic
{
    public partial class MainForm : Form
    {
        public event EventHandler? OnHandleTableSelected;
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

            GridTables.SelectionChanged += (s, e) => OnHandleTableSelected?.Invoke(this.GridTables, EventArgs.Empty);
            TextTableName.TextChanged += (s, e) => OnHandleApplyFilters?.Invoke(this, EventArgs.Empty);
            TextProjectName.TextChanged += (s, e) => OnHandleApplyFilters?.Invoke(this, EventArgs.Empty);
            TextColumnName.TextChanged += (s, e) => OnHandleApplyFilters?.Invoke(this, EventArgs.Empty);
            TextColumnDetail.TextChanged += (s, e) => OnHandleApplyFilters?.Invoke(this, EventArgs.Empty);

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
            var tsmiColumnCopy = new ToolStripMenuItem();
            tsmiColumnCopy.Name = Constants.MenuColumns.Copy;
            tsmiColumnCopy.Text = Constants.MenuColumns.CopyText;
            MenuColumns.Items.AddRange(new ToolStripItem[] { tsmiColumnCopy });
            MenuColumns.Items[tsmiColumnCopy.Name]!.Click += (s, e) => OnHandleHandleSelectionDataToClipboard?.Invoke(this.GridColumns, EventArgs.Empty);

            // カラム一覧
            var tsmiFormSelectStatement = new ToolStripMenuItem();
            tsmiFormSelectStatement.Name = Constants.MenuColumns.SqlSelect;
            tsmiFormSelectStatement.Text = Constants.MenuColumns.SqlSelectText;
            MenuColumns.Items.AddRange(new ToolStripItem[] { tsmiFormSelectStatement });
            MenuColumns.Items[tsmiFormSelectStatement.Name]!.Click += (s, e) => OnHandleSelectStatementToClipboard?.Invoke(this.GridColumns, EventArgs.Empty);

            // カラム一覧
            var tsmiFormSelectStatementA5 = new ToolStripMenuItem();
            tsmiFormSelectStatementA5.Name = Constants.MenuColumns.SqlSelectA5;
            tsmiFormSelectStatementA5.Text = Constants.MenuColumns.SqlSelectA5Text;
            MenuColumns.Items.AddRange(new ToolStripItem[] { tsmiFormSelectStatementA5 });
            MenuColumns.Items[tsmiFormSelectStatementA5.Name]!.Click += (s, e) => OnHandleSelectStatementToClipboardA5?.Invoke(this.GridColumns, EventArgs.Empty);
            #endregion
        }

        #region " コントローラー用 set/get "
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
