﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDic
{
    public static class Constants
    {
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
    }

}
