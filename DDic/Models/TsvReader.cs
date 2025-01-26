namespace DDic.Models
{
    internal class TsvReader
    {
        public static List<string[]> ReadFile(string filePath, int columnNum)
        {
            var rows = new List<string[]>();

            using (var reader = new StreamReader(filePath))
            {
                string? line;
                var buffer = new System.Text.StringBuilder();
                bool inQuotes = false; // 引用符内かどうかのフラグ
                int lineNumber = 0;

                while ((line = reader.ReadLine()) != null)
                {
                    lineNumber++;

                    if (lineNumber == 1)
                    {
                        // ヘッダースキップ
                        continue;
                    }

                    if (inQuotes)
                    {
                        // 引用符内で改行が含まれる場合は、バッファに追加して次の行を待つ
                        buffer.Append("\n").Append(line);
                        inQuotes = false;
                    }
                    else
                    {
                        // 新しい行を処理開始
                        buffer.Clear();
                        buffer.Append(line);
                    }

                    // 現在のバッファを解析
                    var row = ParseRow(buffer.ToString(), ref inQuotes);

                    if (!inQuotes) // フィールド解析が完了した場合のみ処理
                    {
                        if (row.Length != columnNum)
                        {
                            throw new InvalidDataException($"行 {lineNumber}: 列数が3列ではありません (列数: {row.Length})");
                        }

                        rows.Add(row);
                    }
                }

                // ファイル終了後に引用符が閉じられていない場合はエラー
                if (inQuotes)
                {
                    throw new InvalidDataException($"ファイルの終端で引用符が閉じられていません");
                }
            }

            return rows;
        }

        /// <summary>
        /// データをタブ区切り文字で分けて配列で返す。
        /// データ内に引用符(")が存在しない場合は引用符有無フラグをfalseで返す。
        /// 引用符が存在しても閉じられている場合は引用符有無フラグをfalseで返す。
        /// 引用符が閉じられていない場合、改行文字を含むデータとして判断し、引用符有無フラグをtrueで返す。
        /// </summary>
        /// <param name="line">データ</param>
        /// <param name="inQuotes">引用符有無フラグ</param>
        /// <returns></returns>
        private static string[] ParseRow(string line, ref bool inQuotes)
        {
            var fields = new List<string>();
            var currentField = new System.Text.StringBuilder();

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (inQuotes)
                {
                    if (c == '"')
                    {
                        // 次の文字がダブルクォートの場合はエスケープされたもの
                        if (i + 1 < line.Length && line[i + 1] == '"')
                        {
                            currentField.Append('"');
                            i++; // 次のダブルクォートをスキップ
                        }
                        else
                        {
                            inQuotes = false; // 引用終了
                        }
                    }
                    else
                    {
                        currentField.Append(c);
                    }
                }
                else
                {
                    if (c == '\t') // フィールドの区切り
                    {
                        fields.Add(currentField.ToString());
                        currentField.Clear();
                    }
                    else if (c == '"') // フィールドの開始
                    {
                        inQuotes = true;
                    }
                    else
                    {
                        currentField.Append(c);
                    }
                }
            }

            // 最後のフィールドを追加
            fields.Add(currentField.ToString());

            return fields.ToArray();
        }
    }
}
