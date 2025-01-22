using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDic
{
    public static class Constants
    {
        // リソースフォルダ
        public static string resources = "Resources";

        /// <summary>
        /// テーブル一覧の列名を定義する構造体
        /// </summary>
        public static class TableColumns
        {
            public const string ProjectName = "ProjectName";  // プロジェクト名
            public const string TableID = "TableID";          // テーブルID
            public const string TableName = "TableName";      // テーブル名
            public const string Description = "Description";  // 概要
        }

        /// <summary>
        /// カラム一覧の列名を定義する構造体
        /// </summary>
        public static class ColumnColumns
        {
            public const string ProjectName = "ParentProjectName";  // プロジェクト名
            public const string TableID = "ParentTableID";          // テーブルID
            public const string TableName = "ParentTableName";      // テーブル名
            public const string ColumnNo = "ColumnNo";              // 列No
            public const string LogicalName = "LogicalName";        // 論理カラム名
            public const string PhysicalName = "PhysicalName";      // 物理カラム名
            public const string PrimaryKey = "PrimaryKey";          // 主キー
            public const string NotNullable = "NotNullable";        // NULL許容
            public const string DataType = "DataType";              // 型
            public const string DataSize = "DataSize";              // サイズ
            public const string Remarks = "Remarks";                // 備考
        }

        /// <summary>
        /// テーブル一覧の右クリックメニューを定義する構造体
        /// </summary>
        public static class MenuTables
        {
            public const string Copy = "Copy";
            public const string CopyText = "Copy";
        }

        /// <summary>
        /// カラム一覧の右クリックメニューを定義する構造体
        /// </summary>
        public static class MenuColumns
        {
            public const string Copy = "Copy";
            public const string CopyText = "Copy";
            public const string SqlSelect = "CreateSelect";
            public const string SqlSelectText = "Select文生成";
            public const string SqlSelectA5 = "CreateSelect(a5m2)";
            public const string SqlSelectA5Text = "Select文生成(a5m2)";
        }

        /// <summary>
        /// テーブル一覧の右クリック設定(config.ini)
        /// </summary>
        public static class IniTableGrid
        {
            public const string section = "TableGrid";

            public const string copyVisible = "CopyVisible";
        }

        /// <summary>
        /// カラム一覧の右クリック設定(config.ini)
        /// </summary>
        public static class IniColumnGrid
        {
            public const string section = "ColumnGrid";

            public const string copyVisible = "CopyVisible";
            public const string createSqlVisible = "CreateSqlVisible";
            public const string createSqlA5m2Visible = "CreateSqlSql(a5m2)Visible";
            public const string omitSqlColumns = "OmitSqlColumns";
        }

        /// <summary>
        /// ウィンドウの表示設定(config.ini)
        /// </summary>
        public static class IniMain
        {
            public const string section = "Window_Setting";
            public const string x = "X";
            public const string y = "Y";
            public const string width = "Width";
            public const string height = "Height";
            public const string maximized = "Maximized";
            public const string splitDistance = "SplitDistance";
        }

        /// <summary>
        /// 一覧の表示設定(config.ini)
        /// </summary>
        public static class IniGridSetting
        {
            public const string addSection = "_Setting";

            public const string index = "_Index";
            public const string visible = "_Visible";
            public const string width = "_Width";
        }
    }

}
