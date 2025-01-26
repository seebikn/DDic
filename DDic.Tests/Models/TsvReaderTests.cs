using Xunit;
using DDic.Models;

namespace DDic.Tests.Models
{
    public class TsvReaderTests : IDisposable
    {
        private readonly string testTsvFilePath;

        public TsvReaderTests()
        {
            // テスト用の一時TSVファイルパスを設定
            testTsvFilePath = Path.Combine(Path.GetTempPath(), "test_data.tsv");
        }

        /// <summary>
        /// ヘッダー行をスキップし、データ行のみを正しく読み取れることを検証します。
        /// </summary>
        [Fact]
        public void ReadFile_ShouldSkipHeaderAndReadDataRows()
        {
            // Arrange
            var tsvContent = "Header1\tHeader2\tHeader3\n" +
                             "Data1\tData2\tData3\n" +
                             "Data4\tData5\tData6\n";
            File.WriteAllText(testTsvFilePath, tsvContent);
            int expectedColumnNum = 3;

            // Act
            var result = TsvReader.ReadFile(testTsvFilePath, expectedColumnNum);

            // Assert
            Assert.Equal(2, result.Count); // ヘッダーをスキップしているため、データ行は2行
            Assert.Equal(["Data1", "Data2", "Data3"], result[0]);
            Assert.Equal(["Data4", "Data5", "Data6"], result[1]);
        }

        /// <summary>
        /// 引用符を正しく処理できることを確認します。
        /// </summary>
        [Fact]
        public void ReadFile_ShouldHandleQuotedFields()
        {
            // Arrange
            var tsvContent = "Header1\tHeader2\tHeader3\n" +
                             "\"Data1\"\tData2\tData3\n" +
                             "Data4\tData5\tData6\n";
            File.WriteAllText(testTsvFilePath, tsvContent);
            int expectedColumnNum = 3;

            // Act
            var result = TsvReader.ReadFile(testTsvFilePath, expectedColumnNum);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(["Data1", "Data2", "Data3"], result[0]);
            Assert.Equal(["Data4", "Data5", "Data6"], result[1]);
        }

        /// <summary>
        /// 引用符で囲まれたフィールド内の改行を正しく処理できることを確認します。
        /// </summary>
        [Fact]
        public void ReadFile_ShouldHandleQuotedFieldsWithNewlines()
        {
            // Arrange
            var tsvContent = "Header1\tHeader2\tHeader3\n" +
                             "\"Data1\nLine2\"\tData2\tData3\n" +
                             "Data4\tData5\tData6\n";
            File.WriteAllText(testTsvFilePath, tsvContent);
            int expectedColumnNum = 3;

            // Act
            var result = TsvReader.ReadFile(testTsvFilePath, expectedColumnNum);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(["Data1\nLine2", "Data2", "Data3"], result[0]);
            Assert.Equal(["Data4", "Data5", "Data6"], result[1]);
        }

        /// <summary>
        /// 引用符で囲まれたフィールド内の引用符を正しく処理できることを確認します。
        /// </summary>
        [Fact]
        public void ReadFile_ShouldHandleQuotedFieldsWithQouted()
        {
            // Arrange
            var tsvContent = "Header1\tHeader2\tHeader3\n" +
                             "\"Data1\"\"Data1-2\"\tData2\tData3\n" +
                             "Data4\tData5\tData6\n";
            File.WriteAllText(testTsvFilePath, tsvContent);
            int expectedColumnNum = 3;

            // Act
            var result = TsvReader.ReadFile(testTsvFilePath, expectedColumnNum);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(["Data1\"Data1-2", "Data2", "Data3"], result[0]);
            Assert.Equal(["Data4", "Data5", "Data6"], result[1]);
        }

        /// <summary>
        /// 空のファイルを読み込んだ場合、空のリストが返されることを確認します。
        /// </summary>
        [Fact]
        public void ReadFile_ShouldReturnEmptyListForEmptyFile()
        {
            // Arrange
            File.WriteAllText(testTsvFilePath, string.Empty);
            int expectedColumnNum = 3;

            // Act
            var result = TsvReader.ReadFile(testTsvFilePath, expectedColumnNum);

            // Assert
            Assert.Empty(result);
        }

        /// <summary>
        /// 列数が期待値と一致しない場合、InvalidDataException がスローされることを確認します。
        /// </summary>
        [Fact]
        public void ReadFile_ShouldThrowExceptionForIncorrectColumnCount()
        {
            // Arrange
            var tsvContent = "Header1\tHeader2\tHeader3\n" +
                             "Data1\tData2\n"; // 列数が不足している
            File.WriteAllText(testTsvFilePath, tsvContent);
            int expectedColumnNum = 3;

            // Act & Assert
            var exception = Assert.Throws<InvalidDataException>(() => TsvReader.ReadFile(testTsvFilePath, expectedColumnNum));
            Assert.Equal("行 2: 列数が3列ではありません (列数: 2)", exception.Message);
        }

        public void Dispose()
        {
            // テスト終了後に一時TSVファイルを削除
            if (File.Exists(testTsvFilePath))
            {
                File.Delete(testTsvFilePath);
            }
        }
    }
}
