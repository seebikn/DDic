using Xunit;
using DDic.Models;
using System.Data.Common;

namespace DDic.Tests.Models
{
    public class IniFileHandlerTests : IDisposable
    {
        private readonly string testIniFilePath;
        private readonly IniFileHandler iniFileHandler;

        public IniFileHandlerTests()
        {
            // テスト用の一時INIファイルを作成
            testIniFilePath = Path.Combine(Path.GetTempPath(), "test_config.ini");
            iniFileHandler = new IniFileHandler(testIniFilePath);
        }

        /// <summary>
        /// 特定のセクションとキーに対して値を書き込み、その後正しく読み取れることを検証します。
        /// </summary>
        [Fact]
        public void WriteValue_ShouldWriteAndReadValueCorrectly()
        {
            // Arrange
            string section = "Settings";
            string key = "Username";
            string expectedValue = "TestUser";

            // Act
            iniFileHandler.WriteValue(section, key, expectedValue);
            string actualValue = iniFileHandler.ReadValue(section, key);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        /// <summary>
        /// 存在しないキーを読み取ろうとした場合、空の文字列が返されることを確認します。
        /// </summary>
        [Fact]
        public void ReadValue_ShouldReturnEmptyStringForNonExistentKey()
        {
            // Arrange
            string section = "Settings";
            string key = "NonExistentKey";

            // Act
            string value = iniFileHandler.ReadValue(section, key);

            // Assert
            Assert.Equal(string.Empty, value);
        }

        /// <summary>
        /// WriteValue メソッドを呼び出した後、INI ファイルが存在することを確認します。
        /// </summary>
        [Fact]
        public void FileExists_ShouldReturnTrueForExistingFile()
        {
            // Arrange
            iniFileHandler.WriteValue("Settings", "Key", "Value");

            // Act
            bool exists = iniFileHandler.FileExists();

            // Assert
            Assert.True(exists);
        }

        /// <summary>
        /// INI ファイルが存在しない場合、FileExists メソッドが false を返すことを確認します。
        /// </summary>
        [Fact]
        public void FileExists_ShouldReturnFalseForNonExistingFile()
        {
            // Arrange
            if (File.Exists(testIniFilePath))
            {
                File.Delete(testIniFilePath);
            }

            // Act
            bool exists = iniFileHandler.FileExists();

            // Assert
            Assert.False(exists);
        }

        public void Dispose()
        {
            // テスト終了後に一時INIファイルを削除
            if (File.Exists(testIniFilePath))
            {
                File.Delete(testIniFilePath);
            }
        }
    }
}
