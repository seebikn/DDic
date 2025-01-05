namespace DDic
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            groupBox1 = new GroupBox();
            ButtonClearFiltter = new Button();
            TextColumnDetail = new TextBox();
            TextProjectName = new TextBox();
            LabelColumnDetail = new Label();
            LabelProjectName = new Label();
            TextColumnName = new TextBox();
            LabelColumnName = new Label();
            TextTableName = new TextBox();
            LabelTableName = new Label();
            splitContainer1 = new SplitContainer();
            GridTables = new DataGridView();
            ProjectName = new DataGridViewTextBoxColumn();
            TableID = new DataGridViewTextBoxColumn();
            TableName = new DataGridViewTextBoxColumn();
            Description = new DataGridViewTextBoxColumn();
            MenuTables = new ContextMenuStrip(components);
            GridColumns = new DataGridView();
            ParentProjectName = new DataGridViewTextBoxColumn();
            ParentTableID = new DataGridViewTextBoxColumn();
            ParentTableName = new DataGridViewTextBoxColumn();
            ColumnNo = new DataGridViewTextBoxColumn();
            PhysicalName = new DataGridViewTextBoxColumn();
            LogicalName = new DataGridViewTextBoxColumn();
            PrimaryKey = new DataGridViewCheckBoxColumn();
            NotNullable = new DataGridViewCheckBoxColumn();
            DataType = new DataGridViewTextBoxColumn();
            DataSize = new DataGridViewTextBoxColumn();
            Remarks = new DataGridViewTextBoxColumn();
            MenuColumns = new ContextMenuStrip(components);
            groupBox2 = new GroupBox();
            TextTableDescription = new TextBox();
            label1 = new Label();
            TextTableAlias = new TextBox();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridTables).BeginInit();
            ((System.ComponentModel.ISupportInitialize)GridColumns).BeginInit();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(ButtonClearFiltter);
            groupBox1.Controls.Add(TextColumnDetail);
            groupBox1.Controls.Add(TextProjectName);
            groupBox1.Controls.Add(LabelColumnDetail);
            groupBox1.Controls.Add(LabelProjectName);
            groupBox1.Controls.Add(TextColumnName);
            groupBox1.Controls.Add(LabelColumnName);
            groupBox1.Controls.Add(TextTableName);
            groupBox1.Controls.Add(LabelTableName);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(522, 75);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "検索";
            // 
            // ButtonClearFiltter
            // 
            ButtonClearFiltter.Location = new Point(461, 45);
            ButtonClearFiltter.Name = "ButtonClearFiltter";
            ButtonClearFiltter.Size = new Size(49, 23);
            ButtonClearFiltter.TabIndex = 8;
            ButtonClearFiltter.Text = "Clear";
            ButtonClearFiltter.UseVisualStyleBackColor = true;
            // 
            // TextColumnDetail
            // 
            TextColumnDetail.Location = new Point(299, 45);
            TextColumnDetail.Name = "TextColumnDetail";
            TextColumnDetail.Size = new Size(156, 23);
            TextColumnDetail.TabIndex = 7;
            // 
            // TextProjectName
            // 
            TextProjectName.Location = new Point(299, 16);
            TextProjectName.Name = "TextProjectName";
            TextProjectName.Size = new Size(156, 23);
            TextProjectName.TabIndex = 6;
            // 
            // LabelColumnDetail
            // 
            LabelColumnDetail.AutoSize = true;
            LabelColumnDetail.Location = new Point(234, 48);
            LabelColumnDetail.Name = "LabelColumnDetail";
            LabelColumnDetail.Size = new Size(57, 15);
            LabelColumnDetail.TabIndex = 5;
            LabelColumnDetail.Text = "カラム備考";
            // 
            // LabelProjectName
            // 
            LabelProjectName.AutoSize = true;
            LabelProjectName.Location = new Point(234, 19);
            LabelProjectName.Name = "LabelProjectName";
            LabelProjectName.Size = new Size(59, 15);
            LabelProjectName.TabIndex = 4;
            LabelProjectName.Text = "プロジェクト";
            // 
            // TextColumnName
            // 
            TextColumnName.Location = new Point(54, 45);
            TextColumnName.Name = "TextColumnName";
            TextColumnName.Size = new Size(156, 23);
            TextColumnName.TabIndex = 3;
            // 
            // LabelColumnName
            // 
            LabelColumnName.AutoSize = true;
            LabelColumnName.Location = new Point(6, 48);
            LabelColumnName.Name = "LabelColumnName";
            LabelColumnName.Size = new Size(33, 15);
            LabelColumnName.TabIndex = 1;
            LabelColumnName.Text = "カラム";
            // 
            // TextTableName
            // 
            TextTableName.Location = new Point(54, 16);
            TextTableName.Name = "TextTableName";
            TextTableName.Size = new Size(156, 23);
            TextTableName.TabIndex = 2;
            // 
            // LabelTableName
            // 
            LabelTableName.AutoSize = true;
            LabelTableName.Location = new Point(6, 19);
            LabelTableName.Name = "LabelTableName";
            LabelTableName.Size = new Size(42, 15);
            LabelTableName.TabIndex = 0;
            LabelTableName.Text = "テーブル";
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer1.FixedPanel = FixedPanel.Panel1;
            splitContainer1.Location = new Point(12, 93);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(GridTables);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(GridColumns);
            splitContainer1.Size = new Size(1234, 524);
            splitContainer1.SplitterDistance = 350;
            splitContainer1.TabIndex = 4;
            // 
            // GridTables
            // 
            GridTables.AllowUserToAddRows = false;
            GridTables.AllowUserToDeleteRows = false;
            GridTables.AllowUserToOrderColumns = true;
            GridTables.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Yu Gothic UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridTables.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridTables.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridTables.Columns.AddRange(new DataGridViewColumn[] { ProjectName, TableID, TableName, Description });
            GridTables.ContextMenuStrip = MenuTables;
            GridTables.Dock = DockStyle.Fill;
            GridTables.Location = new Point(0, 0);
            GridTables.Name = "GridTables";
            GridTables.ReadOnly = true;
            GridTables.RowHeadersVisible = false;
            GridTables.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            GridTables.Size = new Size(350, 524);
            GridTables.StandardTab = true;
            GridTables.TabIndex = 0;
            // 
            // ProjectName
            // 
            ProjectName.DataPropertyName = "ProjectName";
            dataGridViewCellStyle2.BackColor = SystemColors.InactiveBorder;
            ProjectName.DefaultCellStyle = dataGridViewCellStyle2;
            ProjectName.HeaderText = " ";
            ProjectName.Name = "ProjectName";
            ProjectName.ReadOnly = true;
            ProjectName.Width = 34;
            // 
            // TableID
            // 
            TableID.DataPropertyName = "TableID";
            TableID.HeaderText = "テーブルID";
            TableID.Name = "TableID";
            TableID.ReadOnly = true;
            TableID.Width = 120;
            // 
            // TableName
            // 
            TableName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            TableName.DataPropertyName = "TableName";
            TableName.HeaderText = "テーブル名";
            TableName.Name = "TableName";
            TableName.ReadOnly = true;
            // 
            // Description
            // 
            Description.DataPropertyName = "Description";
            Description.HeaderText = "テーブル概要";
            Description.Name = "Description";
            Description.ReadOnly = true;
            Description.Visible = false;
            // 
            // MenuTables
            // 
            MenuTables.Name = "ContextMenuStrip1";
            MenuTables.Size = new Size(61, 4);
            // 
            // GridColumns
            // 
            GridColumns.AllowUserToAddRows = false;
            GridColumns.AllowUserToDeleteRows = false;
            GridColumns.AllowUserToOrderColumns = true;
            GridColumns.AllowUserToResizeRows = false;
            GridColumns.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            GridColumns.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridColumns.Columns.AddRange(new DataGridViewColumn[] { ParentProjectName, ParentTableID, ParentTableName, ColumnNo, PhysicalName, LogicalName, PrimaryKey, NotNullable, DataType, DataSize, Remarks });
            GridColumns.ContextMenuStrip = MenuColumns;
            GridColumns.Dock = DockStyle.Fill;
            GridColumns.Location = new Point(0, 0);
            GridColumns.Name = "GridColumns";
            GridColumns.ReadOnly = true;
            GridColumns.RowHeadersVisible = false;
            GridColumns.Size = new Size(880, 524);
            GridColumns.StandardTab = true;
            GridColumns.TabIndex = 0;
            // 
            // ParentProjectName
            // 
            ParentProjectName.DataPropertyName = "ParentProjectName";
            dataGridViewCellStyle3.BackColor = SystemColors.InactiveBorder;
            ParentProjectName.DefaultCellStyle = dataGridViewCellStyle3;
            ParentProjectName.HeaderText = " ";
            ParentProjectName.Name = "ParentProjectName";
            ParentProjectName.ReadOnly = true;
            ParentProjectName.Width = 34;
            // 
            // ParentTableID
            // 
            ParentTableID.DataPropertyName = "ParentTableID";
            dataGridViewCellStyle4.BackColor = SystemColors.InactiveBorder;
            ParentTableID.DefaultCellStyle = dataGridViewCellStyle4;
            ParentTableID.HeaderText = "テーブルID";
            ParentTableID.Name = "ParentTableID";
            ParentTableID.ReadOnly = true;
            ParentTableID.Width = 60;
            // 
            // ParentTableName
            // 
            ParentTableName.DataPropertyName = "ParentTableName";
            dataGridViewCellStyle5.BackColor = SystemColors.InactiveBorder;
            ParentTableName.DefaultCellStyle = dataGridViewCellStyle5;
            ParentTableName.HeaderText = "テーブル名";
            ParentTableName.Name = "ParentTableName";
            ParentTableName.ReadOnly = true;
            ParentTableName.Width = 60;
            // 
            // ColumnNo
            // 
            ColumnNo.DataPropertyName = "ColumnNo";
            ColumnNo.HeaderText = "列番号";
            ColumnNo.Name = "ColumnNo";
            ColumnNo.ReadOnly = true;
            ColumnNo.Width = 30;
            // 
            // PhysicalName
            // 
            PhysicalName.DataPropertyName = "PhysicalName";
            PhysicalName.HeaderText = "物理名";
            PhysicalName.Name = "PhysicalName";
            PhysicalName.ReadOnly = true;
            PhysicalName.Width = 160;
            // 
            // LogicalName
            // 
            LogicalName.DataPropertyName = "LogicalName";
            LogicalName.HeaderText = "論理名";
            LogicalName.Name = "LogicalName";
            LogicalName.ReadOnly = true;
            LogicalName.Width = 160;
            // 
            // PrimaryKey
            // 
            PrimaryKey.DataPropertyName = "PrimaryKey";
            PrimaryKey.FalseValue = "0";
            PrimaryKey.HeaderText = "主キー";
            PrimaryKey.Name = "PrimaryKey";
            PrimaryKey.ReadOnly = true;
            PrimaryKey.Resizable = DataGridViewTriState.True;
            PrimaryKey.SortMode = DataGridViewColumnSortMode.Automatic;
            PrimaryKey.TrueValue = "1";
            PrimaryKey.Width = 24;
            // 
            // NotNullable
            // 
            NotNullable.DataPropertyName = "NotNullable";
            NotNullable.FalseValue = "0";
            NotNullable.HeaderText = "NotNull";
            NotNullable.Name = "NotNullable";
            NotNullable.ReadOnly = true;
            NotNullable.Resizable = DataGridViewTriState.True;
            NotNullable.SortMode = DataGridViewColumnSortMode.Automatic;
            NotNullable.TrueValue = "1";
            NotNullable.Width = 34;
            // 
            // DataType
            // 
            DataType.DataPropertyName = "DataType";
            DataType.HeaderText = "型";
            DataType.Name = "DataType";
            DataType.ReadOnly = true;
            DataType.Width = 50;
            // 
            // DataSize
            // 
            DataSize.DataPropertyName = "DataSize";
            DataSize.HeaderText = "size";
            DataSize.Name = "DataSize";
            DataSize.ReadOnly = true;
            DataSize.Width = 40;
            // 
            // Remarks
            // 
            Remarks.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Remarks.DataPropertyName = "Remarks";
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            Remarks.DefaultCellStyle = dataGridViewCellStyle6;
            Remarks.HeaderText = "備考";
            Remarks.Name = "Remarks";
            Remarks.ReadOnly = true;
            // 
            // MenuColumns
            // 
            MenuColumns.Name = "ContextMenuStrip2";
            MenuColumns.Size = new Size(61, 4);
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox2.Controls.Add(TextTableDescription);
            groupBox2.Location = new Point(673, 12);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(573, 75);
            groupBox2.TabIndex = 3;
            groupBox2.TabStop = false;
            groupBox2.Text = "テーブル概要";
            // 
            // TextTableDescription
            // 
            TextTableDescription.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            TextTableDescription.Location = new Point(6, 16);
            TextTableDescription.Multiline = true;
            TextTableDescription.Name = "TextTableDescription";
            TextTableDescription.Size = new Size(561, 53);
            TextTableDescription.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(550, 60);
            label1.Name = "label1";
            label1.Size = new Size(30, 15);
            label1.TabIndex = 1;
            label1.Text = "alias";
            // 
            // TextTableAlias
            // 
            TextTableAlias.Location = new Point(586, 57);
            TextTableAlias.Name = "TextTableAlias";
            TextTableAlias.Size = new Size(58, 23);
            TextTableAlias.TabIndex = 2;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1258, 629);
            Controls.Add(TextTableAlias);
            Controls.Add(label1);
            Controls.Add(groupBox2);
            Controls.Add(splitContainer1);
            Controls.Add(groupBox1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(300, 200);
            Name = "MainForm";
            Text = "Form1";
            Load += Form1_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)GridTables).EndInit();
            ((System.ComponentModel.ISupportInitialize)GridColumns).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox groupBox1;
        private SplitContainer splitContainer1;
        private DataGridView GridTables;
        private DataGridView GridColumns;
        private Label LabelTableName;
        private TextBox TextTableName;
        private Label LabelColumnName;
        private TextBox TextColumnDetail;
        private TextBox TextProjectName;
        private Label LabelColumnDetail;
        private Label LabelProjectName;
        private GroupBox groupBox2;
        private TextBox TextTableDescription;
        private TextBox TextColumnName;
        private Button ButtonClearFiltter;
        private ContextMenuStrip MenuTables;
        private ContextMenuStrip MenuColumns;
        private Label label1;
        private TextBox TextTableAlias;
        private DataGridViewTextBoxColumn ProjectName;
        private DataGridViewTextBoxColumn TableID;
        private DataGridViewTextBoxColumn TableName;
        private DataGridViewTextBoxColumn Description;
        private DataGridViewTextBoxColumn ParentProjectName;
        private DataGridViewTextBoxColumn ParentTableID;
        private DataGridViewTextBoxColumn ParentTableName;
        private DataGridViewTextBoxColumn ColumnNo;
        private DataGridViewTextBoxColumn PhysicalName;
        private DataGridViewTextBoxColumn LogicalName;
        private DataGridViewCheckBoxColumn PrimaryKey;
        private DataGridViewCheckBoxColumn NotNullable;
        private DataGridViewTextBoxColumn DataType;
        private DataGridViewTextBoxColumn DataSize;
        private DataGridViewTextBoxColumn Remarks;
    }
}
